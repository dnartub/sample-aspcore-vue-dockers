﻿using Cqrs.Interfaces;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Host.Cqrs.Queries.VacanciesFromWebSource
{
    /// <summary>
    /// Вакансии с веб-ресурса
    /// <see cref="VacanciesFromWebSourceQueryHandler">
    /// </summary>
    public class VacanciesFromWebSourceQuery: IQuery<List<ISourceVacancy>>
    {
        public Guid SourceId { get; set; }
    }
}
