using MsSqlDatabase.Commands;
using MsSqlDatabase.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Linq;

namespace Commands.Source
{
    /// <summary>
    /// Поиск среди Источников в БД
    /// </summary>
    public class FindSourceInDb : IGetCommand<List<MsSqlDatabase.Entities.Source>>
    {
        Expression<Func<MsSqlDatabase.Entities.Source, Boolean>> _predicate;
        public FindSourceInDb(Expression<Func<MsSqlDatabase.Entities.Source, Boolean>> predicate)
        {
            _predicate = predicate;
        }

        public List<MsSqlDatabase.Entities.Source> Get(IServiceProvider provider)
        {
            var context = provider.GetService<SvContext>();

            return context.Sources.Where(_predicate).ToList();
        }
    }
}
