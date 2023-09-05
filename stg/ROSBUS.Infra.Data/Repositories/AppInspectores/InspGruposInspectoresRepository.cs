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
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System.ComponentModel.DataAnnotations;

namespace ROSBUS.infra.Data.Repositories
{
    public class InspGruposInspectoresRepository : RepositoryBase<AdminContext, InspGruposInspectores, long>, IInspGruposInspectoresRepository
    {

        public InspGruposInspectoresRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspGruposInspectores, bool>> GetFilterById(long id)
        {
            return e => e.Id == id;
        }


        protected override IQueryable<InspGruposInspectores> AddIncludeForGet(DbSet<InspGruposInspectores> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e=> e.InspGrupoInspectoresZona).ThenInclude(giz=>giz.Zona)
                .Include(e=>e.InspGrupoInspectoresRangosHorarios).ThenInclude(git => git.RangoHorario)
                .Include(e=>e.InspGruposInspectoresTurnos).ThenInclude(t => t.Turno)
                .Include(e => e.InspGrupoInspectoresTareas).ThenInclude(git => git.Tarea)
                .Include(e=> e.Linea);
        }


        public override Task DeleteAsync(InspGruposInspectores entity)
        {            
            var notificacion =entity.NotificacionId.HasValue ? this.Context.Notification.Where(n => n.Id == entity.NotificacionId.Value) : null;
            var linea =entity.LineaId.HasValue ? this.Context.Linea.Where(n => n.Id == entity.LineaId.Value) : null;
            var usuarios = this.Context.SysUsersAd.Where(u => u.GruposInspectoresId == entity.Id);
            var rangosHorarios = this.Context.InspGrupoInspectoresRangosHorarios.Where(t => t.GrupoInspectoresId == entity.Id);
            var turnos = this.Context.InspGruposInspectoresTurnos.Where(t => t.GrupoInspectoresId == entity.Id);
            var zonas = this.Context.InspGrupoInspectoresZona.Where(z => z.GrupoInspectoresId == entity.Id);
            var tipostarea = this.Context.InspGrupoInspectoresTareas.Where(tt => tt.GrupoInspectoresId == entity.Id);

            if (notificacion != null)
            {
                if (linea.Count() != 0 || notificacion.Count() != 0 || usuarios.Count() != 0 || rangosHorarios.Count() != 0 || turnos.Count() != 0 || zonas.Count() != 0 || tipostarea.Count() != 0)
                    throw new ValidationException("EL grupo de inspectores no se puede eliminar existen asociaciones");
            }
            else if(linea !=null)
            {
                if (linea.Count() != 0 || usuarios.Count() != 0 || rangosHorarios.Count() != 0 || turnos.Count() != 0 || zonas.Count() != 0 || tipostarea.Count() != 0)
                    throw new ValidationException("EL grupo de inspectores no se puede eliminar existen asociaciones");
            }


            return base.DeleteAsync(entity);
        }

        public override async Task<InspGruposInspectores> AddAsync(InspGruposInspectores entity)
        {
            DbSet<InspGruposInspectores> dbSet = Context.Set<InspGruposInspectores>();
            entity.Descripcion = entity.Descripcion.ToUpper();

            var entry = await dbSet.AddAsync(entity);

            var igiDescripcion = dbSet.Where(t => t.Descripcion == entity.Descripcion);

            if (igiDescripcion.Count() == 0)
            {
                return await base.AddAsync(entity);
            }
            else
            {
                throw new ValidationException(" Ya existe el grupo de Inspectores con la descripción: " + entity.Descripcion);
            }

           
        }


    }
}
