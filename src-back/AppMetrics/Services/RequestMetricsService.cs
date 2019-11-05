using AppMetrics.Interfaces;
using AppMetrics.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppMetrics
{
    public class RequestMetricsService: IRequestMetricsService
    {
        private ConcurrentDictionary<string, int> RequestsCount { get; set; } = new ConcurrentDictionary<string, int>();
        private DateTime StartTime { get; set; } = DateTime.Now;
        private Task FlushTaskAwaiter { get; set; }


        public RequestMetricsService()
        {
            RequestsCount = new ConcurrentDictionary<string, int>();
            StartTime = DateTime.Now;
            FlushTaskAwaiter = Task.CompletedTask;
        }


        public async Task AddRequest(string url)
        {
            await FlushTaskAwaiter;

            if (!RequestsCount.ContainsKey(url))
            {
                RequestsCount.GetOrAdd(url, 1);
                return;
            }

            RequestsCount[url]++;
        }

        public RequestMetrics GetRequestMetricsAndFlush()
        {
            var tcs = new TaskCompletionSource<object>();
            FlushTaskAwaiter = tcs.Task;

            var result = new RequestMetrics();

            var requestsPerSecond = new List<RequestPerSecond>();

            var totalSeconds = (DateTime.Now - StartTime).TotalSeconds;

            foreach (var keyValue in RequestsCount)
            {
                requestsPerSecond.Add(new RequestPerSecond() {
                    RequestUrl = keyValue.Key,
                    CountPerSecond = Math.Round(keyValue.Value / totalSeconds,2),
                    Count = keyValue.Value
                });
            }

            RequestsCount.Clear();
            StartTime = DateTime.Now;

            tcs.SetResult("done");

            if (requestsPerSecond.Count == 0)
            {
                requestsPerSecond.Add(new RequestPerSecond()
                {
                    RequestUrl = "None",
                    CountPerSecond = 0,
                    Count = 0
                });
            }


            result.RequestsPerSecond = requestsPerSecond.ToArray();
            return result;
        }

    }
}
