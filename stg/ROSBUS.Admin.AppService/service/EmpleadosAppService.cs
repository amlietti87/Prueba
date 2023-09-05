using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Operaciones.AppService.Interface;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Operaciones.AppService
{

    public class EmpleadosAppService : AppServiceBase<Empleados, EmpleadosDto, int, IEmpleadosService>, IEmpleadosAppService
    {
        public EmpleadosAppService(IEmpleadosService serviceBase) 
            :base(serviceBase)
        {
        }

        public async override Task<List<ItemDto<int>>> GetItemsAsync<TFilter>(TFilter filter)
        {
            var list = await this.GetPagedListAsync(filter);
            var r = list.Items.Select(filter.GetItmDTO()).ToList();
            return r;
        }

        public async Task<bool> ExisteEmpleado(int id)
        {
            return await this._serviceBase.ExisteEmpleado(id);
        }

        public async Task<bool> ExisteLegajoEmpleado(int id)
        {
            return await this._serviceBase.ExisteLegajoEmpleado(id);
        }

        public async Task<usuario> ObtenerEmpleadoPorDNI(string NroDoc)
        {
            return await this._serviceBase.ObtenerEmpleadoPorDNI(NroDoc);
        }


    }
}
