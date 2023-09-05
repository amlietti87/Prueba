using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Operaciones.AppService.Interface;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Operaciones.AppService
{

    public class CCochesAppService : AppServiceBase<CCoches, CCochesDto, string, ICCochesService>, ICCochesAppService
    {
        public CCochesAppService(ICCochesService serviceBase) 
            :base(serviceBase)
        {
         
        }

        public async override Task<List<ItemDto<string>>> GetItemsAsync<TFilter>(TFilter filter)
        {
            var list = await this.GetPagedListAsync(filter);
            var r = list.Items.Select(filter.GetItmDTO()).ToList();
            return r;
        }

        public async Task<List<int>> GetLineaPorDefecto(int CodEmpleado, DateTime Fecha)
        {
            return await _serviceBase.GetLineaPorDefecto(CodEmpleado, Fecha);
        }

        public async Task<bool> ExisteCoche(string id)
        {
            return await this._serviceBase.ExisteCoche(id);
        }
        public async Task<CCoches> GetCocheById(string id, DateTime FechaSiniestro)
        {
            return await _serviceBase.GetCocheById(id, FechaSiniestro);
        }

        public async Task<List<CCoches>> GetAllCoches(DateTime FechaSiniestro, string Filter)
        {
            return await _serviceBase.GetAllCoches(FechaSiniestro, Filter);
        }

        public async Task<List<CCochesDto>> RecuperarCCochesPorFechaServicioLinea(DateTime Fecha, int? Cod_Servicio, int Cod_Linea)
        {
            return await _serviceBase.RecuperarCCochesPorFechaServicioLinea(Fecha, Cod_Servicio, Cod_Linea);
        }

        public async Task<List<CCochesDto>> RecuperarCCoches(string FilterText)
        {
            return await _serviceBase.RecuperarCCoches(FilterText);
        }
    }
}
