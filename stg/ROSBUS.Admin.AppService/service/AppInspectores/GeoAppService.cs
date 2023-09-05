using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces.Services.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;

namespace ROSBUS.Admin.AppService.service.AppInspectores
{
    public class GeoAppService : AppServiceBase<InspGeo, InspGeoDto, long, IGeoService>, IGeoAppService
    {
        public GeoAppService(IGeoService serviceBase) : base(serviceBase)
        {
        }

        public async Task SaveEntityList(List<InspGeoDto> dtoList)
        {

            List<InspGeo_Hist> historicos=new List<InspGeo_Hist>();

            foreach (var item in dtoList)
            {
                historicos.Add(new InspGeo_Hist()
                {
                    Accion = item.Accion,
                    CurrentTime = item.CurrentTime,
                    Latitud = item.Latitud,
                    Longitud = item.Longitud,
                    UserName = item.UserName
                });
            }

            await this._serviceBase.SaveEntityList(historicos);
        }
    }
}
