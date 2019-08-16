using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Host.Cqrs.MappingProfiles
{
    public class SourceProfile:Profile
    {
        public SourceProfile()
        {
            CreateMap<MsSqlDatabase.Entities.Source, Models.Source>().ReverseMap();
        }
    }
}
