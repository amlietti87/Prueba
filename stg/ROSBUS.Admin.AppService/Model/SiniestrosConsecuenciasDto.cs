using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class SiniestrosConsecuenciasDto :EntityDto<int>
    {
        public int SiniestroId { get; set; }

        public int ConsecuenciaId { get; set; }
        public string ConsecuenciaNombre { get; set; }
        public ConsecuenciasDto Consecuencia { get; set; }

        public string Observaciones { get; set; }

        public int? CategoriaId { get; set; }
        public string CategoriaNombre { get; set; }
        public CategoriasDto Categoria { get; set; }

        public override string Description => "err";

        public int? Cantidad { get; set; }

    }
}
