using Web.Host.Service.Configuration;
using Web.Host.Service.Configuration.Impl;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace Web.Host.Service
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateLogger();

                Log.Information("STARTING: Web.Host.Service");

                // инициализация хоста
                IWebHost webHost = BuildWebHost(args);
                // запуск хоста - ожидание завершения приложения
                webHost.Run();

                Log.Information("STOPED: Web.Host.Service");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "FAIL: Web.Host.Service");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IConfigurationRoot _configuration;
        private static IConfigurationRoot Configuration => _configuration ?? (_configuration = new ConfigurationBuilder()
                                                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                        .AddEnvironmentVariables()
                                                        .Build());

        /// <summary>
        /// Конфигурация и инициализация web-host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IWebHost BuildWebHost(string[] args)
        {
            IServiceGeneralConfig generalConfig = Configuration.GetSection("ServiceGeneralConfig").Get<ServiceGeneralConfig>();

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(Configuration)
                //.UseUrls(generalConfig.Host) // отключено, выдача порта через APPNETCORE_URLS
                .UseSerilog()
                .UseStartup<Startup>()
                .Build(); ;
        }

        /// <summary>
        /// Инициализация глобального логера
        /// </summary>
        private static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }
    }
}
