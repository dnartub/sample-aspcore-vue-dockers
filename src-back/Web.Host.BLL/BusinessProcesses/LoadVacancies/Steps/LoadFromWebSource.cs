using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Host.Cqrs.Models;
using Blc.Interfaces;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps
{
    public class LoadFromWebSource : IBusinessProcessStep<List<ISourceVacancy>>
    {
        public void Cancel()
        {
            ;
        }

        public List<ISourceVacancy> Run()
        {
            throw new NotImplementedException();
        }
    }
}
