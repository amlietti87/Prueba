using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ROSBUS
{
    public class RbusHub : Hub
    {
        public async Task JoinGroup(string group)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, group);
        }


    }
}