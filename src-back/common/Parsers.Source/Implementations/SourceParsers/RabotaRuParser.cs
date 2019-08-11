using CsQuery;
using MsSqlDatabase.Enums;
using Parsers.Source.Implementations.Models;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parsers.Source.Implementations.SourceParsers
{
    /// <summary>
    /// Парсер для Работа.РУ
    /// </summary>
    public class RabotaRuParser : ISourceParser
    {
        private const string hostUrl = "https://www.rabota.ru";

        public bool IsSuitable(MsSqlDatabase.Enums.SourceParsers sourceParser)
        {
            return sourceParser == MsSqlDatabase.Enums.SourceParsers.RabotaRu;
        }

        public List<ISourceVacancy> Parse(string htmlText)
        {
            var result = new List<ISourceVacancy>();

            var cq = CQ.Create(htmlText);

            foreach (IDomObject obj in cq.Find(".list-vacancies__item"))
            {
                var titleNode = obj.Cq()
                    .Find(".list-vacancies__title")
                    .FirstOrDefault();

                var name = titleNode?.GetAttribute("title");
                var url = titleNode?.GetAttribute("href");

                var companyNode = obj.Cq()
                    .Find(".list-vacancies__company-title")
                    .FirstOrDefault();

                var company = companyNode?.GetAttribute("title");
                var cUrl = companyNode?.GetAttribute("href");

                var salary = obj.Cq()
                    .Find(".list-vacancies__salary")
                    .FirstOrDefault()
                    ?.InnerText;

                var annotation = obj.Cq()
                    .Find(".list-vacancies__desc")
                    .FirstOrDefault()
                    ?.InnerText;

                result.Add(new SourceVacancy(){
                    Name = name,
                    Url = hostUrl + url,
                    Address = hostUrl + cUrl,
                    Annotation = annotation,
                    Company = company,
                    Salary = salary
                });
            }

            return result;
        }
    }
}
