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
using TECSO.FWK.ApiServices.Filters;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class GalponController : ManagerSecurityController<Galpon, Decimal, GalponDto, GalponFilter, IGalponAppService>
    {
        public GalponController(IGalponAppService service)
            : base(service)
        {

        }


        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Planificacion", "Taller");
            this.PermissionContainer.AddPermission("SaveGalponPorSucursal", "Planificacion", "Ruta", "Modificar");
            this.PermissionContainer.AddPermission("UpdateRutasPorGalpon", "Planificacion", "Ruta", "Modificar");
            this.PermissionContainer.AddPermission("CanDeleteGalpon", "Planificacion", "Ruta", "Eliminar");

        }

        [HttpPost]
        [ActionAuthorize()]
        public virtual async Task<IActionResult> SaveGalponPorSucursal([FromBody]List<GalponDto> galponDtos)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this.Service.SaveGalponPorUnidadDeNegocio(galponDtos);
                    return ReturnData<GalponDto>(null);
                }
                else
                {
                    return ReturnError<GalponDto>(this.ModelState);
                }
                
            }
            catch (Exception ex)
            {
                return ReturnError<GalponDto>(ex);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetPuntosInicioFin([FromBody]GalponFilter filter)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    return ReturnData<List<RutasDto>>(await this.Service.GetPuntosInicioFin(filter));
                }
                else
                {
                    return ReturnError<RutasDto>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<RutasDto>(ex);
            }
        }


        [HttpPost]
        [ActionAuthorize()]
        public virtual async Task<IActionResult> CanDeleteGalpon([FromBody]GalponDto galpon)
        {
            try
            {

                if (this.ModelState.IsValid)
                {
                    return ReturnData<Boolean>(await this.Service.CanDeleteGalpon(galpon));
                }
                else
                {
                    return ReturnError<GalponDto>(this.ModelState);
                } 
            }
            catch (Exception ex)
            {
                return ReturnError<RutasDto>(ex);
            }
        }

       

        [HttpPost]
        [ActionAuthorize()]
        public virtual async Task<IActionResult> UpdateRutasPorGalpon([FromBody]GalponDto galpon)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this.Service.UpdateRutasPorGalpon(galpon);
                    return ReturnData<GalponDto>(null);
                }
                else
                {
                    return ReturnError<GalponDto>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<GalponDto>(ex);
            }
        }





    }


 

}
