using Hangfire;
using Microsoft.Extensions.Configuration;
using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.Domain.ParametersHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ROSBUS.WebService.Schedules.Tasks
{
    public class ImportadorDenuniasTask
    {
        private readonly IDenunciasAppService _denunciasAppService;

        public IParametersHelper _parameters { get; }

        public ImportadorDenuniasTask(IDenunciasAppService denunciasAppService, IParametersHelper parameters)
        {
            _denunciasAppService = denunciasAppService;
            _parameters = parameters;
        }

        public void Init()
        {

            //https://www.freeformatter.com/cron-expression-generator-quartz.html
            //En esta pagina podemos armar el cron pero genera dos parametros demas que son el primero y el ultimo corresponden a (seconds and years)

           var cron = _parameters.GetParameter<string>("ImportadorDenuniasCron");

            RecurringJob.RemoveIfExists("ImportadorDenuniasTask");
            
            RecurringJob.AddOrUpdate("ImportadorDenuniasTask", () => StartJob(), cron, TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time"));
        }


        [AutomaticRetry(Attempts = 0)]
        public async Task StartJob()
        {
            var result = await _denunciasAppService.ImportWithTask();
            if (!result)
            {
                throw new Exception("Error ImportadorDenuniasTask");
            }

        }
    }
}
