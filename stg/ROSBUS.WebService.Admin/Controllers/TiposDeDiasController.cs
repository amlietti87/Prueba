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
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class TiposDeDiasController : ManagerSecurityController<HTipodia, int, TiposDeDiasDto, TiposDeDiasFilter, ITiposDeDiasAppService>
    {
        public TiposDeDiasController(ITiposDeDiasAppService service)
            : base(service)
        {

        } 

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Admin", "TipoDeDias");
        }

        [HttpGet]
        public virtual async Task<IActionResult> DescripcionPredictivo([FromQuery]TiposDeDiasFilter Filtro)
        {
            try
            {

                if (Filtro == null)
                {
                    Filtro = new TiposDeDiasFilter();
                }

                List<KeyValuePair<bool, string>> items = await this.Service.DescripcionPredictivo(Filtro);

                return ReturnData<List<KeyValuePair<bool, string>>>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

    }


 

}
