using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Filters.ComoLlegoBus;
using ROSBUS.Admin.Domain.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IBanderaAppService : IAppServiceBase<HBanderas, BanderaDto, int>
    {
        Task<String> RecuperarCartel(int idBandera);
        Task<List<ItemDto<int>>> RecuperarBanderasRelacionadasPorSector(BanderaFilter filtro);
        Task<HorariosPorSectorDto> RecuperarHorariosSectorPorSector(BanderaFilter filtro);
        Task<List<ItemDto>> RecuperarLineasActivasPorFecha(BanderaFilter filtro);
        Task<List<string>> OrigenPredictivo(BanderaFilter filtro);
        Task<List<string>> DestinoPredictivo(BanderaFilter filtro);
        Task<FileDto> GetReporteCambiosDeSector(BanderaFilter filter);
        Task<FileDto> GetReporteSabanaSinMinutos(HorariosPorSectorDto horarios);
        Task<List<ItemDto>> RecuperarBanderasPorServicio(BanderaFilter filtro);
        Task<FileDto> GetReporteSabanaConMinutos(HorariosPorSectorDto horarios);
        Task<List<BanderasLineasDto>> GetAllBanderasLineaAsync(ComoLlegoBusFilter filter);
    }
}
