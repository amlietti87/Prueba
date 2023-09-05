using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IHServiciosRepository : IRepositoryBase<HServicios, int>
    {
        Task<List<ItemDto<string>>> RecuperarConductoresPorServicio(HServiciosFilter filtro);
        Task<List<ItemDto>> RecuperarServiciosPorLinea(HServiciosFilter filtro);

        Task<List<ConductoresLegajoDto>> RecuperarConductores(HServiciosFilter filtro);

        Task<List<ItemDto<Decimal>>> RecuperarLineasPorConductor(HServiciosFilter filtro);
        Task RecrearMinutosPorSector(HHorariosConfi entity, HServicios servicioEntity, IEnumerable<int> MediasVueltasAActualizar, IEnumerable<int> MediasVueltasAEliminar, HChoxserExtendedDto Duracion);

        Task RecrearMinutosPorSectorTemplete(int CodHfecha, int CodTdia);

        Task CrearSectores(int CodHfecha, List<int> CodBan);

        Task RecrearSabanaSector(int CodHfecha, int CodTdia, IEnumerable<int> MediasVueltasAActualizar = null);
    }
}
