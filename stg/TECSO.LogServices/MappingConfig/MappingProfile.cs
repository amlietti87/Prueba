using AutoMapper;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace TECSO.LogServices.MappingConfig
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Error, LogDto>()
               .ForMember(d => d.Description, o => o.MapFrom(s => s.ErrorMessage))
               .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
               .ForMember(d => d.LogDate, o => o.MapFrom(s => s.ErrorDate))
               .ForMember(d => d.LogMessage, o => o.MapFrom(s => s.ErrorMessage))
               .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
               .ForMember(d => d.SessionId, o => o.MapFrom(s => s.SessionId))
               .ForMember(d => d.StackTrace, o => o.MapFrom(s => s.StackTrace))
               
               ;

            CreateMap<LogDto, Error>()
                .ForMember(d => d.ErrorDate, o => o.MapFrom(s => s.LogDate))
                .ForMember(d => d.ErrorMessage, o => o.MapFrom(s => s.LogMessage))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                .ForMember(d => d.SessionId, o => o.MapFrom(s => s.SessionId))
                .ForMember(d => d.StackTrace, o => o.MapFrom(s => s.StackTrace));


            CreateMap<Logs, LogDto>();
            CreateMap<LogDto, Logs>();
        }
    }
}
