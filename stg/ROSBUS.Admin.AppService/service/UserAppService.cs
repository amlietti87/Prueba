using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using TECSO.FWK.Domain;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Url;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK;
using TECSO.FWK.Extensions;
using TECSO.FWK.Caching;
using System.Net;
using Newtonsoft.Json;
using ROSBUS.Admin.Domain.ParametersHelper;
using ROSBUS.Admin.Domain.Model;

namespace ROSBUS.Admin.AppService
{

    public class UserAppService : AppServiceBase<SysUsersAd, UserDto, int, IUserService>, IUserAppService
    {
        protected readonly IRoleService _roleService;
        protected readonly IPermissionService _PermissionService;
        private readonly IUserEmailer _userEmailer;
        public IAppUrlService AppUrlService { get; set; }

        private readonly ICacheManager cacheManager;
        private readonly ISysParametersService sysParametersService;
        private readonly IParametersHelper parametersHelper;
        private readonly IAuthService authService;

        public int CantidadIntentos { get; set; }

        public UserAppService(IUserService serviceBase,
            IRoleService roleService,
            IPermissionService permissionService,
            IUserEmailer userEmailer,
            IAppUrlService appUrlService,
            ICacheManager _cacheManager,
            ISysParametersService _sysParametersService,
            IParametersHelper _parametersHelper,
            IAuthService _authService
            )
            : base(serviceBase)
        {
            _roleService = roleService;
            _PermissionService = permissionService;
            _userEmailer = userEmailer;
            AppUrlService = appUrlService;
            cacheManager = _cacheManager;
            sysParametersService = _sysParametersService;
            parametersHelper = _parametersHelper;
            authService = _authService;
        }

        public async Task<List<UserRoleDto>> GetUserRoles(int id)
        {
            var rolesusuarios = await this._serviceBase.GetUserRoles(id);

            return await GetUserRoles(rolesusuarios);
        }

        private async Task<List<UserRoleDto>> GetUserRoles(List<SysUsersRoles> rolesusuarios)
        {
            var allrole = await this._roleService.GetAllAsync(new RoleFilter().GetFilterExpression());

            var result = new List<UserRoleDto>();

            foreach (var r in allrole.Items)
            {
                var ur = new UserRoleDto();
                ur.RoleId = r.Id;
                ur.RoleName = r.Name;
                ur.RoleDisplayName = r.DisplayName;
                ur.IsAssigned = rolesusuarios.Any(e => e.RoleId == r.Id);
                result.Add(ur);
            }

            return result;
        }

        public override async Task<UserDto> UpdateAsync(UserDto dto)
        {
            dto.UserRoles = dto.UserRoles.Where(e => e.IsAssigned).ToList();

            //Hay que validar solo si tiene el rol de inspector y si el idTurno != al id de turno anterior
            if (dto.EsInspector && dto.TurnoId!= dto.TurnoIdAnterior)
            {
                Boolean tieneDiagrama = await this._serviceBase.TieneDiagramaActivo(dto.Id);

                if (tieneDiagrama)
                    throw new DomainValidationException("El empleado tiene una diagramación con estado Borrador. No se puede cambiar");
            }

           


            return await base.UpdateAsync(dto);
        }

        public async Task<SysUsersAd> Login(string Username, string Password, string captcha, Boolean isWebSite = false)
        {
            var usuarioCache = cacheManager.GetCache(Username);
            Boolean requiereCaptcha = false;

            var cantIntentos = usuarioCache.GetOrDefault<string, int>(Username);

            var cantidadIntentosParam = parametersHelper.GetParameter<int>(ParametersHelper.CantidadIntentosLoginKey);

            //if (cantIntentos >= cantidadIntentosParam)
            //{
            //    requiereCaptcha = true;
            //}


            if (requiereCaptcha)
            {
                this.ValidarGoogleCaptcha(captcha, isWebSite);
            }


            var User = await this._serviceBase.Login(Username, Password);
            var isValid = true;
            if (User == null)
            {
                isValid = false;
            }
            else if (User.PermiteLoginManual.GetValueOrDefault())
            {
                var hp = new PasswordHasher<SysUsersAd>();

                var result = hp.VerifyHashedPassword(User, User.PasswordHash, Password);
                if (result != PasswordVerificationResult.Success)
                {
                    isValid = false;
                }
            }

            if (!isValid)
            {
                cantIntentos += 1;
                usuarioCache.Set(Username, cantIntentos);
                if (cantIntentos >= cantidadIntentosParam)
                {
                    throw new LoginValidationException("Usuario o contraseña incorrecta", LoginValidationException.Code_RequiredCaptcha);
                }
                else
                {
                    throw new LoginValidationException("Usuario o contraseña incorrecta", LoginValidationException.Code_UserPasswordInvalid);
                }

            }

            usuarioCache.Set(Username, 0);
            return User;
        }

        private void ValidarGoogleCaptcha(string captcha, Boolean isWebSite)
        {
            string secret = "6LdugHMUAAAAAFuY0tjte1VJYkbx1kpVzrBxXz4w";

            if (isWebSite)
            {
                secret = "6LcL3pUUAAAAAEzVNGX1uu_3wfug1N7022SSvyR9";
            }

            //
            string path = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var client = new WebClient();
            var reply = client.DownloadString(string.Format(path, secret, captcha));
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaGoogle>(reply);

            if (!captchaResponse.Success)
            {
                ///Si el captcha no es valido
                throw new LoginValidationException("Captcha Invalido", LoginValidationException.Code_InvalidCaptcha);

            }

        }

        private class CaptchaGoogle
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }

        public async Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input)
        {
            var user = await this.GetByIdAsync(input.UserId);
            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != input.ResetCode)
            {
                throw new ValidationException("Link expirado");
            }

            var _passwordHasher = new PasswordHasher<SysUsersAd>();

            user.PasswordHash = _passwordHasher.HashPassword(user, input.Password);
            user.PasswordResetCode = null;
            user.EmailConfirmed = true;
            //user.ShouldChangePasswordOnNextLogin = false;

            await this.UpdateAsync(user);

            return new ResetPasswordOutput
            {
                CanLogin = !user.IsDeleted,
                UserName = user.LogonName
            };
        }

        public async Task<SysUsersAd> GetByUserLdapForAdd(string Username)
        {

            SysUsersAd entity = (await _serviceBase.GetAllAsync(e => e.LogonName == Username && !e.IsDeleted)).Items.FirstOrDefault();
            if (entity == null)
            {
                return await this._serviceBase.GetByUserLdap(Username);

            }
            else
            {
                throw new ValidationException("El Usuario ya existe");
            }


        }
        public async Task<SysUsersAd> GetByUserLdap(string Username)
        {
            return await this._serviceBase.GetByUserLdap(Username);
        }

        public async Task<GetPermissionsForEditOutput> GetUserPermissionsForEdit(int id)
        {
            var permissions = await this._PermissionService.GetAllAsync((a) => true);
            var grantedPermissions = await this._serviceBase.GetGrantedPermissionsAsync(id);

            GetPermissionsForEditOutput result = GetPermissionsForEditOutput.GetPermissionsForEdit(permissions.Items, grantedPermissions);

            return result;
        }



        public async Task UpdateUserPermissions(UpdateUserPermissionsInput input)
        {
            if (input.GrantedPermissionNames == null)
            {
                throw new TecsoException("Falta lista de permisos");
            }
            await _serviceBase.SetGrantedPermissionsAsync(input.Id, input.GrantedPermissionNames);
        }

        public async Task<string[]> GetPermissionForCurrentUser()
        {
            var grantedPermissionsUser = await this._PermissionService.GetPermissionForCurrentUser();
            return grantedPermissionsUser;
        }


        public async override Task<UserDto> GetDtoByIdAsync(int id)
        {
            if (id > 0)
            {
                var dto = await base.GetDtoByIdAsync(id);
                dto.UserRoles = await this.GetUserRoles(id);
                return dto;
            }
            else
            {
                return await this.GetDefulatDtoAsync();
            }

        }

        public async Task<UserDto> GetDefulatDtoAsync()
        {
            var allrole = await this._roleService.GetAllAsync(new RoleFilter().GetFilterExpression());
            var roles = new List<UserRoleDto>();
            foreach (var r in allrole.Items)
            {
                var ur = new UserRoleDto();
                ur.RoleId = r.Id;
                ur.RoleName = r.Name;
                ur.RoleDisplayName = r.DisplayName;
                ur.IsAssigned = r.IsDefault;
                roles.Add(ur);
            }

            var newDto = new UserDto() { UserRoles = roles };
            return newDto;
        }

        public async override Task<UserDto> AddAsync(UserDto dto)
        {
            dto.UserRoles = dto.UserRoles.Where(e => e.IsAssigned).ToList();
            var user = MapObject<UserDto, SysUsersAd>(dto);

            user.PermiteLoginManual = true;

            var hp = new PasswordHasher<SysUsersAd>();

            if (string.IsNullOrEmpty(dto.Password))
            {
                dto.Password = dto.NroDoc;
            }
            user.NomUsuario = dto.CanonicalName;
            user.LogonName = dto.NroDoc;
            user.LogicalLogon = user.LogonName;

            user.PasswordHash = hp.HashPassword(user, dto.Password);
            user.EmailConfirmed = false;
            user.SetNewPasswordResetCode();

            var result = MapObject<SysUsersAd, UserDto>(await this.AddAsync(user));
            if (!user.Mail.IsNullOrEmpty())
            { 
                var emailActivationLink = AppUrlService.CreatePasswordResetUrlFormat();
                await _userEmailer.SendPasswordResetLinkAsync(user, emailActivationLink);
            }
            return result;

        }

        public async Task<String> ResetPassword(int id)
        {

            var user = await this.GetByIdAsync(id);

            if (user.PermiteLoginManual.GetValueOrDefault())
            {
                user.SetNewPasswordResetCode();
                await this.UpdateAsync(user);
                var emailActivationLink = AppUrlService.CreatePasswordResetUrlFormat();
                await _userEmailer.SendPasswordResetLinkAsync(user, emailActivationLink);
                return user.PasswordResetCode;
            }
            else
            {
                throw new ValidationException("El tipo usuario no permite restablecer contraseña");
            }


        }


        public async Task<GetUserLineasForEdit> GetUserLineasForEdit(int id)
        {

            //var usuario =   await this.GetByIdAsync(id);

            //var lineafilter = new LineaFilter();
            //lineafilter.SucursalId = usuario.SucursalId;
            //var lineas = this._lineaService.GetAllAsync(lineafilter.GetFilterExpression());

            var lineasusuarios = await this._serviceBase.GetUserLineasForEdit(id);

            var lineas = new List<ItemDecimalDto>();

            foreach (var item in lineasusuarios)
            {
                lineas.Add(new ItemDecimalDto(item.CodLin, item.CodLinNavigation.DesLin.Trim(), true));
            }

            return new GetUserLineasForEdit() { Id = id, Lineas = lineas };

        }

        public async Task SetUserLineasForEdit(GetUserLineasForEdit input)
        {
            await _serviceBase.SetUserLineasForEdit(input.Id, input.Lineas);
        }

        public async Task<List<ItemDto>> GetUserIntrabus(CredentialsIntrabusModel accessToken)
        {
            return await this._serviceBase.GetUserIntrabus(accessToken);
        }
    }
}
