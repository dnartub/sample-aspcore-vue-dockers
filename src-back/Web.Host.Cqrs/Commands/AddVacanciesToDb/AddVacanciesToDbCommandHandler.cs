using Cqrs.Interfaces;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Activators.Creators;

namespace Web.Host.Cqrs.Commands.AddVacanciesToDb
{
    public class AddVacanciesToDbCommandHandler : ICommandHandler<AddVacanciesToDbCommand>
    {
        [DiService]
        public SvContext Context { get; set; }

        public async Task Execute(AddVacanciesToDbCommand command)
        {
            foreach (var vacancy in command.Vacancies)
            {
                var dal = new MsSqlDatabase.Entities.Vacancy()
                {
                    Id = Guid.NewGuid(),
                    SourceId = command.SourceId,
                    LoadingTime = DateTime.Now,
                    Name = vacancy.Name,
                    Salary = vacancy.Salary,
                    Address = vacancy.Address,
                    Company = vacancy.Company,
                    Annotation = vacancy.Annotation,
                    Url = vacancy.Url
                };

                // TODO: Не добавлять, если совпадают по URL

                Context.Add(dal);
            }

            if (command.Vacancies.Any())
            {
                await Context.SaveChangesAsync();
            }
        }

        public async Task Down(AddVacanciesToDbCommand command)
        {
            foreach (var vacancy in command.Vacancies)
            {
                var dal = Context.Vacancies.Where(x => x.SourceId == command.SourceId && x.Url == vacancy.Url).FirstOrDefault();
                if (dal != null)
                {
                    Context.Remove(dal);
                }
            }

            await Context.SaveChangesAsync();
        }
    }
}
