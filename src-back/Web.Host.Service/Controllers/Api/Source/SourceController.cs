using Microsoft.AspNetCore.Mvc;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Host.Service.Controllers.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Serilog;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Commands.Source;

namespace Web.Host.Service.Controllers.Api.Source
{
    /// <summary>
    /// Api по работе с источниками
    /// </summary>
    [Route("api/[controller]")]
    public class SourceController : ServiceController
    {
        private IServiceProvider Provider { get; set; }

        /// <summary>
        /// DI - конструктор
        /// </summary>
        /// <param name="provider"></param>
        public SourceController(IServiceProvider provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// Список всех источников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = new GetAllSourcesFromDb()
                    .Get(Provider);

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
