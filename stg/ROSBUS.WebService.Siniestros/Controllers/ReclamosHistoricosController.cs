using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.WebService.Siniestros.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class ReclamosHistoricosController : ManagerSecurityController<SinReclamosHistoricos, int, ReclamosHistoricosDto, ReclamosHistoricosFilter, IReclamosHistoricosAppService>
    {


        public ReclamosHistoricosController(IReclamosHistoricosAppService service)
            : base(service)
        {
        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Siniestro", "Siniestro");
        }

        [HttpPost]
        public override Task<IActionResult> SaveNewEntity([FromBody] ReclamosHistoricosDto dto)
        { 
            dto.Fecha = DateTime.Now;

            return base.SaveNewEntity(dto);
        }


        public override Task<IActionResult> GetByIdAsync(int Id, bool blockEntity = true)
        {
            return base.GetByIdAsync(Id);
        }

        public override async Task<IActionResult> GetAllAsync(ReclamosHistoricosFilter filter)
        {
            try
            {
                Expression<Func<SinReclamosHistoricos, bool>> exp = e => true;

                if (filter != null)
                {
                    exp = filter.GetFilterExpression();
                }
                var pList = await this.Service.GetDtoAllAsync(exp, filter.GetIncludesForPageList());

                return ReturnData<PagedResult<ReclamosHistoricosDto>>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<ReclamosHistoricosDto>>(ex);
            }
        }

    }


 

}
