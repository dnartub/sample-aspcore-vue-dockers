using AppMetrics.Interfaces;
using AppMetrics.Models;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppMetrics.BackgroundTasks
{
    public class SendMetricsBackgroundTask: BackgroundService
    {
        private IMetricsBeatConfiguration Configuration { get; set; }
        private IRequestMetricsService RequestMetricsService { get; set; }
        private IApplicationMetricsService ApplicationMetricsService { get; set; }
        private IMetricsSender MetricsSender { get; set; }


        public SendMetricsBackgroundTask(
            IMetricsBeatConfiguration configuration, 
            IRequestMetricsService requestMetricsService, 
            IApplicationMetricsService applicationMetricsService,
            IMetricsSender metricsSender
        )
        {
            Configuration = configuration;
            RequestMetricsService = requestMetricsService;
            ApplicationMetricsService = applicationMetricsService;
            MetricsSender = metricsSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information($"[{nameof(SendMetricsBackgroundTask)}] - starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SendMetrics();
                    await Task.Delay(TimeSpan.FromSeconds(Configuration.BeatDelayInSeconds), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    ;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"[{nameof(SendMetricsBackgroundTask)}] Произошла ошибка при отправке метрик");
                    await Task.Delay(TimeSpan.FromSeconds(Configuration.BeatDelayInSeconds), stoppingToken);
                }
            }

            Log.Information($"{nameof(SendMetricsBackgroundTask)} - finished");
        }

        private async Task SendMetrics()
        {
            var appMetrics = await ApplicationMetricsService.GetApplicationMetrics();
            var reqMetrics = RequestMetricsService.GetRequestMetricsAndFlush();

            var metrics = new Metrics()
            {
                ApplicationMetrics = appMetrics,
                RequestMetrics = reqMetrics
            };

            await MetricsSender.Send(metrics);
        }
    }
}
