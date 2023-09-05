using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ROSBUS.WebService.Schedules.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            //Hangfire
            return new RedirectResult("~/Hangfire");
            //return new RedirectResult("~/swagger");
        }
    }
}