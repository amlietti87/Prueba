using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.service;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;

using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    [Authorize]
    public class NotificationController : ManagerController<Notification, int, NotificationDto, NotificationFilter, INotificationAppService>
    {


        public NotificationController(INotificationAppService service)
            :base (service)
        {

        }

        [HttpGet]
        public IActionResult GetUserNotifications()
        {
            try
            {
                var entity = new GetNotificationsOutput();

                for (int i = 0; i < 5; i++)
                {
                    entity.items.Add(new UserNotification()
                    {
                        id = i,
                        state = i,
                        Text = i.ToString(),
                        userId = 1


                    }
                        );
                }
                entity.unreadCount = 1;
                entity.totalCount = entity.items.Count();

                return ReturnData(entity);
            }
            catch (Exception ex)
            {
                return ReturnError<AppMenu>(ex);
            }
        }


        [HttpPost]
        public IActionResult SetNotificationAsRead(string id)
        {
            try
            { 
                return Ok();
            }
            catch (Exception ex)
            {
                return ReturnError<AppMenu>(ex);
            }
        }

    }




    public class UserNotification
    {

        public long userId { get; set; }
        public long state { get; set; }
        public long id { get; set; }
        public string Text { get; set; }



    }

    public class GetNotificationsOutput
    {
        public GetNotificationsOutput()
        {
            this.items = new List<UserNotification>();
        }
        public int unreadCount { get; set; }
        public int totalCount { get; set; }


        public List<UserNotification> items { get; set; }


    }

}