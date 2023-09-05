using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface ISiniestrosAppService : IAppServiceBase<SinSiniestros, SiniestrosDto, int>
    {
        Task<HistorialSiniestros> GetHistorialEmpPract(bool empleado, int id);
        Task<int> GetNroSiniestroMax();

        Task<string> GenerarInforme(int SiniestroId);
        Task<List<AdjuntosDto>> GetAdjuntosSiniestros(int SiniestroId);
        Task AgregarAdjuntos(int siniestroId,  List<AdjuntosDto> result);
        Task DeleteFileById(Guid id);

        Task<ReportModel> GetDatosReporte(SiniestrosDto dto);
    }
}
