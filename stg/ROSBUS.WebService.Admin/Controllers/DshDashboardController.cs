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
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class DshDashboardController : ManagerController<DshDashboard, int, DshDashboardDto, DshDashboardFilter, IDshDashboardAppService>
    {
        private readonly IUserAppService userAppService;

        public DshDashboardController(IDshDashboardAppService service, IUserAppService _userAppService)
            : base(service)
        {
            this.userAppService = _userAppService;
        }

        [HttpPost]
        public virtual async Task<IActionResult> GuardarDashboard([FromBody] UsuarioDashboardInput dto)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this.Service.GuardarDashboard(dto);

                    List<UsuarioDashboardItemDto> returnData =  await this.Service.RecuperarDshUsuarioDashboardItems(authService.GetCurretUserId());

                    return ReturnData<List<UsuarioDashboardItemDto>>(returnData);
                }
                else
                {
                    return ReturnError<string>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }


        [HttpPost]
        public virtual async Task<IActionResult> RecuperarDshUsuarioDashboardItems()
        {
            try
            {
                List<UsuarioDashboardItemDto> returnData = await this.Service.RecuperarDshUsuarioDashboardItems(authService.GetCurretUserId());

                UsuarioDashboardInput dto = new UsuarioDashboardInput();
                dto.Items = returnData;

                var user = this.userAppService.GetById(authService.GetCurretUserId());

                dto.DashboardLayoutId = (user?.DashboardLayoutId).GetValueOrDefault(1);

                return ReturnData<UsuarioDashboardInput>(dto);

            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

    }


 

}
