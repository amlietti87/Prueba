using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class InfInformesService : ServiceBase<InfInformes, string, IInfInformesRepository>, IInfInformesService
    { 
        public InfInformesService(IInfInformesRepository repository)
            : base(repository)
        {
       
        }

        public Task<List<Destinatario>> GetNotificacionesMail(string token)
        {
            return this.repository.GetNotificacionesMail(token);
        }

        public Task<List<InformeConsulta>> ConsultaInformesUserDia()
        {
            return this.repository.ConsultaInformesUserDia();
        }

    }

}
