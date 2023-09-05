using AutoMapper;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ROSBUS.WebService.Geo.MappingConfig
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {

            CreateMap<InspGeo, InspGeoDto>();
            CreateMap<InspGeoDto, InspGeo>();

        }

    }
}
