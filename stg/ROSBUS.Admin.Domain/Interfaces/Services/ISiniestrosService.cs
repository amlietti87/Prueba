using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface ISiniestrosService : IServiceBase<SinSiniestros, int>
    {
        Task<HistorialSiniestros> GetHistorialEmpPract(bool empleado, int id);
        Task<int> GetNroSiniestroMax();
        Task<string> GenerarInforme(int SiniestroId);
        Task<List<SinSiniestroAdjuntos>> GetAdjuntosSiniestros(int siniestroId);
        Task DeleteFileById(Guid id);
        Task<string> GetInforme(string codInforme);
        Task<List<Licencias>> GetLicencias(int cod_empleado);
    }
}
