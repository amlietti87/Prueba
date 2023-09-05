using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.ApiServices.Filters;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class PlaLineaController : ManagerSecurityController<PlaLinea, int, PlaLineaDto, PlaLineaFilter, IPlaLineaAppService>
    {
        private readonly ITipoLineaAppService tipoLineaService;
        private readonly IMemoryCache _Cache;

        public PlaLineaController(IPlaLineaAppService service, ITipoLineaAppService _tipoLineaService, IMemoryCache memoryCache)
            : base(service)
        {
            tipoLineaService = _tipoLineaService;
            _Cache = memoryCache;
        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Planificacion", "Linea");
        }


        [HttpPost]
        [ActionAuthorize()]
        public async override Task<IActionResult> GetPagedList([FromBody] PlaLineaFilter filter)
        {
            return await base.GetPagedList(filter);
        }


        [HttpGet]
        public IActionResult TieneMapasEnBorrador(int  Id)
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
        

    }




}
