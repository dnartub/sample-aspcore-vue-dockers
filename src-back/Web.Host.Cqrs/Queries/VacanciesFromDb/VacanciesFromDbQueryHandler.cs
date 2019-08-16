using AutoMapper;
using Cqrs.Interfaces;
using MsSqlDatabase.Context;
using Parsers.Source.Implementations.Models;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Host.Cqrs.Queries.VacanciesFromDb
{
    public class VacanciesFromDbQueryHandler : IQueryHandler<VacanciesFromDbQuery, List<ISourceVacancy>>
    {
        SvContext _context;
        IMapper _mapper;

        public VacanciesFromDbQueryHandler(SvContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ISourceVacancy> GetResult(VacanciesFromDbQuery query)
        {
            var dals = _context.Vacancies
                .Where(x => x.SourceId == query.SourceId)
                .OrderByDescending(x => x.LoadingTime)
                .Take(50)
                .ToList();

            var result = _mapper.Map<List<SourceVacancy>>(dals)
                .Select(x => x as ISourceVacancy)
                .ToList();

            return result;
        }
    }
}
