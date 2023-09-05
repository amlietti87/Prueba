using ROSBUS.Admin.AppService.service;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class NotificationService : ServiceBase<Notification, int, INotificationRepository>, INotificationService
    {
        public NotificationService(INotificationRepository produtoRepository)
            : base(produtoRepository)
        {

        }

        public Task<List<Destinatario>> GetDestinatariosNotificacionesMail(int idNotificacion)
        {
            return this.repository.GetDestinatariosNotificacionesMail(idNotificacion);
        }
    }
}
