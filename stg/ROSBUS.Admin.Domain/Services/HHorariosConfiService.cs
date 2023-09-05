using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class HHorariosConfiService : ServiceBase<HHorariosConfi,int, IHHorariosConfiRepository>, IHHorariosConfiService
    { 
        public HHorariosConfiService(IHHorariosConfiRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public async Task<List<DetalleSalidaRelevos>> ReporteDetalleSalidasYRelevos(DetalleSalidaRelevosFilter filter)
        { 
            return await this.repository.ReporteDetalleSalidasYRelevos(filter);  
        }

        public async Task<ReportePasajeros> ReporteParadasPasajeros(ReportePasajerosFilter filter)
        {
            var rdc = await this.repository.ReporteParadasPasajeros(filter);
            return rdc;
        }

        public async Task<ReporteDistribucionCoches> ReporteDistribucionCoches(ReporteDistribucionCochesFilter filter)
        {
           var rdc =  await this.repository.ReporteDistribucionCoches(filter); 
           return rdc;
        }

        public async Task<ReporteHorarioPasajeros> ReporteHorarioPasajeros(ReporteHorarioPasajerosFilter filter)
        {
            var rdc = await this.repository.ReporteHorarioPasajeros(filter);
            return rdc;
        }
    }
    
}
