using AutoMapper;
using Cqrs.Interfaces;
using Web.Host.Cqrs.Models;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Utils.Activators.Creators;

namespace Web.Host.Cqrs.Queries.SourceFromDb
{
    public class SourceFromDbQueryHandler : IQueryHandler<SourceFromDbQuery, Source>
    {
        [DiService]
        public SvContext Context { get; set; }
        [DiService]
        public IMapper Mapper { get; set; }

        public Models.Source GetResult(SourceFromDbQuery query)
        {
            var dal = Context.Sources.Find(query.SourceId);
            return Mapper.Map<Models.Source>(dal);
        }
    }
}
