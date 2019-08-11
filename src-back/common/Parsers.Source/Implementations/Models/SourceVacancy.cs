using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parsers.Source.Implementations.Models
{
    /// <summary>
    /// Модель данных вакансии
    /// </summary>
    public class SourceVacancy: ISourceVacancy
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ЗП
        /// </summary>
        public string Salary { get; set; }
        /// <summary>
        /// Место
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Компания
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// описание
        /// </summary>
        public string Annotation { get; set; }

        public string Url { get; set; }

        public override string ToString()
        {
            return $"{Name} / {Salary}";
        }
    }
}
