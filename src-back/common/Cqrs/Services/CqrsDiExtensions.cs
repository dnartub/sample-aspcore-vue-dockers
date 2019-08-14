using Cqrs.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cqrs.Services
{
    public static class CqrsDiExtensions
    {
        /// <summary>
        /// Добавление инфраструктуры CQRS
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCqrsServices(this IServiceCollection services)
        {
            // все соответствия действий и обработчиков
            // CommandClass:ICommand -> CommandHandlerClass:ICommandHandler<CommandClass>
            services.AddSingleton<ICqrsDictionaryService, CqrsDictionaryService>();

            // фабрика выдачи обработчика для команды/запроса
            // выполнение команды/запроса
            services.AddScoped<ICqrsService, CqrsService>();

            return services;
        }
    }
}
