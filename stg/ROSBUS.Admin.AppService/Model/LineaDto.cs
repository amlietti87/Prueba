using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class LineaDto : EntityDto<decimal>
    {

        public LineaDto()
        {
            this.PlaLineaLineaHoraria = new List<PlaLineaLineaHorariaDto>();
        }

        [StringLength(100)]
        public string DesLin { get; set; }
        
        public int? SucursalId { get; set; }

   
        public int? PlaGrupoLineaId { get; set; }

        public string Grupolinea { get; set; }

        public string UrbInter { get; set; }

        public string TipoLinea { get; set; }

        public override string Description => this.DesLin?.Trim();
        public string AsocBan { get; set; }

        public bool? Activo { get; set; }

        public string CodRespInformes { get; set; }


        public List<PlaLineaLineaHorariaDto> PlaLineaLineaHoraria { get; set; }

    }


    
}
