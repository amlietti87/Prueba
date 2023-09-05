using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface.AppInspectores
{
    public interface IGeoAppService : IAppServiceBase<InspGeo, InspGeoDto, long>
    {
        Task SaveEntityList(List<InspGeoDto> dtoList);
    }
}
