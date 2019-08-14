using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.Services
{
    public class CqrsService : ICqrsService
    {
        private ICqrsDictionaryService CqrsDictionaryService { get; set; }
        private IServiceProvider Provider { get; set; }

        public CqrsService(ICqrsDictionaryService cqrsDictionaryService, IServiceProvider provider)
        {
            CqrsDictionaryService = cqrsDictionaryService;
            Provider = provider;
        }

        public void Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handlerInstance = CreateHandlerInstance(command) as ICommandHandler<TCommand>;
            handlerInstance.Execute(command);
        }

        public TResult Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            var handlerInstance = CreateHandlerInstance(query) as IQueryHandler<TQuery, TResult>;
            var result = handlerInstance.GetResult(query);
            return result;
        }


        public ICommandHandler<TCommand> GetHandler<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handlerInstance = CreateHandlerInstance(command);
            return handlerInstance as ICommandHandler<TCommand>;
        }

        public IQueryHandler<TQuery, TResult> GetHandler<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            var handlerInstance = CreateHandlerInstance(query);
            return handlerInstance as IQueryHandler<TQuery, TResult>;
        }

        /// <summary>
        /// Получения экземпляра обработчика действия
        /// </summary>
        /// <typeparam name="TAction"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        private object CreateHandlerInstance<TAction>(TAction action)
        {
            // берем из соответствия IHandler<IAction>
            var handlerType = CqrsDictionaryService.GetHandlerType(typeof(TAction));

            var handlerTypeConstructors = handlerType.GetConstructors();
            
            // должен быть только один DI-конструктор, чтобы исключить многословность c#
            if (handlerTypeConstructors.Length != 1)
            {
                throw new ApplicationException($"Класс {handlerType.FullName} имеет более одного конструктора. В текущей архитектуре может быть только один DI-конструктор");
            }

            var handlerTypeConstructor = handlerTypeConstructors.First();

            // инициализируем параметры конструктора из DI-сервисов
            var parameters = handlerTypeConstructor.GetParameters()
                .Select(p => {
                    // все типы параметров должны быть прописаны в DI-сервисах (никаких int a, string b и т.п. "левых" типов)
                    var paramInstance = Provider.GetService(p.ParameterType);
                    if (paramInstance == null)
                    {
                        throw new ApplicationException($"Тип параметра конструктора `{p.ParameterType.Name} {p.Name}` класса `{handlerType.FullName}` не найден в сервисах DI. Возможно тип `{p.ParameterType.Name}` не добавлен в ServiceCollection на инициализации приложения в классе Startup.ConfigureServices");
                    }
                    return paramInstance;
                })
                .ToArray();

            // вызываем конструктор - создаем экземпляр
            var handlerInstance = handlerTypeConstructor.Invoke(parameters);

            return handlerInstance;
        }
    }
}
