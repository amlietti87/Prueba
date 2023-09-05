using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace TECSO.FWK.ApiServices
{
    public abstract class ControllerBase : Controller
    {
        protected readonly ILogger logger;
        protected readonly IAuthService authService;

        protected ControllerBase()
        {
            logger = ServiceProviderResolver.ServiceProvider.GetService<ILogger>();
            authService = ServiceProviderResolver.ServiceProvider.GetService<IAuthService>();
        }

        private IActionResult ReturnError<T>(String message, string stackTrace, ActionStatus status = ActionStatus.Error)
        {
            return this.ReturnError<T>(new List<string>() { message.ToString() } ,stackTrace, status);
        }

        private IActionResult ReturnError<T>(List<String> message,string stackTrace, ActionStatus status = ActionStatus.Error)
        {
            var userName = authService.GetCurretUserName();
            var sessionId = authService.GetSessionID();
            logger.Log(new LogDto()
            {
                LogDate =DateTime.Now,
                LogMessage =String.Join(",",message),
                LogType = LogType.Error,
                LogLevel = status ==  ActionStatus.Error ? Domain.Entities.LogLevel.Error : Domain.Entities.LogLevel.Warning,
                SessionId = sessionId,
                UserName = userName,
                StackTrace= stackTrace
            });
            return this.ReturnData<T>(default(T), status, message);
        }


        private Model.ResponseModel<T> ResponseModelError<T>(String message, string stackTrace, ActionStatus status = ActionStatus.Error)
        {
            return this.ResponseModelError<T>(new List<string>() { message.ToString() }, stackTrace, status);
        }


        private Model.ResponseModel<T> ResponseModelError<T>(List<String> message, string stackTrace, ActionStatus status = ActionStatus.Error)
        {
            var userName = authService.GetCurretUserName();
            var sessionId = authService.GetSessionID();
            logger.Log(new LogDto()
            {
                LogDate = DateTime.Now,
                LogMessage = String.Join(",", message),
                LogType = LogType.Error,
                LogLevel = status == ActionStatus.Error ? Domain.Entities.LogLevel.Error : Domain.Entities.LogLevel.Warning,
                SessionId = sessionId,
                UserName = userName,
                StackTrace = stackTrace
            });
            return this.ResponseModel<T>(default(T), status, message);
        }


        protected virtual async Task LogError(Exception ex)
        {
            var userName = authService.GetCurretUserName();
            var sessionId = authService.GetSessionID();
            var message = ex.Message;
            if (ex.InnerException!=null)
            {
                message += ex.InnerException.Message;
            }
            var stackTrace = ex.StackTrace;

            await logger.Log(new LogDto()
            {
                LogDate = DateTime.Now,
                LogMessage = message,
                LogType = LogType.Error,
                LogLevel = Domain.Entities.LogLevel.Error ,
                SessionId = sessionId,
                UserName = userName,
                StackTrace = stackTrace
            });
        }



        protected IActionResult ReturnError<T>(Exception ex)
        {
            
            if (ex is ValidationException)
            {
                return this.ReturnValidationError<T>(ex as ValidationException);
            }
            else if (ex is ConcurrencyException)
            {
                var cex = ex as ConcurrencyException;

                if (cex.Code==ConcurrencyException.ConcurrencyException_CurrentUser)
                {
                    return this.ReturnError<string>($"Tienes la entidad bloqueda ¿Deseas desbloquearla?", ex.StackTrace, ActionStatus.ConcurrencyValidator);
                }
                else 
                {
                    return this.ReturnError<string>($"La entidad esta bloqueada por el usuario:  {ex.Message}" , ex.StackTrace, ActionStatus.ValidationError);

                }
            }
            else if (ex is TecsoException)
            {
                return this.ReturnWarningError<T>(ex as TecsoException);
            }

            return this.ReturnError<string>(ex.Message, ex.StackTrace);
        }

        private IActionResult ReturnValidationError<T>(ValidationException ex)
        {
            return this.ReturnError<string>(ex.Message,ex.StackTrace, ActionStatus.ValidationError);
        }

        private IActionResult ReturnWarningError<T>(TecsoException ex)
        {
            return this.ReturnError<string>(ex.Message, ex.StackTrace, ActionStatus.Warning);
        }


        private Model.ResponseModel<T> ResponseModelValidationError<T>(ValidationException ex)
        {
            return this.ResponseModelError<T>(ex.Message, ex.StackTrace, ActionStatus.ValidationError);
        }

        private Model.ResponseModel<T> ResponseModelWarningError<T>(TecsoException ex)
        {
            return this.ResponseModelError<T>(ex.Message, ex.StackTrace, ActionStatus.Warning);
        }



        protected Model.ResponseModel<T> ResponseModel<T>(T objectData, ActionStatus status = ActionStatus.Ok, List<String> messages = null)
        {
            var objectReturn = new Model.ResponseModel<T>()
            {
                DataObject = objectData,
                Status = status.ToString(),
                Messages = messages ?? new List<string>()
            };

            if (status == ActionStatus.Error)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            if (status == ActionStatus.ValidationError)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            if (status == ActionStatus.Warning)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            
            return objectReturn;

        }


        protected IActionResult ReturnData<T>(T objectData, ActionStatus status = ActionStatus.Ok, List<String> messages = null)
        {
            var objectReturn = new Model.ResponseModel<T>()
            {
                DataObject = objectData,
                Status = status.ToString(),
                Messages = messages ?? new List<string>()
            };

            if (status== ActionStatus.Error)
            {
                return this.NotFound(objectReturn);
            }
            if (status == ActionStatus.ValidationError)
            {
                return this.NotFound(objectReturn);
            }
            if (status == ActionStatus.Warning)
            { 
                return this.NotFound(objectReturn);
            }
            //if (status == ActionStatus.OkAndConfirm)
            //{
            //    return this.NotFound(objectReturn);
            //}
            if (status == ActionStatus.ConcurrencyValidator)
            {
                return this.NotFound(objectReturn);
            }
            else
            {
                return Ok(objectReturn);
            }
            
        }
        protected IActionResult ReturnError<T>(ModelStateDictionary ModelState)
        {
            var messages = ModelState.Values.SelectMany(e => e.Errors.Select(GetMessagesModelState)).ToList();

            return  this.ReturnError<T>(messages, Newtonsoft.Json.JsonConvert.SerializeObject(ModelState), ActionStatus.ValidationError);
        }


        protected Model.ResponseModel<T> ResponseModelError<T>(Exception ex)
        {

            if (ex is ValidationException)
            {
                return this.ResponseModelValidationError<T>(ex as ValidationException);
            }
            else if (ex is TecsoException)
            {
                return this.ResponseModelWarningError<T>(ex as TecsoException);
            }

            return this.ResponseModelError<T>(ex.Message, ex.StackTrace);
        }

        protected Model.ResponseModel<T> ResponseModelError<T>(ModelStateDictionary ModelState)
        {
            var messages = ModelState.Values.SelectMany(e => e.Errors.Select(GetMessagesModelState)).ToList();

            return this.ResponseModelError<T>(messages, Newtonsoft.Json.JsonConvert.SerializeObject(ModelState), ActionStatus.ValidationError);
        }

        private string GetMessagesModelState(ModelError e)
        {
            return !string.IsNullOrEmpty(e.ErrorMessage) ? e.ErrorMessage : e.Exception?.Message;
        }
    }
}
