using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Blc.Interfaces;
using System.Threading.Tasks;
using Utils.Activators.Creators;
using Cqrs.Interfaces;
using Web.Host.Cqrs.Models;
using Web.Host.Cqrs.Commands.AddVacanciesToDb;
using Web.Host.BLL.BusinessProcesses.LoadVacancies.Models;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps
{
    public class SaveToDb : IBusinessProcessStep<List<ISourceVacancy>>
    {
        [DiService]
        public ICqrsService CqrsService { get; set; }

        SourceVacanciesModel _sourceVacancies;

        public SaveToDb(SourceVacanciesModel sourceVacancies)
        {
            _sourceVacancies = sourceVacancies;
        }

        public async Task CancelAsync()
        {
            await Task.CompletedTask;
        }

        public async Task<List<ISourceVacancy>> RunAsync()
        {
            var commandAddVacanciesToDb = new AddVacanciesToDbCommand()
            {
                SourceId = _sourceVacancies.Source.Id,
                Vacancies = _sourceVacancies.Vacancies
            };

            await CqrsService.Execute(commandAddVacanciesToDb);

            return _sourceVacancies.Vacancies;
        }
    }
}
