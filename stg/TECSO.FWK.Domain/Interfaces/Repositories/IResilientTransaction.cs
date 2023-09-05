using System;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace TECSO.FWK.Domain.Interfaces
{
    public interface IResilientTransaction<Context>
        where Context : IDbContext
    {
        Task ExecuteAsync(Func<Task> action);

        bool IsResilientTransaction();
    }
}
