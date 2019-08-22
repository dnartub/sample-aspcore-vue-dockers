using Common.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.Source.Interfaces
{
    /// <summary>
    /// Парсер
    /// </summary>
    public interface ISourceParser
    {
        /// <summary>
        /// Парсинг вакансий из html
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        List<ISourceVacancy> Parse(string htmlText);
        /// <summary>
        /// Подходит ли парсер для источника
        /// </summary>
        /// <param name="sourceParser"></param>
        /// <returns></returns>
        bool IsSuitable(SourceParsers sourceParser);
    }
}
