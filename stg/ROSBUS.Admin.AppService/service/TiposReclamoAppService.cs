using ROSBUS.ART.AppService.Interface;
using ROSBUS.ART.AppService.Model;
using ROSBUS.ART.Domain.Entities;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.ART.Domain.Entities.Filters;
using ROSBUS.ART.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.ART.AppService
{

    public class TiposReclamoAppService : AppServiceBase<TiposReclamo, TiposReclamoDto, int, ITiposReclamoService>, ITiposReclamoAppService
    {
        public TiposReclamoAppService(ITiposReclamoService serviceBase) 
            :base(serviceBase)
        {
         
        }

        public override PagedResult<TiposReclamo> GetPagedList<TFilter>(TFilter filter)
        {
            return base.GetPagedList(filter);
        }

        public override Task<PagedResult<TiposReclamo>> GetPagedListAsync<TFilter>(TFilter filter)
        {
            return base.GetPagedListAsync(filter);
        }
        public override async Task<TiposReclamoDto> AddAsync(TiposReclamoDto dto)
        {
            try
            {
                if (await Validate(dto))
                { 
                    return await base.AddAsync(dto);
                } else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> Validate(TiposReclamoDto dto)
        {
            try
            {
                PagedResult<TiposReclamo> tiposReclamo = await base.GetAllAsync(e => !e.Anulado);
                List<TiposReclamo> tiposReclamoFoundWithSameDescription = tiposReclamo.Items.Where(e => e.Descripcion == dto.Descripcion && e.Id != dto.Id).ToList();
                if (tiposReclamoFoundWithSameDescription != null && tiposReclamoFoundWithSameDescription.Count > 0)
                {
                    throw new ValidationException("Ya existe el tipo de Reclamo: " + dto.Description);
                }

                if (dto.Involucrado)
                {
                    List<TiposReclamo> tiposReclamoFoundWithInvolucradoTrue = tiposReclamo.Items.Where(e => e.Id != dto.Id && e.Involucrado).ToList();
                    if (tiposReclamoFoundWithInvolucradoTrue != null && tiposReclamoFoundWithInvolucradoTrue.Count > 0)
                    {
                        throw new ValidationException("Ya un reclamo con el campo Involucrado establecido en Verdadero");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


        public override async Task<TiposReclamoDto> UpdateAsync(TiposReclamoDto dto)
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
