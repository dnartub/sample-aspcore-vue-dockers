using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Host.Cqrs.Models;
using Blc.Interfaces;
using System.Threading.Tasks;
using Utils.Activators.Creators;
using Cqrs.Interfaces;
using Web.Host.Cqrs.Queries.VacanciesFromDb;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps
{
    public class LoadFromDb : IBusinessProcessStep<List<ISourceVacancy>>
    {
        [DiService]
        public ICqrsService CqrsService { get; set; }

        Source _source;

        public LoadFromDb(Source source)
        {
            _source = source;
        }

        public async Task CancelAsync()
        {
            await Task.CompletedTask;
        }

        public async Task<List<ISourceVacancy>> RunAsync()
        {
            var queryGetVacanciesFromDb = new VacanciesFromDbQuery()
            {
                SourceId = _source.Id
            };

            var result = await CqrsService.GetResult(queryGetVacanciesFromDb);

            return result;
        }
    }
}
