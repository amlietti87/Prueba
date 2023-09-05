using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IBanderaService : IServiceBase<HBanderas, int>
    {
        Task<String> RecuperarCartel(int idBandera);
        Task<HorariosPorSectorDto> RecuperarHorariosSectorPorSector(BanderaFilter filtro);
        Task<List<ItemDto<int>>> RecuperarBanderasRelacionadasPorSector(BanderaFilter filtro);
        Task<List<ItemDto>> RecuperarLineasActivasPorFecha(BanderaFilter filtro);

        Task<List<string>> OrigenPredictivo(BanderaFilter filtro);
        Task<List<string>> DestinoPredictivo(BanderaFilter filtro);
        Task<Linea> GetLinea(int idBandera);

        Task<List<HBanderas>> GetAllBanderasWithRamal();
        Task<List<ReporteCambiosPorSector>> GetReporteCambiosDeSector(BanderaFilter filter);
        Task<List<ItemDto>> RecuperarBanderasPorServicio(BanderaFilter filtro);
    }
}
