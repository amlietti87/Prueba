using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class InvolucradosService : ServiceBase<SinInvolucrados, int, IInvolucradosRepository>, IInvolucradosService
    {
        private readonly IReclamosService _reclamosService;
        public InvolucradosService(IInvolucradosRepository produtoRepository, IReclamosService reclamosService)
            : base(produtoRepository)
        {
            _reclamosService = reclamosService;
       
        }

        public Task DeleteFileById(Guid id)
        {
            return this.repository.DeleteFileById(id);
        }

        public async Task<List<SinInvolucradosAdjuntos>> GetAdjuntos(int involucradoId)
        {
            return await this.repository.GetAdjuntos(involucradoId);
        }

        public async Task<HistorialInvolucrados> HistorialSiniestros(int TipoDocId, string NroDoc)
        {
            return await this.repository.HistorialSiniestros(TipoDocId, NroDoc);
        }

        public override async Task DeleteAsync(int id)
        {
            var reclamos = await this._reclamosService.GetPagedListAsync(new ReclamosFilter() { InvolucradoId = id, IsDeleted = false});
            if(reclamos.Items.Count > 0)
            {
                throw new InvalidOperationException("No se puede eliminar el Involucrado porque tiene reclamos asociados.");
            }
            await base.DeleteAsync(id);
        }

        public async Task<List<SinInvolucrados>> GetBySiniestro(int SiniestroId)
        {
            return await this.repository.GetBySiniestro(SiniestroId);
        }
    }

}
