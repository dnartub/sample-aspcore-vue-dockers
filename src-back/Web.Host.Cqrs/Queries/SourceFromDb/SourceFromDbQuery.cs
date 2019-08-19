using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Host.Cqrs.Queries.SourceFromDb
{
    /// <summary>
    /// Источник из БД
    /// <see cref="SourceFromDbQueryHandler">
    /// </summary>
    public class SourceFromDbQuery:IQuery<Task<Models.Source>>
    {
        public Guid SourceId { get; set; }
    }
}
