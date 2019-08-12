using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MsSqlDatabase.Context;

namespace Commands.Source
{
    public class GetAllSourcesFromDb: IGetCommand<List<Models.Source>>
    {
        public GetAllSourcesFromDb()
        {
        }

        public List<Models.Source> Get(IServiceProvider provider)
        {
            var context = provider.GetService<SvContext>();
            var mapper = provider.GetService<IMapper>();

            var dals = context.Sources.ToList();
            return mapper.Map<List<Models.Source>>(dals);
        }
    }
}
