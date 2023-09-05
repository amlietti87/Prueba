using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using TECSO.FWK.Domain;
using ROSBUS.Admin.Domain.ParametersHelper;

namespace ROSBUS.Admin.AppService
{

    public class FdDocumentosErrorAppService : AppServiceBase<FdDocumentosError, FdDocumentosErrorDto, long, IFdDocumentosErrorService>, IFdDocumentosErrorAppService
    {
        private readonly IEmpresaService _empresaService;
        private readonly ISysParametersService parametersHelper;
        public FdDocumentosErrorAppService(IFdDocumentosErrorService serviceBase, IEmpresaService empresaService, ISysParametersService _parametersHelper) 
            :base(serviceBase)
        {
            parametersHelper = _parametersHelper;
            _empresaService = empresaService;
        }

        public void GuardarRevisado(FdDocumentosError doc)
        {
            this._serviceBase.GuardarRevisado(doc);
        }

        public override async Task<PagedResult<FdDocumentosErrorDto>> GetDtoPagedListAsync<TFilter>(TFilter filter)
        {
            var dias = this.parametersHelper.GetAll(e => e.Token == "FDCantDiasRevisarErrores");

            var filt = filter as FdDocumentosErrorFilter;
            if (filt.Revisado !=2 && filt.FechaHasta == null && filt.FechaDesde == null)
            {
                throw new DomainValidationException("El rango de las fechas indicadas en los filtros cuando se seleccionan documentos ya revisados debe ser menor a " + dias.Items[0].Value + " días");
            } else {
                if (filt.Revisado != 2 && filt.FechaHasta == null)
                {
                    throw new DomainValidationException("No se especifica fecha hasta requerida para seleccionar documentos ya revisados");
                } else if (filt.Revisado != 2 && filt.FechaDesde == null) {
                    throw new DomainValidationException("No se especifica fecha desde requerida para seleccionar documentos ya revisados");
                } else if (filt.Revisado != 2 && (DateTime)filt.FechaDesde.Value > (DateTime)filt.FechaHasta.Value) {
                    throw new DomainValidationException("Fecha desde es mayor a fecha hasta");
                } else if (filt.Revisado != 2 && ((DateTime)filt.FechaHasta.Value - (DateTime)filt.FechaDesde.Value).Days > Int32.Parse(dias.Items[0].Value)) {
                    throw new DomainValidationException("El rango de las fechas indicadas en los filtros cuando se seleccionan documentos ya revisados debe ser menor a " + dias.Items[0].Value + " días");
                } else {
                    var list = await _serviceBase.GetPagedListAsync(filter);
                    var listDto = this.MapList<FdDocumentosError, FdDocumentosErrorDto>(list.Items);
                    FdDocumentosErrorFilter fil = filter as FdDocumentosErrorFilter;

                    foreach (var item in listDto)
                    {
                        if (String.IsNullOrWhiteSpace(item.EmpresaDescripcion) && !String.IsNullOrWhiteSpace(item.NombreEmpleado) && item.EmpresaEmpleadoId.HasValue)
                        {
                            item.EmpresaDescripcion = (await this._empresaService.GetByIdAsync(item.EmpresaEmpleadoId.Value)).DesEmpr;
                        }
                    }

                    PagedResult<FdDocumentosErrorDto> pList = new PagedResult<FdDocumentosErrorDto>(list.TotalCount, listDto.ToList());
                    return pList;
                }
            }
        }
    }
}
