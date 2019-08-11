using System;
using System.Collections.Generic;
using System.Text;
using MsSqlDatabase.Context;
using MsSqlDatabase.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace MsSqlDatabase.Commands.Source
{
    /// <summary>
    /// Получение источника по id
    /// </summary>
    public class GetSourceFromDb : IGetCommand<Entities.Source>
    {
        Guid _id;

        public GetSourceFromDb(Guid id)
        {
            _id = id;
        }

        public Entities.Source Get(IServiceProvider provider)
        {
            var context = provider.GetService<SvContext>();

            return context.Sources.Find(_id);
        }
    }
}
