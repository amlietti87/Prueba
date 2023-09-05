using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Model
{

    public partial class DetalleSalidaRelevosFilter
    {
        public Nullable<decimal> cod_lin { get; set; }
        public Nullable<int> cod_hfecha { get; set; }
        public Nullable<int> codTdia { get; set; }  
    }

    public partial class DetalleSalidaRelevos
    {
        public int cod_servicio { get; set; }
        public string num_ser { get; set; }
        public int cod_tdia { get; set; }
        public string LugarSale { get; set; }
        public string BanderaSale { get; set; }
        public Nullable<System.DateTime> Sale { get; set; }
        public Nullable<System.DateTime> Llega { get; set; }
        public bool TienePrimera { get; set; }
        public string LlegaSaleRelevo { get; set; }
        public string BanderaRelevo { get; set; }
        public Nullable<System.DateTime> SaleRelevo { get; set; }
        public Nullable<System.DateTime> LlegaRelevo { get; set; }
        public bool TieneRelevo { get; set; }
        public string LlegaSaleAuxiliar { get; set; }
        public string BanderaAuxiliar { get; set; }
        public Nullable<System.DateTime> SaleAuxiliar { get; set; }
        public Nullable<System.DateTime> LlegaAuxiliar { get; set; }
        public bool TieneAuxiliar { get; set; }
        public string LlegaSaleUltimo { get; set; }
        public string BanderaUltimo { get; set; }
    }
}
