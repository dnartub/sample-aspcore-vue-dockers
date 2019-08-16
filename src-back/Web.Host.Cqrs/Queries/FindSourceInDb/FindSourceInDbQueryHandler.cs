using AutoMapper;
using Cqrs.Interfaces;
using Web.Host.Cqrs.Models;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Activators.Creators;

namespace Web.Host.Cqrs.Queries.FindSourceInDb
{
    public class FindSourceInDbQueryHandler : IQueryHandler<FindSourceInDbQuery, List<Models.Source>>
    {
        [DiService]
        public SvContext Context { get; set; }
        [DiService]
        public IMapper Mapper { get; set; }

        public List<Models.Source> GetResult(FindSourceInDbQuery query)
        {
            var dals = Context.Sources.Where(query.Predicate).ToList();
            return Mapper.Map<List<Models.Source>>(dals);
        }
    }
}
