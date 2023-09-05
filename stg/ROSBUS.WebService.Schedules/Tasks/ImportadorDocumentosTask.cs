using Hangfire;
using Microsoft.Extensions.Configuration;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.ParametersHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ROSBUS.WebService.Schedules.Tasks
{
    public class ImportadorDocumentosTask
    {
        public IParametersHelper _parameters { get; }

        private readonly IFdDocumentosProcesadosAppService _fdDocumentosProcesadosAppService;

        public ImportadorDocumentosTask(IParametersHelper parameters, 
            IFdDocumentosProcesadosAppService fdDocumentosProcesadosAppService)
        {
            _parameters = parameters;
            _fdDocumentosProcesadosAppService = fdDocumentosProcesadosAppService;


        }

        public void Init()
        {

            //https://www.freeformatter.com/cron-expression-generator-quartz.html
            //En esta pagina podemos armar el cron pero genera dos parametros demas que son el primero y el ultimo corresponden a (seconds and years)

           var cron = _parameters.GetParameter<string>("ImportadorDocumentosCron");

            RecurringJob.RemoveIfExists("ImportadorDocumentosTask");
            
            RecurringJob.AddOrUpdate("ImportadorDocumentosTask", () => StartJob(), cron, TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time"));
        }


        [AutomaticRetry(Attempts = 0)]
        public async Task StartJob()
        {
            var result = await _fdDocumentosProcesadosAppService.ImportarDocumentos();
            if (!result)
            {
                throw new Exception("Error ImportadorDocumentosTask");
            }

        }
    }
}
