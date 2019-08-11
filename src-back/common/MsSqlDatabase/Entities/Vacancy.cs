using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MsSqlDatabase.Entities
{
    /// <summary>
    /// Вакансия
    /// </summary>
    [Description("Сущность ваканси")]
    public class Vacancy
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Description("Идентификатор сущности")]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Время загрузки
        /// </summary>
        [Description("Время загрузки")]
        [Required]
        public DateTime LoadingTime { get; set; }

        /// <summary>
        /// Источник
        /// </summary>
        [Description("Источник")]
        [ForeignKey("Source")]
        public Guid SourceId { get; set; }
        public virtual Source Source { get; set; }


        /// <summary>
        /// Название
        /// </summary>
        [Description("Название")]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// ЗП
        /// </summary>
        [Description("ЗП")]
        [Required]
        public string Salary { get; set; }
        /// <summary>
        /// Место
        /// </summary>
        [Description("место")]
        public string Address { get; set; }
        /// <summary>
        /// Компания
        /// </summary>
        [Description("Компания")]
        public string Company { get; set; }
        /// <summary>
        /// описание
        /// </summary>
        [Description("Описание")]
        [Required]
        public string Annotation { get; set; }

        /// <summary>
        /// ссылка на вакансию
        /// </summary>
        [Description("ссылка на вакансию")]
        [Required]
        public string Url { get; set; }
    }
}
