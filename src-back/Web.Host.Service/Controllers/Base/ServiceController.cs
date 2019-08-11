using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Host.Service.Controllers.Base
{
    /// <summary>
    /// Базовый класс контроллер приложения
    /// </summary>
    public class ServiceController : Controller
    {
        /// <summary>
        /// Положительный результат
        /// </summary>
        protected ObjectResult SuccessResult(object obj)
        {
            return StatusCode(StatusCodes.Status200OK, new BaseResult
            {
                Result = ApiResultType.SUCCESS,
                Data = obj
            });
        }

        /// <summary>
        /// Ошибка
        /// </summary>
        protected ObjectResult ErrorResult(string errorMessage)
        {
            return StatusCode(StatusCodes.Status200OK, new BaseResult
            {
                Result = ApiResultType.ERROR,
                Data = errorMessage
            });
        }

        /// <summary>
        /// Предупреждение
        /// </summary>
        protected ObjectResult WarningResult(string errorMessage)
        {
            return StatusCode(StatusCodes.Status200OK, new BaseResult
            {
                Result = ApiResultType.WARNING,
                Data = errorMessage
            });
        }

    }
}
