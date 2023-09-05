﻿using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspTopesGruposInspectoresDto : EntityDto<int>
    {
        public long? GrupoInspectoresId { get; set; }
        public int? TopeHsExtrasInspector { get; set; }
        public int? TopeHsExtrasGrupoInspector { get; set; }
        public int? TopeHsFeriadosInspector { get; set; }
        public int? TopeHsFeriadosGrupoInspector { get; set; }
        public bool? FeriadoPermiteHorasExtras { get; set; }
        public bool? FeriadoPermiteFrancoTrabajado { get; set; }
        public int? TopeHsFrancoTrabajadoInspector { get; set; }
        public int? TopeHsFrancoTrabajadoGrupoInspector { get; set; }
        public int? HorasPorTurno { get; set; }
        public override string Description => this.Description;
    }
}
