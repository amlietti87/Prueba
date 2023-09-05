using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class TipoLineaService : ServiceBase<PlaTipoLinea, int, ITipoLineaRepository>, ITipoLineaService
    { 
        public TipoLineaService(ITipoLineaRepository repository)
            : base(repository)
        {
       
        }

        public async Task<List<PlaTipoLinea>> RecuperarTipoLineaPorSector(HDesignarFilter Filtro)
        {
            var tipoLineas = await repository.RecuperarTipoLineaPorSector(Filtro);
            return tipoLineas;
        }


    }
    
}
