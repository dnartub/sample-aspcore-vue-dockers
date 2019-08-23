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
using Common.Types;

namespace Web.Host.BLL.BusinessProcesses.LoadVacancies.Steps
{
    public class SaveToDb : IBusinessProcessStep<List<ISourceVacancy>>, IBusinessProcessStepCancelable
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
            var commandAddVacanciesToDb = GetCommand();

            await CqrsService.Down(commandAddVacanciesToDb);
        }

        public async Task<List<ISourceVacancy>> RunAsync()
        {
            var commandAddVacanciesToDb = GetCommand();

            await CqrsService.Execute(commandAddVacanciesToDb);

            return _sourceVacancies.Vacancies;
        }

        private AddVacanciesToDbCommand GetCommand()
        {
            return new AddVacanciesToDbCommand()
            {
                SourceId = _sourceVacancies.Source.Id,
                Vacancies = _sourceVacancies.Vacancies
            };
        }
    }
}
