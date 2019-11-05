using AppMetrics.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppMetrics.Interfaces
{
    public interface IMetricsSender
    {
        Task Send(Metrics metrics);
    }
}
