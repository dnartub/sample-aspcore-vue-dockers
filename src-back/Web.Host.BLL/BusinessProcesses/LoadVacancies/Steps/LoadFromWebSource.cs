using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Host.Cqrs.Models;
using Blc.Interfaces;
using System.Threading.Tasks;
using Utils.Activators.Creators;
using Cqrs.Interfaces;
using Web.Host.Cqrs.Queries.VacanciesFromWebSource;
using Web.Host.BLL.BusinessProcesses.LoadVacancies.Models;
using System.Net.Http;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps
{
    public class LoadFromWebSource : IBusinessProcessStep<SourceVacanciesModel>
    {
        [DiService]
        public ICqrsService CqrsService { get; set; }

        Source _source;

        public LoadFromWebSource(Source source)
        {
            _source = source;
        }

        public async Task CancelAsync()
        {
            await Task.CompletedTask;
        }

        public async Task<SourceVacanciesModel> RunAsync()
        {
            var queryGetVacancies = new VacanciesFromWebSourceQuery()
            {
                Source = _source
            };

            var vacancies = await CqrsService.GetResult(queryGetVacancies);

            return new SourceVacanciesModel() {
                Source = _source,
                Vacancies = vacancies
            };
        }
    }
}
