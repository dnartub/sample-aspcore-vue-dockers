using AutoMapper;
using Parsers.Source.Implementations.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Host.Cqrs.MappingProfiles
{
    /// <summary>
    /// Маппинг вакансии между БД и моделью данных
    /// </summary>
    public class VacancyProfile:Profile
    {
        public VacancyProfile()
        {
            CreateMap<MsSqlDatabase.Entities.Vacancy, SourceVacancy>().ReverseMap();
        }
    }
}
