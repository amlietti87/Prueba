using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IHChoxserAppService : IAppServiceBase<HChoxser, HChoxserDto, int>
    {
        Task<ImportadorHChoxserResult> RecuperarPlanilla(HChoxserFilter filter);
        Task<List<HChoxserExtendedDto>> RecuperarDuraciones(HHorariosConfiFilter filter);
        Task ImportarDuraciones(HChoxserFilter input);
        Task UpdateDurYSer(HChoxserExtendedDto Duracion, HHorariosConfiDto Horario);
    }
}
