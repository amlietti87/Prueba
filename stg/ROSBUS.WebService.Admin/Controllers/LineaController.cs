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
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class LineaController : ManagerSecurityController<Linea, decimal, LineaDto, LineaFilter, ILineaAppService>
    {        
        private readonly ITipoLineaAppService tipoLineaService;
        private readonly IMemoryCache _Cache;

        public LineaController(ILineaAppService service, ITipoLineaAppService _tipoLineaService, IMemoryCache memoryCache)
            : base(service)
        {
            //Ejemplo de Log
            //logger.LogCritical("LogCritical");
            //logger.LogError("LogError");
            //logger.LogInformation("LogInformation");
            //logger.LogWarning("LogWarning");
            tipoLineaService = _tipoLineaService;
            _Cache = memoryCache;

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Planificacion", "Linea");
        }




        [HttpPost]
        [ActionAuthorize()]
        public async override Task<IActionResult> GetPagedList([FromBody] LineaFilter filter)
        {

            try
            {

                IReadOnlyList<PlaTipoLinea> tl = this._Cache.Get<IReadOnlyList<PlaTipoLinea>>("tipoLineaService");

                if (tl == null)
                {
                    var f = new TipoLineaFilter();
                    tl = (await this.tipoLineaService.GetAllAsync(f.GetFilterExpression())).Items;
                    this._Cache.Set("tipoLineaService", tl);
                }

                var list = await this.Service.GetPagedListAsync(filter);

                var listDto = this.MapList<Linea, LineaDto>(list.Items);


                foreach (var item in listDto)
                {
                    item.TipoLinea = tl.Where(e => e.Id.ToString() == item.UrbInter).Select(e => e.Nombre).FirstOrDefault();
                }

                PagedResult<LineaDto> pList = new PagedResult<LineaDto>(list.TotalCount, listDto.ToList());


                return ReturnData<PagedResult<LineaDto>>(pList);


            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<LineaDto>>(ex);
            }




        }
 
        [HttpGet]
        public async Task<IActionResult> GetLineasPorUsuario()
        {
            try
            {
                var r = await this.Service.GetLineasPorUsuario();
                return ReturnData<List<ItemDecimalDto>>(r);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<ItemDto>>(ex);
            }
        }


        

        [HttpPost]
        
        public async Task<IActionResult> UpdateLineasAsociadas([FromBody] LineaDto lineaDto)
        {
            try
            {
                await this.Service.UpdateLineasAsociadas(lineaDto);

                return ReturnData<LineaDto>(lineaDto);
            }
            catch (Exception ex)
            {
                return ReturnError<LineaDto>(ex);
            }

        }

        [HttpGet]
        public async Task<IActionResult> RecuperarLineasConLineasAsociadas(int lineaId)
        {
            try
            {
                
                LineaDto l = await this.Service.RecuperarLineaConLineasAsociadas(lineaId);

                return ReturnData<LineaDto>(l);
            }
            catch (Exception ex)
            {
                return ReturnError<LineaDto>(ex);
            }

        }
    } 
}
