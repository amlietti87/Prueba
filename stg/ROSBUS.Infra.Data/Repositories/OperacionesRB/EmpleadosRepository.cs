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
using TECSO.FWK.Domain.Interfaces.Entities;
using Snickler.EFCore;
using System.Data.SqlClient;
using ROSBUS.Operaciones.Domain.Entities;
using ROSBUS.Operaciones.Domain.Interfaces.Repositories;
using TECSO.FWK.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ROSBUS.Admin.Domain.Interfaces.Services;

namespace ROSBUS.infra.Data.Operaciones.Repositories
{
    public class EmpleadosRepository : RepositoryBase<OperacionesRBContext, Empleados, int>, IEmpleadosRepository
    {
        

        public EmpleadosRepository(IOperacionesRBDbContext _context)
            : base(new DbContextProvider<OperacionesRBContext>(_context))
        {
           
        }

        protected override IQueryable<Empleados> AddIncludeForGet(DbSet<Empleados> dbSet)
        {
            return base.AddIncludeForGet(dbSet).Include(e => e.UnidadNegocio);
        }

        protected override IQueryable<Empleados> GetIncludesForPageList(IQueryable<Empleados> query)
        {
            return base.GetIncludesForPageList(query).Include(e => e.UnidadNegocio);
        }
        public override Expression<Func<Empleados, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public override async Task<Empleados> AddAsync(Empleados entity)
        {
            return await base.AddAsync(entity);
        }

        public async Task<bool> ExisteEmpleado(int id)
        {
            var coches = await this.Context.Empleados.Where(e => e.Id == id).CountAsync();
            if (coches > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> ExisteLegajoEmpleado(int id)
        {
            var empleado = await this.Context.LegajosEmpleado.Where(e => e.Id == id && e.FecBaja == null).CountAsync();
            if (empleado > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public override async Task<PagedResult<Empleados>> GetPagedListAsync<TFilter>(TFilter filter)
        {
            try
            {
                filter = CompleteFilterPageList(filter);

                //DbSet<TEntity> dbSet = Context.Set<TEntity>();
                IQueryable<Empleados> query = Context.Set<Empleados>().Where(filter.GetFilterExpression()).AsQueryable();

                var total = await query.CountAsync();

                foreach (var include in filter.GetIncludesForPageList())
                {
                    query = query.Include(include);
                }

                query = query.OrderBy(e => e.Apellido).ThenBy(f => f.Nombre);


                if (!String.IsNullOrEmpty(filter.Sort))
                {
                    query = System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(query, filter.Sort);
                }

                //var total = query.Count();


                var Page = filter.Page.GetValueOrDefault();
                var PageSize = filter.PageSize.GetValueOrDefault();

                var list = query.Skip((Page - 1) * PageSize).Take(PageSize);

                return new PagedResult<Empleados>(total, await list.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task<usuario> ObtenerEmpleadoPorDNI(string NroDoc)
        {   
            List<usuario> empleadoInsp = new List<usuario>();

            var sp = this.Context.LoadStoredProc("dbo.sp_RecuperarCodEmpleadoporDNI")
                .WithSqlParam("DNI", new SqlParameter("DNI", NroDoc));

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                empleadoInsp = handler.ReadToList<usuario>().ToList();
            });           

            return empleadoInsp.FirstOrDefault();
        }
    }
}
