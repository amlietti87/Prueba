using ROSBUS.Admin.AppService.service;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class InspGruposInspectoresRangosHorariosService : ServiceBase<InspGrupoInspectoresRangosHorarios, int, IInspGruposInspectoresRangosHorariosRepository>, IInspGruposInspectoresRangosHorariosService
    {
        public InspGruposInspectoresRangosHorariosService(IInspGruposInspectoresRangosHorariosRepository produtoRepository)
            : base(produtoRepository)
        {

        }
    }
}
