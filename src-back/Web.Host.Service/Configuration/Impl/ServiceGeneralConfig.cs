using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Host.Service.Configuration.Impl
{
    internal class ServiceGeneralConfig: IServiceGeneralConfig
    {
        /// <summary>
        /// Наименование сервиса
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Хост
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Описание сервиса
        /// </summary>
        public string Description { get; set; }
    }
}
