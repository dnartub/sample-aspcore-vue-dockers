using AutoMapper;
using Cqrs.Interfaces;
using MsSqlDatabase.Context;
using Parsers.Source.Implementations.Models;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Activators.Creators;

namespace Web.Host.Cqrs.Queries.VacanciesFromDb
{
    public class VacanciesFromDbQueryHandler : IQueryHandler<VacanciesFromDbQuery, List<ISourceVacancy>>
    {
        [DiService]
        public SvContext Context { get; set; }
        [DiService]
        public IMapper Mapper { get; set; }

        public List<ISourceVacancy> GetResult(VacanciesFromDbQuery query)
        {
            var dals = Context.Vacancies
                .Where(x => x.SourceId == query.SourceId)
                .OrderByDescending(x => x.LoadingTime)
                .Take(50)
                .ToList();

            var result = Mapper.Map<List<SourceVacancy>>(dals)
                .Select(x => x as ISourceVacancy)
                .ToList();

            return result;
        }
    }
}
