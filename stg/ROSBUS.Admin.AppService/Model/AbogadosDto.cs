using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class AbogadosDto :EntityDto<int>
    {
        [Required]
        [StringLength(250)]
        public string ApellidoNombre { get; set; }
        [StringLength(250)]
        public string Domicilio { get; set; }
        public int LocalidadId { get; set; }
        public String Localidad { get; set; }
        [StringLength(50)]
        public string Telefono { get; set; }
        [StringLength(50)]
        public string Celular { get; set; }
        [StringLength(250)]
        [EmailAddress]
        public string Email { get; set; }
        public bool Anulado { get; set; }

        public override string Description => ApellidoNombre;

    }
}
