using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Interface.AppInspectores
{
    public interface IInsDesviosAppService : IAppServiceBase<InsDesvios, InsDesviosDto, long>
    {
        Task<usuario> ObtenerEmpleadoInspector();
        Task<List<InsDesviosDto>> GetDesviosPorUnidaddeNegocio();
    }
}
