using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;
using Snickler.EFCore;
using TECSO.FWK.Domain.Interfaces.Entities;
using ROSBUS.Operaciones.Domain.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class FdCertificadosRepository : RepositoryBase<AdminContext, FdCertificados, int>, IFdCertificadosRepository
    {

        public FdCertificadosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<FdCertificados, bool>> GetFilterById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FdCertificados>> HistorialCertificadosPorUsuario(FdCertificadosFilter fdCertificadosFilter)
        {
            var lista = Context.FdCertificados.Include(e => e.Usuario).Where(e => e.UsuarioId == fdCertificadosFilter.UsuarioId);

            return await lista.OrderByDescending(e => e.FechaActivacion).ToListAsync();
        }

        public override async Task<FdCertificados> AddAsync(FdCertificados entity)
        {
            var oldCertificate = Context.FdCertificados.Where(e => e.UsuarioId == entity.UsuarioId && e.Activo == true).FirstOrDefault();

            if (oldCertificate != null)
            {
                oldCertificate.Activo = false;
                oldCertificate.FechaRevocacion = DateTime.Now;
                await base.UpdateAsync(oldCertificate);
            }
            
            return await base.AddAsync(entity);
        }

        public async Task<FdCertificados> RevocarCertificado(FdCertificadosFilter filter)
        {
            var certificatetorevoke = Context.FdCertificados.Where(e => e.UsuarioId == filter.UsuarioId && e.Activo == true).FirstOrDefault();

            if (certificatetorevoke != null)
            {
                certificatetorevoke.Activo = false;
                certificatetorevoke.FechaRevocacion = DateTime.Now;
                return await base.UpdateAsync(certificatetorevoke);
            }
            else 
            {
                throw new ValidationException("No se puede revocar. El usuario no tiene un certificado activo");
            }
        }

        public async Task<string> GetEmployeeCuil(int currentuserID)
        {
            List<ItemDto> empleado = new List<ItemDto>();

            var sp = this.Context.LoadStoredProc("dbo.[sp_FD_GetCuilEmpleadoByUserId]")
                                  .WithSqlParam("@UserId", currentuserID);

            

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                var res = handler.ReadToList<ItemDto>();

                empleado = res.ToList();
            });

            return empleado.FirstOrDefault().Description.ToString();
        }
    }
}
