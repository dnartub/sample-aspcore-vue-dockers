using AutoMapper;
using Cqrs.Interfaces;
using Cqrs.Models;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cqrs.Queries.FindSourceInDb
{
    public class FindSourceInDbQueryHandler : IQueryHandler<FindSourceInDbQuery, List<Models.Source>>
    {
        SvContext _context;
        IMapper _mapper;

        public FindSourceInDbQueryHandler(SvContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Models.Source> GetResult(FindSourceInDbQuery query)
        {
            var dals = _context.Sources.Where(query.Predicate).ToList();
            return _mapper.Map<List<Models.Source>>(dals);
        }
    }
}
