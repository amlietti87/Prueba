using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;

namespace ROSBUS.infra.Data.Repositories
{
    public class CroCroquisRepository : RepositoryBase<AdminContext, CroCroquis, int>, ICroCroquisRepository
    {

        public CroCroquisRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<CroCroquis, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public override async Task<CroCroquis> AddAsync(CroCroquis entity)
        {
            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {

                    DbSet<CroCroquis> dbSet = Context.Set<CroCroquis>();
                    var entry = await dbSet.AddAsync(entity);
                    await this.SaveChangesAsync();



                    if (entity.idSiniestro.HasValue)
                    {
                        var sin = await this.Context.SinSiniestros.Where(e => e.Id == entity.idSiniestro).FirstOrDefaultAsync();
                        sin.CroquiId = entry.Entity.Id;
                        this.Context.Entry(sin).State = EntityState.Modified;
                        await this.SaveChangesAsync();
                    }

                    ts.Commit();
                    return entry.Entity;

                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
        }
    }
}
