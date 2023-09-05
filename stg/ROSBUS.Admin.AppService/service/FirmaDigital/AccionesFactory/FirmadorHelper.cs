using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Constants;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Operaciones.Domain.Entities;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service.FirmaDigital
{
    public class FirmadorHelper: IFirmadorHelper
    {
        private readonly IPermissionAppService _permissionAppService;
        private readonly IAuthAppService _authAppService;
        private readonly IAuthService _authService;
        private readonly ISysParametersService _sysParametersService;
        private readonly IUserService _userService;
        private readonly IEmpleadosService _empleadosService;
        private readonly IFdDocumentosProcesadosService _docProcesadosService;
        private readonly IFdFirmadorAppService _fdFirmadorAppService;
        private readonly ILogger logger;

        public FirmadorHelper(
            IPermissionAppService permissionAppService, 
            IAuthAppService authAppService, 
            IAuthService authService, 
            ISysParametersService sysParametersService, 
            IUserService userService, 
            IEmpleadosService empleadosService,
            IFdDocumentosProcesadosService docProcesadosService, 
            IFdFirmadorAppService fdFirmadorAppService,
            ILogger _logger
            )

        {
            _permissionAppService = permissionAppService;
            _authAppService = authAppService;
            _authService = authService;
            _sysParametersService = sysParametersService;
            _userService = userService;
            _empleadosService = empleadosService;
            _docProcesadosService = docProcesadosService;
            _fdFirmadorAppService = fdFirmadorAppService;
            logger = _logger;
        }

        public async Task<FdFirmadorDto> RecuperarJNLP(AplicarAccioneDto dto)
        {
            // Constantes para reemplazar en el archivo JNLP
            string baseUrl = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.FD_BASEURL_FIRMADOR)).Items.FirstOrDefault().Value;
            string codeBase = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.FD_CODEBASE_FIRMADOR)).Items.FirstOrDefault().Value;
            string version = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.FDFirmadorVersion)).Items.FirstOrDefault().Value;

            string userFirmador = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.FD_USERNAME_FIRMADOR)).Items.FirstOrDefault().Value;
            string passFirmador = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.FD_PASSWORD_FIRMADOR)).Items.FirstOrDefault().Value;

            //string userFirmador = "admin";
            //string passFirmador = "admin";


            var User = this._userService.GetById(this._authService.GetCurretUserId());
            var empleado = this._empleadosService.GetAll(e => e.Id == User.EmpleadoId).Items.FirstOrDefault();

            if (empleado == null)
            {
                throw new ValidationException("No se puede firmar el documento. El usuario no posee empleado asociado.");
            }

            LoginOutput loginFirmador = null;
            try
            {
                loginFirmador = await _authAppService.Login(userFirmador, passFirmador, "");
            }
            catch (LoginValidationException lex)
            {
                throw new ValidationException("No se puede firmar el documento. " + lex.Message);
            }
            catch (Exception ex)
            {
                await logger.LogError(ex.ToString());
                throw new ValidationException("No se puede firmar el documento. Falta configurar parámetros con usuario y contraseña para Firmador.");
            }    

            if (loginFirmador == null)
            {
                throw new ValidationException("No se puede firmar el documento. Falta configurar parámetros con usuario y contraseña para Firmador.");
            }

            if (string.IsNullOrEmpty(loginFirmador.token))
            {
                throw new ValidationException("No se puede firmar el documento. Usuario y contraseña inválidos para Firmador");
            }

            var userAdFirmador = this._userService.GetAll(e => e.LogonName == loginFirmador.username).Items.FirstOrDefault();

            string[] permissions = await _permissionAppService.GetPermissionForUser(userAdFirmador.Id);

            //TODO: validar permisos
            if (!permissions.Any(e => e == "FirmaDigital.Firmador.FirmarDocumentos"))
            {
                throw new ValidationException("No se puede firmar el documento. El usuario Firmador no posee permiso Firmador.");
            }

            // Armado del archivo JNLP
            XmlDocument doc = new XmlDocument();
            string firmadorFilePath = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.FD_RUTA_PLANTILLA_JNLP)).Items.FirstOrDefault().Value;

            string firmador;
            using (var reader = new StreamReader(firmadorFilePath))
                firmador = reader.ReadToEnd();

            firmador = firmador.Replace("@SessionId", loginFirmador.token); //Reemplazar
            firmador = firmador.Replace("@IdUsuario", empleado.Cuil);
            firmador = firmador.Replace("@Version", version);
            firmador = firmador.Replace("@BaseUrl", baseUrl); // Reemplazar por la URL de la API donde le van a pegar a los endpoitns de GetMetadata, GetCertificado y GetDocumento
            firmador = firmador.Replace("@CodeBase", codeBase); // Reemplazar por la URL donde van a estar los .jar declarados en este firmador   

            doc.LoadXml(firmador);

            
            byte[] bytes = Encoding.Default.GetBytes(doc.OuterXml);

            


            var firmadorDto = await this.registrarPedidoFirma(dto, User, empleado, loginFirmador, doc.OuterXml);

            firmadorDto.file = bytes;

            return firmadorDto;
        }

       


        private async Task<FdFirmadorDto> registrarPedidoFirma(AplicarAccioneDto accionDto, SysUsersAd user, Empleados empleado, LoginOutput loginOutput, string xml)
        {
            string coordenadasEmpleado = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.FDCoordenadas_Empleado)).Items.FirstOrDefault().Value;
            string coordenadasEmpleador = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.FDCoordenadas_Empleador)).Items.FirstOrDefault().Value;

            string pathGetDescarga = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.FDPathGetDescarga)).Items.FirstOrDefault().Value;
            string pathPostSubida = (await _sysParametersService.GetAllAsync(e => e.Token == SysParametersTokens.FDPathPostSubida)).Items.FirstOrDefault().Value;


            FdFirmadorDto firmadorDto = new FdFirmadorDto();
            firmadorDto.SessionId = loginOutput.token;
            firmadorDto.CoordenadasEmpleado = coordenadasEmpleado;
            firmadorDto.CoordenadasEmpleador = coordenadasEmpleador;
            firmadorDto.PathGetDescarga = pathGetDescarga;
            firmadorDto.PathPostSubida = pathPostSubida;
            firmadorDto.UsuarioUserName = user.LogonName?.TrimEnd();
            firmadorDto.UsuarioApellido = empleado.Apellido?.TrimEnd();
            firmadorDto.AccionId = accionDto.AccionId.GetValueOrDefault();
            firmadorDto.UsuarioNombre = empleado.Nombre?.TrimEnd();
            firmadorDto.UsuarioId = empleado.Cuil?.TrimEnd();
            firmadorDto.UsuarioRol = accionDto.Empleador ? "empleador" : "empleado";
            firmadorDto.FdFirmadorDetalle = new List<FdFirmadorDetalleDto>();
            

            foreach (var item in accionDto.Documentos)
            {
                var docProcesado = _docProcesadosService.GetById(item);
                var det = new FdFirmadorDetalleDto();
                det.ArchivoIdEnviado = docProcesado.ArchivoId;
                det.DocumentoProcesadoId = item;
                det.EstadoId = docProcesado.EstadoId;
                firmadorDto.FdFirmadorDetalle.Add(det);
            }
            firmadorDto.FdFirmadorLog = new List<FdFirmadorLogDto>();
            firmadorDto.FdFirmadorLog.Add(new FdFirmadorLogDto()
            {
                DetalleLog = "JNLP enviado " + xml,
                FechaHora = DateTime.Now,
            });

            return await _fdFirmadorAppService.AddAsync(firmadorDto);
        }
    }
}
