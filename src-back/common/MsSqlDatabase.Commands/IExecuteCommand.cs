using System;
using System.Collections.Generic;
using System.Text;

namespace Commands
{
    /// <summary>
    /// Команды изменения данных
    /// </summary>
    public interface IExecuteCommand
    {
        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="provider">доступ к сервисам из DI</param>
        void Execute(IServiceProvider provider);
    }
}
