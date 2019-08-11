using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlDatabase.Commands
{
    /// <summary>
    /// Комманды возвращающие результат
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGetCommand<T>
    {
        /// <summary>
        /// Получение данных
        /// </summary>
        /// <param name="provider">доступ к сервисам из DI</param>
        /// <returns></returns>
        T Get(IServiceProvider provider);
    }
}
