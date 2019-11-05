using System;
using System.Collections.Generic;
using System.Text;

namespace AppMetrics.Models
{
    public class RequestPerSecond
    {
        public string RequestUrl { get; set; }
        public double CountPerSecond { get; set; }
        public int Count { get; set; }
    }
}
