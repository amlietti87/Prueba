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
    public class InspGruposInspectoresTurnosService : ServiceBase<InspGruposInspectoresTurnos, int, IInspGruposInspectoresTurnosRepository>, IInspGruposInspectoresTurnosService
    {
        public InspGruposInspectoresTurnosService(IInspGruposInspectoresTurnosRepository turnoRepository)
            : base(turnoRepository)
        {

        }
    }
}
