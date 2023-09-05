using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.WebService.Mobile.Controllers
{

    [Route("[controller]/[action]")]    
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class BanderaController : TECSO.FWK.ApiServices.ControllerBase
    {
        public IBanderaAppService Service { get; }

        public BanderaController(IBanderaAppService service) 
        {
            this.Service = service;
        }



        
        [HttpGet]
        public virtual async Task<IActionResult> RecuperarLineasActivasPorFecha([FromQuery]BanderaFilter Filtro)
        {
            try
            {
                List<ItemDto> items = await this.Service.RecuperarLineasActivasPorFecha(Filtro);

                return ReturnData<List<ItemDto>>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }



        [HttpGet]
        public virtual async Task<IActionResult> RecuperarBanderasRelacionadasPorSector([FromQuery]BanderaFilter Filtro)
        {

            try
            {
                var items = await this.Service.RecuperarBanderasRelacionadasPorSector(Filtro);

                return ReturnData<List<ItemDto<int>>>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }


        


        [HttpPost]
        public virtual async Task<IActionResult> RecuperarHorariosSectorPorSector([FromBody]BanderaFilter Filtro)
        {
            try
            {       

                var items = await this.Service.RecuperarHorariosSectorPorSector(Filtro);

                return ReturnData<HorariosPorSectorDto>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

 
    }


}