using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;
using System.Linq;
using TECSO.FWK.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System.ComponentModel.DataAnnotations;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Entities.Filters;
using Snickler.EFCore;
using System.Data.SqlClient;
using ROSBUS.Admin.Domain.ParametersHelper;
using TECSO.FWK.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;

namespace ROSBUS.infra.Data.Repositories
{
    public class InspDiagramasInspectoresRepository : RepositoryBase<AdminContext, InspDiagramasInspectores, int>, IInspDiagramasInspectoresRepository
    {
        private readonly IEmpleadosService empleadosService;
        private readonly IParametersHelper parametersHelper;
        private readonly IAuthService authService;

        public InspDiagramasInspectoresRepository(IAdminDbContext _context, IEmpleadosService _empleadosService, IParametersHelper _parametersHelper, IAuthService _authService)
            : base(new DbContextProvider<AdminContext>(_context))
        {
            empleadosService = _empleadosService;
            parametersHelper = _parametersHelper;
            authService = _authService;
        }

        public override Expression<Func<InspDiagramasInspectores, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<InspDiagramasInspectores> AddIncludeForGet(DbSet<InspDiagramasInspectores> dbSet)
        {

            return base.AddIncludeForGet(dbSet)
                .Include(e => e.EstadoDiagrama)
                .Include(e => e.GrupoInspectores)
                .Include(e => e.InspDiagramaInspectoresTurnos).ThenInclude(dit => dit.Turno);

        }

        protected override IQueryable<InspDiagramasInspectores> GetIncludesForPageList(IQueryable<InspDiagramasInspectores> query)
        {
            return base.GetIncludesForPageList(query)
                .Include(e => e.GrupoInspectores)
                .Include(e => e.EstadoDiagrama);
        }

        public override async Task<PagedResult<InspDiagramasInspectores>> GetAllAsync(Expression<Func<InspDiagramasInspectores, bool>> predicate, List<Expression<Func<InspDiagramasInspectores, object>>> includeExpression = null)
        {

            try
            {
                IQueryable<InspDiagramasInspectores> query = Context.Set<InspDiagramasInspectores>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.Include(e => e.GrupoInspectores);
                query = query.Include(e => e.EstadoDiagrama);

                return new PagedResult<InspDiagramasInspectores>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }


        public async Task<DiagramaMesAnioDto> DiagramaMesAnioGrupo(int Id, List<int> turnoId, Boolean blockentity)
        {
            DiagramaMesAnioDto DiagramaInspDiaAnio = new DiagramaMesAnioDto();
            var diagrama = this.Context.InspDiagramasInspectores.Where(d => d.Id == Id).FirstOrDefault();

           

            DiagramaInspDiaAnio.Mes = diagrama.Mes;
            DiagramaInspDiaAnio.Anio = diagrama.Anio;
            List<InspDiagramasInspectoresTurnos> DiagramasInspectoresTurnoId = new List<InspDiagramasInspectoresTurnos>();

            DiagramasInspectoresTurnoId = ((await this.Context.InspDiagramaInspectoresTurnos.Where(t => t.DiagramaInspectoresId == Id && turnoId.Contains(t.TurnoId)).ToListAsync()));


            List<HFrancos> hfrancosDiagrama = new List<HFrancos>();
            List<PersJornadasTrabajadas> persJorTraDiagrama = new List<PersJornadasTrabajadas>();

            List<int?> listIds = new List<int?>();

            DiagramasInspectoresTurnoId.Select(e => e.Id).ToList().ForEach(e => listIds.Add(e));

            var hfrancodiagramaAll = this.Context.HFrancos.Where(f => listIds.Contains(f.DiagramaInspectoresTurnoId) && (f.Fecha.Year == DiagramaInspDiaAnio.Anio && f.Fecha.Month == DiagramaInspDiaAnio.Mes)).ToList();
            var perjornadaTrabajadaAll = this.Context.PersJornadasTrabajadas.Where(p => listIds.Contains(p.DiagramaInspectoresTurnoId) && (p.Fecha.Year == DiagramaInspDiaAnio.Anio && p.Fecha.Month == DiagramaInspDiaAnio.Mes) && p.IsDeleted == false).ToList();


            foreach (var ditId in DiagramasInspectoresTurnoId)
            {
                var hfrancodiagrama = hfrancodiagramaAll.Where(f => f.DiagramaInspectoresTurnoId == ditId.Id);
                var perjornadaTrabajada = perjornadaTrabajadaAll.Where(p => p.DiagramaInspectoresTurnoId == ditId.Id);

                if (hfrancodiagrama != null)
                {
                    foreach (var hf in hfrancodiagrama)
                    {
                        hfrancosDiagrama.Add(hf);
                    }
                }

                if (perjornadaTrabajada != null)
                {
                    foreach (var pjt in perjornadaTrabajada)
                    {
                        persJorTraDiagrama.Add(pjt);
                    }
                }
            }

            var rangos = this.Context.InspRangosHorario.ToList();
            var novedades = this.Context.Novedades.ToList();
            var zonas = this.Context.InspZonas.ToList();

            var cantDias = DateTime.DaysInMonth(diagrama.Anio, diagrama.Mes);

            #region Recuperar Descripción GrupoInspector y Estado

            var grupoInsp = this.Context.InspGruposInspectores.Where(g => g.Id == diagrama.GrupoInspectoresId).FirstOrDefault();

            if(grupoInsp.LineaId == null)
            {
                throw new ValidationException("No existe Linea para el Grupo de Inspectores");
            }


            DiagramaInspDiaAnio.GrupoInspectores = grupoInsp.Descripcion;
            DiagramaInspDiaAnio.GrupoInspectoresId = grupoInsp.Id;

            var estadoDiag = this.Context.InspEstadosDiagrama.Where(e => e.Id == diagrama.EstadoDiagramaId).FirstOrDefault();
            DiagramaInspDiaAnio.Estado = estadoDiag.Descripcion;

            #endregion

            #region Recuperar los Inspectores del GrupoInspector

            List<InspectorDiaDto> inspectoresPorGrupo = new List<InspectorDiaDto>();

            foreach (var diagramaInspectorTurnoId in DiagramasInspectoresTurnoId)
            {

                List<InspectorDiaDto> inspectoresPorTurnos = new List<InspectorDiaDto>();

                var sp = this.Context.LoadStoredProc("dbo.sp_Diagramacion_RecuperarInspectoresporGrupo")

                    .WithSqlParam("grupoid", new SqlParameter("grupoid", grupoInsp.Id))
                    .WithSqlParam("DiagramaInspectoresTurnoId", new SqlParameter("DiagramaInspectoresTurnoId", diagramaInspectorTurnoId.Id));

                await sp.ExecuteStoredProcAsync((handler) =>
                {
                    inspectoresPorTurnos = handler.ReadToList<InspectorDiaDto>().ToList();
                });

                if (inspectoresPorTurnos.Count() > 0)
                {
                    var turno = this.Context.PersTurnos.FirstOrDefault(t => t.Id == diagramaInspectorTurnoId.TurnoId);
                    foreach (var dig in inspectoresPorTurnos)
                    {
                        dig.InspColor = turno.Color;
                        dig.InspTurno = turno.DscTurno;
                        dig.InspTurnoId = turno.Id;
                        dig.GrupoInspectoresId = diagrama.GrupoInspectoresId;
                        inspectoresPorGrupo.Add(dig);
                    }
                }


            }
            #endregion

            if (inspectoresPorGrupo.Count() > 0)
            {
                #region Recuperar los Dias
                List<DiasMesDto> dias = new List<DiasMesDto>();
                List<DiasMesDto> diasAP = new List<DiasMesDto>();

                List<DiasMesDto> diascolor = new List<DiasMesDto>();

                var spd = this.Context.LoadStoredProc("dbo.sp_Diagramacion_RecuperarDiaColor")

                    .WithSqlParam("mes", new SqlParameter("mes", diagrama.Mes))
                    .WithSqlParam("anio", new SqlParameter("anio", diagrama.Anio))
                    .WithSqlParam("linea", new SqlParameter("linea", grupoInsp.LineaId));

                await spd.ExecuteStoredProcAsync((handler) =>
                {
                    diascolor = handler.ReadToList<DiasMesDto>().ToList();
                });


                if(diascolor.Count>0)
                    diasAP.Add(diascolor[0]);


                for (int i = 1; i <= cantDias; i++)
                {
                    var dia = new DiasMesDto();
                    dia.NumeroDia = i;
                    DateTime fecha = new DateTime(diagrama.Anio, diagrama.Mes, i, 0, 0, 0);

                    var diaColor = diascolor.FirstOrDefault(di => di.Fecha == fecha);

                    dia.Fecha = fecha;

                    if (diaColor != null)
                    {
                        dia.Color = diascolor.Where(di => di.Color != null && di.Fecha == fecha).Select(e => e.Color).FirstOrDefault();
                        dia.EsFeriadoString = diascolor.Where(di => di.EsFeriadoString != null && di.Fecha == fecha).Select(e => e.EsFeriadoString).FirstOrDefault();

                    }

                    var d = ((int)fecha.DayOfWeek);
                    switch (d)
                    {
                        case 0:
                            dia.NombreDia = "Dom";
                            break;
                        case 1:
                            dia.NombreDia = "Lun";
                            break;
                        case 2:
                            dia.NombreDia = "Mar";
                            break;
                        case 3:
                            dia.NombreDia = "Mie";
                            break;
                        case 4:
                            dia.NombreDia = "Jue";
                            break;
                        case 5:
                            dia.NombreDia = "Vie";
                            break;
                        case 6:
                            dia.NombreDia = "Sab";
                            break;
                    }

                    dias.Add(dia);
                }

                if(diascolor.Count>0)
                    diasAP.Add(diascolor[diascolor.Count() - 1]);

                DiagramaInspDiaAnio.DiasMesAP = diasAP;

                foreach (var dia in DiagramaInspDiaAnio.DiasMesAP)
                {
                    dia.Inspectores = new List<InspectorDiaDto>();
                    foreach (var insp in inspectoresPorGrupo)
                    {
                        InspectorDiaDto inspector = new InspectorDiaDto();
                        //DateTime day = new DateTime(diagrama.Anio, diagrama.Mes, dia.NumeroDia);
                        inspector.CodEmpleado = insp.CodEmpleado;
                        inspector.Legajo = insp.Legajo;
                        inspector.Apellido = insp.Apellido;
                        inspector.Nombre = insp.Nombre;
                        inspector.InspColor = insp.InspColor;
                        inspector.InspTurno = insp.InspTurno;
                        inspector.InspTurnoId = insp.InspTurnoId;
                        //inspector.GrupoInspectoresId = insp.GrupoInspectoresId;
                        var jorTrabajada = this.Context.PersJornadasTrabajadas.FirstOrDefault(j => j.Fecha == dia.Fecha && j.CodEmpleado == insp.CodEmpleado);

                        if (jorTrabajada != null && jorTrabajada.DiagramaInspectoresTurnoId != null)
                        {
                            //JornadaTrabajada

                            inspector.EsJornada = true;
                            inspector.PasadaSueldos = jorTrabajada.PasadaSueldos;
                            inspector.CodJornada = jorTrabajada.Id;
                            //var rango = this.Context.InspRangosHorario.FirstOrDefault(t => t.Id == jorTrabajada.RangoHorarioId);
                            inspector.HoraDesdeModificada = jorTrabajada.HoraDesdeModif;
                            inspector.HoraHastaModificada = jorTrabajada.HoraHastaModif;
                            //inspector.RangoHorarioId = rango.Id;
                            //inspector.Color = rango.Color;
                           // var zona = this.Context.InspZonas.FirstOrDefault(z => z.Id == jorTrabajada.ZonaId);
                            //inspector.ZonaId = zona.Id;
                            //inspector.NombreZona = zona.Descripcion;
                            //inspector.DetalleZona = zona.Detalle;
                        }
                        else
                        {
                            //Vacio (sin novedad, sin franco, sin jornada trabajada)
                            inspector.EsFranco = false;
                            inspector.EsJornada = false;
                            inspector.EsNovedad = false;
                        }

                        dia.Inspectores.Add(inspector);
                    }
                }

                DiagramaInspDiaAnio.DiasMes = dias;

                #endregion

                #region Diagrama para cada Inspector

                var coEmpleadosList = inspectoresPorGrupo.Select(e => e.CodEmpleado);

                var novxchofLis = this.Context.HNovxchofs.Where(n => coEmpleadosList.Any(e => e == n.Id) && (
                                                                       (n.FecDesde.Year == DiagramaInspDiaAnio.Anio && n.FecDesde.Month == DiagramaInspDiaAnio.Mes) ||
                                                                       (n.FecHasta.Value.Year == DiagramaInspDiaAnio.Anio && n.FecHasta.Value.Month == DiagramaInspDiaAnio.Mes))).ToList();

                var codNovParameter = this.parametersHelper.GetParameter<string>("insp_novedad_permite_franco");
                var colorNovedad = this.parametersHelper.GetParameter<string>("insp_diagrama_novedades_color");


                foreach (var dia in DiagramaInspDiaAnio.DiasMes)
                {
                        DateTime day = new DateTime(diagrama.Anio, diagrama.Mes, dia.NumeroDia);
                    dia.Inspectores = new List<InspectorDiaDto>();
                    foreach (var insp in inspectoresPorGrupo)
                    {
                        //var novxchof = novxchofLis.Where(n => n.Id == insp.CodEmpleado);

                        //var hfranco = hfrancosDiagrama.Where(f => f.Id == insp.CodEmpleado);

                        //var jornadasTrabajada = persJorTraDiagrama.Where(p => p.CodEmpleado == insp.CodEmpleado);

                        InspectorDiaDto inspector = new InspectorDiaDto();
                        inspector.CodEmpleado = insp.CodEmpleado;
                        inspector.Legajo = insp.Legajo;
                        inspector.Apellido = insp.Apellido;
                        inspector.Nombre = insp.Nombre;
                        inspector.InspColor = insp.InspColor;
                        inspector.InspTurno = insp.InspTurno;
                        inspector.InspTurnoId = insp.InspTurnoId;
                        inspector.GrupoInspectoresId = insp.GrupoInspectoresId;

                        var novChof = novxchofLis.FirstOrDefault(n => n.FecDesde <= day && n.FecHasta >= day && n.Id == insp.CodEmpleado);

                        if (novChof == null)
                        {
                            var franco = hfrancosDiagrama.FirstOrDefault(f => f.Fecha == day && f.Id == insp.CodEmpleado);

                            if (franco == null)
                            {
                                var jorTrabajada = persJorTraDiagrama.FirstOrDefault(j => j.Fecha == day && j.CodEmpleado == insp.CodEmpleado);

                                if (jorTrabajada != null)
                                {
                                    //JornadaTrabajada

                                    inspector.EsJornada = true;
                                    inspector.PasadaSueldos = jorTrabajada.PasadaSueldos;
                                    inspector.CodJornada = jorTrabajada.Id;
                                    var rango = rangos.FirstOrDefault(t => t.Id == jorTrabajada.RangoHorarioId);
                                    inspector.HoraDesdeModificada = jorTrabajada.HoraDesdeModif;
                                    inspector.HoraHastaModificada = jorTrabajada.HoraHastaModif;
                                    inspector.RangoHorarioId = rango.Id;
                                    inspector.Color = rango.Color;
                                    var zona = zonas.FirstOrDefault(z => z.Id == jorTrabajada.ZonaId);
                                    inspector.ZonaId = zona.Id;
                                    inspector.NombreZona = zona.Descripcion;
                                    inspector.DetalleZona = zona.Detalle;
                                }
                                else
                                {
                                    //Vacio (sin novedad, sin franco, sin jornada trabajada)
                                    inspector.EsFranco = false;
                                    inspector.EsJornada = false;
                                    inspector.EsNovedad = false;
                                }
                            }
                            else
                            {
                                if (franco.JornadasTrabajadaId.HasValue)
                                {
                                    //FrancoTrabajado
                                    var FrancoTrabajado = persJorTraDiagrama.FirstOrDefault(j => j.Id == franco.JornadasTrabajadaId);
                                    inspector.EsFrancoTrabajado = true;
                                    inspector.EsFranco = true;
                                    inspector.PasadaSueldos = FrancoTrabajado.PasadaSueldos;
                                    inspector.CodJornada = FrancoTrabajado.Id;
                                    var rangoH = rangos.FirstOrDefault(t => t.Id == FrancoTrabajado.RangoHorarioId);
                                    inspector.Color = rangoH.Color;
                                    inspector.HoraDesdeModificada = FrancoTrabajado.HoraDesdeModif;
                                    inspector.HoraHastaModificada = FrancoTrabajado.HoraHastaModif;
                                    inspector.RangoHorarioId = rangoH.Id;
                                    var zona = zonas.FirstOrDefault(z => z.Id == FrancoTrabajado.ZonaId);
                                    inspector.ZonaId = zona.Id;
                                    inspector.NombreZona = zona.Descripcion;
                                    inspector.DetalleZona = zona.Detalle;
                                    inspector.Pago = FrancoTrabajado.Pago.Value? 1 : 0;
                                    

                                }
                                else
                                {
                                    //Franco
                                    inspector.EsFranco = true;
                                    inspector.PasadaSueldos = franco.PasadoSueldos;
                                    var rango = rangos.FirstOrDefault(t => t.Id == franco.RangoHorarioId);
                                    inspector.RangoHorarioId = rango.Id;
                                    inspector.NombreRangoHorario = rango.Descripcion;
                                    inspector.Color = rango.Color;
                                    
                                }
                            }
                        }
                        else
                        {

                            bool param = codNovParameter.Contains(novChof.CodNov.ToString());

                            if (param)
                            {
                                var franco = hfrancosDiagrama.FirstOrDefault(f => f.Fecha == day && f.Id == insp.CodEmpleado);
                                if (franco != null)
                                {
                                    //Novedad con Franco
                                    inspector.EsFranco = true;
                                    inspector.FrancoNovedad = true;
                                    inspector.PasadaSueldos = franco.PasadoSueldos;
                                    var rango = rangos.FirstOrDefault(t => t.Id == franco.RangoHorarioId);
                                    inspector.RangoHorarioId = rango.Id;
                                    inspector.NombreRangoHorario = rango.Descripcion;
                                    inspector.Color = rango.Color;                                    
                                }
                                else
                                {
                                    //Nodavedad sin Franco
                                    inspector.EsNovedad = true;
                                    inspector.FrancoNovedad = true;
                                    inspector.PasadaSueldos = novChof.PasadaSueldos;
                                    var novedad = novedades.FirstOrDefault(n => n.Id == novChof.CodNov);
                                    inspector.DescNovedad = novedad.AbrNov;
                                    inspector.DetalleNovedad = novedad.DesNov;
                                    inspector.Color = colorNovedad;
                                }
                            }
                            else
                            {
                                //Novedad
                                inspector.EsNovedad = true;
                                var novedad = novedades.FirstOrDefault(n => n.Id == novChof.CodNov);
                                inspector.DescNovedad = novedad.AbrNov;
                                inspector.DetalleNovedad = novedad.DesNov;
                                inspector.Color = colorNovedad;
                                
                            }
                        }
                        dia.Inspectores.Add(inspector);
                        if ((inspector.EsFranco && !inspector.EsFrancoTrabajado) || (inspector.EsFranco && inspector.FrancoNovedad))
                        {
                            DiagramaInspDiaAnio.DiasMes[0].Inspectores.FirstOrDefault(i => i.CodEmpleado == insp.CodEmpleado).CantFrancos++;
                        }
                    }
                }
                #endregion
            }

            if (blockentity)
            {
                DiagramaInspDiaAnio.BlockDate = this.Context.BlockEntity(diagrama, this.authService.GetCurretUserId()) ?? throw new InvalidOperationException("No se pudo bloquear entidad");
            }

            return DiagramaInspDiaAnio;       
        }

        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var message = base.GetMachKeySqlException();
            message.Add("UK_insp_DiagramasInspectores_Mes_Anio_GrupoInspectores", "Ya existe diagramación para mes, año y grupo de inspectores seleccionados.");
            return message;
        }

        public override async Task<InspDiagramasInspectores> AddAsync(InspDiagramasInspectores entity)
        {
            try
            {

                //throw new ValidationException("La entidad ya existe");

                DbSet<InspDiagramasInspectores> dbSet = Context.Set<InspDiagramasInspectores>();

                var rangosHorariosPorGrupoDeInspectores = Context.InspGruposInspectoresTurnos.Where(e => e.GrupoInspectoresId == entity.GrupoInspectoresId).ToList();

                foreach (var item in rangosHorariosPorGrupoDeInspectores)
                {
                    entity.InspDiagramaInspectoresTurnos.Add(new InspDiagramasInspectoresTurnos()
                    {
                        DiagramaInspectores = entity,
                        TurnoId = item.TurnoId
                    });
                }

                var entry = await dbSet.AddAsync(entity);
                await this.SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }

        }

        public async Task<DiasMesDto> DiagramacionPorDia(DateTime Fecha)
        {
            var rangos = this.Context.InspRangosHorario;
            var novedades = this.Context.Novedades;
            var zonas = this.Context.InspZonas;

            List<DiasMesDto> diascolor = new List<DiasMesDto>();
            DiasMesDto dia = new DiasMesDto();
            List<InspectorDiaDto> inspectores = new List<InspectorDiaDto>();
            
            var user = this.Context.SysUsersAd.Where(u => u.Id == authService.GetCurretUserId()).FirstOrDefault();
            

            StringBuilder validacion = new StringBuilder();

            if (!user.GruposInspectoresId.HasValue)
            {
                validacion.Append("Comunicarse con Sistemas. Ud. no posee un grupo de inspectores asociado");
                if (!user.EmpleadoId.HasValue)
                {
                    validacion.Append(" y un código de empleado.");
                }
            }
            else
            {
                if (!user.EmpleadoId.HasValue)
                {
                    validacion.Append("Comunicarse con Sistemas. Ud. no posee un código de empleado asociado.");
                }

                var diag = this.Context.InspDiagramasInspectores.Where(e => e.EstadoDiagramaId == InspEstadosDiagramaInspectores.EstadosDiagrama02_Publicado && e.GrupoInspectoresId == user.GruposInspectoresId && e.Anio == Fecha.Year && e.Mes == Fecha.Month).FirstOrDefault();
                if (diag == null)
                {
                    validacion.Append("No existe Diagrama publicado para la fecha solicitada.");
                }
            }


            if (string.IsNullOrEmpty(validacion.ToString()))
            {
                #region Recuperar los Inspectores del GrupoInspector                           


                var sp = this.Context.LoadStoredProc("dbo.sp_Diagramacion_RecuperarInspectoresporGrupo")

                    .WithSqlParam("grupoid", new SqlParameter("grupoid", user.GruposInspectoresId))
                    .WithSqlParam("DiagramaInspectoresTurnoId", new SqlParameter("DiagramaInspectoresTurnoId", DBNull.Value)); ;

                await sp.ExecuteStoredProcAsync((handler) =>
                {
                    inspectores = handler.ReadToList<InspectorDiaDto>().ToList();
                });

                #endregion

                var diagrama = this.Context.InspDiagramasInspectores.FirstOrDefault(d => d.GrupoInspectoresId == user.GruposInspectoresId && d.Mes == Fecha.Month && d.Anio == Fecha.Year);
                if (diagrama != null)
                {
                    var grupoInspectores = await this.Context.InspGruposInspectores.FirstOrDefaultAsync(g => g.Id == user.GruposInspectoresId);
                                        
                    var spd = this.Context.LoadStoredProc("dbo.sp_Diagramacion_RecuperarDiaColor")

                        .WithSqlParam("mes", new SqlParameter("mes", diagrama.Mes))
                        .WithSqlParam("anio", new SqlParameter("anio", diagrama.Anio))
                        .WithSqlParam("linea", new SqlParameter("linea", grupoInspectores.LineaId));

                    await spd.ExecuteStoredProcAsync((handler) =>
                    {
                        diascolor = handler.ReadToList<DiasMesDto>().ToList();
                    });

                    dia = diascolor.FirstOrDefault(d => d.Fecha == Fecha);
                    dia.Inspectores = new List<InspectorDiaDto>();

                    var codNovParameter = this.parametersHelper.GetParameter<string>("insp_novedad_permite_franco");

                    var codEmpleadosList = inspectores.Select(e => e.CodEmpleado);

                    var novxchofList = this.Context.HNovxchofs.Where(n => codEmpleadosList.Any(e => e == n.Id) && (n.FecDesde <= Fecha && n.FecHasta >= Fecha));

                    var francosList = this.Context.HFrancos.Where(f => codEmpleadosList.Any(e => e == f.Id) && (f.Fecha == Fecha));

                    var jorTrabajadaList = this.Context.PersJornadasTrabajadas.Where(p => codEmpleadosList.Any(e => e == p.CodEmpleado) && (p.Fecha == Fecha) && p.IsDeleted == false);


                    #region Recuperar Dia Para cada Inspector
                    foreach (var insp in inspectores)
                    { 
                        insp.InspTurno = this.Context.PersTurnos.FirstOrDefault(t => t.Id == insp.InspTurnoId).DscTurno;
                        var novxchof = novxchofList.FirstOrDefault(n => n.Id == insp.CodEmpleado);

                        if (novxchof == null)
                        {
                            var franco = francosList.FirstOrDefault(f => f.Id == insp.CodEmpleado);

                            if (franco == null )
                            {
                                var jorTrabajada = jorTrabajadaList.FirstOrDefault(p => p.CodEmpleado == insp.CodEmpleado);

                                if (jorTrabajada!=null)
                                {
                                    //JornadaTrabajada
                                    insp.EsJornada = true;
                                    var rango = rangos.FirstOrDefault(t => t.Id == jorTrabajada.RangoHorarioId);
                                    insp.HoraDesde = jorTrabajada.HoraDesdeModif;
                                    insp.HoraHasta = jorTrabajada.HoraHastaModif;
                                    insp.RangoHorarioId = rango.Id;
                                    var zona = zonas.Where(z => z.Id == jorTrabajada.ZonaId).FirstOrDefault();
                                    insp.ZonaId = zona.Id;
                                    insp.NombreZona = zona.Descripcion;
                                    insp.DetalleZona = zona.Detalle; 
                                }
                            }
                            else
                            {
                                if (franco.JornadasTrabajadaId.HasValue)
                                {
                                    //FrancoTrabajado
                                    var FrancoTrabajado = jorTrabajadaList.FirstOrDefault(j => j.Id == franco.JornadasTrabajadaId);
                                    insp.EsFranco = true;
                                    insp.EsFrancoTrabajado = true;
                                    var rango = rangos.FirstOrDefault(t => t.Id == FrancoTrabajado.RangoHorarioId);
                                    insp.HoraDesde = FrancoTrabajado.HoraDesdeModif;
                                    insp.HoraHasta = FrancoTrabajado.HoraHastaModif;
                                    insp.RangoHorarioId = rango.Id;
                                    insp.Color = rango.Color;
                                   var zona = zonas.Where(z => z.Id == FrancoTrabajado.ZonaId).FirstOrDefault();
                                    insp.ZonaId = zona.Id;
                                    insp.NombreZona = zona.Descripcion;
                                    insp.DetalleZona = zona.Detalle;
                                   
                                }
                                else
                                {
                                    //Franco
                                    insp.EsFranco = true;
                                    var rango = rangos.FirstOrDefault(t => t.Id == franco.RangoHorarioId);
                                    insp.NombreRangoHorario = rango.Descripcion;
                                    insp.Color = rango.Color;

                                }
                            }
                        }
                        else
                        {
                            bool param = codNovParameter.Contains(novxchof.CodNov.ToString());

                            if (param)
                            {
                                var franco = francosList.FirstOrDefault(f => f.Id == insp.CodEmpleado);

                                if (franco != null)
                                {
                                    //Novedad con Franco
                                    insp.EsFranco = true;
                                    insp.FrancoNovedad = true;
                                    var rango = rangos.FirstOrDefault(t => t.Id == franco.RangoHorarioId);
                                    insp.NombreRangoHorario = rango.Descripcion;
                                    insp.Color = rango.Color;
                                }
                                else
                                {
                                    //Novedad sin Franco
                                    insp.EsNovedad = true;
                                    var novedad = novedades.FirstOrDefault(n => n.Id == novxchof.CodNov);
                                    insp.DescNovedad = novedad.AbrNov;
                                    insp.DetalleNovedad = novedad.DesNov;
                                    insp.Color = this.parametersHelper.GetParameter<string>("insp_diagrama_novedades_color");
                                }
                            }
                            else
                            {
                                //Novedad
                                insp.EsNovedad = true;
                                insp.DetalleNovedad = novedades.Where(n => n.Id == novxchof.CodNov).FirstOrDefault().DesNov;

                                insp.Color = this.parametersHelper.GetParameter<string>("insp_diagrama_novedades_color");
                            }
                        }
                        dia.Inspectores.Add(insp);
                    }
                    #endregion
                }
                else
                {
                    throw new ValidationException("Comunicarse con Sistemas. No existe diagramación para su grupo de inspectores para la fecha consultada.");
                }
            }
            else
            {
                throw new ValidationException(validacion.ToString());
            }

            dia.Inspectores = dia.Inspectores.OrderBy(e => e.DescripcionInspector).ToList();
            var inpsActual = inspectores.FirstOrDefault(e => e.EmpleadoId == user.EmpleadoId);

            if (inpsActual != null)
            {
                dia.Inspectores.Remove(inpsActual);
                dia.Inspectores.Insert(0, inpsActual);
            }

            //var inspectoresOrdenados =  inspectores.OrderBy(e => e.DescripcionInspector);

            ////Ponemos en primer lugar el inspector que esta logueado
            //var inpsActual = inspectores.Where(e => e.EmpleadoId == user.EmpleadoId).FirstOrDefault();
            //if (inpsActual!=null)
            //{
            //    inspectoresOrdenados.ToList().Remove(inpsActual);
            //    inspectoresOrdenados.ToList().Insert(0, inpsActual);
            //}
            
            return dia;
        }

        public async Task<InspectorDiaDto> EliminarCelda(DiasMesDto model)
        {
            InspectorDiaDto inspector = new InspectorDiaDto();
            inspector.Apellido = model.Inspectores[0].Apellido;
            inspector.CodEmpleado = model.Inspectores[0].CodEmpleado;
            inspector.GrupoInspectoresId = model.Inspectores[0].GrupoInspectoresId;

            var blockDate = model.BlockDate;

            if (model.Inspectores[0].EsJornada)
            {
                var jornada = this.Context.PersJornadasTrabajadas.Where(j => j.Id == model.Inspectores[0].CodJornada).FirstOrDefault();

                inspector.EsFranco = false;
                inspector.EsJornada = false;
                inspector.EsNovedad = false;
                inspector.HoraDesde = DateTime.Today;
                inspector.HoraHasta = DateTime.Today;
                await this.ValidarConcurrenciaTurno(jornada.DiagramaInspectoresTurnoId, blockDate);
                this.Context.PersJornadasTrabajadas.Remove(jornada);
                await this.SaveChangesAsync();
            }
            else if (model.Inspectores[0].EsFrancoTrabajado)
            {

                //Elimina pers_jornadasTrabajadas
                var jornada = this.Context.PersJornadasTrabajadas.Where(j => j.Id == model.Inspectores[0].CodJornada).FirstOrDefault();
                await this.ValidarConcurrenciaTurno(jornada.DiagramaInspectoresTurnoId, blockDate);
                this.Context.PersJornadasTrabajadas.Remove(jornada);
                //Elimina h_francos
                var hfranco = this.Context.HFrancos.Where(f => f.JornadasTrabajadaId == jornada.Id).FirstOrDefault();
                this.Context.HFrancos.Remove(hfranco);


                inspector.EsFranco = false;
                inspector.EsJornada = false;
                inspector.EsNovedad = false;

                await this.SaveChangesAsync();
            }
            else if (model.Inspectores[0].EsFranco)
            {
                var hfranco = this.Context.HFrancos.Where(h => h.Id == model.Inspectores[0].CodEmpleado && h.Fecha == model.Fecha).FirstOrDefault();
                await this.ValidarConcurrenciaTurno(hfranco.DiagramaInspectoresTurnoId, blockDate);

                if (model.Inspectores[0].FrancoNovedad)
                {
                    var hNovxchofs = this.Context.HNovxchofs.Where(n => n.Id == model.Inspectores[0].CodEmpleado && (n.FecDesde <= model.Fecha && n.FecHasta >= model.Fecha)).FirstOrDefault();

                    //NovedadHNovxchof
                    inspector.EsNovedad = true;
                    inspector.FrancoNovedad = true;
                    inspector.DescNovedad = this.Context.Novedades.Where(n => n.Id == hNovxchofs.CodNov).FirstOrDefault().AbrNov;
                    inspector.Color = this.parametersHelper.GetParameter<string>("insp_diagrama_novedades_color");
                }
                else
                {
                    inspector.EsFranco = false;
                    inspector.EsJornada = false;
                    inspector.EsNovedad = false;
                }

                this.Context.HFrancos.Remove(hfranco);
                await this.SaveChangesAsync();
            }

            return inspector;
        }

        private async Task ValidarConcurrenciaTurno(int? diagramaInspectoresTurnoId, DateTime? blockDate)
        {
            var diag = this.Context.InspDiagramaInspectoresTurnos.Where(e => e.Id == diagramaInspectoresTurnoId).FirstOrDefault();

            if (!blockDate.HasValue)
            {
                throw new ArgumentException("Debe enviar fecha de bloqueo");
            }

            if (!diagramaInspectoresTurnoId.HasValue)
            {
                throw new ArgumentException("Debe enviar fecha de bloqueo");
            }


            if (diag!=null)
            {
                await this.ValidateCocurrencySave(diag.DiagramaInspectoresId, blockDate.GetValueOrDefault());
            }
        }

        public async Task SaveDiagramacion(List<HFrancos> hFrancos, List<PersJornadasTrabajadas> jornadasTrabajadas)
        {
            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {
                    var ultimoNumero = new SysUltimosNumeros();

                    ultimoNumero = await Context.SysUltimosNumeros.Where(e => e.Id == "pers_jornadasTrabajadas").FirstOrDefaultAsync();
                    List<HFrancos> FrancosAdd = new List<HFrancos>();
                    foreach (var hfranco in hFrancos)
                    {
                        HFrancos oldhFranco = await Context.HFrancos.FirstOrDefaultAsync(hf => hf.Id == hfranco.Id && hf.Fecha == hfranco.Fecha);
                        if (oldhFranco == null)
                        {
                            if (hfranco.JornadasTrabajada != null )
                            {
                                ultimoNumero.UltNumero += 1;
                                hfranco.JornadasTrabajada.Id = ultimoNumero.UltNumero;
                                hfranco.JornadasTrabajadaId = ultimoNumero.UltNumero;
                                
                                Context.PersJornadasTrabajadas.Add(hfranco.JornadasTrabajada);
                            }

                            
                            FrancosAdd.Add(hfranco);
                            //await Context.HFrancos.AddAsync(hfranco);
                            //await this.SaveChangesAsync();
                        }
                        else
                        {

                            if (hfranco.JornadasTrabajada != null)
                            {
                                ultimoNumero.UltNumero += 1;
                                hfranco.JornadasTrabajada.Id = ultimoNumero.UltNumero;
                                hfranco.JornadasTrabajadaId = ultimoNumero.UltNumero;
                                Context.PersJornadasTrabajadas.Add(hfranco.JornadasTrabajada);
                            }
                            oldhFranco.CodNov = hfranco.CodNov;
                            oldhFranco.Observacion = hfranco.Observacion;
                            oldhFranco.RangoHorarioId = hfranco.RangoHorarioId;
                            oldhFranco.DiagramaInspectoresTurnoId = hfranco.DiagramaInspectoresTurnoId;
                            oldhFranco.JornadasTrabajadaId = hfranco.JornadasTrabajadaId;

                            Context.HFrancos.Update(oldhFranco);
                            await this.SaveChangesAsync();
                        }
                    }

                    foreach (var fAdd in FrancosAdd)
                    {
                        Context.HFrancos.Add(fAdd);
                        await this.SaveChangesAsync();
                    }


                    foreach (var persJornada in jornadasTrabajadas.Where(e => e.Id != 0))
                    {

                        PersJornadasTrabajadas oldJornada = await Context.PersJornadasTrabajadas.FirstOrDefaultAsync(j => j.Id == persJornada.Id);

                        oldJornada.CodEmpleado = persJornada.CodEmpleado;
                        oldJornada.CodTurno = persJornada.CodTurno;
                        oldJornada.Duracion = persJornada.Duracion;
                        oldJornada.HoraDesdeModif = persJornada.HoraDesdeModif;
                        oldJornada.HoraHastaModif = persJornada.HoraHastaModif;
                        oldJornada.RangoHorarioId = persJornada.RangoHorarioId;
                        oldJornada.ZonaId = persJornada.ZonaId;
                        oldJornada.CodGalpon = persJornada.CodGalpon;
                        oldJornada.CodArea = persJornada.CodArea;
                        oldJornada.DiagramaInspectoresTurnoId = persJornada.DiagramaInspectoresTurnoId;
                        oldJornada.Pago = persJornada.Pago;

                        Context.PersJornadasTrabajadas.Update(oldJornada);
                        await this.SaveChangesAsync();
                    }

                    

                    foreach (var persJornada in jornadasTrabajadas.Where(e => e.Id == 0).Reverse())
                    {
                        ultimoNumero.UltNumero += 1;

                        persJornada.Id = ultimoNumero.UltNumero;

                        Context.PersJornadasTrabajadas.Add(persJornada);
                        await this.SaveChangesAsync();
                    }

                    Context.Entry(ultimoNumero).State = EntityState.Modified;

                    await this.SaveChangesAsync();

                    ts.Commit();
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        public override async Task DeleteAsync(InspDiagramasInspectores entity)
        {
            try
            {
                DbSet<InspDiagramasInspectores> dbSet = Context.Set<InspDiagramasInspectores>();

                //var estado = Context.InspEstadosDiagrama.Where(e => e.Id == entity.EstadoDiagramaId);

                if (InspEstadosDiagramaInspectores.EstadosDiagrama02_Publicado == entity.EstadoDiagramaId)
                {
                    throw new ValidationException("El diagrama no puede ser borrado porque no está en Borrador.");
                }


                var turnos = Context.InspDiagramaInspectoresTurnos.Where(ti => ti.DiagramaInspectoresId == entity.Id).ToList();
                List<int?> turnosId = new List<int?>();
                foreach (var tur in turnos)
                {
                    turnosId.Add(tur.Id);
                }


                if (this.Context.PersJornadasTrabajadas.FirstOrDefault(e=> e.PasadaSueldos == "S" &&  turnosId.Contains(e.DiagramaInspectoresTurnoId))!=null)
                {
                    throw new ValidationException("El diagrama no puede ser borrado porque existe diagramación pasada a sueldos.");
                }

                if (this.Context.HFrancos.FirstOrDefault(e => e.PasadoSueldos == "S" && turnosId.Contains(e.DiagramaInspectoresTurnoId)) != null)
                {
                    throw new ValidationException("El diagrama no puede ser borrado porque existe diagramación pasada a sueldos.");
                }


                //DateTime novDesde = new DateTime(entity.Anio, entity.Mes, 1);
                //DateTime novHasta = new DateTime(entity.Anio, entity.Mes, DateTime.DaysInMonth(entity.Anio, entity.Mes));
                //var usuarios = this.Context.SysUsersAd.Where(u => u.EmpleadoId !=null && u.GruposInspectoresId == entity.GrupoInspectoresId).ToList();
                //var allEmpleados = usuarios.Select(e => e.EmpleadoId.Value).ToList();

                
                Boolean isValid = this.ValidarNovedadePorDiagrama(entity.Id);
                if (!isValid)
                {
                    throw new ValidationException("El diagrama no puede ser borrado porque existe diagramación pasada a sueldos.");
                }

                var jtrabs = this.Context.PersJornadasTrabajadas.Where(e => turnosId.Contains(e.DiagramaInspectoresTurnoId)).ToList();
                var francos = this.Context.HFrancos.Where(e => turnosId.Contains(e.DiagramaInspectoresTurnoId)).ToList();

                this.Context.PersJornadasTrabajadas.RemoveRange(jtrabs);
                this.Context.HFrancos.RemoveRange(francos);
                this.Context.InspDiagramaInspectoresTurnos.RemoveRange(turnos);
                this.Context.InspDiagramasInspectores.Remove(entity);

                await this.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
        }

        private bool ValidarNovedadePorDiagrama(int idDiagrama)
        {
            Boolean isValid = false;

            var sp = this.Context.LoadStoredProc("dbo.sp_Diagramacion_Novedad_PuedeBorrar")
                    .WithSqlParam("idDiagrama", new SqlParameter("idDiagrama", idDiagrama));

            sp.ExecuteStoredProc((handler) =>
            {
                isValid = handler.ReadToValue<Boolean>().GetValueOrDefault();
            });

            return isValid;
        }

       

    }
}
