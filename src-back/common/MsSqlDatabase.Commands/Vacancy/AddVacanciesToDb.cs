using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using MsSqlDatabase.Context;

namespace Commands.Vacancy
{
    /// <summary>
    /// Добавление вакансий в БД
    /// </summary>
    public class AddVacanciesToDb : IExecuteCommand
    {
        List<ISourceVacancy> _vacancies;
        Guid _sourceId;

        public AddVacanciesToDb(List<ISourceVacancy> vacancies, Guid sourceId)
        {
            _vacancies = vacancies;
            _sourceId = sourceId;
        }

        public void Execute(IServiceProvider provider)
        {
            var context = provider.GetService<SvContext>();

            foreach (var vacancy in _vacancies)
            {
                var dal = new MsSqlDatabase.Entities.Vacancy()
                {
                    Id = Guid.NewGuid(),
                    SourceId = _sourceId,
                    LoadingTime = DateTime.Now,
                    Name = vacancy.Name,
                    Salary = vacancy.Salary,
                    Address = vacancy.Address,
                    Company = vacancy.Company,
                    Annotation = vacancy.Annotation,
                    Url = vacancy.Url 
                };

                // TODO: Не добавлять, если совпадают по URL

                context.Add(dal);
            }

            if (_vacancies.Any())
            {
                context.SaveChanges();
            }
        }
    }
}
