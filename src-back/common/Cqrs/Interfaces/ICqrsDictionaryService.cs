using System;
using System.Collections.Generic;
using System.Text;

namespace Cqrs.Interfaces
{
    /// <summary>
    /// Сервис выдачи обратчика соответствующего действию
    /// </summary>
    public interface ICqrsDictionaryService:IDictionary<Type,Type> 
    {
        /// <summary>
        /// Тип обработчика, для указанного типа действия
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        Type GetHandlerType(Type actionType);
    }
}
