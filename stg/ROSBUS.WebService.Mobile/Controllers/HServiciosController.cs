using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class HServiciosController : ManagerController<HServicios, int, HServiciosDto, HServiciosFilter, IHServiciosAppService>
    {
        public HServiciosController(IHServiciosAppService service)
            : base(service)
        {

        }



        [HttpGet]
        public virtual async Task<IActionResult> RecuperarServiciosPorLinea([FromQuery]HServiciosFilter Filtro)
        {
            try
            {
                if (Filtro == null)
                {
                    Filtro = new HServiciosFilter();
                }

                List<ItemDto> items = await this.Service.RecuperarServiciosPorLinea(Filtro);

                return ReturnData<List<ItemDto>>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }


        [HttpGet]
        public virtual async Task<IActionResult> RecuperarConductoresPorServicio([FromQuery]HServiciosFilter Filtro)
        {
            try
            {
                if (Filtro == null)
                {
                    Filtro = new HServiciosFilter();
                }

                List<ItemDto<string>> items = await this.Service.RecuperarConductoresPorServicio(Filtro);

                return ReturnData<List<ItemDto<string>>>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }




    }




}
