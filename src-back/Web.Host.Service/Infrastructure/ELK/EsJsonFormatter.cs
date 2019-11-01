using Serilog.Formatting.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Host.Service.Infrastructure.ELK
{
    public class EsJsonFormatter: ElasticsearchJsonFormatter
    {
        public EsJsonFormatter() : base(inlineFields: true)
        {

        }
    }
}
