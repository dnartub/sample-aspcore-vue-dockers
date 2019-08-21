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
using Web.Host.BLL.BusinessProcesses.LoadVacancies;

namespace Web.Host.Service.Controllers.Api.Vacancy
{
    /// <summary>
    /// Api по работе с вакансиями
    /// </summary>
    [Route("api/[controller]")]
    public class VacancyController : ServiceController
    {
        ICqrsService _cqrsService;
        IServiceProvider _provider;

        public VacancyController(IServiceProvider provider,  ICqrsService cqrsService)
        {
            _provider = provider;
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
                var result = await GetVacancies(null);
                return base.SuccessResult(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка при получении списка актуальных вакансий");
                return base.ErrorResult($"Ошибка при получении списка актуальных вакансий: {ex.Message}");
            }
        }

        private async Task<List<ISourceVacancy>> GetVacancies(Guid? sourceId)
        {
            // TODO: фабрика БП
            var result = await new LoadVacanciesBP() {
                Provider = _provider
            }
            .RunAsync(sourceId);

            return result;
        }
    }
}
