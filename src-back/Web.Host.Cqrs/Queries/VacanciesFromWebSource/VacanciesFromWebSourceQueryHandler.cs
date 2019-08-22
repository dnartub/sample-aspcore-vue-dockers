using Cqrs.Interfaces;
using Web.Host.Cqrs.Queries.SourceFromDb;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Activators.Creators;
using System.Threading.Tasks;
using Common.Types;

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
            // выбираем нужный парсер
            var parser = Parsers
                .FirstOrDefault(x => x.IsSuitable(query.Source.SourceParser));

            // подключаем его к загрузчику
            Loader
                .UseUrl(query.Source.Url)
                .Use(parser);

            // загружаем
            var result = await Loader.Load();

            return result;
        }
    }
}
