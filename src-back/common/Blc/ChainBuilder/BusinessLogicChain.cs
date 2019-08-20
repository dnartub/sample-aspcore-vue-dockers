using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Activators;
using Utils.Activators.Creators;
using System.Linq.Expressions;
using System.Reflection;
using Blc.Interfaces;
using System.Runtime.CompilerServices;
using Utils.Extensions.Reflection;

namespace Blc.ChainBuilder
{
    public class BusinessLogicChain<TChainResult>
    {
        public static BusinessLogicChainStep<TStep,TStepResult, TChainResult> New<TStep, TStepResult>(object request) where TStep: IBusinessProcessStep<TStepResult>
        {
            return new BusinessLogicChainStep<TStep,TStepResult, TChainResult>(request, true);
        }

    }


    public class BusinessLogicChainStep<TPreviosStep, TPreviosStepResult, TChainResult> where TPreviosStep : IBusinessProcessStep<TPreviosStepResult>
    {
        public object Request { get; set; }
        public object PreviosChainStep { get; set; }
        // цепочка действий на Exception
        public object OnExceptionFunc { get; set; }
        // Тип исключения при котором вызывать OnExceptionFunc
        public Type ExceptionType { get; set; }
        // реакция на предыдущие результаты: результат(Func<TPreviosStepResult,bool>) - цепочка действий(Func)
        public Dictionary<object, object> OnResults { get; set; } = new Dictionary<object, object>();
        // Первый шаг
        public bool IsFirst { get; set; }

        /// <summary>
        /// создание экзепляра на действие Then
        /// </summary>
        /// <param name="previosChainStep"></param>
        public BusinessLogicChainStep(object previosChainStep)
        {
            PreviosChainStep = previosChainStep;
        }

        /// <summary>
        /// Создание экзепляра на действие New
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isFirst"></param>
        public BusinessLogicChainStep(object request, bool isFirst)
        {
            Request = request;
            IsFirst = true;
        }

        /// <summary>
        /// Следующий шаг
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <typeparam name="TStepResult"></typeparam>
        /// <returns></returns>
        public BusinessLogicChainStep<TStep,TStepResult, TChainResult> Then<TStep, TStepResult>() where TStep : IBusinessProcessStep<TStepResult>
        {
            return new BusinessLogicChainStep<TStep, TStepResult, TChainResult>(this);
        }

        /// <summary>
        /// Реакция на исключение при выполенении предыдущего шага(описанного в Then или New)
        /// </summary>
        /// <typeparam name="TExeception"></typeparam>
        /// <typeparam name="TFinalStep"></typeparam>
        /// <param name="onFailure"></param>
        /// <returns></returns>
        public BusinessLogicChainStep<TPreviosStep, TPreviosStepResult, TChainResult> OnException<TExeception, TFinalStep>(Func<TPreviosStepResult, BusinessLogicChainStep<TFinalStep, TChainResult, TChainResult>> onFailure) where TExeception : Exception where TFinalStep : IBusinessProcessStep<TChainResult>
        {
            OnExceptionFunc = onFailure;
            ExceptionType = typeof(TExeception);
            return this;
        }

        /// <summary>
        /// Реакция на результат выполенении предыдущего шага(описанного в Then или New)
        /// </summary>
        /// <typeparam name="TFinalStep"></typeparam>
        /// <param name="onPreviosResult"></param>
        /// <param name="onResult"></param>
        /// <returns></returns>
        public BusinessLogicChainStep<TPreviosStep, TPreviosStepResult, TChainResult> OnStepResult<TFinalStep>(Func<TPreviosStepResult, bool> onPreviosResult, Func<TPreviosStepResult, BusinessLogicChainStep<TFinalStep, TChainResult, TChainResult>> onResult) where TFinalStep : IBusinessProcessStep<TChainResult>
        {
            OnResults.Add(onPreviosResult, onResult);
            return this;
        }


        public TPreviosStepResult Run(IServiceProvider provider = null)
        {
            try
            {
                return RunAsync(provider).Result;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Выполнение всех шагов
        /// * В случае необработнного исключения (OnException) вызваются методы Cancel всех предыдущих шагов
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<TPreviosStepResult> RunAsync(IServiceProvider provider = null)
        {
            var chainSteps = GetStepsOfMainProcess();


            var executedStepsForCancel = new List<object>();

            var pRequest = chainSteps.First().GetType().GetProperty(nameof(this.Request));
            
            var previosResult = pRequest.GetValue(chainSteps.First());

            foreach (var chainStep in chainSteps)
            {
                // классически, либо с инициализацией свойств из DI
                var stepInstance = CreateBusinessProcessStepInstance(provider, chainStep, previosResult);

                try
                {
                    previosResult = await stepInstance.InvokeMethodAsync("Run", null);

                    // сохраняем в выполненные
                    executedStepsForCancel.Add(stepInstance);

                    var p = chainStep.GetType().GetProperty(nameof(this.OnResults));
                    var onResults = p.GetValue(chainStep) as Dictionary<object, object>;

                    // ветвитель - switcher
                    if (onResults.Count > 0)
                    {
                        // любой ключ первым возвратившим true
                        var key = onResults.Keys
                            .FirstOrDefault(k =>
                            {
                                var keyResult = k.FuncInvoke(previosResult);
                                return (bool)keyResult;
                            });

                        if (key != null)
                        {
                            // вызываем функцию по выполнению другой цепочки
                            var func = onResults[key];
                            var lastChainStep = func.FuncInvoke(previosResult);
                            var result = await lastChainStep.InvokeMethodAsync("Run", new object[] { provider });
                            return (TPreviosStepResult)result;
                        }
                    }
                }
                catch (TargetInvocationException ex)
                {
                    var throwException = ex.InnerException;

                    var p = chainStep.GetType().GetProperty(nameof(this.OnExceptionFunc));
                    var func = p.GetValue(chainStep);
                    if (func != null && (ExceptionType == typeof(Exception) || ExceptionType == throwException.GetType()))
                    {
                        var lastChainStep = func.FuncInvoke(previosResult);
                        try
                        {
                            var result = await lastChainStep.InvokeMethodAsync("Run", new object[] { provider });
                            return (TPreviosStepResult)result;
                        }
                        catch (TargetInvocationException exRunOnFailure)
                        {

                            throwException = exRunOnFailure.InnerException;
                        }
                    }

                    // отменяем ранее выполненные шаги
                    executedStepsForCancel.Reverse();
                    foreach (var cancelStep in executedStepsForCancel)
                    {
                        await cancelStep.InvokeMethodAsync("Cancel", null);
                    }

                    throw throwException;
                }
            }

            return (TPreviosStepResult) previosResult;
        }

        // Все шаги главной ветки, начиная с первого
        private List<object> GetStepsOfMainProcess()
        {
            object currentStep = this;

            var chainSteps = new List<object>();
            while (currentStep != null)
            {
                chainSteps.Add(currentStep);

                var p = currentStep.GetType().GetProperty(nameof(this.PreviosChainStep));

                currentStep = p.GetValue(currentStep);
            }

            chainSteps.Reverse();

            return chainSteps;
        }

        // создаем экземляр класса BusinessProcessStep<>, зная chainStep<TypeBusinessProcessStep,,>, передавая на конструктор  previosResult
        private object CreateBusinessProcessStepInstance(IServiceProvider provider, object chainStep, object previosResult)
        {
            var stepType = chainStep.GetType().GetGenericArguments().First();

            var stepInstance = provider == null
                    ? InstanceCreator
                        .Use<ClassicCreator>(new object[] { previosResult })
                        .Create(stepType)
                    : InstanceCreator
                        .Use<ServiceProviderPropertyCreator>(provider, new object[] { previosResult })
                        .Create(stepType);

            return stepInstance;
        }

    }

}
