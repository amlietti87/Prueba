using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters.ART;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using ROSBUS.Admin.Domain.Entities.ART;

namespace ROSBUS.WebService.ART.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class PatologiasController : ManagerSecurityController<ArtPatologias, int, ArtPatologiasDto, PatologiasFilter, IPatologiasAppService>
    {
        public PatologiasController(IPatologiasAppService service)
            : base(service)
        {

        }
        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("ART", "Patologia");
        }

    }


 

}
