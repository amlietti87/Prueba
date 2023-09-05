using System.Collections.Generic;
using System.Threading.Tasks;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service
{
    public interface INotificationService : IServiceBase<Notification, int>
    {
        Task<List<Destinatario>> GetDestinatariosNotificacionesMail(int idNotificacion);
    }
}