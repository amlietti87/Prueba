using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class CoordenadasDto :EntityDto<int>
    {         
        public override string Description => string.Format("{0}-{1}", Calle1.Trim(), Calle2.Trim());

        public string Abreviacion { get; set; }
        public string CodigoNombre { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }

        [Required]
        public string Calle1 { get; set; }

        [Required]
        public string Calle2 { get; set; }
        public string Descripcion { get; set; }

        public string DescripcionCalle1 { get; set; }
        public string DescripcionCalle2 { get; set; }

        public bool? BeforeMigration { get; set; }        
        public int? LocalidadId { get; set; }
        public String Localidad { get; set; }

        public bool Anulado { get; set; }

        public string NumeroExternoIVU { get; set; }

    }
}
