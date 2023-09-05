using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class LineaAppService : AppServiceBase<Linea, LineaDto, decimal, ILineaService>, ILineaAppService
    {
        private IBanderaService banderaservice;
        private readonly IRamalColorService ramalservice;
        private readonly IAuthService authService;

        public LineaAppService(ILineaService serviceBase, IBanderaService banderaservice, IRamalColorService ramalservice, IAuthService authService)
            : base(serviceBase)
        {

            this.banderaservice = banderaservice;
            this.ramalservice = ramalservice;
            this.authService = authService;
        }


        public async override Task<LineaDto> UpdateAsync(LineaDto dto)
        {

            var entity = await this.GetByIdAsync(dto.Id);

            if (dto.Activo.GetValueOrDefault() && !entity.Activo.GetValueOrDefault())
            {
                var a = this.ramalservice.ExistExpression(e => e.LineaId == dto.Id &&  e.Activo);
                if (!a)
                {
                    throw new ValidationException("No puede activar una línea sin ramales activo");
                } 
            }

            MapObject(dto, entity);
            await this.UpdateAsync(entity);
            return MapObject<Linea, LineaDto>(entity);
        }

        public async override Task<LineaDto> AddAsync(LineaDto dto)
        {
            dto.Activo = true;
            var entity = MapObject<LineaDto, Linea>(dto);
                      
            if (dto.SucursalId.HasValue)
            {
                var spl = new SucursalesxLineas();
                spl.Id = dto.SucursalId.Value;
                spl.Lineas = entity;
                entity.SucursalesxLineas.Add(spl);
            }

            foreach (var item in entity.PlaLineaLineaHoraria)
            {
                item.Linea = entity;
            }

            entity.CodLinCaudalimetro = 0;

            return MapObject<Linea, LineaDto>(await this.AddAsync(entity));
        }

        public async Task<SearchResultDto> Search(string filterText)
        {
            SearchResultDto result = new SearchResultDto();

            #region linea
            var filter = new LineaFilter();
            filter.FilterText = filterText;
            var lineas = await this._serviceBase.GetAllAsync(filter.GetFilterExpression(), filter.GetIncludesForPageList());
            foreach (var item in lineas.Items)
            {
                if (item.SucursalesxLineas.Any())
                {
                    var v = new SearchResultLinea();
                    v.SucursalId = item.SucursalesxLineas.FirstOrDefault()?.Id;
                    v.Id = item.Id;
                    v.Description = item.DesLin.Trim();
                    result.Lineas.Add(v);
                }

            }
            #endregion


            #region Bandera
            var filterBan = new BanderaFilter();
            filterBan.FilterText = filterText;
            var banderas = await this.banderaservice.GetAllAsync(filterBan.GetFilterExpression(), filterBan.GetIncludesForPageList());
            foreach (var item in banderas.Items)
            {
                if (item.SucursalId.HasValue || (item.RamalColor != null && item.RamalColor.PlaLinea != null  && item.RamalColor.PlaLinea.Sucursal!=null))
                {
                    var v = new SearchResultBandera();
                    v.Id = item.Id;
                    v.Description = item.DesBan.Trim();
                    v.SucursalId = item.SucursalId ?? item.RamalColor.PlaLinea.Sucursal.Id;
                    result.Banderas.Add(v);
                }
            }
            #endregion



            #region ramales
            var ramalFilter = new RamalColorFilter();
            ramalFilter.FilterText = filterText;
            var ramales = await this.ramalservice.GetAllAsync(ramalFilter.GetFilterExpression(), ramalFilter.GetIncludesForPageList());
            foreach (var item in ramales.Items)
            {
                if (item.PlaLinea.Sucursal!= null)
                {
                    var v = new SearchResultRamal(item.Id, item.Nombre.Trim(), item.LineaId);
                    v.SucursalId = item.PlaLinea.Sucursal.Id;
                    result.Ramales.Add(v);
                }
            }
            #endregion

            result.TotalRegistrosEncontrados = result.Lineas.Count + result.Ramales.Count + result.Banderas.Count;

            return result;


        }

        public async Task<List<ItemDecimalDto>> GetLineasPorUsuario()
        {
            var filter = new LineaFilter();
            filter.UserId = this.authService.GetCurretUserId();
           // filter.Activo = true;

            filter.SinFechaBaja = true;


            if (!filter.UserId.HasValue)
            {
                throw new ArgumentException("UserId");
            }

            var lineas = await this._serviceBase.GetAllAsync(filter.GetFilterExpression());          

            var r = lineas.Items.Select(e =>  new ItemDecimalDto(e.Id, e.DesLin.Trim())).ToList();

            return r.OrderBy(e=> e.Description).ToList();            
        }

        public async Task UpdateLineasAsociadas(LineaDto lineaDto)
        {
            LineaFilter filter = new LineaFilter() { Id = lineaDto.Id, IncludePlaLineaHoraria = true };

            var linea = await this._serviceBase.GetByIdAsync(filter);

            linea.UrbInter = lineaDto.UrbInter;
            linea.DesLin = lineaDto.DesLin;
            linea.CodRespInformes = lineaDto.CodRespInformes;
            foreach (var item in linea.PlaLineaLineaHoraria.Reverse())
            {
                if (!lineaDto.PlaLineaLineaHoraria.Any(e=> e.PlaLineaId==item.PlaLineaId))
                {
                    linea.PlaLineaLineaHoraria.Remove(item);
                }
            }

            foreach (var item in lineaDto.PlaLineaLineaHoraria)
            {
                if (!linea.PlaLineaLineaHoraria.Any(e => e.PlaLineaId == item.PlaLineaId))
                {
                    linea.PlaLineaLineaHoraria.Add(new PlaLineaLineaHoraria() { LineaId=item.LineaId, PlaLineaId=item.PlaLineaId });
                }
            }

            await this._serviceBase.UpdateAsync(linea);

        }

        public async Task<LineaDto> RecuperarLineaConLineasAsociadas(decimal id)
        {
            LineaFilter filter = new LineaFilter() { Id = id };

            filter.IncludePlaLineaHoraria = true;

            var entity = await this.GetByIdAsync(filter);

            return MapObject<Linea, LineaDto>(entity);
        }

        public async Task <List<LineaRosBusDto>> GetAllRosBusLineasAsync()
        {
            List<LineaRosBusDto> RosBusLines = new List<LineaRosBusDto>();
            LineaFilter lineaFilter = new LineaFilter();

            var Lineas = (await this._serviceBase.GetAllAsync(lineaFilter.GetFilterExpression())).Items.ToList();

            foreach (var linea in Lineas)
            {
                LineaRosBusDto RosBusLine = new LineaRosBusDto();
                RosBusLine.CodLin = Convert.ToInt32(linea.Id);
                RosBusLine.DesLin = linea.DesLin.Trim();
                RosBusLines.Add(RosBusLine);
            }

            return RosBusLines;
        }
    }
}
