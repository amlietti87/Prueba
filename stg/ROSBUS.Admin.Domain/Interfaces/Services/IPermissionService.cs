﻿using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IPermissionService : IServiceBase<SysPermissions, long>
    {
        Task<string[]> GetPermissionForCurrentUser();
        Task<string[]> GetPermissionForUser(int id);
    }
}
