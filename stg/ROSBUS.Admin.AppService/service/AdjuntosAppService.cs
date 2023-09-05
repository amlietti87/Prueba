using Microsoft.AspNetCore.Http;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class AdjuntosAppService : AppServiceBase<Adjuntos, AdjuntosDto, Guid, IAdjuntosService>, IAdjuntosAppService
    {
        public AdjuntosAppService(IAdjuntosService serviceBase)
            : base(serviceBase)
        {

        }

        public async Task<List<AdjuntosDto>> AgregarAdjuntos(IFormFileCollection files)
        {
            List<AdjuntosDto> result = new List<AdjuntosDto>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        formFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();

                        var adjuntos = new Adjuntos();
                        adjuntos.Archivo = fileBytes;
                        adjuntos.Nombre = formFile.FileName;
                        adjuntos.Id = Guid.NewGuid();
                        await this._serviceBase.AddAsync(adjuntos);

                        result.Add(new AdjuntosDto() { Id = adjuntos.Id, Nombre = adjuntos.Nombre });
                    }

                }
            }


            return result;
        }


        public async Task<AdjuntosDto> UploadOrUpdateFile(IFormFile file, Guid? Id)
        {
            AdjuntosDto result = new AdjuntosDto();

            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    var adjuntos = new Adjuntos();
                    adjuntos.Archivo = fileBytes;
                    adjuntos.Nombre = file.FileName;
                    if (!Id.HasValue)
                    {
                        adjuntos.Id = Guid.NewGuid();
                        await this._serviceBase.AddAsync(adjuntos);
                        result = new AdjuntosDto() { Id = adjuntos.Id, Nombre = adjuntos.Nombre };
                    }
                    else
                    {
                        adjuntos.Id = Id.Value;
                       await this._serviceBase.UpdateAsync(adjuntos);
                        result = new AdjuntosDto() { Id = adjuntos.Id, Nombre = adjuntos.Nombre };
                    }

                }

            }



            return result;
        }
    }
}
