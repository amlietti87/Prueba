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
    public class FdDocumentosErrorService : ServiceBase<FdDocumentosError,long, IFdDocumentosErrorRepository>, IFdDocumentosErrorService
    { 
        public FdDocumentosErrorService(IFdDocumentosErrorRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }


        public void GuardarRevisado(FdDocumentosError doc)
        {
            this.repository.GuardarRevisado(doc);
        }
    }
    
}
