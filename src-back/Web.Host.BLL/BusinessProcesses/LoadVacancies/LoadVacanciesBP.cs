using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps;
using Web.Host.Cqrs.Models;
using Blc.Interfaces;
using Blc.ChainBuilder;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies
{
    public class LoadVacanciesBP : IBusinessProcess<Guid, List<ISourceVacancy>>
    {
        public List<ISourceVacancy> Run(Guid sourceId)
        {
            var t =  BusinessLogicChain<List<ISourceVacancy>>
                        .New<GetSourceFromDb, Source>(sourceId)
                        .Then<LoadFromWebSource, List<ISourceVacancy>>()
                            .OnException<HttpRequestException, LoadFromDb>(source =>
                                    BusinessLogicChain<List<ISourceVacancy>>
                                    .New<LoadFromDb, List<ISourceVacancy>>(source)
                             )
                        .Then<SaveToDb, List<ISourceVacancy>>()
                        .Run();

            return t;
        }
    }
}
