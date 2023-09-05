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
    public class InspTareasRealizadasController : ManagerSecurityController<InspTareasRealizadas, int, InspTareasRealizadasDto, InspTareasRealizadasFilter, IInspTareasRealizadasAppService>
    {
        public InspTareasRealizadasController(IInspTareasRealizadasAppService service)
            : base(service)
        {

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Inspectores", "RegistrarTareas");
        }


    }


 

}
