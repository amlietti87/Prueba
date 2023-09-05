using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IHFechasConfiAppService : IAppServiceBase<HFechasConfi, HFechasConfiDto, int>
    {
        Task<List<PlaHorarioFechaLineaListView>> GetLineasHorarias();
        Task<ItemDto> CopiarHorario(int cod_hfecha, DateTime fec_desde, bool CopyConductores);
        Task<Boolean> HorarioDiagramado(int CodHfecha, int? CodTdia, List<int> CodServicio);
        Task<List<string>> ObtenerDestinatarios(decimal CodLin);
        Task<int> RemapearRecoridoBandera(HFechasConfiFilter filter);
        Task<FileDto> GenerarExcelHorarios(ExportarExcelFilter filter);
        Task<ReportModel> GetDatosReporteRelevos(ExportarExcelFilter data);
        Task<string> GetTitulo(ExportarExcelFilter data);
        Task<HBasecDto> UpdateHBasec(HBasecDto hbasec);
        Task GuardarBanderaPorSector(HFechasConfiDto data);
    }
}
