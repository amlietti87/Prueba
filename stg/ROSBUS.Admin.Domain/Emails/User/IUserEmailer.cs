using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ROSBUS.Admin.Domain
{
    public interface IUserEmailer
    {
        Task SendPasswordResetLinkAsync(SysUsersAd user, string link = null);

    }
}
