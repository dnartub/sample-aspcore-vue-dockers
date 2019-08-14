using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Linq;
using AutoMapper;

namespace Cqrs.Source
{
    /// <summary>
    /// Поиск среди Источников в БД
    /// </summary>
    public class FindSourceInDb : IGetCommand<List<Models.Source>>
    {
        Expression<Func<MsSqlDatabase.Entities.Source, Boolean>> _predicate;
        public FindSourceInDb(Expression<Func<MsSqlDatabase.Entities.Source, Boolean>> predicate)
        {
            _predicate = predicate;
        }

        public List<Models.Source> Get(IServiceProvider provider)
        {
            var context = provider.GetService<SvContext>();
            var mapper = provider.GetService<IMapper>();

            var dals = context.Sources.Where(_predicate).ToList();
            return mapper.Map<List<Models.Source>>(dals);
        }
    }
}
