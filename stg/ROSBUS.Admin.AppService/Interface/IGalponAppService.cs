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
    public interface IGalponAppService : IAppServiceBase<Galpon, GalponDto, Decimal>
    {
        Task SaveGalponPorUnidadDeNegocio(List<GalponDto> galponesDto);
        Task<List<RutasDto>> GetPuntosInicioFin(GalponFilter filter);
        Task UpdateRutasPorGalpon(GalponDto galpon);
        Task<bool> CanDeleteGalpon(GalponDto galpon);
    }
}
