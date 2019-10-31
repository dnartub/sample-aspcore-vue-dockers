using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Host.Service.Controllers.Base;
using System.Linq;
using Serilog;
using System.Threading.Tasks;
using Cqrs.Interfaces;
using Web.Host.Cqrs.Queries.AllSourcesFromDb;
using System.Net;

namespace Web.Host.Service.Controllers.Api.Source
{
    /// <summary>
    /// Api по работе с источниками
    /// </summary>
    [Route("api/[controller]")]
    public class SourceController : ServiceController
    {
        ICqrsService _cqrsService;

        /// <summary>
        /// DI - конструктор
        /// </summary>
        /// <param name="provider"></param>
        public SourceController(ICqrsService cqrsService)
        {
            _cqrsService = cqrsService;
        }

        /// <summary>
        /// Список всех источников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _cqrsService.GetResult(new AllSourcesFromDbQuery());

                Log.Information("Список всех источников {@result}", result);

                return base.SuccessResult(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка при получении списка источников");
                return base.ErrorResult($"Ошибка при получении списка источников: {ex.Message}");
            }
        }

    }
}
