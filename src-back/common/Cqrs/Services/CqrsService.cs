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

        public async Task Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handlerInstance = CreateHandlerInstance(command) as ICommandHandler<TCommand>;
            await handlerInstance.Execute(command);
        }

        public async Task Down<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handlerInstance = CreateHandlerInstance(command) as ICommandHandler<TCommand>;
            await handlerInstance.Down(command);
        }

        public TResult GetResult<TResult>(IQuery<TResult> query) 
        {
            // передача типа на generic-метод
            MethodInfo method = typeof(CqrsService).GetMethod(nameof(GetResultTemplete));
            MethodInfo genericMethod = method.MakeGenericMethod(query.GetType(), typeof(TResult));
            var result = genericMethod.Invoke(this, new object[] { query });
            return (TResult)result;
        }

        public TResult GetResultTemplete<TQuery,TResult>(TQuery query) where TQuery: IQuery<TResult>
        {
            var handlerInstance = CreateHandlerInstance(query) as IQueryHandler<TQuery, TResult>;
            return handlerInstance.GetResult(query);
        }

        public ICommandHandler<TCommand> GetHandler<TCommand>() where TCommand : ICommand
        {
            var handlerInstance = CreateHandlerInstance<TCommand>();
            return handlerInstance as ICommandHandler<TCommand>;
        }

        public IQueryHandler<TQuery, TResult> GetHandler<TQuery, TResult>() where TQuery : IQuery<TResult>
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
            var handlerType = CqrsDictionaryService.GetHandlerType(action.GetType());
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
                .Use<ServiceProviderPropertyCreator>(Provider)
                .Create(handlerType);
        }
    }
}
