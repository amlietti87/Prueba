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
    public class InspRespuestasIncognitosRepository : RepositoryBase<AdminContext, InspRespuestasIncognitos, int>, IInspRespuestasIncognitosRepository
    {

        public InspRespuestasIncognitosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspRespuestasIncognitos, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        //public override Task DeleteAsync(InspGruposInspectores entity)
        //{
        //    //TODO agregar asociaciones Turnos etc.
        //    var notificacion = this.Context.Notification.Where(n => n.Id == entity.NotificacionId.Value);
        //    var usuarios = this.Context.SysUsersAd.Where(u => u.GruposInspectoresId == entity.Id);

        //    if (notificacion.Count() != 0 || usuarios.Count() != 0)
        //    {
        //        throw new ValidationException("EL grupo de inspectores no se puede eliminar existen asociaciones");
        //    }
        //    return base.DeleteAsync(entity);
        //}

        public override async Task<InspRespuestasIncognitos> AddAsync(InspRespuestasIncognitos entity)
        {
            return await base.AddAsync(entity);
        }

        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("UK_insp_RespuestasIncognitos", "Descripción está repetida");
            return d;
        }
    }
}
