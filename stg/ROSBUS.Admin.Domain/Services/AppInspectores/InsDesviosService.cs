using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces.Repositories.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces.Services.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services.AppInspectores
{
    public class InsDesviosService : ServiceBase<InsDesvios, long, IInsDesviosRepository>, IInsDesviosService
    {
        public InsDesviosService(IInsDesviosRepository repository) : base(repository)
        {
        }

    }

}
