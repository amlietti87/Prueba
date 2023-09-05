using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.Domain.Interfaces.Services.ART;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.AppService;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ROSBUS.Admin.AppService.service.ART
{
    public class EstadosAppService : AppServiceBase<ArtEstados, ArtEstadosDto, int, IEstadosService>, IEstadosAppService
    {
        public EstadosAppService(IEstadosService serviceBase) : base(serviceBase)
        {
        }

        public override async Task<ArtEstadosDto> AddAsync(ArtEstadosDto dto)
        {
            try
            {
                if (await Validate(dto))
                {
                    return await base.AddAsync(dto);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> Validate(ArtEstadosDto dto)
        {
            try
            {
                PagedResult<ArtEstados> estados = await base.GetAllAsync(e => !e.Anulado);
                List<ArtEstados> estadosFoundWithSameDescription = estados.Items.Where(e => e.Descripcion == dto.Descripcion && e.Id != dto.Id).ToList();
                if (estadosFoundWithSameDescription != null && estadosFoundWithSameDescription.Count > 0)
                {
                    throw new ValidationException("Ya existe el Estado: " + dto.Description);
                }

                if (dto.Predeterminado)
                {
                    List<ArtEstados> estadosFoundWithPredeterminadoTrue = estados.Items.Where(e => e.Id != dto.Id && e.Predeterminado).ToList();
                    if (estadosFoundWithPredeterminadoTrue != null && estadosFoundWithPredeterminadoTrue.Count > 0)
                    {
                        throw new ValidationException("Ya existe un estado predeterminado");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public override async Task<ArtEstadosDto> UpdateAsync(ArtEstadosDto dto)
        {
            try
            {
                if (await Validate(dto))
                {
                    return await base.UpdateAsync(dto);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
