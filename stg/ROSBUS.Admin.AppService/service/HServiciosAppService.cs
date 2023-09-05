using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class HServiciosAppService : AppServiceBase<HServicios, HServiciosDto, int, IHServiciosService>, IHServiciosAppService
    {
        private readonly IHFechasConfiService _HFechasConfiService;
        private readonly IHHorariosConfiService _hHorariosConfiService;
        private readonly IHChoxserService _HChoxserService;

        public HServiciosAppService(IHServiciosService serviceBase, IHChoxserService hChoxserService, IHFechasConfiService hFechasConfiService, IHHorariosConfiService hHorariosConfiService) 
            :base(serviceBase)
        {
            this._HChoxserService = hChoxserService;
            this._HFechasConfiService = hFechasConfiService;
            this._hHorariosConfiService = hHorariosConfiService;
        }


        public async override Task DeleteAsync(int id)
        {
            if (this._HChoxserService.ExistExpression(e => e.Id == id))
            {
                throw new ValidationException("El servicio tiene una duracion planificada, elimine la duracion y vuelva a intentar");
            }

            HServicios servicios = await this.GetByIdAsync(id);
            HHorariosConfi hHorariosConfi= await this._hHorariosConfiService.GetByIdAsync(servicios.CodHconfi);
            

            if (_HFechasConfiService.ExistExpression(e => e.Id == hHorariosConfi.CodHfecha && e.PlaEstadoHorarioFechaId == PlaEstadoHorarioFecha.Aprobado))
            {
                if (await _HFechasConfiService.HorarioDiagramado(hHorariosConfi.CodHfecha, null, new List<int>() { id }))
                {
                    throw new ValidationException("El servicio ya fue diagramado");
                }
            }

            await base.DeleteAsync(id);
        }

        public async Task<List<ItemDto<string>>> RecuperarConductoresPorServicio(HServiciosFilter filtro)
        {
            return await this._serviceBase.RecuperarConductoresPorServicio(filtro);
        }

        public async Task<List<ItemDto>> RecuperarServiciosPorLinea(HServiciosFilter filtro)
        {
            return await this._serviceBase.RecuperarServiciosPorLinea(filtro);
        }

        public async Task<List<ConductoresLegajoDto>> RecuperarConductores(HServiciosFilter filtro)
        {
            return await this._serviceBase.RecuperarConductores(filtro);
        }

        public async Task<List<ItemDto<Decimal>>> RecuperarLineasPorConductor(HServiciosFilter filtro)
        {
            return await this._serviceBase.RecuperarLineasPorConductor(filtro);
        }



        public override Task<HServiciosDto> UpdateAsync(HServiciosDto dto)
        {
            return base.UpdateAsync(dto);
        }
    }
}
