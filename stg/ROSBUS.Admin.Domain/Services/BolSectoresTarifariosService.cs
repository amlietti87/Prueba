using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Domain.Services;
using System.Threading.Tasks;
using TECSO.FWK.Domain;

namespace ROSBUS.Admin.Domain.Services
{
    public class BolSectoresTarifarioService : ServiceBase<BolSectoresTarifarios, int, IBolSectoresTarifariosRepository>, IBolSectoresTarifariosService
    {
        public BolSectoresTarifarioService(IBolSectoresTarifariosRepository repository)
            : base(repository)
        {

        }

        public async override Task DeleteAsync(int id)
        {
            var sector = await GetByIdAsync(id);

            if(sector.HSectores.Count != 0)
            {
                throw new DomainValidationException("Existen sectores asociados a este sector tarifario.");
            }

            await base.DeleteAsync(id);
        }
    }
}
