using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Blc.Interfaces;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps
{
    public class SaveToDb : IBusinessProcessStep<List<ISourceVacancy>>
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
