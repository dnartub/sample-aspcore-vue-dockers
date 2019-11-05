using System;
using System.Collections.Generic;
using System.Text;

namespace AppMetrics.Models
{
    public class Metrics
    {
        public DateTime Timestamp { get; private set; }
        public Guid Id { get; private set; }

        public RequestMetrics RequestMetrics { get; set; }
        public ApplicationMetrics ApplicationMetrics { get; set; }

        public Metrics()
        {
            Timestamp = DateTime.Now;
            Id = Guid.NewGuid();
        }
    }
}
