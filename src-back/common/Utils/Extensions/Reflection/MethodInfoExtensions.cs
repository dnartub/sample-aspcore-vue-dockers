using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Extensions.Reflection
{
    public static class MethodInfoExtensions
    {
        public static async Task<object> InvokeAsync(this MethodInfo method, object instance, object[] parameters)
        {
            var task = (Task)method.Invoke(instance, parameters);

            await task.ConfigureAwait(false);

            var resultProperty = task.GetType().GetProperty("Result");
            var result = resultProperty.GetValue(task);
            return result;
        }
    }
}
