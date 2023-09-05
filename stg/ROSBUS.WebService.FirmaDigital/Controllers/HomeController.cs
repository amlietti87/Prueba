using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ROSBUS.WebService.FirmaDigital.Controllers
{ 
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
