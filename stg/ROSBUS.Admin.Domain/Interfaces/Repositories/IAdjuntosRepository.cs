using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IAdjuntosRepository : IRepositoryBase<Adjuntos, Guid>
    {
        Task<List<ItemDto<Guid>>> GetAdjuntosItemDto(AdjuntosFilter filter);
        Task<ItemDto<Guid>> GetAdjuntoItemDto(Guid id);
    }
}
