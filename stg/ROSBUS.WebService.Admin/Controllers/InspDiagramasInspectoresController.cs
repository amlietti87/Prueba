using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Filters.AppInspectores;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.WebService.Admin.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class InspDiagramasInspectoresController : ManagerSecurityController<InspDiagramasInspectores, int, InspDiagramasInspectoresDto, InspDiagramasInspectoresFilter, IInspDiagramasInspectoresAppService>
    {
        public InspDiagramasInspectoresController(IInspDiagramasInspectoresAppService service)
            : base(service)
        {

        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Inspectores", "Diagramacion");
        }

        [HttpPost]
        public virtual async Task<IActionResult> DiagramaMesAnioGrupo([FromBody]DiagramaRequestModel model)
        {
            try
            {
                DiagramaMesAnioDto items = await this.Service.DiagramaMesAnioGrupo(model.Id, model.TurnoId, model.Blockentity);

                if(items.DiasMes != null)
                {
                    return ReturnData<DiagramaMesAnioDto>(items);
                }
                else
                {
                    throw new ValidationException("No existe Usuarios para el Grupo de Inspectores");
                }

                
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }

        }


        [HttpPost]
        public virtual async Task<IActionResult> EliminarCelda([FromBody]DiasMesDto model)
        {
            try
            {
                InspectorDiaDto celda = await this.Service.EliminarCelda(model);

                return ReturnData<InspectorDiaDto>(celda);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }

        }

        [HttpGet]
        public virtual async Task<IActionResult> DiagramacionPorDia(DateTime Fecha)
        {
            try
            {
              DiasMesDto items = await this.Service.DiagramacionPorDia(Fecha);

                return ReturnData<DiasMesDto>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }

        }
        
        [HttpGet]
        public virtual async Task<IActionResult> TurnosDeLaDiagramacion(int Id)
        {
            try
            {
                List<InspDiagramasInspectoresTurnosDto> items = await this.Service.TurnosDeLaDiagramacion(Id);

                return ReturnData<List<InspDiagramasInspectoresTurnosDto>>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }

        }

        [HttpPost]
        public virtual async Task<IActionResult> SaveDiagramacion([FromBody] DiagramacionSave diagramacion)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    await this.Service.ValidateCocurrencySave(diagramacion.Id, diagramacion.BlockDate);
                    await this.Service.SaveDiagramacion(diagramacion.Inspectores, diagramacion.Id);
                    return ReturnData<string>("");
                }
                else
                {
                    return this.ReturnError<string>(this.ModelState);
                }
                
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }

        }

        [HttpPost]
        public virtual async Task<IActionResult> PublicarDiagramacion([FromBody] InspDiagramasInspectores Diagramacion)
        {
            try
            {
                await this.Service.PublicarDiagramacion(Diagramacion);
                return ReturnData<string>("");
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> ImprimirDiagrama([FromBody]DiagramaRequestModel model)
        {
            try
            {                
                return ReturnData(await this.Service.ImprimirDiagrama(model.Id, model.TurnoId));
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }


        [HttpPost]
        public override Task<IActionResult> UnBlockEntity(int Id)
        {
            return base.UnBlockEntity(Id);
        }


    }

}
