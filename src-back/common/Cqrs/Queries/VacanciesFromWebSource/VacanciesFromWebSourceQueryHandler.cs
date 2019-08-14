using Cqrs.Interfaces;
using Cqrs.Queries.SourceFromDb;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cqrs.Queries.VacanciesFromWebSource
{
    public class VacanciesFromWebSourceQueryHandler : IQueryHandler<VacanciesFromWebSourceQuery, List<ISourceVacancy>>
    {
        ICqrsService _cqrsService;
        IEnumerable<ISourceParser> _parsers;
        IWebSourceLoader _loader;

        public VacanciesFromWebSourceQueryHandler(ICqrsService cqrsService, IEnumerable<ISourceParser> parsers, IWebSourceLoader loader )
        {
            _cqrsService = cqrsService;
            _parsers = parsers;
            _loader = loader;
        }


        public List<ISourceVacancy> GetResult(VacanciesFromWebSourceQuery query)
        {
            // TODO: по возможности убрать в бизнес слой
            var querySource = new SourceFromDbQuery()
            {
                SourceId = query.SourceId
            };
            var source = _cqrsService.Execute<SourceFromDbQuery, Models.Source>(querySource);


            if (source == null)
            {
                throw new Exception($"Нет данных об источнике {query.SourceId}");
            }

            // выбираем нужный парсер
            var parser = _parsers
                .FirstOrDefault(x => x.IsSuitable(source.SourceParser));

            // подключаем его к загрузчику
            _loader
                .UseUrl(source.Url)
                .Use(parser);

            // загружаем
            var result = _loader.Load().Result; // в интерфейсе команд нужны асинхронные методы, чтобы убрать такие корявости - но в этом примере, не до тонкой индеальности

            return result;
        }
    }
}
