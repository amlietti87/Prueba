using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class LineaRosBusDto 
    {

        public LineaRosBusDto()
        {
            
        }

        [StringLength(100)]
        public string DesLin { get; set; }
        
        public int CodLin { get; set; }

    }


    
}
