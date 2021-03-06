﻿using System;
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

namespace Blc.ChainBuilder
{
    public class BusinessLogicChain<TChainResult>
    {
        public static BusinessLogicChainStep<TStep,TStepResult, TChainResult> New<TStep, TStepResult>(object request) where TStep: class, IBusinessProcessStep<TStepResult>
        {
            return new BusinessLogicChainStep<TStep,TStepResult, TChainResult>(request);
        }

    }

    public class BusinessLogicChainStep<TCurrentStep, TCurrentStepResult, TChainResult> : IBusinessLogicChainStepInvoker where TCurrentStep : class, IBusinessProcessStep<TCurrentStepResult>
    {
        IBusinessLogicChainStepInvoker _previosBusinessLogicChainStep;

        // входящие данные для первого шага - сквозные по всем шагам
        private object Request { get; set; }
        // цепочка действий на Exception
        private Delegate OnExceptionFunc { get; set; }
        // Тип исключения при котором вызывать OnExceptionFunc
        private Type ExceptionType { get; set; }
        // реакция на предыдущие результаты: результат(Func<TPreviosStepResult,bool>) - цепочка действий(Func)
        private Dictionary<Func<TCurrentStepResult, bool>, Delegate> OnResults { get; set; } = new Dictionary<Func<TCurrentStepResult, bool>, Delegate>();
        // экземпляр реализации IBusinessProcessStep  - создается при выполнении текущего шага BusinessLogicChainStep
        private object PreviosResult { get; set; } 

        /// <summary>
        /// создание экзепляра на действие Then
        /// </summary>
        /// <param name="previosChainStep"></param>
        internal BusinessLogicChainStep(IBusinessLogicChainStepInvoker previosChainStep, object request)
        {
            _previosBusinessLogicChainStep = previosChainStep;
            Request = request;
        }

        /// <summary>
        /// Создание экзепляра на действие New
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isFirst"></param>
        internal BusinessLogicChainStep(object request)
        {
            Request = request;
        }

        /// <summary>
        /// Следующий шаг
        /// </summary>
        /// <typeparam name="TNextStep"></typeparam>
        /// <typeparam name="TNextStepResult"></typeparam>
        /// <returns></returns>
        public BusinessLogicChainStep<TNextStep,TNextStepResult, TChainResult> Then<TNextStep, TNextStepResult>() where TNextStep : class, IBusinessProcessStep<TNextStepResult>
        {
            return new BusinessLogicChainStep<TNextStep, TNextStepResult, TChainResult>(this, this.Request);
        }

        /// <summary>
        /// Реакция на исключение при выполенении предыдущего шага(описанного в Then или New)
        /// </summary>
        /// <typeparam name="TExeception"></typeparam>
        /// <typeparam name="TFinalStep"></typeparam>
        /// <param name="onFailure"></param>
        /// <returns></returns>
        public BusinessLogicChainStep<TCurrentStep, TCurrentStepResult, TChainResult> OnException<TExeception, TPreviosStepResult, TFinalStep>(Func<TPreviosStepResult, BusinessLogicChainStep<TFinalStep, TChainResult, TChainResult>> onFailure) where TExeception : Exception where TFinalStep : class, IBusinessProcessStep<TChainResult>
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
        public BusinessLogicChainStep<TCurrentStep, TCurrentStepResult, TChainResult> OnStepResult<TFinalStep>(Func<TCurrentStepResult, bool> onPreviosResult, Func<TCurrentStepResult, BusinessLogicChainStep<TFinalStep, TChainResult, TChainResult>> onResult) where TFinalStep : class,IBusinessProcessStep<TChainResult>
        {
            OnResults.Add(onPreviosResult, onResult);
            return this;
        }

        /// <summary>
        /// Выполнение всех шагов
        /// * В случае необработнного исключения (OnException) вызваются методы Cancel всех предыдущих шагов
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<TChainResult> RunAsync(IServiceProvider provider = null)
        {
            var chainSteps = GetStepsOfMainProcess();


            var executedSteps = new List<IBusinessLogicChainStepInvoker>();

            var previosResult = this.Request;

            foreach (IBusinessLogicChainStepInvoker chainStep in chainSteps)
            {
                try
                {
                    previosResult = await chainStep.RunBusinessProcessStep(provider, previosResult);

                    // сохраняем в выполненные
                    executedSteps.Add(chainStep);

                    /// альтернативные цепочки - действия на результат шага (<see cref="OnResult">)
                    var finalResult = await chainStep.RunBusinessLogicChainOnResult(previosResult, provider);
                    if (finalResult != null)
                    {
                        return (TChainResult)finalResult;
                    }
                }
                catch (Exception ex)
                {
                    var throwEx = ex;

                    /// альтернативная цепочка - действия на исключение (<see cref="OnException">)
                    try
                    {
                        var finalResult = await chainStep.RunBusinessLogicChainOnException(ex, previosResult, provider);
                        if (finalResult != null)
                        {
                            return (TChainResult)finalResult;
                        }
                    }
                    catch (Exception exInProcessOnException)
                    {
                        throwEx = exInProcessOnException;
                    }


                    // отменяем ранее выполненные шаги
                    executedSteps.Reverse();
                    foreach (var cancelStep in executedSteps)
                    {
                        await cancelStep.CancelBusinessProcessStep(provider);
                    }

                    throw ex;
                }
            }

            return (TChainResult) previosResult;
        }

        // Все шаги главной ветки, начиная с первого
        private List<IBusinessLogicChainStepInvoker> GetStepsOfMainProcess()
        {
            IBusinessLogicChainStepInvoker currentStep = this;

            var chainSteps = new List<IBusinessLogicChainStepInvoker>();
            while (currentStep != null)
            {
                chainSteps.Add(currentStep);
                currentStep = currentStep.PreviosBusinessLogicChainStep;
            }

            chainSteps.Reverse();

            return chainSteps;
        }

        #region == internal non-generic methods/props for invoke ==

        IBusinessLogicChainStepInvoker IBusinessLogicChainStepInvoker.PreviosBusinessLogicChainStep
        {
            get
            {
                return _previosBusinessLogicChainStep;
            }
            set
            {
                _previosBusinessLogicChainStep = value;
            }
        }

        private TCurrentStep CreateCurrentBusinessProcessStep(InstanceCreator instanceCreator, IServiceProvider provider, object previosResult)
        {
            var currentBusinessProcessStep = provider == null
                       ? instanceCreator
                           .Create<ClassicCreator, TCurrentStep>(new object[] { previosResult })
                       : instanceCreator
                           .Create<ServiceProviderPropertyCreator, TCurrentStep>(provider, new object[] { previosResult });
            return currentBusinessProcessStep;
        }

        async Task<object> IBusinessLogicChainStepInvoker.RunBusinessProcessStep(IServiceProvider provider, object previosResult)
        {
            using (var instanceCreator = InstanceCreator.GetContext())
            {
                var result = await CreateCurrentBusinessProcessStep(instanceCreator, provider, previosResult)
                    .RunAsync();

                PreviosResult = previosResult;

                return result;
            }
        }

        async Task IBusinessLogicChainStepInvoker.CancelBusinessProcessStep(IServiceProvider provider)
        {
            if (PreviosResult == null)
            {
                return;
            }

            using (var instanceCreator = InstanceCreator.GetContext())
            {
                var сancelableStep = CreateCurrentBusinessProcessStep(instanceCreator, provider, PreviosResult) as IBusinessProcessStepCancelable;
                if (сancelableStep != null)
                {
                    await сancelableStep.CancelAsync();
                }
            }
        }

        async Task<object> IBusinessLogicChainStepInvoker.RunBusinessLogicChainOnException(Exception cathcedException, object previosResult, IServiceProvider provider)
        {
            if (OnExceptionFunc != null && (ExceptionType == typeof(Exception) || ExceptionType == cathcedException.GetType()))
            {
                // получаем последний шаг в альтернативной цепочке
                var finalChainStep = OnExceptionFunc.DynamicInvoke(previosResult) as IBusinessLogicChainStepInvoker;
                // вызываем функцию по выполнению альтернативной цепочки
                var finalResult = await finalChainStep.RunBusinessLogicChain(provider);
                return finalResult;
            }

            return null;
        }

        async Task<object> IBusinessLogicChainStepInvoker.RunBusinessLogicChainOnResult(object currentResult, IServiceProvider provider)
        {
            // ветвитель - switcher
            if (OnResults.Count > 0)
            {
                var stepName = typeof(TCurrentStep).Name;

                // любой ключ первым возвратившим true
                var key = OnResults.Keys.FirstOrDefault(k => k((TCurrentStepResult)currentResult));

                if (key != null)
                {
                    // получаем последний шаг в альтернативной цепочке
                    var func = OnResults[key];
                    var finalChainStep = func.DynamicInvoke(currentResult) as IBusinessLogicChainStepInvoker;

                    // вызываем функцию по выполнению альтернативной цепочки
                    var finalResult = await finalChainStep.RunBusinessLogicChain(provider);

                    return finalResult;
                }
            }

            return null;
        }


        async Task<object> IBusinessLogicChainStepInvoker.RunBusinessLogicChain(IServiceProvider provider)
        {
            return await RunAsync(provider);
        }

        #endregion

    }

}
