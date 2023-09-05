using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Repositories;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;

namespace ROSBUS.infra.Data.Repositories
{
    public class LdapRepository : ILdapRepository
    {

        
        public LdapRepository()
            : base()
        {

        }

        public async Task<bool> Login(string Username, string Password)
        {
            try
            {
                //Username = "Administrator";
                //Password = "hclpdBANCO24";
                if (LDapUtils.LdapConfiguration.IsDevelopment)
                {
                    return await Task.FromResult<Boolean>(true);
                } 
                
                using (var pc = CreatePrincipalContext())
                {
                    var result = pc.ValidateCredentials(Username, Password, ContextOptions.Negotiate);
                    return await Task.FromResult<Boolean>(result); 
                }
            }
            
            catch (Exception)
            {
                throw;
            }
        }
        
      

        protected virtual PrincipalContext CreatePrincipalContext()
        {
            return new PrincipalContext(ContextType.Domain, 
                LDapUtils.LdapConfiguration?.GetFulldomain(), 
                LDapUtils.LdapConfiguration?.UserName, 
                LDapUtils.LdapConfiguration?.Password);
        }


        public async Task<SysUsersAd> GetByUserName(string Username)
        {
            try
            {
                UserPrincipal _userData = null;

                SysUsersAd user = null;

                using (var pc = new PrincipalContext(ContextType.Domain, LDapUtils.LdapConfiguration?.GetFulldomain(), LDapUtils.LdapConfiguration?.UserName, LDapUtils.LdapConfiguration?.Password))
                {
                    _userData = UserPrincipal.FindByIdentity(pc, Username);
                    if (_userData==null)
                    {
                        throw new ValidationException("Usuario Inexistente");
                    }
                    user = new SysUsersAd()
                    {
                        LogonName= Username,
                        DisplayName= _userData.DisplayName,
                        Mail= _userData.UserPrincipalName,
                        NomUsuario= _userData.GivenName,
                    };
                }

                return await Task.FromResult<SysUsersAd>(user);

            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
