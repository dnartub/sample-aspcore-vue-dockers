using Cqrs.Interfaces;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Host.Cqrs.Models;

namespace Web.Host.Cqrs.Queries.VacanciesFromWebSource
{
    /// <summary>
    /// Вакансии с веб-ресурса
    /// <see cref="VacanciesFromWebSourceQueryHandler">
    /// </summary>
    public class VacanciesFromWebSourceQuery: IQuery<Task<List<ISourceVacancy>>>
    {
        public Source Source { get; set; }
    }
}
