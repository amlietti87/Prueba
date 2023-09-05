using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class Configu
    {
        public decimal CodGru { get; set; }
        public decimal CodEmpr { get; set; }
        public decimal CodSuc { get; set; }
        public int CodSucCast { get; set; }
        
        public decimal CodLin { get; set; }
        public decimal CodGal { get; set; }
        public decimal CodSubg { get; set; }
        public decimal PlanCam { get; set; }
        public DateTime? FecBaja { get; set; }


        public Grupos Grupo { get; set; }
        public Empresa Empresa { get; set; }
        public Sucursales Sucursal { get; set; }
        public Linea Linea { get; set; }
        public Galpon Galpon { get; set; }
        public SubGalpon SubGalpon { get; set; }

        public PlanCam PlanCamNav { get; set; }
    }
}
