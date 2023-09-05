using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class FdCertificadosDto :EntityDto<int>
    {
        public FdCertificadosDto()
        {
        }
        public int UsuarioId { get; set; }
        public Guid ArchivoId { get; set; }
        public DateTime FechaActivacion { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaRevocacion { get; set; }
        public string UsuarioNombre { get; set; }
        public string ArchivoNombre { get; set; }

        public override string Description => String.Empty;
    }



}
