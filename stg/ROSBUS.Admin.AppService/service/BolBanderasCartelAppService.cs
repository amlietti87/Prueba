using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.AppService
{

    public class BolBanderasCartelAppService : AppServiceBase<BolBanderasCartel, BolBanderasCartelDto, int, IBolBanderasCartelService>, IBolBanderasCartelAppService
    {
        public BolBanderasCartelAppService(IBolBanderasCartelService serviceBase, IPlaDistribucionDeCochesPorTipoDeDiaService plaDistribucionService, IHFechasConfiService _hFechasConfi)
            : base(serviceBase)
        {
            _plaDistribucionService = plaDistribucionService;
            hFechasConfi = _hFechasConfi;
        }

        public IPlaDistribucionDeCochesPorTipoDeDiaService _plaDistribucionService { get; }

        private readonly IHFechasConfiService hFechasConfi;

        public async Task<BolBanderasCartelDto> RecuperarCartelPorImportador(BolBanderasCartelFilter filter)
        {
            var entity = (await this._serviceBase.GetAllAsync(filter.GetFilterExpression(), filter.GetIncludesForPageList())).Items.FirstOrDefault();
            var hfecha = (await this.hFechasConfi.GetByIdAsync(filter.CodHfecha.GetValueOrDefault()));
            var dto = this.MapObject<BolBanderasCartel, BolBanderasCartelDto>(entity);
            if (dto == null)
            {
                dto = new BolBanderasCartelDto() { CodHfecha = filter.CodHfecha.Value, BolBanderasCartelDetalle = new List<BolBanderasCartelDetalleDto>(), Id = 0 };
                
            }
            dto.CodLinea = Convert.ToInt32(hfecha.CodLin);

            if (!filter.PlanillaId.IsNullOrEmpty())
            {
                var planilla = await _plaDistribucionService.RecuperarPlanilla(new PlaDistribucionDeCochesPorTipoDeDiaFilter() { CodHfecha = filter.CodHfecha, PlanillaId = filter.PlanillaId });
                var banderas = planilla.GroupBy(g => new { g.CodBan, g.DescripcionBandera });
                var banderasNuevas = banderas.Where(e => !dto.BolBanderasCartelDetalle.Any(a => e.Key.CodBan == a.CodBan)).ToList();
                foreach (var item in banderasNuevas)
                {
                    dto.BolBanderasCartelDetalle.Add(new BolBanderasCartelDetalleDto()
                    {
                        AbrBan = item.Key.DescripcionBandera,
                        CodBan = item.Key.CodBan,
                        CodBanderaCartel = dto.Id,
                        Movible = "N"
                    });
                }

                dto.BolBanderasCartelDetalle.ForEach(c =>
                {
                    c.EsPosicionamiento = planilla.Any(e => e.CodBan == c.CodBan && e.EsPosicionamiento);
                });
                
            }
            return dto;
        }
    }
}
