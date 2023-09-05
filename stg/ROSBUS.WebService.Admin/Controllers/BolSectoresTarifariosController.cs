using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Admin.Controllers
{
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class BolSectoresTarifariosController : ManagerController<BolSectoresTarifarios, int, BolSectoresTarifariosDto, BolSectoresTarifariosFilter, IBolSectoresTarifariosAppService>
    {
        public BolSectoresTarifariosController(IBolSectoresTarifariosAppService service)
        : base(service)
            {

            }
        //public override Task<IActionResult> GetItemsAsync(BolSectoresTarifariosFilter filter)
        //{
        //    return base.GetItemsAsync(filter);
        //}

        public override Task<IActionResult> DeleteById(int Id)
        {
            return base.DeleteById(Id);
        }
    }


}
