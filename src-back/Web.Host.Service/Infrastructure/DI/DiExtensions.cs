using AutoMapper;
using Cqrs.Interfaces;
using Cqrs.Services;
using HttpRequest.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsSqlDatabase.Context;
using MsSqlDatabase.Initializers;
using Parsers.Source.Implementations.Models;
using Parsers.Source.Implementations.SourceParsers;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Web.Host.Service.Configuration;
using Web.Host.Service.Configuration.Impl;

namespace Web.Host.Service.Infrastructure.DI
{
    /// <summary>
    /// Расширения для добавления DI-сервисов
    /// </summary>
    public static class DiExtensions
    {
        /// <summary>
        /// Подключение контекстов БД
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            // БД, либо в памяти, либо по по строке подключения
            string sqlConnection = configuration.GetConnectionString("SqlProvider");

            if (string.IsNullOrEmpty(sqlConnection))
            {
                // для того, чтобы данные были доступны между потоками/запросами 
                // memory context должен быть сигнлетоном, иначе все добапвленные данные одним процессом будут очищены при его запершении на Dispose
                // (такие конструкции применяются только для тестов, либо для тонкого управления конкуретными данными)
                services.AddDbContext<SvContext>(options => options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()), ServiceLifetime.Singleton);
            }
            else
            {
                services.AddDbContext<SvContext>(options => options.UseSqlServer(sqlConnection));
            }

            return services;
        }

        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IServiceGeneralConfig>(provider => configuration.GetSection("ServiceGeneralConfig").Get<ServiceGeneralConfig>());
            // статус сервиса
            var now = DateTime.Now;
            services.AddSingleton<IServiceStatus, ServiceStatus>(sp => {
                var config = sp.GetService<IServiceGeneralConfig>();
                return new ServiceStatus()
                {
                    Name = config.Name,
                    Description = config.Description,
                    Started = now
                };
            });

            return services;
        }

        /// <summary>
        /// Инициализация данных по умолчанию
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            // добавление источникв в БД
            new SourseParserInitializer(svContext: app.ApplicationServices.GetService<SvContext>())
                .Execute();

            return app;
        }

        /// <summary>
        /// Конфигурация Http-Запросов
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpRequests(this IServiceCollection services)
        {
            services.AddSingleton<IHttpRequests, HttpRequests>(provider => {

                var config = provider.GetService<IServiceGeneralConfig>();

                // одновременное кол-во запросов к одному хосту
                //ServicePointManager.DefaultConnectionLimit = 

                return new HttpRequests(client => {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User-Agent", config.Name);
                    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                });
            });

            return services;
        }

        /// <summary>
        /// Подключение загрузчика с источника
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebSourceLoader(this IServiceCollection services)
        {
            services.AddScoped<IWebSourceLoader, WebSourceLoader>();

            return services;
        }

        /// <summary>
        /// Подключение парсеров источников
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSourceParsers(this IServiceCollection services)
        {
            services.AddScoped<ISourceParser, RabotaYandexSourceParser>();
            services.AddScoped<ISourceParser, RabotaRuParser>();

            return services;
        }

        /// <summary>
        /// Маппинг профили
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutomapperConfiguration(this IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.AddMaps(new[] {
                    typeof(Web.Host.Cqrs.AssemblyLinks.MapperAssemblyLink)
                });
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }

        /// <summary>
        /// CORs: Взаимодействие с дев-сервером фронта
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        // полный доступ для dev-сервера
                        builder.WithOrigins("http://127.0.0.1:8080", "http://localhost:8080")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            return services;
        }

        
    }
}
