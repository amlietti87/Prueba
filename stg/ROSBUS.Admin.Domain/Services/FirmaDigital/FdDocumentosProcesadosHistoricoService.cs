using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;

namespace ROSBUS.Admin.Domain.Services
{
    public class FdDocumentosProcesadosHistoricoService : ServiceBase<FdDocumentosProcesadosHistorico,long, IFdDocumentosProcesadosHistoricoRepository>, IFdDocumentosProcesadosHistoricoService
    { 
        public FdDocumentosProcesadosHistoricoService(IFdDocumentosProcesadosHistoricoRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

    }
    
}
