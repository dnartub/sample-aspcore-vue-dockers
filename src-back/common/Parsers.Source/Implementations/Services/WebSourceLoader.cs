using Common.Types;
using HtmlAgilityPack;
using HttpRequest.Core;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.Source.Implementations.Models
{
    /// <summary>
    /// Реализация загрузчика с источника
    /// </summary>
    public class WebSourceLoader : IWebSourceLoader
    {
        private ISourceParser SourceParser { get; set; }

        private string Url { get; set; }


        private IHttpRequests HttpRequests { get; set; }
        /// <summary>
        /// DI
        /// </summary>
        public WebSourceLoader(IHttpRequests httpRequests)
        {
            HttpRequests = httpRequests;
        }

        public async Task<List<ISourceVacancy>> Load()
        {
            if (SourceParser == null)
            {
                throw new Exception("Не определен адаптер для разбора данных с ресурса.");
            }

            if (Url == null)
            {
                throw new Exception("Не определен адрес ресурса ресурса.");
            }

            var htmlText = await GetSourcHtmlText();

            var results = SourceParser.Parse(htmlText);

            return await Task.FromResult(results);
        }

        public IWebSourceLoader Use(ISourceParser parser)
        {
            SourceParser = parser;
            return this;
        }

        public IWebSourceLoader UseUrl(string url)
        {
            Url = url;
            return this;
        }

        /// <summary>
        /// Получение html c источника
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetSourcHtmlText()
        {
            //return await HttpRequests.GetPage(Url);
            var web = new HtmlWeb();
            var doc = web.Load(Url);
            return await Task.FromResult(doc.Text); 
            // TODO: для некоторых сайтов, надо делать Load page 2,3 и т.д - неолбходимо добавить url Для доп загрузок - UseUrl -> Add to List Url
        }

    }
}
