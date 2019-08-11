using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.Source.Interfaces
{
    /// <summary>
    /// Модель данных о вакансии
    /// </summary>
    public interface ISourceVacancy
    {
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// ЗП
        /// </summary>
        string Salary { get; set; }
        /// <summary>
        /// Место
        /// </summary>
        string Address { get; set; }
        /// <summary>
        /// Компания
        /// </summary>
        string Company { get; set; }
        /// <summary>
        /// описание
        /// </summary>
        string Annotation { get; set; }
        /// <summary>
        /// Ссылка на вакансию
        /// </summary>
        string Url { get; set; }
    }
}
