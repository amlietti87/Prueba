using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class InspRangosHorarioDto : AuditedEntityDto<int>
    {
        public string Descripcion { get; set; }
        public bool Anulado { get; set; }
        public bool EsFrancoTrabajado { get; set; }
        public bool EsFranco { get; set; }
        public int? NovedadId { get; set; }
        public HoraDto HoraDesde { get; set; }
        public HoraDto HoraHasta { get; set; }
        public string Color { get; set; }
        public override string Description => this.HoraDesde.fecha.HasValue? this.HoraDesde.fecha.Value.ToString("HH:mm") +"-"+  this.HoraHasta.fecha.Value.ToString("HH:mm") : this.Descripcion;
    }


    public class HoraDto
    {
        public HoraDto()
        {

        }

        public HoraDto(DateTime? _fecha)
        {
            if (_fecha.HasValue)
            {
                fecha = _fecha.Value;
                second = _fecha.Value.Second;
                hour = _fecha.Value.Hour;
                minute = _fecha.Value.Minute;
            }
        }

        public int? hour { get; set; }
        public int? minute { get; set; }
        public int? second { get; set; }

        public DateTime? fecha { get; set; }



        public DateTime? getFechaCompleta()
        {
            if (this.hour == null || this.minute == null || this.second == null || this.fecha == null)
                {
                    return null;
                }

                return new DateTime(this.fecha.Value.Year, this.fecha.Value.Month, this.fecha.Value.Day, this.hour.Value, this.minute.Value, this.second.Value);
            }
        
    }
}
