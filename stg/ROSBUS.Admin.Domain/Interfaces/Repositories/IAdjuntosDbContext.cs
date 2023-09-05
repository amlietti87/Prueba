﻿using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Interfaces;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IAdjuntosDbContext : IDbContext
    {
    }

    public interface IAdjuntosDBResilientTransaction : IResilientTransaction<IAdjuntosDbContext> 
    {
        
    }

}
