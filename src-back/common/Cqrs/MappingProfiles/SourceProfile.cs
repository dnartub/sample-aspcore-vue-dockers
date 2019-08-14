using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cqrs.MappingProfiles
{
    public class SourceProfile:Profile
    {
        public SourceProfile()
        {
            CreateMap<MsSqlDatabase.Entities.Source, Models.Source>().ReverseMap();
        }
    }
}
