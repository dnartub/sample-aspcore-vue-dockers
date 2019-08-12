using MsSqlDatabase.Context;
using MsSqlDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MsSqlDatabase.Initializers
{
    /// <summary>
    /// А-ля DropCreateDatabaseAlways, которого нет у NetCore
    /// </summary>
    public class SourseParserInitializer
    {
        private SvContext SvContext { get; set; }

        public SourseParserInitializer(SvContext svContext)
        {
            SvContext = svContext;
        }

        /// <summary>
        /// Добавление источников по умолчанию
        /// </summary>
        public void Execute()
        {
            var existed = SvContext.Sources.Where(x => x.SourceParser == Enums.SourceParsers.RabotaYandex).FirstOrDefault();
            if (existed != null)
            {
                return;
            }

            var source = new Source()
            {
                Id = Guid.Parse("e6c17a32-4e91-4b18-815b-5a33268834e4"),
                SourceParser = Enums.SourceParsers.RabotaYandex,
                Url = "https://rabota.yandex.ru/search?job_industry=275"
            };

            SvContext.Add(source);

            var source2 = new Source()
            {
                Id = Guid.Parse("55619064-c2ac-4d6d-bf18-fa3107196515"),
                SourceParser = Enums.SourceParsers.RabotaRu,
                Url = "https://kaluga.rabota.ru/vacancy"
            };

            SvContext.Add(source2);

            SvContext.SaveChanges();
        }
    }
}
