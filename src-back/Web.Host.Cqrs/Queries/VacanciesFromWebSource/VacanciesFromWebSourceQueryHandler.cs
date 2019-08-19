using Cqrs.Interfaces;
using Web.Host.Cqrs.Queries.SourceFromDb;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Activators.Creators;
using System.Threading.Tasks;

namespace Web.Host.Cqrs.Queries.VacanciesFromWebSource
{
    public class VacanciesFromWebSourceQueryHandler : IQueryHandler<VacanciesFromWebSourceQuery, Task<List<ISourceVacancy>>>
    {
        [DiService]
        public ICqrsService CqrsService { get; set; }
        [DiService]
        public IEnumerable<ISourceParser> Parsers { get; set; }
        [DiService]
        public IWebSourceLoader Loader { get; set; }

        public async Task<List<ISourceVacancy>> GetResult(VacanciesFromWebSourceQuery query)
        {
            // TODO: по возможности убрать в бизнес слой
            var querySource = new SourceFromDbQuery()
            {
                SourceId = query.SourceId
            };
            var source = await CqrsService.GetResult(querySource);


            if (source == null)
            {
                throw new Exception($"Нет данных об источнике {query.SourceId}");
            }

            // выбираем нужный парсер
            var parser = Parsers
                .FirstOrDefault(x => x.IsSuitable(source.SourceParser));

            // подключаем его к загрузчику
            Loader
                .UseUrl(source.Url)
                .Use(parser);

            // загружаем
            var result = await Loader.Load();

            return result;
        }
    }
}
