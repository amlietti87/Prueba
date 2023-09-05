using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class CroCroquisAppService : AppServiceBase<CroCroquis, CroCroquisDto, int, ICroCroquisService>, ICroCroquisAppService
    {

        public CroCroquisAppService(ICroCroquisService serviceBase) 
            :base(serviceBase)
        {
        }


        public override async Task<CroCroquisDto> AddAsync(CroCroquisDto dto)
        {

            var entity = MapObject<CroCroquisDto, CroCroquis>(dto);

            entity.idSiniestro = dto.SiniestroId;


            var resul = await this.AddAsync(entity);

            //if (dto.SiniestroId.HasValue)
            //{
            //    var siniestro = await _httpServiceSiniestros.GetByIdAsync(dto.SiniestroId.Value);
            //    siniestro.CroquiId = resul.Id;
            //    var dtoSiniestro = MapObject<SinSiniestros, SiniestrosDto>(siniestro);
            //    await _httpServiceSiniestros.UpdateAsync(dtoSiniestro);
            //}
            
           

            return MapObject<CroCroquis, CroCroquisDto>(resul);



        }

        public async override Task<CroCroquisDto> UpdateAsync(CroCroquisDto dto)
        {

            var entity = await this.GetByIdAsync(dto.Id);



            MapObject(dto, entity);



            await this.UpdateAsync(entity);
            return MapObject<CroCroquis, CroCroquisDto>(entity);

        }


    }
}
