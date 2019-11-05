using AppMetrics.Interfaces;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Serilog;

namespace AppMetrics.Filters
{
    public class GlobalActionFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // logic before action goes here
            await AddMetricsFromContext(context);

            // the actual action
            await next(); 
            
            // logic after the action goes here
        }

        private async Task AddMetricsFromContext(ActionExecutingContext context)
        {
            try
            {
                var requestMetricsService = context.HttpContext.RequestServices.GetService<IRequestMetricsService>();
                if (requestMetricsService == null)
                {
                    throw new Exception($"Не задан DI-сервис {nameof(IRequestMetricsService)}");
                }

                var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var actionMethodInfo = controllerActionDescriptor.MethodInfo;
                var methodInfo = $"{actionMethodInfo.DeclaringType.FullName}.{actionMethodInfo.Name}({string.Join(", ", actionMethodInfo.GetParameters().Select(pi => $"{pi.ParameterType.Name} {pi.Name}"))})";

                
                await requestMetricsService.AddRequest(methodInfo);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AddMetricsFromContext exception");
            }
        }
    }
}
