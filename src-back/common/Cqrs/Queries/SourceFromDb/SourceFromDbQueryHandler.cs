using AutoMapper;
using Cqrs.Interfaces;
using Cqrs.Models;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cqrs.Queries.SourceFromDb
{
    public class SourceFromDbQueryHandler : IQueryHandler<SourceFromDbQuery, Models.Source>
    {
        SvContext _context;
        IMapper _mapper;

        public SourceFromDbQueryHandler(SvContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Models.Source GetResult(SourceFromDbQuery query)
        {
            var dal = _context.Sources.Find(query.SourceId);
            return _mapper.Map<Models.Source>(dal);
        }
    }
}
