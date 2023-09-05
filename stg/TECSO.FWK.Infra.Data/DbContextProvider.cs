using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Interfaces;
using TECSO.FWK.Domain.Interfaces.Repositories;
using TECSO.FWK.Infra.Data.Interface;

namespace TECSO.FWK.Infra.Data
{
    public class DbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext, IDbContext
    {

        private IDbContext context;

        public DbContextProvider(IDbContext _context)
        {
            context = _context;
        }

        public TDbContext GetDbContext()
        {
            var c = context as TDbContext; 
            return c;
        } 
    }
}
