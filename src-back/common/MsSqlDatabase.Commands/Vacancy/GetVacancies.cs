using Parsers.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Commands.Vacancy
{
    /// <summary>
    /// Получение вакансий
    /// </summary>
    public class GetVacancies : IGetCommand<List<ISourceVacancy>>
    {
        Guid _sourceId;

        public GetVacancies(Guid sourceId)
        {
            _sourceId = sourceId;
        }


        public List<ISourceVacancy> Get(IServiceProvider provider)
        {
            try
            {
                // получаем с ресурса
                var result = new GetVacanciesFromWebSource(_sourceId)
                    .Get(provider);

                // добавляем в БД
                new AddVacanciesToDb(result, _sourceId)
                    .Execute(provider);

                return result;
            }
            catch (HttpRequestException)
            {
                // сайт недоступен, берем из БД
                var result = new GetVacanciesFromDb(_sourceId)
                    .Get(provider);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
