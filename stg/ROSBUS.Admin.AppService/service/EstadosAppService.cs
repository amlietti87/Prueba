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

    public class EstadosAppService : AppServiceBase<SinEstados, EstadosDto, int, IEstadosService>, IEstadosAppService
    {
        public EstadosAppService(IEstadosService serviceBase) 
            :base(serviceBase)
        {
         
        }

        public override async Task<EstadosDto> AddAsync(EstadosDto dto)
        {

            var entity = MapObject<EstadosDto, SinEstados>(dto);
            foreach (var item in entity.SinSubEstados.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            var result = await this.AddAsync(entity);
            return MapObject<SinEstados, EstadosDto>(entity);
        }





        public async override Task<EstadosDto> UpdateAsync(EstadosDto dto)
        {
            var entity = await this.GetByIdAsync(dto.Id);
            MapObject(dto, entity);



            foreach (var item in entity.SinSubEstados.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            await this.UpdateAsync(entity);


            return MapObject<SinEstados, EstadosDto>(entity);
        }

    }
}
