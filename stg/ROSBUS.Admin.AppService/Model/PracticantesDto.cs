using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class PracticantesDto :EntityDto<int>
    {
        [Required]
        [StringLength(250)]
        public string ApellidoNombre { get; set; }
        public int? TipoDocId { get; set; }
        [StringLength(100)]
        public string NroDoc { get; set; }
        [StringLength(50)]
        public string Celular { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int LocalidadId { get; set; }
        public String Localidad { get; set; }
        [StringLength(250)]
        public string Domicilio { get; set; }
        [StringLength(250)]
        public string Telefono { get; set; }

        public bool Anulado { get; set; }

        public TipoDniDto TipoDoc { get; set; }

        public override string Description => ApellidoNombre;
    }
}
