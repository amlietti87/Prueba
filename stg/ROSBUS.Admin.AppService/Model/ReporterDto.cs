using ROSBUS.Admin.Domain.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class ReporterDto : EntityDto<int>
    {

        public ReporterDto() { 

        }



        public override string Description => String.Empty;
    }



}
