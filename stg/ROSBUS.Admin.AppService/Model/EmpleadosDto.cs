using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class EmpleadosDto :EntityDto<int>
    {
        public string Cuil { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }

        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Area { get; set; }
        public string Categoria { get; set; }
        public string Convenio { get; set; }
        public int? CodLinea { get; set; }
        public DateTime? FecVacaciones { get; set; }
        public DateTime? FecAntiguedad { get; set; }
        public bool? Jubilado { get; set; }
        public bool? GestionTiempoReal { get; set; }
        public string CalleDomicilio { get; set; }
        public string NroDomicilio { get; set; }
        public string BlockDomicilio { get; set; }
        public string PisoDomicilio { get; set; }
        public string DeptoDomicilio { get; set; }
        public int? CodLocalidad { get; set; }
        public DateTime? FecProcesado { get; set; }
        public string Pin { get; set; }
        public string CodObrasocial { get; set; }
        public string Obrasocial { get; set; }
        public DateTime? FecNacimiento { get; set; }
        public DateTime? FecProbJubilacion { get; set; }
        public int? AportesAntPrivilegiados { get; set; }
        public int? AportesAntSimples { get; set; }
        public string ConvColectivo { get; set; }
        public bool? IntimadoJubilarse { get; set; }
        public string ObsJubilacion { get; set; }
        public string IdLector { get; set; }
        public string Un { get; set; }
        public float? LatDomicilio { get; set; }
        public float? LonDomicilio { get; set; }
        public string Sexo { get; set; }
        public int? cod_sucursal { get; set; } // Ficticia

        public override string Description => string.Format("{0}, {1}", this.Nombre, this.Apellido);
    }
}
