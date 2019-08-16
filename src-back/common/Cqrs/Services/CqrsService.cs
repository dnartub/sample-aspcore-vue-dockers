using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utils.Activators;
using Utils.Activators.Creators;

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

        // этот метод вызывается через рефлексию в виду замороченног преобразования к generic-интерфейсу
        public TResult GenericTagetMethodExecuteQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            var handlerInstance = CreateHandlerInstance(query) as IQueryHandler<TQuery, TResult>;
            var result = handlerInstance.GetResult(query);
            return result;
        }

        public TResult Execute<TResult>(IQuery query) 
        {
            // передача типа на generic-метод
            MethodInfo method = typeof(CqrsService).GetMethod(nameof(GenericTagetMethodExecuteQuery));
            MethodInfo genericMethod = method.MakeGenericMethod(query.GetType(),typeof(TResult));
            var result = genericMethod.Invoke(this, new object[] { query });
            return (TResult)result;
        }

        public ICommandHandler<TCommand> GetHandler<TCommand>() where TCommand : ICommand
        {
            var handlerInstance = CreateHandlerInstance<TCommand>();
            return handlerInstance as ICommandHandler<TCommand>;
        }

        public IQueryHandler<TQuery, TResult> GetHandler<TQuery, TResult>() where TQuery : IQuery
        {
            var handlerInstance = CreateHandlerInstance<TQuery>();
            return handlerInstance as IQueryHandler<TQuery, TResult>;
        }

        private object CreateHandlerInstance<TAction>()
        {
            // берем из соответствия IHandler<IAction>
            var handlerType = CqrsDictionaryService.GetHandlerType(typeof(TAction));
            return CreateHandlerInstance(handlerType);
        }

        private object CreateHandlerInstance<TAction>(TAction action)
        {
            // берем из соответствия IHandler<IAction>
            var handlerType = CqrsDictionaryService.GetHandlerType(typeof(TAction));
            return CreateHandlerInstance(handlerType);
        }

        /// <summary>
        /// Получения экземпляра обработчика действия
        /// </summary>
        /// <typeparam name="TAction"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        private object CreateHandlerInstance(Type handlerType)
        {
            return InstanceCreator
                .Use<ServiceProviderCreator>(Provider)
                .Create(handlerType);
        }
    }
}
