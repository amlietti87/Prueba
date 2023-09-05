using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class TipoParadaAppService : AppServiceBase<PlaTipoParada, TipoParadaDto, int, ITipoParadaService>, ITipoParadaAppService
    {
        public TipoParadaAppService(ITipoParadaService serviceBase) 
            :base(serviceBase)
        {
         
        }

        public async override Task<TipoParadaDto> UpdateAsync(TipoParadaDto dto)
        {
            var entity = await this.GetByIdAsync(dto.Id);
            MapObject(dto, entity);

            

            foreach (var item in entity.PlaTiempoEsperadoDeCarga.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            await this.UpdateAsync(entity);


            return MapObject<PlaTipoParada, TipoParadaDto>(entity);
        }


        public async  override Task<TipoParadaDto> AddAsync(TipoParadaDto dto)
        {
           

            var entity = MapObject<TipoParadaDto, PlaTipoParada>(dto);

            foreach (var item in entity.PlaTiempoEsperadoDeCarga.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            return MapObject<PlaTipoParada, TipoParadaDto>(await this.AddAsync(entity)); 
            
        }

    }
}
