using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class PlaTalleresIvuDto :EntityDto<int>
    {
        public decimal CodGal { get; set; }
        public int CodGalIvu { get; set; }

        public GalponDto CodGalNavigation { get; set; }

        public override string Description => CodGalNavigation?.Nombre;


    }
}
