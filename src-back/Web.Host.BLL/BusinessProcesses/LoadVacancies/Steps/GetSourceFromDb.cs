using Blc.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Host.Cqrs.Models;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps
{
    public class GetSourceFromDb : IBusinessProcessStep<Source>
    {
        public void Cancel()
        {
            ;
        }

        public Source Run()
        {
            throw new NotImplementedException();
        }
    }
}
