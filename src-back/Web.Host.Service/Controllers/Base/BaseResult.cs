using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Host.Service.Controllers.Base
{
    /// <summary>
    /// Ответ от сервера
    /// </summary>
    public class BaseResult
    {
        /// <summary>
        /// Результат выполенения
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// Данные
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// Результаты выполенения
    /// </summary>
    public class ApiResultType
    {
        /// <summary>
        /// Положительный
        /// </summary>
        public static readonly string SUCCESS = "success";
        /// <summary>
        /// Ошибка
        /// </summary>
        public static readonly string ERROR = "error";
        /// <summary>
        /// Предупреждение
        /// </summary>
        public static readonly string WARNING = "warning";
    }
}
