using Microsoft.AspNetCore.Mvc;
using ROSBUS.ART.AppService.Interface;
using ROSBUS.ART.AppService.Model;
using ROSBUS.ART.Domain.Entities;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.ART.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.ART.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class TiposAcuerdoController : ManagerSecurityController<TiposAcuerdo, int, TiposAcuerdoDto, TiposAcuerdoFilter, ITiposAcuerdoAppService>
    {
        public TiposAcuerdoController(ITiposAcuerdoAppService service)
            : base(service)
        {

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Reclamo", "TipoAcuerdo");
        }




    }


 

}
