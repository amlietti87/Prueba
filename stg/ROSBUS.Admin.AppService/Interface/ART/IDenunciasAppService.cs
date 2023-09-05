using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Interface;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.Partials;
using System.Threading.Tasks;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities.Filters.ART;
using ROSBUS.Admin.Domain.Report;

namespace ROSBUS.Admin.AppService.Interface.ART
{
    public interface IDenunciasAppService : IAppServiceBase<ArtDenuncias, ArtDenunciasDto, int>
    {
        Task<List<HistorialDenuncias>> HistorialDenunciaPorPrestador(int EmpleadoId);
        Task<List<HistorialReclamosEmpleado>> HistorialReclamosEmpleado(int EmpleadoId);
        Task<List<HistorialDenunciasPorEstado>> HistorialDenunciasPorEstado(int EmpleadoId);

        Task<List<AdjuntosDto>> GetAdjuntosDenuncias(int DenunciaId);
        Task AgregarAdjuntos(int DenunciaId, List<AdjuntosDto> result);
        Task DeleteFileById(Guid id);
        Task<ArtDenunciasDto> Anular(ArtDenunciasDto denuncia);

        Task<List<ImportadorExcelDenuncias>> RecuperarPlanilla(DenunciaImportadorFileFilter filter);
        Task<FileDto> GetReporteExcel(ExcelDenunciasFilter filter);
        Task<List<ImportadorExcelDenuncias>> UploadExcel(byte[] excelFile);

        Task<ReportModel> GetDatosReporte(ArtDenunciasDto dto);

        Task ImportarDenuncias(DenunciaImportadorFileFilter input);
        Task<Boolean> ImportWithTask();

    }
}
