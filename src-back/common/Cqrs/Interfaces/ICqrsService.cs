using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.Interfaces
{
    /// <summary>
    /// Фабрика получения обработчиков действий (комманд и запросов)
    /// Выполнение обработчиков
    /// </summary>
    public interface ICqrsService
    {
        /// <summary>
        /// Выполнение команды на изменение данных
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        void Execute<TCommand>(TCommand command) where TCommand : ICommand;
        /// <summary>
        /// Выполнение запроса на получение данных
        /// </summary>
        TResult GetResult<TResult>(IQuery<TResult> query);
        /// <summary>
        /// Получение обработчика для выполнения команды
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        ICommandHandler<TCommand> GetHandler<TCommand>() where TCommand : ICommand;

        /// <summary>
        /// Получение обработчика для выпонения запроса
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryHandler<TQuery, TResult> GetHandler<TQuery, TResult>() where TQuery : IQuery<TResult>;
    }
}
