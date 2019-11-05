using AppMetrics.Interfaces;
using AppMetrics.Models;
using Nest;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppMetrics.Services
{
    public class ElasticsearchService : IMetricsSender
    {
        private ElasticClient Client { get; set; }
        private string IndexFormat { get; set; }

        public ElasticsearchService(string indexFormat, Func<ElasticClient> action)
        {
            Client = action();
            IndexFormat = indexFormat;
        }

        public async Task Send(Models.Metrics metrics)
        {
            try
            {
                await CreateIndexIfNotExists();

                // индекс данных в зависимости от даты
                var indexName = GetIndex();
                // отправлям данные
                var result = await Client.CreateAsync(metrics, i => i
                    .Index(indexName)
                );

                if (result.Result == Result.Error)
                {
                    throw result.OriginalException;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка при добавлении метрик через ElasticClient");
            }
        }

        public string GetIndex()
        {
            var index = String.Format(IndexFormat, DateTime.Now);
            return index;
        }

        public async Task CreateIndexIfNotExists()
        {
            var indexName = GetIndex();

            var indexExistsResponse = await Client.Indices.ExistsAsync(indexName);

            if (!indexExistsResponse.Exists)
            {
                await Client.Indices.CreateAsync(indexName, i => i
                    .Map<Models.Metrics>(m => m
                        .AutoMap(5)
                    ));
            }
        }

    }
}
