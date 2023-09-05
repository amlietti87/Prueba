using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Model
{
    public class CCochesDto : EntityDto<string>
    {

        public int Ficha { get; set; }
        public string Interno { get; set; }
        public DateTime FecIng { get; set; }
        public string Dominio { get; set; }
        public decimal Anio { get; set; }
        public string NroChasis { get; set; }
        public string NroMotor { get; set; }
        public string Marca { get; set; }
        public DateTime FecHab { get; set; }
        public string NroHab { get; set; }
        public decimal? Kilometraje { get; set; }
        public int CodGruTar { get; set; }
        public decimal CodEmpr { get; set; }
        public string Carroceria { get; set; }
        public string Titular { get; set; }
        public string Proveedor { get; set; }
        public int? CodTpoAsiento { get; set; }
        public int? CantAsientos { get; set; }
        public string AireAcondicionado { get; set; }
        public string Cortinas { get; set; }
        public string Gps { get; set; }
        public string Visible { get; set; }
        public int? AsientosHab { get; set; }
        public string RampaDiscapacitados { get; set; }
        public string InternoSap { get; set; }
        public string InternoAnterior { get; set; }
        public int? CodEmpresaSube { get; set; }
        public string NroInternoBa { get; set; }
        public string DesEmpresa { get; set; }
        public EmpresaDto Empresa { get; set; }
        public override string Description => string.Format("{0}", this.Ficha);
    }
}
