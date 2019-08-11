using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Host.Service.Configuration
{
    public interface IServiceStatus
    {
        /// <summary>
        /// Наименование сервиса
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Описание сервиса
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Дата-время запуска сервиса
        /// </summary>
        DateTime Started { get; set; }
    }
}
