using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface ICCochesService : IServiceBase<CCoches, string>
    {
        Task<List<int>> GetLineaPorDefecto(int CodEmpleado, DateTime Fecha);
        Task<bool> ExisteCoche(string id);
        Task<CCoches> GetCocheById(string id, DateTime FechaSiniestro);

        Task<List<CCoches>> GetAllCoches(DateTime FechaSiniestro, string Filter);

        Task<List<CCochesDto>> RecuperarCCochesPorFechaServicioLinea(DateTime Fecha, int? Cod_Servicio, int Cod_Linea);

        Task<List<CCochesDto>> RecuperarCCoches(string Filter);
    }
}
