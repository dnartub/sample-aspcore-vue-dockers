using AppMetrics.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMetrics.Configuration
{
    public class MetricsBeatConfiguration : IMetricsBeatConfiguration
    {
        public string Url { get; set; }
        public int BeatDelayInSeconds { get; set; }
        public string IndexFormat { get; set; }
    }
}
