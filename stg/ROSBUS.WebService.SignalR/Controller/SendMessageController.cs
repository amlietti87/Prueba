using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ROSBUS.WebService.SignalR.Model;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ROSBUS.WebService.SignalR
{
    [Route("api/[controller]/[action]")]
    public class SendMessageController : Controller
    {
        private readonly IHubContext<RbusHub> hubContext;

        public SendMessageController(IHubContext<RbusHub> _hubContext)
        {
            this.hubContext = _hubContext;
        }

        [HttpGet]
        public string Send(string message, string groupName)
        {
            MessageSignalR messageSignalR = new MessageSignalR();
            messageSignalR.id = Guid.NewGuid().ToString();
            messageSignalR.groupname = groupName;
            messageSignalR.message = message;


            this.hubContext.Clients.Group(groupName).SendAsync("Send",messageSignalR);
            return "Ok";
        }

        
    }
}
