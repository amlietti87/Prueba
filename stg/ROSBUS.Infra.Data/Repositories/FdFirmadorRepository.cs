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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TECSO.FWK.Caching;

namespace ROSBUS.infra.Data.Repositories
{
    public class FdFirmadorRepository : RepositoryBase<AdminContext,FdFirmador, long>, IFdFirmadorRepository
    {
        private readonly ICacheManager _cacheManager;
        public FdFirmadorRepository(IAdminDbContext _context, ICacheManager cacheManager)
            :base(new DbContextProvider<AdminContext>(_context))
        {
            _cacheManager = cacheManager;
        }
        
        public override Expression<Func<FdFirmador, bool>> GetFilterById(long id)
        {
            return e => e.Id == id;
        }

        public async Task<FdFirmador> GetFirmadorByToken(string token, int idRecibo)
        {


            var query = await  this.Context.FdFirmadores.Where(e => e.SessionId == token).AsNoTracking().FirstOrDefaultAsync();

            query.FdFirmadorDetalle = await this.Context.FdFirmadorDetalle.Where(e => e.DocumentoProcesadoId == idRecibo && e.FirmadorId == query.Id).ToListAsync();

            return query;


        }


        public async Task UpdateLogs(FdFirmador entity)
        {
            foreach (var item in entity.FdFirmadorLog.Where(e=> e.Id==0).Reverse())
            {
                this.Context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }

            await this.Context.SaveChangesAsync();
        }
    }
}
