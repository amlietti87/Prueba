using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.WebService.Admin.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class InfInformesController : ManagerSecurityController<InfInformes, string, InfInformesDto, InfInformesFilter, IInfInformesAppService>
    {
        public InfInformesController(IInfInformesAppService service)
            : base(service)
        {

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Inspectores", "Informes");
        }

        public override async Task<IActionResult> SaveNewEntity([FromBody] InfInformesDto dto)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var entity = await this.Service.AddAsync(dto);
                    if (String.IsNullOrEmpty(entity.CodInspector))
                    {
                        return ReturnData<string>(entity.NroInforme, messages: new List<string> { "Notifique a Sistemas que no existe el empleado asociado a su Usuario." });
                    }
                    else
                    {
                        return ReturnData<string>(entity.NroInforme);
                    }
                }
                else
                {
                    return ReturnError<string>(this.ModelState);
                }

                
            }
            catch (Exception ex)
            {
                return ReturnError<InfInformesDto>(ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> EnviarInformeJson([FromBody] InformeErrorModel informeError)
        {
            try
            {
                await this.logger.LogError(informeError.model);
            }
            catch (Exception ex)
            {
                await this.logger.LogError(ex.Message);
            }

            return ReturnData<Boolean>(true);
        }
        
        [HttpGet]
        public async Task<IActionResult> ConsultaInformeUserDia()
        {
            try
            {
                var items = await this.Service.ConsultaInformesUserDia();

                return ReturnData(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }

        }

    }


 

}
