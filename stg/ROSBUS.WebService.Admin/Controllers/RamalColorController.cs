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
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class RamalColorController : ManagerSecurityController<PlaRamalColor, Int64, RamalColorDto, RamalColorFilter, IRamalColorAppService>
    {
        public RamalColorController(IRamalColorAppService service)
            : base(service)
        {

        }



        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Planificacion", "RamaColor");
        }


        [HttpGet]
        public IActionResult TieneMapasEnBorrador(int Id)
        {
            try
            {
                Boolean tiene = this.Service.TieneMapasEnBorrador(Id);

                return this.ReturnData<Boolean>(tiene);
            }
            catch (Exception ex)
            {
                return ReturnError<Boolean>(ex);
            }

        }

        public override Task<IActionResult> GetItemsAsync(RamalColorFilter filter)
        {
            return base.GetItemsAsync(filter);
        }

        [HttpGet]
        public async virtual Task<IActionResult> GetAllAsyncSinSentidos(RamalColorFilter filter)
        {
            try
            {
                var r = await this.Service.GetItemsAsyncSinSentidos(filter);
                return ReturnData<List<ItemDto<long>>>(r);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<RamalColorDto>>(ex);
            }
        }


    }




}
