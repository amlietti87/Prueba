using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class BanderaDto : EntityDto<int>
    {
        //Para el alta de las banderas de posicionamiento solo se precisan los campos: abreviación, sentido, tipo de bandera, origen, destino y descripción(visual)
        public BanderaDto()
        {
            this.Rutas = new List<RutasDto>();
        }
        [Required]
        [StringLength(4)]
        public string Nombre { get; set; }
        public override string Description => this.Nombre;
        public long? RamalColorId { get; set; }
        public string RamalColorNombre { get; set; }

        public string AbrBan { get; set; }

        public long LineaId { get; set; }
        public string LineaNombre { get; set; }

        [StringLength(50)]
        public string CodigoVarianteLinea { get; set; }
        [Required]
        [StringLength(15)]
        public string Descripcion { get; set; }


        [StringLength(10)]
        public string Sentido { get; set; }
        public bool Cortado { get; set; }

        [StringLength(100)]
        public string Ramalero { get; set; }
        public bool Activo { get; set; }
        public List<RutasDto> Rutas { get; set; }
        [Required]
        public int TipoBanderaId { get; set; }

        [Required]
        public int? SucursalId { get; set; }


        public int UnidadDeNegocioId { get; set; }


        public int? SentidoBanderaId { get; set; }
        [Required]
        public string Origen { get; set; }
        [Required]
        public string Destino { get; set; }

        public string PorDonde { get; set; }
          
        public List<PlaCodigoSubeBanderaDto> PlaCodigoSubeBandera { get; set; }
        [StringLength(31)]
        public string DescripcionPasajeros { get; set; }

    }


    public partial class PlaCodigoSubeBanderaDto : EntityDto<long>
    {
        public int CodBan { get; set; }
        public decimal CodEmpresa { get; set; }
        public long CodigoSube { get; set; }
        public string Descripcion { get; set; }
        public string EmpresaNombre { get; set; }

        public int? SentidoBanderaSubeId { get; set; }

        public string SentidoBanderaSubeNombre { get; set; }

        public override string Description => CodigoSube.ToString();
    }


}
