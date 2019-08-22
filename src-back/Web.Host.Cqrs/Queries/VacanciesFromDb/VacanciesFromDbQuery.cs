using Common.Types;
using Cqrs.Interfaces;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Host.Cqrs.Queries.VacanciesFromDb
{
    /// <summary>
    /// Возвращает для источника вакансии из БД
    /// <see cref="VacanciesFromDbQueryHandler">
    /// </summary>
    public class VacanciesFromDbQuery: IQuery<Task<List<ISourceVacancy>>>
    {
        public Guid SourceId { get; set; }
    }
}
