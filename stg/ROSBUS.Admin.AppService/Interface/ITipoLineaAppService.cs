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
    public interface ITipoLineaAppService : IAppServiceBase<PlaTipoLinea, TipoLineaDto, int>
    {
        Task<List<PlaTipoLinea>> RecuperarTipoLineaPorSector(HDesignarFilter Filtro);
    }
}
