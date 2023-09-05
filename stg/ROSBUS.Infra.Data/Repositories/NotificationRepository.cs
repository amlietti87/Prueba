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
using ROSBUS.Admin.Domain.Entities.Partials;
using System.Data.SqlClient;
using Snickler.EFCore;

namespace ROSBUS.infra.Data.Repositories
{
    public class NotificationRepository : RepositoryBase<AdminContext, Notification, int>, INotificationRepository
    {

        public NotificationRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public async Task<List<Destinatario>> GetDestinatariosNotificacionesMail(int idNotificacion)
        {
            try
            {
                List<Destinatario> result = new List<Destinatario>();

                var Token = await this.Context.Notification.FirstOrDefaultAsync(e => e.Id == idNotificacion);

                var sp = this.Context.LoadStoredProc("dbo.GetNotificacionesMail")
                    .WithSqlParam("Token", new SqlParameter("Token", Token.Token));


                await sp.ExecuteStoredProcAsync((handler) =>
                {
                    result = handler.ReadToList<Destinatario>().ToList();
                });

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override Expression<Func<Notification, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

    }
}
