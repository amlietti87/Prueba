using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;

namespace ROSBUS.Admin.AppService.service
{
    public class InspGruposInspectoresAppService : AppServiceBase <InspGruposInspectores, InspGruposInspectoresDto, long, IInspGruposInspectoresService>, IInspGruposInspectoresAppService
    {
        public InspGruposInspectoresAppService(IInspGruposInspectoresService serviceBase)
             : base(serviceBase)
        {

        }

        public override async Task<InspGruposInspectoresDto> UpdateAsync(InspGruposInspectoresDto dto)
        {
            var entity = await this.GetByIdAsync(dto.Id);
            MapObject(dto, entity);

            //foreach (var item in entity.InspGrupoInspectoresZona)
            //{
            //    if (item.Id<0)
            //    {
            //        item.Id = 0;
            //    }
            //}
            await this.UpdateAsync(entity);
            return MapObject<InspGruposInspectores, InspGruposInspectoresDto>(entity);
        }
    }
}
