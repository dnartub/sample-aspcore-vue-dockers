using Web.Host.Cqrs.Commands.AddVacanciesToDb;
using Cqrs.Interfaces;
using Web.Host.Cqrs.Queries.AllSourcesFromDb;
using Web.Host.Cqrs.Queries.FindSourceInDb;
using Web.Host.Cqrs.Queries.VacanciesFromDb;
using Web.Host.Cqrs.Queries.VacanciesFromWebSource;
using Microsoft.AspNetCore.Mvc;
using MsSqlDatabase.Entities;
using Parsers.Source.Implementations.SourceParsers;
using Parsers.Source.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.Host.Service.Controllers.Base;

namespace Web.Host.Service.Controllers.Api.Vacancy
{
    /// <summary>
    /// Api по работе с вакансиями
    /// </summary>
    [Route("api/[controller]")]
    public class VacancyController : ServiceController
    {
        IWebSourceLoader _webSourceLoader;
        IEnumerable<ISourceParser> _parsers;
        ICqrsService _cqrsService;

        public VacancyController(IWebSourceLoader webSourceLoader, IEnumerable<ISourceParser> parsers, ICqrsService cqrsService)
        {
            _webSourceLoader = webSourceLoader;
            _parsers = parsers;
            _cqrsService = cqrsService;
        }

        /// <summary>
        /// Актуальные вакансии, список
        /// </summary>
        /// <returns></returns>
        [HttpGet("{sourceId:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid sourceId)
        {
            try
            {
                var result = await GetVacancies(sourceId); 
                return base.SuccessResult(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка при получении списка актуальных вакансий");
                return base.ErrorResult($"Ошибка при получении списка актуальных вакансий: {ex.Message}");
            }
        }

        /// <summary>
        /// Актуальные вакансии "Работа.RU"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var query = new FindSourceInDbQuery() {
                    Predicate = x => x.SourceParser == MsSqlDatabase.Enums.SourceParsers.RabotaRu
                };
                var source = (await _cqrsService.GetResult(query))
                    .FirstOrDefault();

                if (source == null)
                {
                    throw new Exception("Нет данных об источнике Работа.RU");
                }

                var result = await GetVacancies(source.Id);

                return base.SuccessResult(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка при получении списка актуальных вакансий");
                return base.ErrorResult($"Ошибка при получении списка актуальных вакансий: {ex.Message}");
            }
        }

        // TODO: выделить бизнес-слой и там управлять логикой вызова команд получения из источника и добавления в БД (Step Builder)
        private async Task<List<ISourceVacancy>> GetVacancies(Guid sourceId)
        {
            try
            {
                // получаем с ресурса
                var queryGetVacancies = new VacanciesFromWebSourceQuery()
                {
                    SourceId = sourceId
                };

                var result = await _cqrsService.GetResult(queryGetVacancies);

                // добавляем в БД
                var commandAddVacanciesToDb = new AddVacanciesToDbCommand()
                {
                    SourceId = sourceId,
                    Vacancies = result
                };
                await _cqrsService.Execute(commandAddVacanciesToDb);

                return result;
            }
            catch (HttpRequestException)
            {
                // сайт недоступен, берем из БД
                var queryGetVacanciesFromDb = new VacanciesFromDbQuery()
                {
                    SourceId = sourceId
                };

                var result = await _cqrsService.GetResult(queryGetVacanciesFromDb);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
