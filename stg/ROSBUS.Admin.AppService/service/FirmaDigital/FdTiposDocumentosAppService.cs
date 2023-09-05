using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class FdTiposDocumentosAppService : AppServiceBase<FdTiposDocumentos, FdTiposDocumentosDto, int, IFdTiposDocumentosService>, IFdTiposDocumentosAppService
    {
        public FdTiposDocumentosAppService(IFdTiposDocumentosService serviceBase) 
            :base(serviceBase)
        {

        }

        public override async Task<FdTiposDocumentosDto> UpdateAsync(FdTiposDocumentosDto dto)
        {
            if (this._serviceBase.ExistExpression(e=> e.Id!=dto.Id && e.Prefijo.ToLower() == dto.Prefijo.ToLower()))
            {
                throw new ValidationException("Prefijo archivo está repetido.");
            }
            return await base.UpdateAsync(dto);
        }

        public override async Task<FdTiposDocumentosDto> AddOrUpdateAsync(FdTiposDocumentosDto dto)
        {
            if (this._serviceBase.ExistExpression(e => e.Id != dto.Id && e.Prefijo.ToLower() == dto.Prefijo.ToLower()))
            {
                throw new ValidationException("Prefijo archivo está repetido.");
            }
            return await base.AddOrUpdateAsync(dto);
        }

        public override async Task<FdTiposDocumentosDto> AddAsync(FdTiposDocumentosDto dto)
        {
            if (this._serviceBase.ExistExpression(e => e.Id != dto.Id && e.Prefijo.ToLower() == dto.Prefijo.ToLower()))
            {
                throw new ValidationException("Prefijo archivo está repetido.");
            }
            return await base.AddAsync(dto);
        }
    }
}
