using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Host.Service.Configuration
{
    public interface IServiceGeneralConfig
    {
        /// <summary>
        /// Наименование сервиса
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Хост
        /// </summary>
        string Host { get; }

        /// <summary>
        /// Описание сервиса
        /// </summary>
        string Description { get; }
    }
}
