using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Host.Cqrs.Models;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Models
{
    public class SourceVacanciesModel
    {
        public Source Source { get; set; }
        public List<ISourceVacancy> Vacancies  { get; set; }
}
}
