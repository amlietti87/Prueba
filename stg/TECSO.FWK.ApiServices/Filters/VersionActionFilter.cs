using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TECSO.FWK.ApiServices.Filters
{

    public class VersionActionFilter : IActionFilter
    {
        private readonly Version CurrentVersion;

        public VersionActionFilter(IConfiguration configuration) : base()
        {
            var version = configuration.GetValue<string>("Version");
            if (!string.IsNullOrWhiteSpace(version))
            {
                CurrentVersion = new Version(version);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (CurrentVersion != null)
            {
                var VersionHeader = context.HttpContext.Request.Headers["Version"];

                if (VersionHeader.Any())
                {
                    var VersionString = VersionHeader.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(VersionString))
                    {
                        var ClientVersion = new Version(VersionString);

                        if (CurrentVersion > ClientVersion)
                        {
                            //  context.HttpContext.Response.StatusCode = (int)HttpStatusCode.HttpVersionNotSupported;
                            context.Result = new ObjectResult("El cliente tiene diferente version con respecto a los servicios")
                            {
                                StatusCode = (int)HttpStatusCode.HttpVersionNotSupported,
                            };
                        }
                    }


                }
            }
            // do something before the action executes
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}
