using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class HServiciosService : ServiceBase<HServicios,int, IHServiciosRepository>, IHServiciosService
    { 
        public HServiciosService(IHServiciosRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public Task RecrearMinutosPorSector(HHorariosConfi entity, HServicios servicioEntity, IEnumerable<int> MediasVueltasAActualizar, IEnumerable<int> MediasVueltasAEliminar, HChoxserExtendedDto Duracion = null)
        {
            return this.repository.RecrearMinutosPorSector(entity, servicioEntity, MediasVueltasAActualizar, MediasVueltasAEliminar, Duracion);
        }

        public async Task<List<ItemDto<string>>> RecuperarConductoresPorServicio(HServiciosFilter filtro)
        {
            return await this.repository.RecuperarConductoresPorServicio(filtro);
        }

        public async Task<List<ItemDto>> RecuperarServiciosPorLinea(HServiciosFilter filtro)
        {
            return await this.repository.RecuperarServiciosPorLinea( filtro);
        }


        public async Task<List<ConductoresLegajoDto>> RecuperarConductores(HServiciosFilter filtro)
        {
            return await this.repository.RecuperarConductores(filtro);
        }

        public async Task<List<ItemDto<Decimal>>> RecuperarLineasPorConductor(HServiciosFilter filtro)
        {
            return await this.repository.RecuperarLineasPorConductor(filtro);
        }
    }
    
}
