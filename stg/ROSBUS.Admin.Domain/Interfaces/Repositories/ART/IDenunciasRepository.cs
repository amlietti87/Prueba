using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Threading.Tasks;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces.Services.ART;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Entities.Filters.ART;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories.ART
{
    public interface IDenunciasRepository: IRepositoryBase<ArtDenuncias, int>
    {
        Task<List<HistorialDenuncias>> HistorialDenunciaPorPrestador(int EmpleadoId);
        Task<List<HistorialReclamosEmpleado>> HistorialReclamosEmpleado(int EmpleadoId);
        Task<List<HistorialDenunciasPorEstado>> HistorialDenunciasPorEstado(int EmpleadoId);
        Task<List<ArtDenunciaAdjuntos>> GetAdjuntosDenuncias(int DenunciaId);
        Task DeleteFileById(Guid id);
        Task<ArtDenuncias> Anular(ArtDenuncias denuncia);
        Task<List<ReporteDenunciasExcel>> GetReporteExcel(ExcelDenunciasFilter filter);

        Task ImportarDenuncias(List<ImportadorExcelDenuncias> planilla);

        Task<List<Destinatario>> GetNotificacionesMail(string Token);
    }
}
