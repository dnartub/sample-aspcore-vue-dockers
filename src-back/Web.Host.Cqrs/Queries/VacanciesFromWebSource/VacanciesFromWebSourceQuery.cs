using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Host.Cqrs.Queries.VacanciesFromWebSource
{
    /// <summary>
    /// Вакансии с веб-ресурса
    /// <see cref="VacanciesFromWebSourceQueryHandler">
    /// </summary>
    public class VacanciesFromWebSourceQuery: IQuery
    {
        public Guid SourceId { get; set; }
    }
}
