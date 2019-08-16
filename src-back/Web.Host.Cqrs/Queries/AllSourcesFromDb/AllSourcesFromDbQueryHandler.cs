using AutoMapper;
using Cqrs.Interfaces;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Host.Cqrs.Queries.AllSourcesFromDb
{
    public class AllSourcesFromDbQueryHandler : IQueryHandler<AllSourcesFromDbQuery, List<Models.Source>>
    {
        SvContext _context;
        IMapper _mapper;

        public AllSourcesFromDbQueryHandler(SvContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Models.Source> GetResult(AllSourcesFromDbQuery query)
        {
            var dals = _context.Sources.ToList();
            return _mapper.Map<List<Models.Source>>(dals);
        }
    }
}
