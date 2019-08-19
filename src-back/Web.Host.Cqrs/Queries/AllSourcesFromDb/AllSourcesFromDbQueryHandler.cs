using AutoMapper;
using Cqrs.Interfaces;
using Microsoft.EntityFrameworkCore;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Activators.Creators;

namespace Web.Host.Cqrs.Queries.AllSourcesFromDb
{
    public class AllSourcesFromDbQueryHandler : IQueryHandler<AllSourcesFromDbQuery, Task<List<Models.Source>>>
    {
        [DiService]
        public SvContext Context { get; set; }
        [DiService]
        public IMapper Mapper { get; set; }

        public async Task<List<Models.Source>> GetResult(AllSourcesFromDbQuery query)
        {
            var dals = await Context.Sources.ToListAsync();
            return Mapper.Map<List<Models.Source>>(dals);
        }
    }
}
