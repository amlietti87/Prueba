using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters.ComoLlegoBus;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class PlaLineaService : ServiceBase<PlaLinea,int, IPlaLineaRepository>, IPlaLineaService
    { 
        public PlaLineaService(IPlaLineaRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public async Task<ReporteHorarioRuta> GetHorariosRuta(string routelongname)
        {
            return await this.repository.GetHorarioRuta(routelongname);
        }

        public async Task<List<PlaLinea>> GetLineasValidas(string nlRosBusValidas, string tiposdias)
        {
            return await this.repository.GetLineasValidas(nlRosBusValidas, tiposdias);
        }

        public async Task<List<int>> GetMedioPagosRuta(string name)
        {
            return await this.repository.GetMedioPagosRuta(name);
        }

        public async Task<ReporteParadasRuta> GetParadasRutas(int? codBan, string Lineas)
        {
            return await this.repository.GetParadasRuta(codBan, Lineas);
        }

        public async Task<List<PuntosRuta>> GetPuntosRutaBanderaAsync(int? codBan)
        {
            return await this.repository.GetPuntosRutaBanderaAsync(codBan);
        }

        public async Task<ReporteTarifasRuta> GetTarifasRutasAsync(int? codBan, string NombresLineas)
        {
            return await this.repository.GetTarifasRutaAsync(codBan, NombresLineas);
        }

        public async Task<decimal> GetTarifaUrbana(string linename)
        {
            return await this.repository.GetTarifaUrbana(linename);
        }

        protected override Task ValidateEntity(PlaLinea entity, SaveMode mode)
        {
             var otroigual = this.repository.ExistExpression(e =>
             e.Id != entity.Id
            && !e.IsDeleted
            && e.DesLin == entity.DesLin);
            
            if (otroigual)
            {
                throw new DomainValidationException("El nombre de la linea no puede repetirse.");
            }
            return base.ValidateEntity(entity, mode);
        }

    }
    
}
