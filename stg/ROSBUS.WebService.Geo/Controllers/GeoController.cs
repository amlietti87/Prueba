using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities.Filters.AppInspectores;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.AppService.Interface.AppInspectores;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace ROSBUS.WebService.Geo.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    //public class GeoController : TECSO.FWK.ApiServices.ManagerSecurityController<InspGeo, long, InspGeoDto, GeoFilter, IGeoAppService>
    public class GeoController : TECSO.FWK.ApiServices.ManagerController<InspGeo, long, InspGeoDto, GeoFilter, IGeoAppService>
    {
        public GeoController(IGeoAppService service)
            : base(service)
        {

        }

        [HttpPost]
        public virtual async Task<IActionResult> SaveEntityList([FromBody] List<InspGeoDto> dtoList)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this.Service.SaveEntityList(dtoList);
                    return ReturnData<GeolocalizationResponse>(new GeolocalizationResponse() { Status=true });
                }
                else
                {
                    return ReturnError<InspGeoDto>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<InspGeoDto>(ex);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> CerrarSesion([FromBody] List<InspGeoDto> dtoList)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this.Service.SaveEntityList(dtoList);
                    return ReturnData<GeolocalizationResponse>(new GeolocalizationResponse() { Status = true });
                }
                else
                {
                    return ReturnError<InspGeoDto>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<InspGeoDto>(ex);
            }
        }

    }
}