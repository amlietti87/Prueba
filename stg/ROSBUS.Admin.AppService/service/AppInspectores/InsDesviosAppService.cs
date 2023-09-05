using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Entities.Filters.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Interfaces.Services.AppInspectores;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service.AppInspectores
{
    public class InsDesviosAppService : AppServiceBase<InsDesvios, InsDesviosDto, long, IInsDesviosService>, IInsDesviosAppService
    {
        private readonly IUserService userService;
        private readonly IEmpleadosService empleadosService;
        private readonly IAuthService authService;
        public InsDesviosAppService(IInsDesviosService serviceBase, IUserService _userService, IEmpleadosService _empleadosService, IAuthService _authService) 
            : base(serviceBase)
        {
            empleadosService = _empleadosService;
            userService = _userService;
            authService = _authService;
        }

        public async Task<usuario> ObtenerEmpleadoInspector()
        {
            int userId = authService.GetCurretUserId();

            var usuario = await userService.GetByIdAsync(userId);

            var inspector = await empleadosService.ObtenerEmpleadoPorDNI(usuario.NroDoc.Replace(" ",""));
            
            if(inspector == null)
            {
                throw new ValidationException("Notifique a Sistemas que no existe el empleado asociado a su usuario.");
            }
            inspector.codSucursal = inspector.codSucursal.TrimStart('0');
            return inspector;

        }


        public async Task<List<InsDesviosDto>> GetDesviosPorUnidaddeNegocio()
        {
            int userId = authService.GetCurretUserId();

            var usuario = await userService.GetByIdAsync(userId);
            List<InsDesvios> desviosUser = new List<InsDesvios>();

            var inspector = await empleadosService.ObtenerEmpleadoPorDNI(usuario.NroDoc.Replace(" ", ""));
            if (inspector != null)
            {
                var desvios = await this._serviceBase.GetAllAsync(e => e.SucursalId == Convert.ToInt32(inspector.codSucursal) && !e.IsDeleted);
                desviosUser =  desvios.Items.ToList();
                desviosUser = desviosUser.OrderByDescending(d => d.FechaHora).ToList();
            }

            return MapList<InsDesvios, InsDesviosDto>(desviosUser).ToList();
           
        }


    }
}
