using System;
using System.Collections.Generic;
using System.Text;
using MsSqlDatabase.Context;
using MsSqlDatabase.Entities;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace Commands.Source
{
    /// <summary>
    /// Получение источника по id
    /// </summary>
    public class GetSourceFromDb : IGetCommand<Models.Source>
    {
        Guid _id;

        public GetSourceFromDb(Guid id)
        {
            _id = id;
        }

        public Models.Source Get(IServiceProvider provider)
        {
            var context = provider.GetService<SvContext>();
            var mapper = provider.GetService<IMapper>();

            var dal = context.Sources.Find(_id);
            return mapper.Map<Models.Source>(dal);
        }
    }
}
