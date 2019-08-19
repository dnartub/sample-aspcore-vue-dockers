using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Web.Host.Cqrs.Queries.FindSourceInDb
{
    /// <summary>
    /// Поиск среди Источников в БД
    /// <see cref="FindSourceInDbQueryHandler"/>
    /// </summary>
    public class FindSourceInDbQuery:IQuery<Task<List<Models.Source>>>
    {
        public Expression<Func<MsSqlDatabase.Entities.Source, Boolean>> Predicate { get; set; }
    }
}
