using AppMetrics.Interfaces;
using AppMetrics.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace AppMetrics
{
    public class ApplicationMetricsService: IApplicationMetricsService
    {
        public async Task<ApplicationMetrics> GetApplicationMetrics()
        {
            var cpu = await GetCpuPercentUsage();
            var memory = GetMemoryUsage();

            return new ApplicationMetrics()
            {
                CpuPercent = Math.Round(cpu, 3),
                MemoryMb = Math.Round(memory / 1024.0 / 1024.0, 2)
            };
        }

        private long GetMemoryUsage()
        {
            return Process.GetCurrentProcess().WorkingSet64;
        }

        private async Task<double> GetCpuPercentUsage()
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

            await Task.Delay(500);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
            return cpuUsageTotal * 100;
        }
    }
}
