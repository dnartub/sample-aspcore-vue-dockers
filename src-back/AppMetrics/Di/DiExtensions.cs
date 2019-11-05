using AppMetrics.BackgroundTasks;
using AppMetrics.Configuration;
using AppMetrics.Interfaces;
using AppMetrics.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMetrics.Di
{
    public static class DiExtensions
    {
        public static IServiceCollection AddMetricsBeat(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("MetricsBeat").Get<MetricsBeatConfiguration>();

            services.AddSingleton<IMetricsBeatConfiguration>(provider => config);

            services.AddSingleton<IRequestMetricsService, RequestMetricsService>();
            services.AddSingleton<IApplicationMetricsService, ApplicationMetricsService>();

            services.AddSingleton<IMetricsSender>(provider => new ElasticsearchService(config.IndexFormat, () => {
                var settings = new ConnectionSettings(new Uri(config.Url))
                        .DefaultMappingFor<Models.Metrics>(m => m
                            .PropertyName(p => p.Timestamp, "@timestamp")
                        );
                return new ElasticClient(settings);
            }));

            // note: Для scope и transient Интерфейсов разделенных между фоновыми задачами  на конструкторе BackgroundTask, необходима другая инициализация
            services.AddSingleton<IHostedService, SendMetricsBackgroundTask>();

            return services;
        }
    }
}
