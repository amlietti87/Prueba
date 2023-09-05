using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IHHorariosConfiRepository: IRepositoryBase<HHorariosConfi,int>
    {
        Task<ReporteDistribucionCoches> ReporteDistribucionCoches(ReporteDistribucionCochesFilter filter);
        Task<List<DetalleSalidaRelevos>> ReporteDetalleSalidasYRelevos(DetalleSalidaRelevosFilter filter);
        Task<ReporteHorarioPasajeros> ReporteHorarioPasajeros(ReporteHorarioPasajerosFilter filter);

        Task<ReportePasajeros> ReporteParadasPasajeros(ReportePasajerosFilter filter);
    }
}
