using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;

namespace ROSBUS.Admin.AppService
{

    public class FdDocumentosProcesadosHistoricoAppService : AppServiceBase<FdDocumentosProcesadosHistorico, FdDocumentosProcesadosHistoricoDto, long, IFdDocumentosProcesadosHistoricoService>, IFdDocumentosProcesadosHistoricoAppService
    {
        public FdDocumentosProcesadosHistoricoAppService(IFdDocumentosProcesadosHistoricoService serviceBase) 
            :base(serviceBase)
        {
         
        } 
    }
}
