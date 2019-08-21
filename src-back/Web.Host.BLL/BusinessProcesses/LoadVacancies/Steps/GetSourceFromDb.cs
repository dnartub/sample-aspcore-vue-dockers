using Blc.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Host.Cqrs.Models;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps
{
    public class GetSourceFromDb : IBusinessProcessStep<Source>
    {
        public async Task CancelAsync()
        {
            await Task.CompletedTask;
        }

        public Task<Source> RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
