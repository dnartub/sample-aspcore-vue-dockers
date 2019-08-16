using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Host.Cqrs.Queries.VacanciesFromDb
{
    /// <summary>
    /// Возвращает для источника вакансии из БД
    /// <see cref="VacanciesFromDbQueryHandler">
    /// </summary>
    public class VacanciesFromDbQuery: IQuery
    {
        public Guid SourceId { get; set; }
    }
}
