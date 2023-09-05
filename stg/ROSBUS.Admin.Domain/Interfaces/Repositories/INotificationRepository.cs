using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces
{
    public interface INotificationRepository : IRepositoryBase<Notification, int>
    {
        Task<List<Destinatario>> GetDestinatariosNotificacionesMail(int idNotificacion);
    }
}
