using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class MinutosPorSectorDto
    {
        public MinutosPorSectorDto()
        {
            this.Minutos = new List<HMinxtipoDto>();
            this.Sectores = new List<HSectoresDto>();
        }

        public List<HMinxtipoDto> Minutos { get; set; }
        public List<HSectoresDto> Sectores { get; set; }
    }


    public class HMinxtipoDto : EntityDto<int>
    {
        public HMinxtipoDto()
        {
            this.HDetaminxtipo = new List<HDetaminxtipoDto>();
        }
        public int CodHfecha { get; set; }
        public int CodTdia { get; set; }
        public int CodBan { get; set; }
        public string TipoHora { get; set; }
        public string TipoHoraDesc { get; set; }
        public decimal TotalMin { get; set; }
        public decimal Suma { get; set; }
        public override string Description => string.Empty;
        public List<HDetaminxtipoDto> HDetaminxtipo { get; set; }
    }


    public class HDetaminxtipoDto
    {
        public int CodMinxtipo { get; set; }
        public decimal? Minuto { get; set; }
        public int CodHsector { get; set; }
        public int Orden { get; set; }

        public Boolean IsNew { get; set; }
    }

    public class HSectoresDto
    {
        public int CodSec { get; set; }
        public int CodHsector { get; set; }
        public int CodSectorTarifario { get; set; }
        public string DescripcionSectorTarifario { get; set; }
        public int Orden { get; set; }
        public bool VerEnResumen { get; set; }
        public bool? LlegaA { get; set; }
        public bool? SaleDe { get; set; }
        public string Calle1 { get; set; }
        public string Calle2 { get; set; }

    }


    

}
