using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IHHorariosConfiAppService : IAppServiceBase<HHorariosConfi, HHorariosConfiDto, int>
    {
        Task DeleteDuracionesServicio(int idServicio);
        Task<bool> UpdateCantidadCochesReales(HHorariosConfiDto dto);
        Task<FileDto> ReporteDistribucionCoches(ReporteDistribucionCochesFilter filter);
        Task<FileDto> ReporteDetalleSalidasYRelevos(DetalleSalidaRelevosFilter filter); 
        Task<FileDto> ReporteHorarioPasajeros(ReporteHorarioPasajerosFilter filter);
        Task<FileDto> ReporteParadasPasajeros(ReportePasajerosFilter filter);
        Task<HHorariosConfiDto> AddOrUpdateDurYSer(HHorariosConfiDto dto, HChoxserExtendedDto duracion);
    }
}
