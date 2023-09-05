using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.WebService.Siniestros.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class InvolucradosController : ManagerSecurityController<SinInvolucrados, int, InvolucradosDto, InvolucradosFilter, IInvolucradosAppService>
    {
        private readonly IAdjuntosAppService adjuntosAppService;


        public InvolucradosController(IInvolucradosAppService service, IAdjuntosAppService adjuntosAppService)
            : base(service)
        {
            this.adjuntosAppService = adjuntosAppService;
        }

        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Siniestro", "Involucrado");
            this.PermissionContainer.Permissions.Remove(this.PermissionContainer.Permissions.FirstOrDefault(e => e.ActionName == PermissionContainerBase.GetPagedList));
            this.PermissionContainer.AddPermission(PermissionContainerBase.GetPagedList, "Siniestro", "Involucrado", "Visualizar");
        }

        [HttpGet]
        public async Task<IActionResult> HistorialSiniestros(int TipoDocId, string NroDoc)
        {
            try
            {
                var data = await Service.HistorialSiniestros(TipoDocId, NroDoc);
                return ReturnData<HistorialInvolucrados>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<HistorialInvolucrados>(ex);
            }
        }



        [HttpGet("{InvolucradoId}")]
        public async Task<IActionResult> GetAdjuntos(int InvolucradoId)
        {
            try
            {
                List<AdjuntosDto> data = await Service.GetAdjuntos(InvolucradoId);
                return ReturnData<List<AdjuntosDto>>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }


        [HttpPost()]
        public async Task<IActionResult> UploadFiles(int InvolucradoId)
        {

            try
            {
                var result = await adjuntosAppService.AgregarAdjuntos(this.Request.Form.Files);

                await this.Service.AgregarAdjuntos(InvolucradoId, result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }



        [HttpPost]
        public async Task<IActionResult> DeleteFileById(Guid Id)
        {
            try
            {

                await adjuntosAppService.DeleteAsync(Id);

                await this.Service.DeleteFileById(Id);

                return ReturnData<string>("Deleted");
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        public override async Task<IActionResult> GetAllAsync(InvolucradosFilter filter)
        {
            try
            {
                Expression<Func<SinInvolucrados, bool>> exp = e => true;

                if (filter != null)
                {
                    exp = filter.GetFilterExpression();
                }
                var pList = await this.Service.GetDtoAllAsync(exp, filter.GetIncludesForPageList());

                return ReturnData<PagedResult<InvolucradosDto>>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<InvolucradosDto>>(ex);
            }
        }


        public override Task<IActionResult> UpdateEntity([FromBody] InvolucradosDto dto)
        {
            return base.UpdateEntity(dto);
        }

        public override async Task<IActionResult> SaveNewEntity([FromBody] InvolucradosDto dto)
        {
            return await base.SaveNewEntity(dto);
        }
    }




}
