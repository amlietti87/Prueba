using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Enums.Siniestros;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Report;
using ROSBUS.Admin.Domain.Report.Report.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class SiniestrosAppService : AppServiceBase<SinSiniestros, SiniestrosDto, int, ISiniestrosService>, ISiniestrosAppService
    {
        public SiniestrosAppService(ISiniestrosService serviceBase, 
            IInvolucradosService involucradosService, 
            IConsecuenciasService consecuenciasService, 
            IEmpleadosService empleadoservice, 
            IAuthService authservice, 
            ILocalidadesService localidaddervice, 
            IAdjuntosService adjuntosService, 
            ICCochesService cochesService, 
            IReclamosService reclamosService): base(serviceBase)
        {
            _involucradosService = involucradosService;
            _consecuenciasService = consecuenciasService;
            _empleadoservice = empleadoservice;
            _authservice = authservice;
            _localidaddervice = localidaddervice;
            _adjuntosService = adjuntosService;
            _cochesService = cochesService;
            _reclamosService = reclamosService;

            
        }



        private readonly IEmpleadosService _empleadoservice;
        private readonly IAuthService _authservice;
        private readonly ILocalidadesService _localidaddervice;
        private readonly IInvolucradosService _involucradosService;
        private readonly IReclamosService _reclamosService;
        private readonly IAdjuntosService _adjuntosService;
        private readonly IConsecuenciasService _consecuenciasService;
        private readonly ICCochesService _cochesService;

        public override async Task<PagedResult<SiniestrosDto>> GetDtoAllAsync(Expression<Func<SinSiniestros, bool>> predicate, List<Expression<Func<SinSiniestros, object>>> includeExpression = null)
        {
            var list = await this._serviceBase.GetAllAsync(predicate,  new List<Expression<Func<SinSiniestros, object>>>
            {
                e=> e.Sucursal
            });

            var listDto = this.MapList<SinSiniestros, SiniestrosDto>(list.Items);

            PagedResult<SiniestrosDto> pList = new PagedResult<SiniestrosDto>(list.TotalCount, listDto.ToList());

            return pList;
        }

        public async Task<HistorialSiniestros> GetHistorialEmpPract(bool empleado, int id)
        {
            return await _serviceBase.GetHistorialEmpPract(empleado, id);
        }

        public async Task<int> GetNroSiniestroMax()
        {
            return await _serviceBase.GetNroSiniestroMax();
        }

        public async Task<string> GenerarInforme(int SiniestroId)
        {
            return await this._serviceBase.GenerarInforme(SiniestroId);
        }

        public async override Task<List<ItemDto<int>>> GetItemsAsync<TFilter>(TFilter filter)
        {
            var list = await this.GetPagedListAsync(filter);
            var r = list.Items.Select(filter.GetItmDTO()).ToList();
            return r;
        }

        public async Task<ReportModel> GetDatosReporte(SiniestrosDto dto)
        {
            var localidades = await _localidaddervice.GetAllLocalidades();
            List<SinInvolucrados> involucradosDB = await this._involucradosService.GetBySiniestro(dto.Id);

            SiniestroReportModel Siniestro = new SiniestroReportModel();

            //Licencias empleado

            List<LicenciasVencimiento> Licencias = new List<LicenciasVencimiento>();

            // Map Siniestro
            Siniestro.FechaDenuncia = dto.FechaDenuncia.ToString("dd/MM/yyyy HH:mm");
            Siniestro.FechaSiniestro = dto.Fecha.ToString("dd/MM/yyyy");
            Siniestro.Hora = new DateTime(dto.Hora.Ticks).ToString("HH:mm");
            Siniestro.Localidad = dto.Localidad;
            Siniestro.Lugar = dto.Lugar;
            Siniestro.UnidadDeNegocio = dto.Sucursal.Description;
            Siniestro.NroSiniestro = dto.NroSiniestro;
            Siniestro.NroInterno = dto.CocheInterno;
            Siniestro.Croqui = dto.CroquiBase64;
            Siniestro.FormaSiniestro = dto.Comentario;
            Siniestro.DescripcionDanios = dto.ObsDanios;
            
            if (!String.IsNullOrWhiteSpace(dto.CocheId))
            {
                if (await _cochesService.ExisteCoche(dto.CocheId))
                {
                    var coche = (await _cochesService.GetByIdAsync(dto.CocheId));
                    if (coche != null)
                    {
                        Siniestro.Empresa = coche.Empresa.DesEmpr;
                        Siniestro.Modelo = coche.Anio.ToString() + " " + coche.Marca?.Trim();
                    }

                }
            }

            if (dto.CocheFicha.HasValue)
            {
                Siniestro.Ficha = dto.CocheFicha.Value.ToString();
            }
            Siniestro.Dominio = dto.CocheDominio;

            if (dto.CocheLinea != null)
            {
                Siniestro.Linea = dto.CocheLinea.DesLin;
            }
            if (dto.EmpPract == "E" && dto.ConductorId.HasValue)
            {
                Siniestro.LabelDatosEmpPract = "Siniestro del conductor";
                Siniestro.LabelEmpPract = "Conductor";
                Siniestro.ApNomEmpPract = dto.NombreConductor + " (" + dto.ConductorLegajo + ")";
                Siniestro.DocEmpPract = "DNI: " + dto.DniConductor;
                Siniestro.EmpresaEmpPract = "Empresa: " + dto.ConductorEmpresa.DesEmpr;
                var existe = await _empleadoservice.ExisteEmpleado(dto.ConductorId.Value);
                if (existe)
                {
                    var empleado = await _empleadoservice.GetByIdAsync(dto.ConductorId.Value);

                    Siniestro.DomicilioEmpPract = (empleado?.CalleDomicilio?.Trim() + " " + empleado?.NroDomicilio?.Trim()).Trim();

                    Siniestro.LocalidadEmpPract = localidades.Items.FirstOrDefault(e => e.Id == empleado.CodLocalidad)?.GetDescription();

                    var licencias = await this._serviceBase.GetLicencias(dto.ConductorId.Value);
                    foreach (var item in licencias)
                    {
                        if (!String.IsNullOrEmpty(Siniestro.Licencias))
                        {
                            Siniestro.Licencias += " - " + item.Descripcion + " " + item.FechaVencimiento.ToString("dd/MM/yyyy");
                        }
                        else {

                            Siniestro.Licencias += item.Descripcion + " " + item.FechaVencimiento.ToString("dd/MM/yyyy");
                        }

                    }

                }
            }
            else
            {
                Siniestro.LabelDatosEmpPract = "Siniestro del practicante";
                Siniestro.LabelEmpPract = "Practicante";
                Siniestro.ApNomEmpPract = dto.Practicante?.ApellidoNombre + " " + dto.Practicante?.TipoDoc?.Descripcion + " " + dto.Practicante?.NroDoc;
                Siniestro.DomicilioEmpPract = dto.Practicante?.Domicilio;
                Siniestro.LocalidadEmpPract = dto.Practicante?.Localidad;
            }


            // Map Involucrados, Lesionados, Testigos
            List<InvolucradoTercero> Terceros = new List<InvolucradoTercero>();
            List<InvolucradoLesionado> Lesionados = new List<InvolucradoLesionado>();
            List<InvolucradoTestigo> Testigos = new List<InvolucradoTestigo>();
            List<InvolucradoMuebleInmueble> MueblesInmuebles = new List<InvolucradoMuebleInmueble>();
            foreach (var item in involucradosDB)
            {
                // ¿Es un tercero? Si tiene Conductor y Vehiculo
                if (item.VehiculoId != null && item.ConductorId != null)
                {
                    InvolucradoTercero involucradoTerceroForReport = new InvolucradoTercero();
                    involucradoTerceroForReport.ApellidoNombre = item.ApellidoNombre;
                    involucradoTerceroForReport.Domicilio = item.Domicilio;
                    involucradoTerceroForReport.Localidad = localidades.Items.FirstOrDefault(e => e.Id == item.LocalidadId)?.GetDescription();
                    involucradoTerceroForReport.NroInvolucrado = item.NroInvolucrado;
                    involucradoTerceroForReport.Telefono = item.Telefono + " - " + item.Celular;
                    involucradoTerceroForReport.Tipo = item.TipoInvolucrado?.Descripcion;
                    involucradoTerceroForReport.TipoNroDoc = item.TipoDoc?.Descripcion + " " + item.NroDoc;
                    involucradoTerceroForReport.DetalleDanio = item.Detalle;
                    //Map Vehiculo
                    involucradoTerceroForReport.VehiculoMarca = item.Vehiculo?.Marca;
                    involucradoTerceroForReport.VehiculoModelo = item.Vehiculo?.Modelo;
                    involucradoTerceroForReport.VehiculoDominio = item.Vehiculo?.Dominio;
                    if (item.Vehiculo.Seguro != null)
                    {
                        involucradoTerceroForReport.VehiculoCiaSeguro = item.Vehiculo?.Seguro?.Descripcion;
                    }
                    involucradoTerceroForReport.VehiculoNroPoliza = item.Vehiculo?.Poliza;
                    //Map Conductor
                    involucradoTerceroForReport.ConductorApellidoNombre = item.Conductor?.ApellidoNombre;
                    involucradoTerceroForReport.ConductorDomicilio = item.Conductor?.Domicilio;
                    involucradoTerceroForReport.ConductorLocalidad = localidades.Items.FirstOrDefault(e => e.Id == item.Conductor?.LocalidadId)?.GetDescription(); ;
                    involucradoTerceroForReport.ConductorTelefono = item.Conductor?.Telefono + " - " + item.Conductor?.Celular;
                    involucradoTerceroForReport.ConductorTipoNroDoc = item.Conductor?.TipoDoc?.Descripcion + " " + item.Conductor?.NroDoc;

                    Terceros.Add(involucradoTerceroForReport);
                }

                // ¿Es un lesionado? Si tiene Lesionado
                if (item.LesionadoId != null)
                {
                    InvolucradoLesionado involucradoLesionadoForReport = new InvolucradoLesionado();
                    involucradoLesionadoForReport.ApellidoNombre = item.ApellidoNombre;
                    involucradoLesionadoForReport.DetalleDanio = item.Detalle;
                    involucradoLesionadoForReport.DomicilioLocalidad = item.Domicilio + " " + localidades.Items.FirstOrDefault(e => e.Id == item.LocalidadId)?.GetDescription();
                    involucradoLesionadoForReport.NroInvolucrado = item.NroInvolucrado;
                    involucradoLesionadoForReport.Telefono = item.Telefono + " - " + item.Celular;
                    if (item.Lesionado != null)
                    {
                        involucradoLesionadoForReport.TipoLesionado = item.Lesionado?.GetDescription();
                    }
                    involucradoLesionadoForReport.TipoNroDoc = item.TipoDoc?.Descripcion + " " + item.NroDoc;

                    Lesionados.Add(involucradoLesionadoForReport);
                }

                // ¿Es un Mueble/Inmueble? Si tiene MuebleInmueble
                if (item.MuebleInmuebleId != null)
                {
                    // Mueble / Inmueble
                    InvolucradoMuebleInmueble involucradoMuebleInmuebleForReport = new InvolucradoMuebleInmueble();
                    involucradoMuebleInmuebleForReport.MuebleInmuebleId = item.MuebleInmueble.Id;
                    involucradoMuebleInmuebleForReport.MuebleInmuebleTipo = item.MuebleInmueble.TipoInmueble?.Descripcion;
                    involucradoMuebleInmuebleForReport.MuebleInmuebleDomicilio = item.MuebleInmueble?.Lugar;
                    involucradoMuebleInmuebleForReport.MuebleInmuebleLocalidad = localidades.Items.FirstOrDefault(e => e.Id == item.MuebleInmueble?.LocalidadId)?.GetDescription(); ;

                    involucradoMuebleInmuebleForReport.ApellidoNombre = item.ApellidoNombre;
                    involucradoMuebleInmuebleForReport.DetalleDanio = item.Detalle;
                    involucradoMuebleInmuebleForReport.Domicilio = item.Domicilio;
                    involucradoMuebleInmuebleForReport.Localidad = localidades.Items.FirstOrDefault(e => e.Id == item.MuebleInmueble?.LocalidadId)?.GetDescription();
                    involucradoMuebleInmuebleForReport.NroInvolucrado = item.NroInvolucrado;
                    involucradoMuebleInmuebleForReport.Telefono = item.Telefono + " - " + item.Celular;
                    involucradoMuebleInmuebleForReport.Tipo = item.TipoInvolucrado?.Descripcion;
                    involucradoMuebleInmuebleForReport.TipoNroDoc = item.TipoDoc?.Descripcion + " " + item.NroDoc;

                    MueblesInmuebles.Add(involucradoMuebleInmuebleForReport);
                }

                // ¿Es un Testigo? Si no tiene Lesionado ni MuebleInmueble ni Conductor ni Vehiculo
                if (item.ConductorId == null && item.VehiculoId == null && item.LesionadoId == null && item.MuebleInmuebleId == null)
                {
                    InvolucradoTestigo involucradoTestigoForReport = new InvolucradoTestigo();
                    involucradoTestigoForReport.ApellidoNombre = item.ApellidoNombre;
                    involucradoTestigoForReport.TipoNroDoc = item.TipoDoc?.Descripcion + " " + item.NroDoc;
                    involucradoTestigoForReport.Telefono = item.Telefono + " - " + item.Celular;
                    involucradoTestigoForReport.Domicilio = item.Domicilio + " " + localidades.Items.FirstOrDefault(e => e.Id == item.LocalidadId)?.GetDescription();
                    Testigos.Add(involucradoTestigoForReport);
                }
            }

            ReportModel rp = new ReportModel();
            rp.ReportName = ReportName.SiniestroReportNamespace;
            rp.AddDataSources("Cabecera", new List<SiniestroReportModel> { Siniestro });
            rp.AddDataSources("Involucrados", Terceros);
            rp.AddDataSources("Lesionados", Lesionados);
            rp.AddDataSources("Testigos", Testigos);
            rp.AddDataSources("MueblesInmuebles", MueblesInmuebles);
            rp.AddDataSources("LicenciasVencimiento", Licencias);
            return rp;
        }

        public async override Task<SiniestrosDto> GetDtoByIdAsync(int id)
        {
            SinSiniestros entity = await this._serviceBase.GetByIdAsync(id);

            var localidades = await _localidaddervice.GetAllLocalidades();

            var dto = this.MapObject<SinSiniestros, SiniestrosDto>(entity);

            string nro_informe = await this._serviceBase.GetInforme(entity.CodInforme);
            dto.nro_informe = nro_informe;

            var coche = this._cochesService.GetAll(e=> e.Id==entity.CocheId).Items.FirstOrDefault();
            if (coche!=null)
            {
                dto.CocheDominio = coche.Dominio?.Replace(" ", string.Empty);
            }

            return dto;
        }

        public override async Task<SiniestrosDto> AddAsync(SiniestrosDto dto)
        {
            var nrosin = (await this.GetNroSiniestroMax()) + 1;
            dto.NroSiniestro = nrosin.ToString();
            dto.CocheDominio = dto.CocheDominio?.Replace(" ", string.Empty);
            var entity = MapObject<SiniestrosDto, SinSiniestros>(dto);
            foreach (var item in entity.SinSiniestrosConsecuencias.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            var result = await this.AddAsync(entity);
            return MapObject<SinSiniestros, SiniestrosDto>(entity);
        }

        public async override Task<PagedResult<SiniestrosDto>> GetDtoPagedListAsync<TFilter>(TFilter filter)
        {
            var list = await _serviceBase.GetPagedListAsync(filter);
            var listDto = this.MapList<SinSiniestros, SiniestrosDto>(list.Items);

            SiniestrosFilter fil = filter as SiniestrosFilter;


            if (!String.IsNullOrWhiteSpace(fil.ApellidoInvolucrado))
            {

                foreach (var sin in listDto)
                {
                    var involucrados = await _involucradosService.GetBySiniestro(sin.Id);

                    List<string> listInv = new List<string>();
                    sin.ApellidoInvolucrado = string.Empty;

                    foreach (var inv in involucrados.Where(e => e.ApellidoNombre.ToLower().Trim().Contains(fil.ApellidoInvolucrado.ToLower().Trim())).ToList())
                    {
                        listInv.Add(inv.ApellidoNombre.Trim());
                    }

                    sin.ApellidoInvolucrado = string.Join(", ", listInv);

                }
            }

            foreach (var sin in listDto)
            {
                if (sin.SiniestrosConsecuencias != null && sin.SiniestrosConsecuencias.Count >= 1)
                {
                    var sincon = sin.SiniestrosConsecuencias.OrderBy(e => e.Id).FirstOrDefault();

                    sin.PrimerConsecuencia = (await _consecuenciasService.GetByIdAsync(sincon.ConsecuenciaId)).Descripcion;

                }

                var reclamos = await _reclamosService.GetAllAsync(e => e.SiniestroId == sin.Id && e.IsDeleted == false, new List<Expression<Func<SinReclamos, object>>> { e => e.Estado });

                if (reclamos.Items.Count == 0)
                {
                    sin.EstadoDeReclamos = string.Empty;
                }
                else if (reclamos.Items.Count == 1)
                {
                    sin.EstadoDeReclamos = reclamos.Items.FirstOrDefault().Estado.Descripcion;
                }
                else
                {
                    sin.EstadoDeReclamos = "Tiene varios reclamos";
                }
            }

            PagedResult<SiniestrosDto> pList = new PagedResult<SiniestrosDto>(list.TotalCount, listDto.ToList());
            return pList;
        }

        public async override Task<SiniestrosDto> UpdateAsync(SiniestrosDto dto)
        {
            if (!string.IsNullOrEmpty(dto.CocheDominio))
            {
                dto.CocheDominio = dto.CocheDominio.Replace(" ", string.Empty);
            }
            
            var entity = await this.GetByIdAsync(dto.Id);



            MapObject(dto, entity);




            foreach (var item in entity.SinSiniestrosConsecuencias.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            foreach (var item in entity.SinInvolucrados.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            foreach (var item in entity.SinInvolucrados.SelectMany(e => e.SinDetalleLesion).Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }




            await this.UpdateAsync(entity);

            if (dto.GenerarInforme.HasValue && dto.GenerarInforme.Value && String.IsNullOrWhiteSpace(entity.CodInforme))
            {

                await this.GenerarInforme(entity.Id);
            }




            return MapObject<SinSiniestros, SiniestrosDto>(entity);
        }

        public async Task<List<AdjuntosDto>> GetAdjuntosSiniestros(int SiniestroId)
        {
            List<ItemDto<Guid>> adjuntos = new List<ItemDto<Guid>>();

            var sinAdj = await this._serviceBase.GetAdjuntosSiniestros(SiniestroId);



            AdjuntosFilter filter = new AdjuntosFilter();
            filter.Ids = sinAdj.Select(e => e.AdjuntoId).ToList(); ;

            adjuntos = await _adjuntosService.GetAdjuntosItemDto(filter);

            return adjuntos.Select(e => new AdjuntosDto() { Id = e.Id, Nombre = e.Description }).ToList();
        }

        public async Task AgregarAdjuntos(int siniestroID, List<AdjuntosDto> result)
        {
            var allEntity = await this._serviceBase.GetAllAsync(e => e.Id == siniestroID);

            var entity = allEntity.Items.FirstOrDefault();
            if (entity != null)
            {
                foreach (var item in result)
                {
                    entity.SinSiniestroAdjuntos.Add(new SinSiniestroAdjuntos() { AdjuntoId = item.Id, SiniestroId = siniestroID });
                }

            }

            await this._serviceBase.UpdateAsync(entity);

        }

        public Task DeleteFileById(Guid id)
        {
            return this._serviceBase.DeleteFileById(id);
        }
    }
}
