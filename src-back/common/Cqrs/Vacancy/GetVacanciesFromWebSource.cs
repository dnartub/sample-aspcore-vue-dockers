using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MsSqlDatabase.Context;
using System.Linq;
using AutoMapper;
using Parsers.Source.Implementations.Models;
using Cqrs.Source;

namespace Cqrs.Vacancy
{
    /// <summary>
    /// Получение вакансий с источника
    /// </summary>
    public class GetVacanciesFromWebSource : IGetCommand<List<ISourceVacancy>>
    {
        Guid _sourceId;

        public GetVacanciesFromWebSource(Guid sourceId)
        {
            _sourceId = sourceId;
        }

        public List<ISourceVacancy> Get(IServiceProvider provider)
        {
            var source = new GetSourceFromDb(_sourceId)
                    .Get(provider);

            if (source == null)
            {
                throw new Exception($"Нет данных об источнике {_sourceId}");
            }

            // выбираем нужный парсер
            var parser = provider
                .GetService<IEnumerable<ISourceParser>>()
                .FirstOrDefault(x => x.IsSuitable(source.SourceParser));

            // подключаем его к загрузчику
            var loader = provider
                .GetService<IWebSourceLoader>()
                .UseUrl(source.Url)
                .Use(parser);

            // загружаем
            var result = loader.Load().Result; // в интерфейсе команд нужны асинхронные методы, чтобы убрать такие корявости - но в этом примере, не до тонкой индеальности

            return result;
        }
    }
}
