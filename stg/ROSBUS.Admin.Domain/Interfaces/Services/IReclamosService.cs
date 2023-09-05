using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IReclamosService : IServiceBase<SinReclamos, int>
    {
        Task<List<SinReclamoAdjuntos>> GetAdjuntos(int reclamoId);
        Task DeleteFileById(Guid id);
        Task<List<SinReclamos>> GetByInvolucrado(int InvolucradoId);
        Task<SinReclamos> CambioEstado(SinReclamos reclamo, SinReclamosHistoricos historico);
        Task<SinReclamos> Anular(SinReclamos reclamo);
        Task<List<ReporteReclamosExcel>> GetReporteExcel(ExcelReclamosFilter filter);
        Task<List<ImportadorExcelReclamos>> RecuperarPlanilla(ReclamoImportadorFileFilter filter);
        Task ImportarReclamos(ReclamoImportadorFileFilter input);
    }
}
