using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class RamalColorDto : EntityDto<Int64>
    {

        public RamalColorDto()
        {
         
        }

        [StringLength(100)]
        public string Nombre { get; set; }

        public string NombreLinea
        {
            get; set;
        }

        public string NombreUN {
            get;set;
        }

        public int? UnidadDeNegocioId { get; set; }

        public Boolean Activo { get; set; }

        public long LineaId { get; set; }

        public override string Description => this.Nombre;

        public int? ColorTupid { get; set; }

        public string RouteLongName { get; set; }
        public string RouteShortName { get; set; }

    }

    //public partial class RamalSubeDto : EntityDto<Int64>
    //{ 
    //    public long RamalColorId { get; set; }
    //    public int EmpresaId { get; set; }
    //    public string CodigoSube { get; set; }
    //    public string EmpresaNombre { get; set; }

    //    public override string Description => this.CodigoSube;
    //}


}
