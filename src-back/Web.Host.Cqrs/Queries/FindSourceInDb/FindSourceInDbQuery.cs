using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Web.Host.Cqrs.Queries.FindSourceInDb
{
    /// <summary>
    /// Поиск среди Источников в БД
    /// <see cref="FindSourceInDbQueryHandler"/>
    /// </summary>
    public class FindSourceInDbQuery:IQuery<List<Models.Source>>
    {
        public Expression<Func<MsSqlDatabase.Entities.Source, Boolean>> Predicate { get; set; }
    }
}
