using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Interfaces;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IOperacionesRBDbContext : IDbContext
    {
    }

    public interface IOperacionesRBDBResilientTransaction : IResilientTransaction<IOperacionesRBDbContext> 
    {
        
    }

}
