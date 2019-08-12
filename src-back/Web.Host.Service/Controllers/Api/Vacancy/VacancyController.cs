using Commands.Source;
using Commands.Vacancy;
using Microsoft.AspNetCore.Mvc;
using MsSqlDatabase.Entities;
using Parsers.Source.Implementations.SourceParsers;
using Parsers.Source.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private IWebSourceLoader WebSourceLoader { get; set; }
        private IEnumerable<ISourceParser> Parsers { get; set; }
        private IServiceProvider Provider { get; set; }

        public VacancyController(IWebSourceLoader webSourceLoader, IEnumerable<ISourceParser> parsers, IServiceProvider provider)
        {
            WebSourceLoader = webSourceLoader;
            Parsers = parsers;
            Provider = provider;
        }

        /// <summary>
        /// Актуальные вакансии, список
        /// </summary>
        /// <returns></returns>
        [HttpGet("{sourceId:Guid}")]
        public IActionResult Get([FromRoute] Guid sourceId)
        {
            try
            {
                var result = new GetVacancies(sourceId)
                    .Get(Provider);

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
        public IActionResult Get()
        {
            try
            {
                var source = new FindSourceInDb(x=>x.SourceParser == MsSqlDatabase.Enums.SourceParsers.RabotaRu)
                    .Get(Provider)
                    .FirstOrDefault();

                if (source == null)
                {
                    throw new Exception("Нет данных об источнике Работа.RU");
                }

                var result = new GetVacancies(source.Id)
                    .Get(Provider);

                return base.SuccessResult(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка при получении списка актуальных вакансий");
                return base.ErrorResult($"Ошибка при получении списка актуальных вакансий: {ex.Message}");
            }
        }
    }
}
