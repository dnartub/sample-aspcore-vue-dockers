using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MsSqlDatabase.Context;
using System.Linq;
using AutoMapper;
using Parsers.Source.Implementations.Models;

namespace Cqrs.Vacancy
{
    /// <summary>
    /// Получение вакансий из БД
    /// </summary>
    public class GetVacanciesFromDb: IGetCommand<List<ISourceVacancy>>
    {
        Guid _sourceId;

        public GetVacanciesFromDb(Guid sourceId)
        {
            _sourceId = sourceId;
        }

        public List<ISourceVacancy> Get(IServiceProvider provider)
        {
            var context = provider.GetService<SvContext>();
            var mapper = provider.GetService<IMapper>();

            var dals =  context.Vacancies
                .Where(x=>x.SourceId == _sourceId)
                .OrderByDescending(x=>x.LoadingTime)
                .Take(50)
                .ToList();

            var result = mapper.Map<List<SourceVacancy>>(dals)
                .Select(x=> x as ISourceVacancy)
                .ToList();

            return result;
        }
    }
}
