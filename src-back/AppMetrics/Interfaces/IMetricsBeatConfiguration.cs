using System;
using System.Collections.Generic;
using System.Text;

namespace AppMetrics.Interfaces
{
    public interface IMetricsBeatConfiguration
    {
        string Url { get;  }
        int BeatDelayInSeconds { get;  }
    }
}
