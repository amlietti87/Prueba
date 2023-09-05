using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class UserService : ServiceBase<SysUsersAd, int, IUserRepository>, IUserService
    {
        ILdapRepository _ldap;

        public UserService(IUserRepository repository, ILdapRepository ldap)
            : base(repository)
        {
            _ldap = ldap;
        }

        public async Task<List<SysUsersRoles>> GetUserRoles(int id)
        {
            return await this.repository.GetUserRoles(id);
        }

        public async Task<SysUsersAd> Login(string Username, string Password)
        {
            var User = await this.repository.GetUser(Username);

            if (User != null)
            {
                if (User.PermiteLoginManual.GetValueOrDefault())
                {
                    return User;
                }

                if (await _ldap.Login(Username, Password))
                {
                    return User;
                } 
            }
            return null;
        }

        public async Task<SysUsersAd> GetByUserLdap(string Username)
        {
            return await _ldap.GetByUserName(Username);
        }

        public async Task<List<SysPermissions>> GetGrantedPermissionsAsync(int userId)
        {
            return await repository.GetGrantedPermissionsAsync(userId);
        }

        public async Task SetGrantedPermissionsAsync(int userId, List<string> grantedPermissionNames)
        {
            await repository.SetGrantedPermissionsAsync(userId, grantedPermissionNames);
        }



        protected async override Task ValidateEntity(SysUsersAd entity, SaveMode mode)
        {
            if (mode == SaveMode.Add)
            {
                 SysUsersAd existuser = this.repository.GetAll(e => e.LogonName == entity.LogonName && !e.IsDeleted).Items.FirstOrDefault();

                if (existuser != null)
                    throw new DomainValidationException("El Usuarioya existe");

            }


           

            await base.ValidateEntity(entity, mode);
        }
       
        public async Task<List<PlaLineasUsuarios>> GetUserLineasForEdit(int userId)
        {
           return await repository.GetUserLineasForEdit(userId);
        }

        public async Task SetUserLineasForEdit(int id, List<ItemDecimalDto> lineas)
        {
            await repository.SetUserLineasForEditAsync(id, lineas);
        }

        public async Task<bool> TieneDiagramaActivo(int userId)
        {
            return await repository.TieneDiagramaActivo(userId);
        }

        public async Task<List<ItemDto>> GetUserIntrabus(CredentialsIntrabusModel accessToken)
        {
            return await this.repository.GetUserIntrabus(accessToken);
        }
    }
}
