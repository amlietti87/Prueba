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
    public interface IRutasAppService : IAppServiceBase<GpsRecorridos, RutasDto, int>
    {
        Task<RutasDto> AprobarRutaAsync(int id);

        Task<List<string>> ValidateAprobarRuta(int id);
        Task<List<string>> ValidateAprobarRuta(RutasDto dto);

        Task<bool> CanDeleteGalpon(GalponDto taller);
        Task<List<RutasDto>> GetRutas(RutasViewFilter filter);

        Task<List<ItemDto>> RecuperarHbasecPorLinea(int cod_lin);
        Task<PuntoBandasHorariasDto> MinutosPorSector(MinutosPorSectorFilter filter);
        Task<FileDto> MinutosPorSectorReporte(MinutosPorSectorFilter filter);
        Task<List<RutasDto>> GetRutasFiltradas(RutasFilteredFilter filter);
    }
}
