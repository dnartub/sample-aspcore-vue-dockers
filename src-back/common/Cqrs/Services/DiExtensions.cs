using Cqrs.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Utils.Activators;

namespace Cqrs.Services
{
    public static class DiExtensions
    {
        /// <summary>
        /// Добавление инфраструктуры CQRS
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCqrsServices(this IServiceCollection services, Type[] typesFromAsseblies)
        {
            // все соответствия действий и обработчиков (поддключаеммая сборка определяется типом из этой сборки)
            // CommandClass:ICommand -> CommandHandlerClass:ICommandHandler<CommandClass>
            services.AddSingleton<ICqrsDictionaryService, CqrsDictionaryService>(sp => new CqrsDictionaryService(typesFromAsseblies));

            // фабрика выдачи обработчика для команды/запроса
            // выполнение команды/запроса
            services.AddScoped<ICqrsService, CqrsService>();

            return services;
        }
    }
}
