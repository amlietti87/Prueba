using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IInvolucradosService : IServiceBase<SinInvolucrados, int>
    {
        Task<HistorialInvolucrados> HistorialSiniestros(int TipoDocId, string NroDoc);
        Task<List<SinInvolucradosAdjuntos>> GetAdjuntos(int involucradoId);
        Task DeleteFileById(Guid id);
        Task<List<SinInvolucrados>> GetBySiniestro(int SiniestroId);
    }
}
