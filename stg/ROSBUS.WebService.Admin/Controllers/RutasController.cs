using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class RutasController : ManagerSecurityController<GpsRecorridos, int, RutasDto, RutasFilter, IRutasAppService>
    {
        public RutasController(IRutasAppService service)
            : base(service)
        {

        }


        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Planificacion", "Ruta");
            this.PermissionContainer.AddPermission("AprobarRutaAsync", "Planificacion", "Ruta", "AprobarRuta");
            this.PermissionContainer.AddPermission("MinutosPorSectorReporte", "Planificacion", "Ruta", "MinutosPorSector");
            
        }

        [HttpPost]
        public virtual async Task<IActionResult> AprobarRutaAsync(int Id)
        {
            try
            {
                await this.Service.AprobarRutaAsync(Id);
                return ReturnData<string>("Aproved");
            }
            catch (Exception ex)
            {
                return ReturnError<RutasDto>(ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> ValidateAprobarRuta(int id)
        {
            try
            {
                var list = await this.Service.ValidateAprobarRuta(id);
                if (list.Any())
                {
                    return ReturnData<string>(string.Join(Environment.NewLine, list.ToArray()), ActionStatus.OkAndConfirm);
                }
                else
                {
                    return ReturnData<string>("Aproved");
                }
            }
            catch (Exception ex)
            {
                return ReturnError<RutasDto>(ex);
            }
        }



        [HttpPost]
        public async Task<IActionResult> ValidateAprobarRutaDto([FromBody] RutasDto dto)
        {
            try
            {
                var list = await this.Service.ValidateAprobarRuta(dto);
                if (list.Any())
                {
                    return ReturnData<string>(string.Join(Environment.NewLine, list.ToArray()), ActionStatus.OkAndConfirm);
                }
                else
                {
                    return ReturnData<string>("Aproved");
                }
            }
            catch (Exception ex)
            {
                return ReturnError<RutasDto>(ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetRutas([FromBody] RutasViewFilter filter)
        {
            try
            {
                var pList = await this.Service.GetRutas(filter);                 
                return ReturnData<List<RutasDto>>(pList);
            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<RutasDto>>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetRutasFiltradas ([FromBody] RutasFilteredFilter filter)
        {
            try
            {
                var pList = await this.Service.GetRutasFiltradas(filter);
                return ReturnData<List<RutasDto>>(pList);
            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<RutasDto>>(ex);
            }
        }



        [HttpGet]
        public async Task<IActionResult> RecuperarHbasecPorLinea(int cod_lin)
        {
            try
            {
                var pList = await this.Service.RecuperarHbasecPorLinea(cod_lin);
                return ReturnData<List<ItemDto>>(pList);
            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<RutasDto>>(ex);
            }
        }


        [HttpPost]
        public virtual async Task<IActionResult> MinutosPorSectorReporte([FromBody]MinutosPorSectorFilter filter)
        {
            try
            {
                return ReturnData<FileDto>(await this.Service.MinutosPorSectorReporte(filter));
            }
            catch (Exception ex)
            {
                return ReturnError<RutasDto>(ex);
            }
        }

       [HttpPost]
        public override async Task<IActionResult> GetPagedList([FromBody] RutasFilter filter)
        {
            try
            {

                TECSO.FWK.Domain.Entities.PagedResult<RutasDto> pList = await this.Service.GetDtoPagedListAsync<RutasFilter>(filter);

                List<RutasDto> rutasDto = new List<RutasDto>();
                RutasDto mapaOriginal = pList.Items.Where(e => e.EsOriginal == 2).FirstOrDefault();

                if (filter.Sort != null && mapaOriginal != null && filter.Sort.Contains("EstadoRuta.Nombre"))
                {
                    rutasDto = pList.Items.Where(e => e.EsOriginal != 2).ToList();
                    rutasDto.Insert(0, mapaOriginal);

                }

                if (rutasDto.Count > 0)
                {
                    pList.Items = rutasDto.ToList();
                }
                else
                {
                    pList.Items = pList.Items.OrderByDescending(x => x.EsOriginal).ToList();
                }
                
                
                return ReturnData<TECSO.FWK.Domain.Entities.PagedResult<RutasDto>>(pList);
            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<RutasDto>>(ex);
            }
        }
    }
}
