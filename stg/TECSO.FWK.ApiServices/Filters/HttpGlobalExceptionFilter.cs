using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using TECSO.FWK.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TECSO.FWK.ApiServices.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment env;


        //public HttpGlobalExceptionFilter(IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        public HttpGlobalExceptionFilter(IHostingEnvironment env)
        {
            this.env = env;
        }

        public void OnException(ExceptionContext context)
        {

            var json = new JsonErrorResponse
            {
                Messages = new[] { "An error occurred. Try it again." }
            };

            if (env.IsDevelopment())
            {
                json.DeveloperMessage = context.Exception;
            }
            //TODO: quitar
            json.DeveloperMessage = context.Exception;


            context.Result = new JsonResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.ExceptionHandled = true;
        }
    }
    public class JsonErrorResponse
    {
        public string[] Messages { get; set; }

        public object DeveloperMessage { get; set; }
    }





    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ApiAuthorizeAttribute : AuthorizeAttribute, IAuthorizeAttribute
    {
        
        
    }

    public class PermissionsAuthorizeAttribute : TypeFilterAttribute
    {
        public PermissionsAuthorizeAttribute(params string[] Permissions) 
            : base(typeof(PermissionsRequirementFilter))
        {
            Arguments = new object[] { Permissions };
        }
    }



    public class PermissionsRequirementFilter : IAuthorizationFilter
    {
        public string[] _Permissions { get; set; }
        public IPermissionProvider _service { get; private set; }

        public PermissionsRequirementFilter(IPermissionProvider service , params string[] Permissions )
        {
            _Permissions = Permissions;
            this._service = service;
        }
            

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_Permissions != null && _Permissions.Any())
            { 
                String[] up = this._service.GetPermissionForCurrentUser().Result;

                if (!up.Any(a => _Permissions.Contains(a)))
                {
                    context.Result = new ForbidResult();
                }                
            }
        }
    }

    public class ActionAuthorizeAttribute : TypeFilterAttribute
    {
        public ActionAuthorizeAttribute()
            : base(typeof(ActionAuthorizeFilter))
        {

            //Arguments = new object[] { propertyName };
        }
    }
    

    public class ActionAuthorizeFilter: IActionFilter
    {

        public IPermissionProvider _service { get; private set; }

        public ActionAuthorizeFilter(IPermissionProvider permissionProvider)
        {
            _service = permissionProvider;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            ISecurityController controller = context.Controller as ISecurityController;

            if (controller!=null)
            {
                string action = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;

                var per = controller.PermissionContainer.Permissions.FirstOrDefault(e=> e.ActionName == action);

                String[] up = this._service.GetPermissionForCurrentUser().Result;

                if (!up.Any(a => per.PermissionName == a))
                {
                    context.Result = new ForbidResult();
                }

            }
        }
    }


    public class PermissionsByActionAuthorizeAttribute : TypeFilterAttribute
    {
        public PermissionsByActionAuthorizeAttribute(Type type, int newOrder = 1)
            : base(typeof(PermissionsByActionFilter))
        {
        
            this.Order = newOrder;
            //Arguments = new object[] { new[] { new PermissionType("GetPagedList", GetPagedListPermiso) } };
            IPermissionContainer permissionContainer = Activator.CreateInstance(type) as IPermissionContainer;

            Arguments = new object[] { permissionContainer };
        }
    }    

    public class PermissionsByActionFilter : IAuthorizationFilter
    {
        //public PermissionType[] _Permissions { get; set; }
        public IPermissionContainer permissionContainer { get; set; }
        public IPermissionProvider _service { get; private set; }

        public PermissionsByActionFilter(IPermissionContainer _permissionContainer, IPermissionProvider permissionProvider)
        {
            permissionContainer = _permissionContainer;
            this._service = permissionProvider;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (permissionContainer != null && permissionContainer.Permissions != null && permissionContainer.Permissions.Any())
            {
                //busco los permios del usuario
                String[] usuarioPermisos = this._service.GetPermissionForCurrentUser().Result;

                string action = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName; //context.RouteData.Route

                //busco el permiso para validar la accion
                var permiso = permissionContainer.Permissions.Where(e => e.ActionName == action).FirstOrDefault();

                //La accion esta dentro de la lista de acciones a revisar permiso
                if (permiso != null)
                {
                    //Verifico el el permiso este asignado al usuario
                    if (!usuarioPermisos.Any(a => permiso.PermissionName == a))
                    {
                        context.Result = new ForbidResult();
                    }
                }
            }
        }
    }
    

    public interface IAuthorizeAttribute
    { 
    
    }

    
}


 

