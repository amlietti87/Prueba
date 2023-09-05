using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Interfaces;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace TECSO.FWK.Infra.Data.Interface
{
    public interface IDbContextProvider<out TContext> 
        where TContext : DbContext
    {
        TContext GetDbContext(); 
    }
}
