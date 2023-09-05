using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class AdjuntosService : ServiceBase<Adjuntos, Guid, IAdjuntosRepository>, IAdjuntosService
    { 
        public AdjuntosService(IAdjuntosRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public Task<List<ItemDto<Guid>>> GetAdjuntosItemDto(AdjuntosFilter filter)
        {
            return this.repository.GetAdjuntosItemDto(filter);
        }

        public Task<ItemDto<Guid>> GetAdjuntoItemDto(Guid Id)
        {
            return this.repository.GetAdjuntoItemDto(Id);
        }
    }
    
}
