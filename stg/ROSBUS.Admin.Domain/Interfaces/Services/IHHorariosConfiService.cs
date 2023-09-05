using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IHHorariosConfiService : IServiceBase<HHorariosConfi, int>
    {
        Task<ReporteDistribucionCoches> ReporteDistribucionCoches(ReporteDistribucionCochesFilter filter);
        Task<List<DetalleSalidaRelevos>> ReporteDetalleSalidasYRelevos(DetalleSalidaRelevosFilter filter);

        Task<ReporteHorarioPasajeros> ReporteHorarioPasajeros(ReporteHorarioPasajerosFilter filter);

        Task<ReportePasajeros> ReporteParadasPasajeros(ReportePasajerosFilter filter);

    }
}
