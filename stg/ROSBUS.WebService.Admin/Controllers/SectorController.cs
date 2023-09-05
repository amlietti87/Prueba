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
    public class SectorController : ManagerController<PlaSector, Int64, SectorDto, SectorFilter, ISectorAppService>
    {
        public SectorController(ISectorAppService service)
            : base(service)
        {

        }

        [HttpPost]
        public async Task<IActionResult> GetSectorView([FromBody] SectorConPuntosFilter filter)
        {
            try
            {
                RutaSectoresDto result = await this.Service.GetSectorView(filter);

                return ReturnData<RutaSectoresDto>(result);
                 
            }
            catch (Exception ex)
            {
                return ReturnError<RutaSectoresDto>(ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> RecuperarSentidoPorSector(HDesignarFilter Filtro)
        {
            try
            {
                var sectores = await Service.RecuperarSentidoPorSector(Filtro);
                return ReturnData<List<PlaSentidoPorSector>>(sectores);
            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<PlaSentidoPorSector>>(ex);
            }
        }


    }




}
