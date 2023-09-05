using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Model
{
    public class EmpresaDto : EntityDto<Decimal>
    {
        public string DesEmpr { get; set; }
        public string DomEmpr { get; set; }
        public string PosEmpr { get; set; }
        public string TelEmpr { get; set; }
        public string ActEmpr { get; set; }
        public string DepEmpr { get; set; }
        public string Cuit { get; set; }
        public string Anses { get; set; }
        public decimal? Reducc { get; set; }
        public int? Art { get; set; }
        public int? Zona { get; set; }
        public int? CodActi { get; set; }
        public DateTime? FecBaja { get; set; }
        public string Cuenta { get; set; }
        public string CodIva { get; set; }
        public string Siglas { get; set; }
        public decimal? AlicuotaLRT { get; set; }
        public decimal? CuotaLRT { get; set; }
        public string NroIsib { get; set; }
        public string NroAgenteIb { get; set; }
        public string CtaDeposito { get; set; }
        public string IdSap { get; set; }
        public string CuentaSap { get; set; }
        public string NroSap { get; set; }


        public Boolean Baja
        {
            get
            {
                return this.FecBaja.HasValue;
            }
        }

        public override string Description => this.DesEmpr;
    }
}
