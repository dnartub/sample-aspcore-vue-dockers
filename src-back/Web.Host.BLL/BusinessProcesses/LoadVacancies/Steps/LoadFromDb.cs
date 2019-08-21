using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Host.Cqrs.Models;
using Blc.Interfaces;
using System.Threading.Tasks;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps
{
    public class LoadFromDb : IBusinessProcessStep<List<ISourceVacancy>>
    {
        public async Task CancelAsync()
        {
            await Task.CompletedTask;
        }

        public Task<List<ISourceVacancy>> RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
