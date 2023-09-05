using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface ISiniestrosRepository: IRepositoryBase<SinSiniestros, int>
    {
        Task<HistorialSiniestros> GetHistorialEmpPract(bool empleado, int id, string dni);
        Task<int> GetNroSiniestroMax();
        Task<string> GenerarInforme(int SiniestroId);
        Task<List<SinSiniestroAdjuntos>> GetAdjuntosSiniestros(int siniestroId);
        Task DeleteFileById(Guid id);

        Task<List<Licencias>> GetLicencias(int cod_empleado);
        Task<string> GetInforme(string codInforme);
    }
}
