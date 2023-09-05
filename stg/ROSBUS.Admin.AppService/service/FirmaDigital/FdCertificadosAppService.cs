using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class FdCertificadosAppService : AppServiceBase<FdCertificados, FdCertificadosDto, int, IFdCertificadosService>, IFdCertificadosAppService
    {

        private readonly IAdjuntosAppService _adjuntosAppService;
        private readonly IDefaultEmailer _defaultEmailer;
        private readonly ISysParametersAppService _sysParametersAppService;
        private readonly IAuthService _authService;
        public FdCertificadosAppService(IFdCertificadosService serviceBase, IAdjuntosAppService adjuntosAppService, ISysParametersAppService sysParametersAppService, IDefaultEmailer defaultEmailer, IAuthService authService) 
            :base(serviceBase)
        {
            this._adjuntosAppService = adjuntosAppService;
            this._defaultEmailer = defaultEmailer;
            this._sysParametersAppService = sysParametersAppService;
            this._authService = authService;
        }

        public async Task<FileDto> downloadCertificate(FdCertificadosFilter filter)
        {
            FileDto fileDto = new FileDto();
            var certificadoDownload = (await this._adjuntosAppService.GetAllAsync(e => e.Id == filter.ArchivoId)).Items.FirstOrDefault();
            fileDto.ByteArray = certificadoDownload.Archivo;
            fileDto.FileDescription = "ArchivoFirmador";
            fileDto.FileName = certificadoDownload.Nombre;
            fileDto.FileType = "application/x-pkcs12";
            fileDto.ForceDownload = true;

            return fileDto;
        }

        public async Task<List<FdCertificadosDto>> HistorialCertificadosPorUsuario(FdCertificadosFilter fdCertificadosFilter)
        {
            
           var historial = await this._serviceBase.HistorialCertificadosPorUsuario(fdCertificadosFilter);

            var dtos = MapObject<List<FdCertificados>, List<FdCertificadosDto>>(historial);

            foreach (var item in dtos)
            {
                var arch = await _adjuntosAppService.GetByIdAsync(item.ArchivoId);
                item.ArchivoNombre = arch.Nombre;
            }



            return dtos;
        }

        public async Task<FdCertificadosDto> RevocarCertificado(FdCertificadosFilter filter)
        {
            var certificadorevocado = await this._serviceBase.RevocarCertificado(filter);

            var certificadorevocadoDTO = MapObject<FdCertificados, FdCertificadosDto>(certificadorevocado);
            
            return certificadorevocadoDTO;
        }

        public async Task<string> sendCertificateByEmail(FdCertificadosFilter filter)
        {
            var currentuserID = this._authService.GetCurretUserId();
            var empleadocuil = await this._serviceBase.GetEmployeeCuil(currentuserID);
            decimal filesize = 0;

            List<KeyValuePair<System.IO.Stream, string>> archivos = new List<KeyValuePair<System.IO.Stream, string>>();

            var archivo = await this._adjuntosAppService.GetByIdAsync(filter.ArchivoId);

            archivos.Add(new KeyValuePair<System.IO.Stream, string>(new System.IO.MemoryStream(archivo.Archivo), archivo.Nombre));

            foreach (var item in archivos)
            {
                filesize = filesize + item.Key.Length;
            }
            decimal sizeinmb = filesize / 1048576;

            var parameter = (await this._sysParametersAppService.GetAllAsync(e => e.Token == "MBMaximoMail")).Items.FirstOrDefault();

            if (parameter == null)
            {
                throw new ValidationException("No se encuentra parámetro MBMaximoMail");
            }
            else
            {
                decimal output;
                if (decimal.TryParse(parameter.Value, out output))
                {
                    if (sizeinmb <= output)
                    {
                        await this._defaultEmailer.SendDefaultAsync(filter.UserEmail, "Certificado empleado " + empleadocuil.ToString(), "Adjunto certificado empleado " + empleadocuil.ToString(), archivos);
                    }
                    else
                    {
                        throw new ValidationException(String.Format("Los archivos adjuntos pesan más de {0} MB, no se puede enviar correo", output));
                    }
                }
                else
                {
                    throw new ValidationException("El valor del parámetro MBMaximoMail no se puede convertir a decimal");
                }
            }

            return "OK";
        }
    }
}
