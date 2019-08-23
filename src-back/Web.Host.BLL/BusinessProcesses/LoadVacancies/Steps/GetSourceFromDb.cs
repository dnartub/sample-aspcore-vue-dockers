using Blc.Interfaces;
using Cqrs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Activators.Creators;
using Web.Host.Cqrs.Models;
using Web.Host.Cqrs.Queries.FindSourceInDb;
using Web.Host.Cqrs.Queries.SourceFromDb;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps
{
    public class GetSourceFromDb : IBusinessProcessStep<Source>
    {
        [DiService]
        public ICqrsService CqrsService { get; set; }

        Guid? _sourceId;

        public GetSourceFromDb(Guid? sourceId)
        {
            _sourceId = sourceId;
        }

        public async Task<Source> RunAsync()
        {
            if (_sourceId.HasValue)
            {
                return await GetById(_sourceId.Value);
            }
            else
            {
                return await GetDefault();
            }
        }

        public async Task<Source> GetDefault()
        {
            var query = new FindSourceInDbQuery()
            {
                Predicate = x => x.SourceParser == Common.Types.SourceParsers.RabotaRu
            };
            var source = (await CqrsService.GetResult(query))
                .FirstOrDefault();

            if (source == null)
            {
                throw new Exception("Нет данных об источнике Работа.RU");
            }

            return source;
        }

        public async Task<Source> GetById(Guid sourceId)
        {
            var querySource = new SourceFromDbQuery()
            {
                SourceId = sourceId
            };
            var source = await CqrsService.GetResult(querySource);

            return source;
        }
    }
}
