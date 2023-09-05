using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IHServiciosService : IServiceBase<HServicios, int>
    {
        Task<List<ItemDto<string>>> RecuperarConductoresPorServicio(HServiciosFilter filtro);
        Task<List<ItemDto>> RecuperarServiciosPorLinea(HServiciosFilter filtro);

        Task<List<ConductoresLegajoDto>> RecuperarConductores(HServiciosFilter filtro);

        Task<List<ItemDto<Decimal>>> RecuperarLineasPorConductor(HServiciosFilter filtro);
        Task RecrearMinutosPorSector(HHorariosConfi entity, HServicios servicioEntity, IEnumerable<int> MediasVueltasAActualizar, IEnumerable<int> MediasVueltasAEliminar, HChoxserExtendedDto Duracion);        
    }
}
