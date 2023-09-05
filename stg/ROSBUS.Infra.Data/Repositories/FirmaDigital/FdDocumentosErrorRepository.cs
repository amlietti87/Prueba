using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;
using System.Linq;
using TECSO.FWK.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using Microsoft.EntityFrameworkCore;

namespace ROSBUS.infra.Data.Repositories
{
    public class FdDocumentosErrorRepository : RepositoryBase<AdminContext,FdDocumentosError, long>, IFdDocumentosErrorRepository
    {

        public FdDocumentosErrorRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<FdDocumentosError, bool>> GetFilterById(long id)
        {
            return e => e.Id == id;
        }

        public void GuardarRevisado(FdDocumentosError doc)
        {
            doc.Revisado = true;
            this.Context.Entry(doc).State = EntityState.Modified;


            this.Context.SaveChanges();
        }
    }
}
