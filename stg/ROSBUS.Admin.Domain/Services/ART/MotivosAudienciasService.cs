using ROSBUS.Admin.Domain.Interfaces.Repositories.ART;
using ROSBUS.Admin.Domain.Interfaces.Services.ART;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;
using ROSBUS.Admin.Domain.Entities.ART;

namespace ROSBUS.Admin.Domain.Services.ART
{
    public class MotivosAudienciasService : ServiceBase<ArtMotivosAudiencias, int, IMotivosAudienciasRepository>, IMotivosAudienciasService
    {
        public MotivosAudienciasService(IMotivosAudienciasRepository repository) : base(repository)
        {
        }

    }
}
