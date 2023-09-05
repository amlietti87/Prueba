using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Entities.ART;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories.ART
{
    public interface IEstadosRepository: IRepositoryBase<ArtEstados, int>
    {
    }
}
