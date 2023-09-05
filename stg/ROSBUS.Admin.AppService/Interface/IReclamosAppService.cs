using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IReclamosAppService : IAppServiceBase<SinReclamos, ReclamosDto, int>
    {
        Task<List<AdjuntosDto>> GetAdjuntos(int reclamoId);
        Task AgregarAdjuntos(int reclamoId, List<AdjuntosDto> result);
        Task DeleteFileById(Guid id);
        Task<ReclamosDto> Anular(ReclamosDto reclamo);
        Task<ReclamosDto> CambioEstado(ReclamosDto reclamo, ReclamosHistoricosDto historico);
        Task<FileDto> GetReporteExcel(ReclamosFilter filter);
        Task<List<ImportadorExcelReclamos>> UploadExcel(byte[] excelFile);
        Task<List<ImportadorExcelReclamos>> RecuperarPlanilla(ReclamoImportadorFileFilter filter);
        Task ImportarReclamos(ReclamoImportadorFileFilter input);

        Task<Boolean> CheckNuevoReclamoNoNecesario(ReclamosFilter filter);
    }
}
