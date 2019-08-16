using Cqrs.Interfaces;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Host.Cqrs.Commands.AddVacanciesToDb
{
    public class AddVacanciesToDbCommandHandler : ICommandHandler<AddVacanciesToDbCommand>
    {
        SvContext _context;

        public AddVacanciesToDbCommandHandler(SvContext context)
        {
            _context = context;
        }
        public void Execute(AddVacanciesToDbCommand command)
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

                _context.Add(dal);
            }

            if (command.Vacancies.Any())
            {
                _context.SaveChanges();
            }
        }
    }
}
