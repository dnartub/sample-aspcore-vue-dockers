using Web.Host.Service.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Host.Service.Controllers
{
    /// <summary>
    /// Статус приложения
    /// </summary>
    [Route("api/[controller]")]
    public class StatusController : Controller
    {
        private IServiceStatus ServiceStatus { get; }

        public StatusController(IServiceStatus serviceStatus)
        {
            ServiceStatus = serviceStatus;
        }

        /// <summary>
        /// Статус приложения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = Ok(ServiceStatus);
            return await Task.FromResult(result);
        }
    }
}
