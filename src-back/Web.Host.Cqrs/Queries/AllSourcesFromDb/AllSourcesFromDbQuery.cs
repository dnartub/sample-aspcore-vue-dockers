using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Host.Cqrs.Queries.AllSourcesFromDb
{
    /// <summary>
    /// Все источники из БД
    /// <see cref="AllSourcesFromDbQueryHandler"/>
    /// </summary>
    public class AllSourcesFromDbQuery:IQuery<Task<List<Models.Source>>>
    {
    }
}
