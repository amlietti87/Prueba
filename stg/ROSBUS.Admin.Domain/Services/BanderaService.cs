using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class BanderaService : ServiceBase<HBanderas, int, IBanderaRepository>, IBanderaService
    {
        private readonly IBanderaRepository banderaRepository;
        private readonly IRamalColorRepository ramalColorRepository;
        public BanderaService(IBanderaRepository produtoRepository, IRamalColorRepository ramalColor)
            : base(produtoRepository)
        {
            banderaRepository = produtoRepository;
            ramalColorRepository = ramalColor;
        }

        public async Task<List<ReporteCambiosPorSector>> GetReporteCambiosDeSector(BanderaFilter filter)
        {
            return await this.repository.GetReporteCambiosDeSector(filter);
        }

        public async Task<List<string>> OrigenPredictivo(BanderaFilter filtro)
        {

            return await this.repository.OrigenPredictivo(filtro);
        }

        public async Task<List<string>> DestinoPredictivo(BanderaFilter filtro)
        {

            return await this.repository.DestinoPredictivo(filtro);
        }

        public async Task<List<ItemDto<int>>> RecuperarBanderasRelacionadasPorSector(BanderaFilter filtro)
        {
            return await this.repository.RecuperarBanderasRelacionadasPorSector(filtro);
        }

        public async Task<List<HBanderas>> GetAllBanderasWithRamal()
        {
            return await this.repository.GetAllBanderasWithRamal();
        }
        public Task<String> RecuperarCartel(int idBandera)
        {
            return this.repository.RecuperarCartel(idBandera);
        }

        public async Task<Linea> GetLinea(int idBandera)
        {
            return await this.repository.GetLinea(idBandera);
        }

        public async Task<HorariosPorSectorDto> RecuperarHorariosSectorPorSector(BanderaFilter filtro)
        {
            return await this.repository.RecuperarHorariosSectorPorSector(filtro);
        }

        public async Task<List<ItemDto>> RecuperarLineasActivasPorFecha(BanderaFilter filtro)
        {
            return await this.repository.RecuperarLineasActivasPorFecha(filtro);
        }

        public async Task<List<ItemDto>> RecuperarBanderasPorServicio(BanderaFilter filtro)
        {
            return await this.repository.RecuperarBanderasPorServicio(filtro);
        }

        public async override Task<HBanderas> AddAsync(HBanderas entity)
        {
            
            return await base.AddAsync(entity);
        }

        protected async override Task ValidateEntity(HBanderas entity, SaveMode mode)
        {

            if (entity.TipoBanderaId == 1)
            {
                if (entity.RamalColorId.HasValue)
                {
                    var Ramal = await ramalColorRepository.GetByIdAsync(entity.RamalColorId.Value);
                    var banderas = await this.banderaRepository.GetAllAsync(e => e.Id != entity.Id && e.RamalColor.LineaId == Ramal.LineaId && e.AbrBan.Trim() == entity.AbrBan.Trim() && e.TipoBanderaId == 1);

                    if (banderas.Items.Count > 0)
                    {
                        throw new DomainValidationException("Existen banderas de la misma linea con esta abreviación");
                    }

                }
            }
            else if (entity.TipoBanderaId == 2)
            {
                var banderas = await this.banderaRepository.GetAllAsync(e => e.Id != entity.Id && e.TipoBanderaId == 2 && e.AbrBan.Trim() == entity.AbrBan.Trim());

                if (banderas.Items.Count > 0)
                {
                    throw new DomainValidationException("Existen banderas de posicionamiento con esta abreviación");
                }
            }

            base.ValidateEntity(entity, mode);
        }
    }

}
