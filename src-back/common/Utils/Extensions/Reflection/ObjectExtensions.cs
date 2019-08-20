using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Extensions.Reflection
{
    public static class ObjectExtensions
    {
        public static async Task<object> InvokeMethodAsync(this object instance, string methodName, object[] parameters)
        {
            var type = instance.GetType();

            var method = type.GetMethod(methodName);
            if (method == null)
            {
                throw new Exception($"Метод '{methodName}' отсутствует в типе '{type.FullName}'");
            }
            
            // указан async
            if (method.GetCustomAttributes(true).Any(a => a.GetType() == typeof(AsyncStateMachineAttribute)))
            {
                var result = await method.InvokeAsync(instance, parameters);
                return result;
            }
            // не указан async, но возращает Task или Task<T>
            else if (method.ReturnType == typeof(Task))
            {
                var result = await method.InvokeAsync(instance, parameters);
                return result;
            }
            else
            {
                var result = method.Invoke(instance, parameters);
                return result;
            }
        }

        public static object FuncInvoke(this object funcInstance, params object[] args)
        {
            var result = (funcInstance as Delegate).DynamicInvoke(args);
            return result;
        }
    }
}
