using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class PuntoBandasHorariasDto : EntityDto<int>
    {
        public PuntoBandasHorariasDto()
        {
            this.TipoDia = new Dictionary<int, string>();
            this.Franjas = new Dictionary<int, Tuple<TimeSpan, TimeSpan>>();
            this.Filas = new List<PuntoBandasHorariasRowDto>();
        }

        public override string Description
        {
            get
            {
                return null;
            }
        }

        public Dictionary<int, string> TipoDia { get; set; }
        public Dictionary<int, Tuple<TimeSpan, TimeSpan>> Franjas { get; set; }
        public List<PuntoBandasHorariasRowDto> Filas { get; set; }
    }

    public class PuntoBandasHorariasRowDto
    {
        public PuntoBandasHorariasRowDto()
        {
            this.Demoras = new List<DemoraBandasHorariasDto>();
        }


        public string PuntoInicio { get; set; }
        public string PuntoFin { get; set; }
        public decimal? Largo { get; set; }
        public List<DemoraBandasHorariasDto> Demoras { get; set; }
    }


    public class DemoraBandasHorariasDto
    {
        public int Franja { get; set; }
        public int TipoDia { get; set; }
        public TimeSpan Demora { get; set; }
    }

}
