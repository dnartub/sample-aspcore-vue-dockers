using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Host.Cqrs.Queries.SourceFromDb
{
    /// <summary>
    /// Источник из БД
    /// <see cref="SourceFromDbQueryHandler">
    /// </summary>
    public class SourceFromDbQuery:IQuery
    {
        public Guid SourceId { get; set; }
    }
}
