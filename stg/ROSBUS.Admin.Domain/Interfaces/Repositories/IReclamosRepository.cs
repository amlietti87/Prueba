using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IReclamosRepository : IRepositoryBase<SinReclamos, int>
    {
        Task DeleteFileById(Guid id);
        Task<List<SinReclamoAdjuntos>> GetAdjuntos(int reclamoId);

        Task<List<SinReclamos>> GetByInvolucrado(int InvolucradoId);

        Task<SinReclamos> Anular(SinReclamos reclamo);

        Task<SinReclamos> CambioEstado(SinReclamos reclamo, SinReclamosHistoricos historico);

        Task<List<ReporteReclamosExcel>> GetReporteExcel(ExcelReclamosFilter filter);

        Task ImportarReclamos(List<ImportadorExcelReclamos> planilla);

    }
}
