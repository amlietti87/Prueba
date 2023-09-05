using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IHServiciosAppService : IAppServiceBase<HServicios, HServiciosDto, int>
    {
        Task<List<ItemDto>> RecuperarServiciosPorLinea(HServiciosFilter filtro);
        Task<List<ItemDto<string>>> RecuperarConductoresPorServicio(HServiciosFilter filtro);

        Task<List<ConductoresLegajoDto>> RecuperarConductores(HServiciosFilter filtro);

        Task<List<ItemDto<Decimal>>> RecuperarLineasPorConductor(HServiciosFilter filtro);
    }
}
