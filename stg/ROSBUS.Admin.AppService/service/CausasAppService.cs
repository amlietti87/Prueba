using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class CausasAppService : AppServiceBase<SinCausas, CausasDto, int, ICausasService>, ICausasAppService
    {
        public CausasAppService(ICausasService serviceBase) 
            :base(serviceBase)
        {         
        }
        public override async Task<CausasDto> AddAsync(CausasDto dto)
        {

            var entity = MapObject<CausasDto, SinCausas>(dto);
            foreach (var item in entity.SinSubCausas.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            var result = await this.AddAsync(entity);
            return MapObject<SinCausas, CausasDto>(entity);
        }
        public async override Task<CausasDto> UpdateAsync(CausasDto dto)
        {
            var entity = await this.GetByIdAsync(dto.Id);
            MapObject(dto, entity);



            foreach (var item in entity.SinSubCausas.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            await this.UpdateAsync(entity);


            return MapObject<SinCausas, CausasDto>(entity);
        }

        

    }
}
