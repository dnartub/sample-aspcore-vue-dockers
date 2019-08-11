using MsSqlDatabase.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.Source.Interfaces
{
    /// <summary>
    /// Загрузчик с источника
    /// </summary>
    public interface IWebSourceLoader
    {
        /// <summary>
        /// С какого адреса
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IWebSourceLoader UseUrl(string url);

        /// <summary>
        /// Адаптер
        /// (инверсия зависимостей)
        /// </summary>
        /// <param name="parser"></param>
        IWebSourceLoader Use(ISourceParser parser);

        /// <summary>
        /// Загрузка и парсинг данных
        /// </summary>
        /// <returns></returns>
        Task<List<ISourceVacancy>> Load();
    }
}
