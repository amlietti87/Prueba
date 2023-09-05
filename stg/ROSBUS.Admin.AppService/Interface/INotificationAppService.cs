using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface INotificationAppService : IAppServiceBase<Notification, NotificationDto, int>
    {
        Task<List<Destinatario>> GetDestinatariosNotificacionesMail(int IdNotificacion);
    }
}
