using Cqrs.Interfaces;
using Web.Host.Cqrs.Queries.SourceFromDb;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Activators.Creators;

namespace Web.Host.Cqrs.Queries.VacanciesFromWebSource
{
    public class VacanciesFromWebSourceQueryHandler : IQueryHandler<VacanciesFromWebSourceQuery, List<ISourceVacancy>>
    {
        [DiService]
        public ICqrsService CqrsService { get; set; }
        [DiService]
        public IEnumerable<ISourceParser> Parsers { get; set; }
        [DiService]
        public IWebSourceLoader Loader { get; set; }

        public List<ISourceVacancy> GetResult(VacanciesFromWebSourceQuery query)
        {
            // TODO: по возможности убрать в бизнес слой
            var querySource = new SourceFromDbQuery()
            {
                SourceId = query.SourceId
            };
            var source = CqrsService.GetResult(querySource);


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
            var result = Loader.Load().Result; // в интерфейсе команд нужны асинхронные методы, чтобы убрать такие корявости - но в этом примере, не до тонкой индеальности

            return result;
        }
    }
}
