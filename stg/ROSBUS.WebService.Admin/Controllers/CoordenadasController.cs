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
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class CoordenadasController : ManagerController<PlaCoordenadas, int, CoordenadasDto, CoordenadasFilter, ICoordenadasAppService>
    {
        public CoordenadasController(ICoordenadasAppService service)
            : base(service)
        {

        }

        public override Task<IActionResult> GetItemsAsync(CoordenadasFilter filter)
        {
            return base.GetItemsAsync(filter);
        }


        public async override Task<IActionResult> GetAllAsync(CoordenadasFilter filter)
        {
            try
            { 
 
                var pList = await this.Service.GetDtoAllAsync(filter.GetFilterExpression());

                return ReturnData<PagedResult<CoordenadasDto>>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<CoordenadasDto>>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RecuperarCoordenadasPorFecha (CoordenadasFilter filter)
        {
            try
            {
                var coordenadas = await this.Service.RecuperarCoordenadasPorFecha(filter);
                return ReturnData<List<PlaCoordenadas>>(coordenadas);
            }
            catch(Exception ex)
            {
                return ReturnError<PagedResult<CoordenadasDto>>(ex);
            }
        }

    }


 

}
