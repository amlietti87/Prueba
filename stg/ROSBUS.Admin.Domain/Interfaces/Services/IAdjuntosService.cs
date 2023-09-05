using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IAdjuntosService : IServiceBase<Adjuntos, Guid>
    {
        Task<List<ItemDto<Guid>>> GetAdjuntosItemDto(AdjuntosFilter filter);
        Task<ItemDto<Guid>> GetAdjuntoItemDto(Guid Id);
    }
}
