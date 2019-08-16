using AutoMapper;
using Cqrs.Interfaces;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Activators.Creators;

namespace Web.Host.Cqrs.Queries.AllSourcesFromDb
{
    public class AllSourcesFromDbQueryHandler : IQueryHandler<AllSourcesFromDbQuery, List<Models.Source>>
    {
        [DiService]
        public SvContext Context { get; set; }
        [DiService]
        public IMapper Mapper { get; set; }

        public List<Models.Source> GetResult(AllSourcesFromDbQuery query)
        {
            var dals = Context.Sources.ToList();
            return Mapper.Map<List<Models.Source>>(dals);
        }
    }
}
