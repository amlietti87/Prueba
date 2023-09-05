using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IHChoxserService : IServiceBase<HChoxser, int>
    {
        Task<ImportadorHChoxserResult> RecuperarPlanilla(HChoxserFilter filter);
        Task<List<HChoxserExtendedDto>> RecuperarDuraciones(HHorariosConfiFilter filter);
        Task ImportarDuraciones(HChoxserFilter input);
        Task DeleteDuracionesServicio(int idServicio);
    }
}
