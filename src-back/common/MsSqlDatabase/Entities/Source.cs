using MsSqlDatabase.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MsSqlDatabase.Entities
{
    public class Source
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Description("Идентификатор сущности")]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Адрес загрузки
        /// </summary>
        [Description("Адрес загрузки")]
        public string Url { get; set; }

        /// <summary>
        /// Парсер для источника
        /// </summary>
        [Description("Парсер для источника")]
        public SourceParsers SourceParser { get; set; }


        /// <summary>
        /// Навигация - вакансии с источника
        /// </summary>
        public virtual ICollection<Vacancy> Vacancies { get; set; }

        public Source()
        {
            Vacancies = new List<Vacancy>();
        }
    }
}
