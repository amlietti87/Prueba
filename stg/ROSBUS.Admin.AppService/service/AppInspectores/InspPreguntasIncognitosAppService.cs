﻿using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;

namespace ROSBUS.Admin.AppService.service
{
    public class InspPreguntasIncognitosAppService : AppServiceBase <InspPreguntasIncognitos, InspPreguntasIncognitosDto, int, IInspPreguntasIncognitosService>, IInspPreguntasIncognitosAppService
    {
        public InspPreguntasIncognitosAppService(IInspPreguntasIncognitosService serviceBase)
             : base(serviceBase)
        {

        }

        public override async Task<InspPreguntasIncognitosDto> UpdateAsync(InspPreguntasIncognitosDto dto)
        {
            var entity = await this.GetByIdAsync(dto.Id);
            MapObject(dto, entity);

            //foreach (var item in entity.InspGrupoInspectoresZona)
            //{
            //    if (item.Id<0)
            //    {
            //        item.Id = 0;
            //    }
            //}
            await this.UpdateAsync(entity);
            return MapObject<InspPreguntasIncognitos, InspPreguntasIncognitosDto>(entity);
        }
    }
}
