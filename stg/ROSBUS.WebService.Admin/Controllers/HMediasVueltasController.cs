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
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class HMediasVueltasController : ManagerController<HMediasVueltas, int, HMediasVueltasDto, HMediasVueltasFilter, IHMediasVueltasAppService>
    {
        private readonly IReporterHttpAppService _reporterHttpAppService;
        private readonly ILineaAppService _lineaService;
        private readonly IHFechasConfiAppService _HFechasConfiServise;
        private readonly ITiposDeDiasAppService _TiposDiasAppService;
        public HMediasVueltasController(IHMediasVueltasAppService service, IReporterHttpAppService reporterHttpAppService, ILineaAppService lineaService, IHFechasConfiAppService hFechasConfiService, ITiposDeDiasAppService tiposDiasAppService)
            : base(service)
        {
            this._reporterHttpAppService = reporterHttpAppService;
            this._lineaService = lineaService;
            this._HFechasConfiServise = hFechasConfiService;
            this._TiposDiasAppService = tiposDiasAppService;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAllAsync(HMediasVueltasFilter filter)
        {
            try
            {

                PagedResult<HMediasVueltasDto> pList = await this.Service.GetDtoAllAsync(filter.GetFilterExpression(), filter.GetIncludesForPageList());
                                
                pList.Items = SortBy(pList.Items, filter.Sort).ToList();

                return ReturnData<PagedResult<HMediasVueltasDto>>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<HMediasVueltasDto>>(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReportPuntaPunta([FromBody]HMediasVueltasFilter data)
        {
            try
            {
                var datos = await this.Service.GetDatosReportePuntaPunta(data);

                var byteArray = await this._reporterHttpAppService.GenerarReporteGenerico(datos);
                var lin = await this._lineaService.GetByIdAsync(data.CodLinea.Value);
                var fec = await this._HFechasConfiServise.GetByIdAsync(data.CodHfecha.Value);
                var tipodia = await this._TiposDiasAppService.GetByIdAsync(data.CodTdia.Value);

                


                FileDto file = new FileDto();

                file.ByteArray = byteArray;
                file.ForceDownload = true;
                file.FileType = "application / pdf";
                file.FileName = "Punta y punta " + lin.DesLin.Trim() + " " + tipodia.DesTdia.Trim() + " " + fec.FecDesde.ToString("yyyy.MM.dd")+".pdf";
                file.FileDescription = "Reporte Punta Punta";
                return ReturnData<FileDto>(file);
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        private static IEnumerable<T> SortBy<T>(IEnumerable<T> items, string Sort)
        {
            if (!String.IsNullOrEmpty(Sort))
            {
                return System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(items.AsQueryable(), Sort);
            }
            return items;
        }


        [HttpGet]
        public async Task<IActionResult> LeerMediasVueltasIncompletas(HMediasVueltasFilter filter)
        {
            try
            {
                var result = await this.Service.LeerMediasVueltasIncompletas(filter);                
                return ReturnData<List<HMediasVueltasView>>(result);
            }
            catch (Exception ex)
            {
                return ReturnError<List<HMediasVueltasView>>(ex);
            }
        }

    }




}
