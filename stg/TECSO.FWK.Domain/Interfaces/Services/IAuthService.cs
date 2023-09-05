using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TECSO.FWK.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        int GetCurretUserId();

        string GetCurretUserName();

        string GetCurretToken();

        string GetSessionID();
    
    }

    public interface IPermissionProvider
    {
        Task<string[]> GetPermissionForCurrentUser();
    }

}
