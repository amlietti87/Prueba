using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class TipoParadaService : ServiceBase<PlaTipoParada,int, ITipoParadaRepository>, ITipoParadaService
    {
        private readonly ITiposDeDiasRepository tiposDeDiasRepository;

        public TipoParadaService(ITipoParadaRepository produtoRepository, ITiposDeDiasRepository _tiposDeDiasRepository)
            : base(produtoRepository)
        {
            this.tiposDeDiasRepository = _tiposDeDiasRepository;
        }

        protected async override Task ValidateEntity(PlaTipoParada entity, SaveMode mode)
        {
            await base.ValidateEntity(entity, mode);


            var tposDiasHAbilitados = tiposDeDiasRepository.GetAll(e => e.Activo && !e.IsDeleted);

            


            if (entity.PlaTiempoEsperadoDeCarga == null || entity.PlaTiempoEsperadoDeCarga.Count == 0)
                throw new DomainValidationException("Se debe configurar al menos un tiempo esperado de carga");

            foreach (var item in entity.PlaTiempoEsperadoDeCarga.GroupBy(e => e.TipodeDiaId).ToList())
            {
                if (item.Any(i => item.Any(o => i.HoraDesde <= o.HoraHasta && o.HoraDesde <= i.HoraHasta && i != o)))
                    throw new DomainValidationException("Existen horarios superpuestos en un mismo tipo de día");

                TimeSpan counter = new TimeSpan();

                foreach (var item2 in item)
                    counter = counter.Add((item2.HoraHasta - item2.HoraDesde) + TimeSpan.FromMinutes(1));

                if (counter != TimeSpan.FromHours(24))
                    throw new DomainValidationException("Se deben completar las 24hs para un mismo tipo de día definido");
            }

            if (tposDiasHAbilitados.Items.ToList().Any(e => !entity.PlaTiempoEsperadoDeCarga.Any(t => t.TipodeDiaId == e.Id)))
            {
                throw new DomainValidationException("Deben estar configurados todos los tipos de dias.");
            }
        }

    }
    
}
