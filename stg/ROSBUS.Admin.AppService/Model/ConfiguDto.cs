using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class ConfiguDto
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

        public GruposDto Grupo { get; set; }
        public EmpresaDto Empresa { get; set; }
        public sucursalesDto Sucursal { get; set; }
        public LineaDto Linea { get; set; }
        public GalponDto Galpon { get; set; }
       // public SubGalponDto SubGalpon { get; set; }

        public PlanCamDto PlanCamNav { get; set; }

        public string GrupoGrilla { get; set; }
        public string EmpresaGrilla { get; set; }
        public string SucursalGrilla { get; set; }
        public ItemDto selectLinea {
            get {
                if (Linea != null)
                {
                    return new ItemDto() { Id = (int)Linea.Id, Description = Linea.DesLin.Trim() };
                }
                else
                {
                    return null;
                }
            } }
        public string GalponGrilla { get; set; }
        public string PlanCamNavGrilla { get; set; }
    }
}
