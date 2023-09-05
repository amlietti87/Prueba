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
using ROSBUS.Admin.Domain.Entities.FirmaDigital;

namespace ROSBUS.WebService.FirmaDigital.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class FdDocumentosProcesadosHistoricoController : ManagerController<FdDocumentosProcesadosHistorico, long, FdDocumentosProcesadosHistoricoDto, FdDocumentosProcesadosHistoricoFilter, IFdDocumentosProcesadosHistoricoAppService>
    {
        public FdDocumentosProcesadosHistoricoController(IFdDocumentosProcesadosHistoricoAppService service)
            : base(service)
        {

        }


      

    }


 

}
