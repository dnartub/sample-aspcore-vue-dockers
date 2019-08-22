using CsQuery;
using Common.Types;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parsers.Source.Implementations.SourceParsers
{
    /// <summary>
    /// Парсер для Работа.Яндекс
    /// </summary>
    public class RabotaYandexSourceParser : ISourceParser
    {
        public bool IsSuitable(Common.Types.SourceParsers sourceParser)
        {
            return sourceParser == Common.Types.SourceParsers.RabotaYandex;
        }

        public List<ISourceVacancy> Parse(string htmlText)
        {
            throw new NotImplementedException("Метод не реализован. Яндекс блокирует повторные запросы, считая их автоматическими, и выдает капчу");
        }
    }
}
