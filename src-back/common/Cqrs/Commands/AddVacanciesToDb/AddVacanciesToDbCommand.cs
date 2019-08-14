using Cqrs.Interfaces;
using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cqrs.Commands.AddVacanciesToDb
{
    /// <summary>
    /// Добавление вакансий в БД
    /// <see cref="AddVacanciesToDbCommandHandler">
    /// </summary>
    public class AddVacanciesToDbCommand:ICommand
    {
        public List<ISourceVacancy> Vacancies { get; set; }
        public Guid SourceId { get; set; }
    }
}
