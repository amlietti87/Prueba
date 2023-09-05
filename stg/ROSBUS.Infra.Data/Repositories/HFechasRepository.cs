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
using ROSBUS.Admin.Domain.Entities.Filters;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TECSO.FWK.Domain;

namespace ROSBUS.infra.Data.Repositories
{
    public class HFechasRepository : RepositoryBase<AdminContext,HFechas, int>, IHFechasRepository
    {

        public HFechasRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<HFechas, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public virtual async Task<HFechas> GetHFechaAsync(HFechasFilter filter) 
        {
            try
            {
                IQueryable<HFechas> query = Context.Set<HFechas>().AsQueryable();

                foreach (var include in filter.GetIncludesForGetById())
                {
                    query = query.Include(include);
                }

                HFechas entity = await query.Where(f => f.Id == filter.RutaID)
                              .Where(f => f.CodTdia == filter.TipoDiaID)
                              .Where(f => f.Fecha <= DateTime.Today)
                              .OrderByDescending(f => f.Fecha).FirstOrDefaultAsync();

                if ((object)entity == null)
                    throw new EntityNotFoundException(typeof(HFechas), (object)filter.Id);
                return entity;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task<HFechas> RecuperarProximaFecha(int cod_linea, int idTipoDia, DateTime fecDesde)
        {
            return await Context.Set<HFechas>().Where(e => e.Id == cod_linea &&  e.CodTdia == idTipoDia && e.Fecha >= fecDesde).OrderBy(e => e.Fecha).FirstOrDefaultAsync();
        }


        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("UK_pla_distribucion_TipoDia", "No se puede generar la operación, porque ya existe el tipo de dia");
            return d;             
        }

    }
}
