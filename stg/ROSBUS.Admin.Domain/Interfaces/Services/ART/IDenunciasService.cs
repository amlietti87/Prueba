using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.Partials;
using System.Threading.Tasks;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Entities.Filters.ART;

namespace ROSBUS.Admin.Domain.Interfaces.Services.ART
{
    public interface IDenunciasService: IServiceBase<ArtDenuncias, int>
    {
        Task<List<HistorialDenuncias>> HistorialDenunciaPorPrestador(int EmpleadoId);
        Task<List<HistorialReclamosEmpleado>> HistorialReclamosEmpleado(int EmpleadoId);
        Task<List<HistorialDenunciasPorEstado>> HistorialDenunciasPorEstado(int EmpleadoId);
        Task<List<ArtDenunciaAdjuntos>> GetAdjuntosDenuncias(int DenunciaId);
        Task DeleteFileById(Guid id);
        Task<ArtDenuncias> Anular(ArtDenuncias denuncia);
        Task<List<ReporteDenunciasExcel>> GetReporteExcel(ExcelDenunciasFilter filter);
        Task ImportarDenuncias(DenunciaImportadorFileFilter input);
        Task<List<ImportadorExcelDenuncias>> RecuperarPlanilla(DenunciaImportadorFileFilter filter);
        Task ImportarDenunciasFromTask(List<ImportadorExcelDenuncias> planilla, string fileName);

        Task<List<Destinatario>> GetNotificacionesMail(string Token);

    }
}
