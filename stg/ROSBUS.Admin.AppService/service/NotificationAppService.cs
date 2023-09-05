using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;

namespace ROSBUS.Admin.AppService.service
{
    public class NotificationAppService : AppServiceBase <Notification, NotificationDto, int, INotificationService>, INotificationAppService
    {
        public NotificationAppService(INotificationService serviceBase)
             : base(serviceBase)
        {

        }

        public Task<List<Destinatario>> GetDestinatariosNotificacionesMail(int IdNotificacion)
        {
            return this._serviceBase.GetDestinatariosNotificacionesMail(IdNotificacion);
        }
    }
}
