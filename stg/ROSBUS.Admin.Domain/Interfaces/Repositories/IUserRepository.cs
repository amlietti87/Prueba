using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepositoryBase<SysUsersAd,int>
    {
        Task<List<SysUsersRoles>> GetUserRoles(int id);
        Task<SysUsersAd> GetUser(string Username);
        Task<List<SysPermissions>> GetGrantedPermissionsAsync(int UserId);
        Task SetGrantedPermissionsAsync(int userId, List<string> grantedPermissionNames);
        Task<List<PlaLineasUsuarios>> GetUserLineasForEdit(int userId);
        Task SetUserLineasForEditAsync(int id, List<ItemDecimalDto> lineas);
        Task<bool> TieneDiagramaActivo(int userId);
        Task<List<ItemDto>> GetUserIntrabus(CredentialsIntrabusModel accessToken);
    }
}
