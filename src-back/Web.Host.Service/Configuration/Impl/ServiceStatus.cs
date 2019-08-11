using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Host.Service.Configuration.Impl
{
    internal class ServiceStatus:IServiceStatus
    {
        /// <summary>
        /// Наименование сервиса
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание сервиса
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата-время запуска сервиса
        /// </summary>
        public DateTime Started { get; set; }
    }
}
