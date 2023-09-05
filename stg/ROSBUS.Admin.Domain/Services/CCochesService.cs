using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters; 
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Operaciones.Domain.Entities;
using ROSBUS.Operaciones.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Operaciones.Domain.Services
{
    public class CCochesService : ServiceBase<CCoches,string, ICCochesRepository>, ICCochesService
    { 
        public CCochesService(ICCochesRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public async Task<List<int>> GetLineaPorDefecto(int CodEmpleado, DateTime Fecha)
        {
            return await this.repository.GetLineaPorDefecto(CodEmpleado, Fecha);
        }

        public async Task<bool> ExisteCoche(string id)
        {
            return await this.repository.ExisteCoche(id);
        }


        public async Task<CCoches> GetCocheById(string id, DateTime FechaSiniestro)
        {
            return await this.repository.GetCocheById(id, FechaSiniestro);
        }

        public async Task<List<CCoches>> GetAllCoches(DateTime FechaSiniestro, string Filter)
        {
            return await this.repository.GetAllCoches(FechaSiniestro, Filter);
        }
       

        public async Task<List<CCochesDto>> RecuperarCCochesPorFechaServicioLinea(DateTime Fecha, int? Cod_Servicio, int Cod_Linea)
        {
            return await this.repository.RecuperarCCochesPorFechaServicioLinea(Fecha, Cod_Servicio, Cod_Linea);
        }


        public async Task<List<CCochesDto>> RecuperarCCoches(string FilterText)
        {
            return await this.repository.RecuperarCCoches(FilterText);
        }
    }
    
}
