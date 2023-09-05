using Microsoft.AspNetCore.Http;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IAdjuntosAppService : IAppServiceBase<Adjuntos, AdjuntosDto, Guid>
    {
        Task<List<AdjuntosDto>> AgregarAdjuntos(IFormFileCollection files);

        Task<AdjuntosDto> UploadOrUpdateFile(IFormFile file, Guid? Id);
    }
}
