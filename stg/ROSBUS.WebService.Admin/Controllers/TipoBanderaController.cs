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

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class TipoBanderaController : ManagerController<PlaTipoBandera, int, TipoBanderaDto, TipoBanderaFilter, ITipoBanderaAppService>
    {
        public TipoBanderaController(ITipoBanderaAppService service)
            : base(service)
        {

        }

        [HttpGet]
        public async override Task<IActionResult> GetAllAsync(TipoBanderaFilter filter)
        {
            try
            {
                Expression<Func<PlaTipoBandera, bool>> exp = e => true;

                if (filter != null)
                {
                    exp = filter.GetFilterExpression();
                }
                PagedResult<TipoBanderaDto> pList = await this.Service.GetDtoAllAsync(exp);
                pList.Items = pList.Items.OrderByDescending(x => x.Id).ToList();

                return ReturnData<PagedResult<TipoBanderaDto>>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<TipoBanderaDto>>(ex);
            }
        }


    }


 

}
