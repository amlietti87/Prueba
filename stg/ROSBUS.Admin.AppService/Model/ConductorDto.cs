using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class ConductorDto : EntityDto<int>
    {
        public override string Description => ApellidoNombre;

        public string ApellidoNombre { get; set; }
        public int? TipoDocId { get; set; }
        public string NroDoc { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? LocalidadId { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string NroLicencia { get; set; }


        public ItemDto selectLocalidades { get; set; }

    }
}
