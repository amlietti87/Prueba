using AutoMapper;
using AutoMapper.EquivalencyExpression;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.ART.AppService.Model;

namespace ROSBUS.WebService.ART
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SysParameters, SysParametersDto>()
                .ForMember(d => d.Descripcion, o => o.MapFrom(s => s.Description));
            CreateMap<SysParametersDto, SysParameters>();


            CreateMap<SysDataTypes, SysDataTypesDto>();
            CreateMap<SysDataTypesDto, SysDataTypes>();

            
        }
    }
}
