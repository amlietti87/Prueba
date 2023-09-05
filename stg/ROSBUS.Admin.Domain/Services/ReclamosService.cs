using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Enums;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Caching;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class ReclamosService : ServiceBase<SinReclamos, int, IReclamosRepository>, IReclamosService
    {
        public ICacheManager cacheManager;
        readonly IEstadosRepository _estadosRepository;
        public ReclamosService(IReclamosRepository produtoRepository, 
                               IEstadosRepository estadosRepository,
                               ICacheManager _cacheManager)
            : base(produtoRepository)
        {
            _estadosRepository = estadosRepository;
            cacheManager = _cacheManager;
        }

        public Task DeleteFileById(Guid id)
        {
            return this.repository.DeleteFileById(id);
        }

        public async Task<List<SinReclamoAdjuntos>> GetAdjuntos(int reclamoId)
        {
            return await this.repository.GetAdjuntos(reclamoId);
        }

        public async Task<List<SinReclamos>> GetByInvolucrado(int InvolucradoId)
        {
            return await this.repository.GetByInvolucrado(InvolucradoId);
        }

        public async Task<SinReclamos> CambioEstado(SinReclamos reclamo, SinReclamosHistoricos historico)
        {
            return await this.repository.CambioEstado(reclamo, historico);
        }

        public async Task<SinReclamos> Anular(SinReclamos reclamo)
        {
            return await this.repository.Anular(reclamo);
        }


        public async Task<List<ReporteReclamosExcel>> GetReporteExcel(ExcelReclamosFilter filter)
        {
            return await repository.GetReporteExcel(filter);
        }

        protected async override Task ValidateEntity(SinReclamos entity, SaveMode mode)
        {
            //validaciones
            if (entity.EstadoId == 0)
            {
                throw new DomainValidationException("Estado requerido");
            }

            if (entity.SubEstadoId == 0)
            {
                throw new DomainValidationException("Sub-Estado requerido");
            }

            if (!entity.EmpresaId.HasValue || entity.EmpresaId == 0)
            {
                throw new DomainValidationException("Empresa requerida");
            }

            if (!entity.SucursalId.HasValue || entity.SucursalId == 0)
            {
                throw new DomainValidationException("Empresa requerida");
            }

            if (entity.TipoReclamoId == (int)TiposReclamo.Siniestros)
            {
                if (!entity.SiniestroId.HasValue)
                {
                    throw new DomainValidationException("Siniestro requerido para tipo de reclamo siniestros");
                }
                if (!entity.InvolucradoId.HasValue)
                {
                    throw new DomainValidationException("Involucrado requerido para tipo de reclamo siniestros");
                }
                if (entity.EmpleadoId.HasValue)
                {
                    throw new DomainValidationException("No puede insertar empleado para tipo de reclamo siniestros");
                }
                if (entity.DenunciaId.HasValue)
                {
                    throw new DomainValidationException("No puede insertar denuncia para tipo de reclamo siniestros");
                }
            }
            else if (entity.TipoReclamoId == (int)TiposReclamo.ART || entity.TipoReclamoId == (int)TiposReclamo.Laboral)
            {
                if (entity.InvolucradoId.HasValue)
                {
                    throw new DomainValidationException("No puede insertar involucrado para tipo de reclamo ART o Laboral");
                }
                if (!entity.EmpleadoId.HasValue)
                {
                    throw new DomainValidationException("Empleado requerido para tipo de reclamo ART o Laboral");
                }
            }
            else
            {
                throw new DomainValidationException("Tipo de reclamo no reconocido");
            }

            var estado = await _estadosRepository.GetByIdAsync(entity.EstadoId);

            if (estado.Judicial == false)
            {
                if (!String.IsNullOrWhiteSpace(entity.Autos))
                {
                    throw new DomainValidationException("Autos no puede tener valor con un estado que no sea judicial");
                }
                if (!String.IsNullOrWhiteSpace(entity.NroExpediente))
                {
                    throw new DomainValidationException("Nro. expediente no puede tener valor con un estado que no sea judicial");
                }
                if (entity.JuzgadoId.HasValue)
                {
                    throw new DomainValidationException("Juzgado no puede tener valor con un estado que no sea judicial");
                }
                if (entity.AbogadoEmpresaId.HasValue)
                {
                    throw new DomainValidationException("Abogado empresa no puede tener valor con un estado que no sea judicial");
                }
            }

            await base.ValidateEntity(entity, mode);
        }

        public async Task<List<ImportadorExcelReclamos>> RecuperarPlanilla(ReclamoImportadorFileFilter filter)
        {
            var mvCache = await cacheManager.GetCache<string, List<ImportadorExcelReclamos>>("ImportadorExcelReclamos")
                                          .GetAsync(filter.PlanillaId, e => TransformarDatos(filter));

            return mvCache;
        }

        public async Task ImportarReclamos(ReclamoImportadorFileFilter input)
        {
            var planilla = await this.RecuperarPlanilla(new ReclamoImportadorFileFilter() { PlanillaId = input.PlanillaId });


            await this.repository.ImportarReclamos(planilla);
        }

        private async Task<List<ImportadorExcelReclamos>> TransformarDatos(ReclamoImportadorFileFilter filtro)
        {
            List<ImportadorExcelReclamos> result = new List<ImportadorExcelReclamos>();

            var Importados = await cacheManager.GetCache<string, List<ImportadorExcelReclamos>>("ImportadorExcelReclamos").GetAsync(filtro.PlanillaId, e => null);



            return result;
        }
    }
    
}
