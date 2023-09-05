using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
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

namespace ROSBUS.infra.Data.Repositories
{
    public class FdTiposDocumentosRepository : RepositoryBase<AdminContext, FdTiposDocumentos, int>, IFdTiposDocumentosRepository
    {
        public IFdAccionesRepository _FdAccionesRepository;
        public FdTiposDocumentosRepository(IAdminDbContext _context, IFdAccionesRepository FdAccionesRepository)
            : base(new DbContextProvider<AdminContext>(_context))
        {
            _FdAccionesRepository = FdAccionesRepository;
        }

        public override Expression<Func<FdTiposDocumentos, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public override async Task<FdTiposDocumentos> AddAsync(FdTiposDocumentos entity)
        {
            try
            {
                DbSet<FdTiposDocumentos> dbSet = Context.Set<FdTiposDocumentos>();
                entity.Prefijo = entity.Prefijo.ToUpper();

                var entry = await dbSet.AddAsync(entity);

               
                var tdprefijo = dbSet.Where(t => t.Prefijo == entity.Prefijo);
               
                if (tdprefijo.Count() == 0)
                {
                    var tdp = this.Context.FdTiposDocumentos.Where(e => e.EsPredeterminado == true).FirstOrDefault();
                    var atdp = this.Context.FdAcciones.Where(e => e.TipoDocumentoId == tdp.Id);

                    foreach (var ac in atdp)
                    {
                        var x = new FdAcciones();
                        x.AccionBdempleador = ac.AccionBdempleador;
                        x.AccionPermitidaId = ac.AccionPermitidaId;
                        x.EstadoActualId = ac.EstadoActualId;
                        x.EstadoNuevoId = ac.EstadoNuevoId;
                        x.GeneraNotificacion = ac.GeneraNotificacion;
                        x.MostrarBdempleado = ac.MostrarBdempleado;
                        x.MostrarBdempleador = ac.MostrarBdempleador;
                        x.NotificacionId = ac.NotificacionId;
                        x.EsFin = ac.EsFin;
                        x.TipoDocumentoId = entity.Id;
                        x.PermiteMarcarLote = ac.PermiteMarcarLote;
                        x.MenorFechaPrimero = ac.MenorFechaPrimero;

                        entity.FdAcciones.Add(x);
                    }
                    if (entity.EsPredeterminado)
                    {
                        tdp.EsPredeterminado = false;
                        Context.FdTiposDocumentos.Update(tdp);
                    }
                    await this.SaveChangesAsync();
                    return entry.Entity;
                }
                else
                {
                    throw new ValidationException(" Ya existe el tipo de documento con el Prefijo: " + entity.Prefijo);
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
            
        }

        public override async Task<FdTiposDocumentos> UpdateAsync(FdTiposDocumentos entity)
        {
            try
            {
                var tdPred = this.Context.FdTiposDocumentos.Where(e => e.EsPredeterminado == true).FirstOrDefault();

                if (entity.EsPredeterminado && entity.Id != tdPred.Id)
                {
                    tdPred.EsPredeterminado = false;
                    this.Context.FdTiposDocumentos.Update(tdPred);
                }

                return await base.UpdateAsync(entity);
            }
            catch(Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
        }

        public override async  Task DeleteAsync(FdTiposDocumentos entity)
        {
            try
            {
                var acciones = this.Context.FdAcciones.Where(a => a.TipoDocumentoId == entity.Id);
                var docs = this.Context.FdDocumentosProcesados.Where(d => d.TipoDocumentoId == entity.Id);
                var docsError = this.Context.FdDocumentosError.Where(e => e.TipoDocumentoId == entity.Id);


                if (docs.Count() != 0 || docsError.Count() != 0)
                {
                    throw new ValidationException("El tipo de documento no se puede eliminar existen asociaciones");
                }

                foreach (var accion in acciones)
                {
                    Context.Set<FdAcciones>().Remove(accion);
                }

                 await Task.FromResult(Context.Set<FdTiposDocumentos>().Remove(entity));

            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
