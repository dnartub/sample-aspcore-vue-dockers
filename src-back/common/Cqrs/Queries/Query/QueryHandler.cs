using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cqrs.Queries.Query
{
    abstract class QueryHandler : IQueryHandler<Query, object>
    {
        public object GetResult(Query query)
        {
            throw new NotImplementedException();
        }
    }
}
