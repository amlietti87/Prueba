using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class BanderasLineasDto 
    {

        public BanderasLineasDto()
        {
            
        }

        public string descripcionPasajeros { get; set; }
        
        public int CodBandera { get; set; }

    }


    
}
