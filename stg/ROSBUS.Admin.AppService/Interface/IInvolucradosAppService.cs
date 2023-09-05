using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IInvolucradosAppService : IAppServiceBase<SinInvolucrados, InvolucradosDto, int>
    {
        Task<HistorialInvolucrados> HistorialSiniestros(int TipoDocId, string NroDoc);
        Task<List<AdjuntosDto>> GetAdjuntos(int involucradoId);
        Task AgregarAdjuntos(int involucradoId, List<AdjuntosDto> result);
        Task DeleteFileById(Guid id);
    }
}
