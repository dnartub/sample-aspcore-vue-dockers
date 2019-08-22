using AutoMapper;
using Cqrs.Interfaces;
using Web.Host.Cqrs.Models;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Activators.Creators;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Web.Host.Cqrs.Queries.FindSourceInDb
{
    public class FindSourceInDbQueryHandler : IQueryHandler<FindSourceInDbQuery, Task<List<Models.Source>>>
    {
        [DiService]
        public SvContext Context { get; set; }
        [DiService]
        public IMapper Mapper { get; set; }

        public async Task<List<Models.Source>> GetResult(FindSourceInDbQuery query)
        {
            var dals = await Context.Sources.Where(query.Predicate).ToListAsync();
            return Mapper.Map<List<Models.Source>>(dals);
        }
    }
}
