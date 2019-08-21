using Blc.Interfaces;
using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utils.Activators.Creators;
using Web.Host.Cqrs.Models;
using Web.Host.Cqrs.Queries.SourceFromDb;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps
{
    public class GetSourceFromDb : IBusinessProcessStep<Source>
    {
        [DiService]
        public ICqrsService CqrsService { get; set; }

        Guid _sourceId;

        public GetSourceFromDb(Guid sourceId)
        {
            _sourceId = sourceId;
        }


        public async Task CancelAsync()
        {
            await Task.CompletedTask;
        }

        public async Task<Source> RunAsync()
        {
            var querySource = new SourceFromDbQuery()
            {
                SourceId = _sourceId
            };
            var source = await CqrsService.GetResult(querySource);

            return source;
        }
    }
}
