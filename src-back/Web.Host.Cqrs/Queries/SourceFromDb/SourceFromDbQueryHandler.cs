using AutoMapper;
using Cqrs.Interfaces;
using Web.Host.Cqrs.Models;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Utils.Activators.Creators;
using System.Threading.Tasks;

namespace Web.Host.Cqrs.Queries.SourceFromDb
{
    public class SourceFromDbQueryHandler : IQueryHandler<SourceFromDbQuery, Task<Source>>
    {
        [DiService]
        public SvContext Context { get; set; }
        [DiService]
        public IMapper Mapper { get; set; }

        public async Task<Models.Source> GetResult(SourceFromDbQuery query)
        {
            var dal = await Context.Sources.FindAsync(query.SourceId);
            return Mapper.Map<Models.Source>(dal);
        }
    }
}
