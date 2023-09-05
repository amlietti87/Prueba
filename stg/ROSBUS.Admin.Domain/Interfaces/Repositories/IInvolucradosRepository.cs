using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IInvolucradosRepository: IRepositoryBase<SinInvolucrados, int>
    {
        Task<HistorialInvolucrados> HistorialSiniestros(int TipoDocId, string NroDoc);
        Task DeleteFileById(Guid id);
        Task<List<SinInvolucradosAdjuntos>> GetAdjuntos(int involucradoId);
        Task<List<SinInvolucrados>> GetBySiniestro(int SiniestroId);
    }
}
