using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TECSO.FWK.Domain.Interfaces;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace TECSO.FWK.Infra.Data.Transaction
{
    public class ResilientTransaction<context> : IResilientTransaction<context>
        where context : DbContext, IDbContext
    {
        private DbContext _context;

        public ResilientTransaction(IDbContext context) =>
            _context = (context as DbContext) ?? throw new ArgumentNullException(nameof(context));

        public static ResilientTransaction<context> New(context context) =>
            new ResilientTransaction<context>(context);


        public bool IsResilientTransaction()
        {
            return isResilientTransaction;
        }

        private bool isResilientTransaction { get; set; }

        public async Task ExecuteAsync(Func<Task> action)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                isResilientTransaction = true;
                await action();
                isResilientTransaction = false;
                await _context.SaveChangesAsync();
            });
        }
    }
}
