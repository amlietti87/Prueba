using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Model.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IHFechasConfiService : IServiceBase<HFechasConfi, int>
    {
        Task<List<PlaHorarioFechaLineaListView>> GetLineasHorarias();
        Task<ItemDto> CopiarHorario(int cod_hfecha, DateTime fec_desde, bool CopyConductores);
        Task<Boolean> HorarioDiagramado(int CodHfecha, int? CodTdia, List<int> CodServicio);
        Task<List<string>> ObtenerDestinatarios(decimal CodLin);
        Task<int> RemapearRecoridoBandera(HFechasConfiFilter filter);
        Task<List<HKilometros>> GetKilometrosAsync(List<int> CodBan, List<int> CodSec);
        Task<List<ReporteHorarioExcelModel>> GenerarExcelHorarios(ExportarExcelFilter filter);
        Task<List<RelevoModel>> GetDatosReporteRelevos(ExportarExcelFilter filtro);
        Task<string> GetTitulo(ExportarExcelFilter filtro);
        Task <HBasec> UpdateHBasec(int CodBan, int CodHFecha, int? CodRec);
        Task GuardarBanderaPorSector(HFechasConfi data);
    }
}
