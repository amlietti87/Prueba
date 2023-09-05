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

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class BolBanderasCartelController : ManagerController<BolBanderasCartel, int, BolBanderasCartelDto, BolBanderasCartelFilter, IBolBanderasCartelAppService>
    {
        public BolBanderasCartelController(IBolBanderasCartelAppService service)
            : base(service)
        {

        }


        [HttpGet]
        public virtual async Task<IActionResult> RecuperarCartelPorImportador(BolBanderasCartelFilter filter)
        {
            try
            {
                if (!filter.CodHfecha.HasValue)
                {
                    throw new ArgumentException("Codigo de fecha es requerido");
                }

                

                var BolBanderasCartelDto = await this.Service.RecuperarCartelPorImportador(filter);
                return ReturnData<BolBanderasCartelDto>(BolBanderasCartelDto);

            }
            catch (Exception ex)
            {
                return ReturnError<BolBanderasCartelDto>(ex);
            }
        }



    }


 

}
