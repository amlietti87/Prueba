using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model.AppInspectores
{
    public class PersTopesHorasExtrasDto : EntityDto<int>
    {

       
        public int? CodGalpon { get; set; }
        public int Hs50Persona { get; set; }
        public int Hs50Taller { get; set; }
        public int FrancosTrabajadosPersona { get; set; }
        public int FrancosTrabajadosTaller { get; set; }
        public int MinutosNocturnosPersona { get; set; }
        public int MinutosNocturnosTaller { get; set; }
        public int FeriadosPersona { get; set; }
        public int FeriadosTaller { get; set; }
        public DateTime? HoraFeriado { get; set; }
        public DateTime? HoraFranco { get; set; }
        public int? CodArea { get; set; }
        public DateTime? HorasComunes { get; set; }
        public int? IdGrupoInspectores { get; set; }
        public bool? PermiteHsExtrasFeriado { get; set; }
        public bool? PermiteFrancosTrabajadosFeriado { get; set; }


        public double? MinutosFeriados
        {
            get
            {
                if (this.HoraFeriado.HasValue)
                {
                    return this.HoraFeriado.Value.TimeOfDay.TotalMinutes;
                }
                return null;

            }
        }


        public double? MinutosFrancos
        {
            get
            {
                if (this.HoraFranco.HasValue)
                {
                    return this.HoraFranco.Value.TimeOfDay.TotalMinutes;
                }
                return null;

            }
        }

        public double? MinutosComunes
        {
            get
            {
                if (this.HorasComunes.HasValue)
                {
                    return this.HorasComunes.Value.TimeOfDay.TotalMinutes;
                }
                return null;

            }
        }

        public override string Description => "";
    }
}
