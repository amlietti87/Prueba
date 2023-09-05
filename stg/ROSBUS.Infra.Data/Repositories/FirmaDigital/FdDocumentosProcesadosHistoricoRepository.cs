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
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class FdDocumentosProcesadosHistoricoRepository : RepositoryBase<AdminContext,FdDocumentosProcesadosHistorico, long>, IFdDocumentosProcesadosHistoricoRepository
    {

        public FdDocumentosProcesadosHistoricoRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<FdDocumentosProcesadosHistorico, bool>> GetFilterById(long id)
        {
            return e => e.Id == id;
        }
        protected override IQueryable<FdDocumentosProcesadosHistorico> AddIncludeForGet(DbSet<FdDocumentosProcesadosHistorico> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(d => d.Usuario);
        }
        protected override IQueryable<FdDocumentosProcesadosHistorico> GetIncludesForPageList(IQueryable<FdDocumentosProcesadosHistorico> query)
        {
            return base.GetIncludesForPageList(query)
                .Include(d => d.Usuario);
        }

    }
}
