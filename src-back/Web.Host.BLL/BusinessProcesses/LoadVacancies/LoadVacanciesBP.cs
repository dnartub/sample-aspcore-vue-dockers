using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps;
using Web.Host.Cqrs.Models;
using Blc.Interfaces;
using Blc.ChainBuilder;
using System.Threading.Tasks;
using Web.Host.BLL.BusinessProcesses.LoadVacancies.Models;
using Utils.Activators.Creators;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies
{
    public class LoadVacanciesBP : IBusinessProcess<Guid, List<ISourceVacancy>>
    {
        [DiService]
        public IServiceProvider Provider { get; set; }

        public async Task<List<ISourceVacancy>> RunAsync(Guid sourceId) => 
            await BusinessLogicChain<List<ISourceVacancy>>
                        .New<GetSourceFromDb, Source>(sourceId)
                        .Then<LoadFromWebSource, SourceVacanciesModel>()
                            .OnException<HttpRequestException, Source, LoadFromDb>(source =>
                                    BusinessLogicChain<List<ISourceVacancy>>
                                    .New<LoadFromDb, List<ISourceVacancy>>(source)
                             )
                        .Then<SaveToDb, List<ISourceVacancy>>()
                        .RunAsync(Provider);

    }
}
