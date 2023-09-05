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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TECSO.FWK.Domain.Auditing;
using System.Data.SqlClient;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Interfaces.Services;
using Snickler.EFCore;
using System.ComponentModel.DataAnnotations;
using ROSBUS.Admin.Domain.Entities.Partials;
using TECSO.FWK.Domain.Mail;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.ParametersHelper;
using ROSBUS.Admin.Domain.Entities.AppInspectores;

namespace ROSBUS.infra.Data.Repositories
{
    public class InfInformesRepository : RepositoryBase<AdminContext,InfInformes, string>, IInfInformesRepository
    {
        private readonly IAuthService authService;
        private readonly IDefaultEmailer emailSender;
        private readonly IHServiciosService hServicioService;
        private readonly IEmpleadosService empleadosService;
        private readonly IParametersHelper parametersHelper;
        private readonly ILogger logger;
        private readonly IUserService userService;
        public InfInformesRepository(IAdminDbContext _context, IAuthService _authService, IDefaultEmailer _emailSender, IHServiciosService _hServiciosService,
            IEmpleadosService _empleadosService, IUserService _userService, IParametersHelper _parametersHelper, ILogger logger)
            :base(new DbContextProvider<AdminContext>(_context))
        {
            authService = _authService;
            emailSender = _emailSender;
            hServicioService = _hServiciosService;
            empleadosService = _empleadosService;
            userService = _userService;
            parametersHelper = _parametersHelper;
            this.logger = logger;
        }

        public override Expression<Func<InfInformes, bool>> GetFilterById(string id)
        {
            return e => e.Id == id;
        }

        public override async Task<InfInformes> AddAsync(InfInformes entity)
        {

            try
            {
                var serv = this.Context.HServicios.FirstOrDefault(s => s.Id == int.Parse(entity.NumSer) && s.CodHconfiNavigation.CodHfechaNavigation.CodLin == entity.CodLin);
                if (serv == null)
                {
                    throw new ValidationException("El servicio ingresado no correspode a la Linea seleccionada");
                }

                DbSet<InfInformes> dbSet = Context.Set<InfInformes>();

                #region Numero de servicio
                int numSer = Int32.Parse(entity.NumSer);

                var servicio = this.Context.HServicios.Where(s => s.Id == numSer).FirstOrDefault();
                entity.NumSer = servicio.NumSer.TrimStart('0');
                #endregion

                entity.FecInforme = DateTime.Now;

                entity.CodUsuinf = authService.GetCurretUserId();

                entity.ControlSancion = 0;

                entity.Imprimio = 0;

                entity.Vencido = 0;

                #region Ultimo Numero- Codigo Informe


                var ultimoNumero = new SysUltimosNumeros();

                ultimoNumero = await Context.SysUltimosNumeros.Where(e => e.Id == "inf_informes").FirstOrDefaultAsync();

                ultimoNumero.UltNumero += 1;

                entity.Id = ultimoNumero.UltNumero.ToString().PadLeft(6, '0');
                #endregion



                #region Obtencion codigo Inspector

                var usuario = await userService.GetByIdAsync(entity.CodUsuinf.Value);
                var inspector = await empleadosService.ObtenerEmpleadoPorDNI(usuario.NroDoc.Replace(" ", ""));

                if (inspector != null)
                {
                    entity.CodInspector = inspector.cod_emp;
                    var sucursal = this.Context.SucursalesxEmpresas.Where(s => s.CodEmpr == inspector.codEmpresa);
                    entity.CodSucursalInspector = sucursal.FirstOrDefault().Id;
                }

                #endregion


                #region Numero Informe maximo - Tipo Informe I

                var numMax = this.Context.InfInformes.Where(i => i.TpoInforme == "I").Max(u => u.NroInforme);

                int maxNum = Int32.Parse(numMax);
                maxNum = maxNum += 1;
                entity.NroInforme = maxNum.ToString().PadLeft(6, '0');

                entity.TpoInforme = "I";

                #endregion

                #region Resp Informe de la Linea

                var linea = this.Context.Linea.Where(l => l.Id == entity.CodLin).FirstOrDefault();
                entity.CodRespinf = linea.CodRespInformes;

                #endregion

                var entry = await dbSet.AddAsync(entity);

                Context.Entry(ultimoNumero).State = EntityState.Modified;

                #region Notificacion TecnoBus Informes

                GpsMensajesCoches gpsMensajesCoches = new GpsMensajesCoches();
                var cCoche = await this.Context.CCoches.Where(c => c.Id == entity.NroInterno).FirstOrDefaultAsync();
                HServiciosFilter hSf = new HServiciosFilter();
                var conductores = await hServicioService.RecuperarConductores(hSf);
                var conductor = conductores.FirstOrDefault(c => c.Id == entity.CodEmp);
                string motivo = await this.Context.MotivoInfra.Where(m => m.Id == entity.CodMotivo).Select(m => m.DesMotivo).FirstOrDefaultAsync();
                var gpsEstadosActualesRepli = await this.Context.GpsEstadosActualesRepli.FirstOrDefaultAsync(gps => gps.Ficha == cCoche.Ficha);
                if (gpsEstadosActualesRepli != null)
                {
                    gpsMensajesCoches.Id2 = gpsEstadosActualesRepli.Id;
                    gpsMensajesCoches.CodTdia = gpsEstadosActualesRepli.Codtip.Value;
                    gpsMensajesCoches.Servicio = gpsEstadosActualesRepli.Servi.Value;
                    Int32 legajo = 0;
                    Int32.TryParse(gpsEstadosActualesRepli.Legajo, out legajo);
                    gpsMensajesCoches.Legajo = legajo;
                }
                gpsMensajesCoches.Codigo = 0;
                gpsMensajesCoches.Origen = 0;
                gpsMensajesCoches.Usuario = "App Inspectores";
                gpsMensajesCoches.Texto = ("Informe Infracción Nro. " + entity.NroInforme + " Fecha: " + entity.FecInforme.Value.ToString("dd/MM/yyyy") +
                                            " Hora: " + entity.FecInforme.Value.ToString("HH:mm") +
                                            "  Conductor: " + conductor.Description + " Ubicación: " + entity.DscLugar +
                                            " Motivo: " + motivo + this.parametersHelper.GetParameter<string>("insp_leyendaNotifTecnobus"));

                if ((gpsMensajesCoches.Texto ?? "").Length > 500)
                {
                    gpsMensajesCoches.Texto = gpsMensajesCoches.Texto.Substring(0, 500);
                }

                var fechaHora = DateTime.Now;
                gpsMensajesCoches.Fecha = fechaHora.ToString("dd/MM/yyyy");
                gpsMensajesCoches.Hora = fechaHora.ToString("HH:mm:ss");
                gpsMensajesCoches.CodUsuario = 0;
                gpsMensajesCoches.Dia = fechaHora;
                gpsMensajesCoches.Maquina = "AppInspectores";
                gpsMensajesCoches.Enviado = "N";
                gpsMensajesCoches.Ficha = cCoche.Ficha;
                gpsMensajesCoches.CodLin = entity.CodLin.Value;


                Context.GpsMensajesCoches.Add(gpsMensajesCoches);


                #endregion

                await this.SaveChangesAsync();

                await this.NotificarInforme(entity);



                return entry.Entity;
            }
            catch (Exception ex)
            {
                await this.logger.LogError(ex.Message);
                await this.logger.LogError(ex.StackTrace);
                throw;
            }
        }        


        public async Task<List<Destinatario>> GetNotificacionesMail(string token)
        {
            try
            {
                List<Destinatario> result = new List<Destinatario>();
                var sp = this.Context.LoadStoredProc("dbo.GetNotificacionesMail")
                    .WithSqlParam("Token", new SqlParameter("Token", token));


                await sp.ExecuteStoredProcAsync((handler) =>
                {
                    result = handler.ReadToList<Destinatario>().ToList();
                });

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task NotificarInforme(InfInformes entity)
        {
            var usuarioCurren = await this.Context.SysUsersAd.Where(u => u.Id == entity.CodUsuinf).FirstOrDefaultAsync();

            if (usuarioCurren.GruposInspectoresId.HasValue)
            {

                var notificacionId = await this.Context.InspGruposInspectores.Where(g => g.Id == usuarioCurren.GruposInspectoresId).FirstOrDefaultAsync();

                if (notificacionId.NotificacionId.HasValue)
                {
                    var token = await this.Context.Notification.Where(n => n.Id == notificacionId.NotificacionId).FirstOrDefaultAsync();
                    string motivo = await this.Context.MotivoInfra.Where(m => m.Id == entity.CodMotivo).Select(m => m.DesMotivo ).FirstOrDefaultAsync();
                    HServiciosFilter hSf = new HServiciosFilter();
                    //TODO Conductores recuperar con el nuevo campo empleado de SysUserAD
                    var conductores = await hServicioService.RecuperarConductores(hSf);
                    var conductor =  conductores.Where(c => c.Id == entity.CodEmp).FirstOrDefault();
                    string desLinea = await this.Context.Linea.Where(l => l.Id == entity.CodLin).Select(l => l.DesLin).FirstOrDefaultAsync();
                    var cCoche = await this.Context.CCoches.Where(c => c.Id == entity.NroInterno).FirstOrDefaultAsync();
                    string notificado = entity.Notificado == 0 ? "NO" : "SI";
                    string fechaNotificado = entity.FechaNotificado.HasValue ? entity.FechaNotificado.Value.ToString("dd/MM/yyyy") : "-";
                    

                    StringBuilder arrayforerrors = new StringBuilder("<b></b>");
                    arrayforerrors.AppendLine();
                    //arrayforerrors.AppendFormat("Excel {0}", file);
                    //arrayforerrors.AppendLine();
                    arrayforerrors.AppendFormat(@"<table style=''>
                                                    <thead> 
                                                        <p>Informe de Infracción Nro. {0}</p>
                                                        <p>Fecha: {1}   Hora: {2}</p>
                                                        <p>Inspector: {15}</p>
                                                        <strong>Datos Conductor </strong>
                                                        <p>Conductor: {3} - {4}</p>
                                                        <p>Linea: {5}   Servicio: {6}   Coche: {7} - {8} - {9}</p>
                                                        <strong>Datos Infracción</strong>
                                                        <p>Ubicación: {10}</p>
                                                        <p>Motivo: {11}  </p>
                                                        <p>Notificado: {12}   Fecha Notificado: {13}</p>
                                                        <p>Observaciones: {14}</p>
                                                    </thead>
                                                </table>", entity.NroInforme, entity.FecInfraccion.Value.ToString("dd/MM/yyyy"), entity.FecInfraccion.Value.ToString("HH:mm"),
                                                            conductor.Legajo, conductor.Description, desLinea, entity.NumSer, cCoche.Ficha, cCoche.Interno ,cCoche.Dominio, 
                                                            entity.DscLugar, motivo, notificado, fechaNotificado, string.IsNullOrEmpty(entity.ObsInforme)? "": entity.ObsInforme, usuarioCurren.NomUsuario);
                   
                

                List<Destinatario> destinatarios = await this.GetNotificacionesMail(token.Token);
                    if (destinatarios != null && destinatarios.Count >= 1)
                    {
                        //await this._logger.LogInformation($"Vamos a enviar los mails a {string.Join(";", destinatarios.Select(d => d.Email))}. ");

                        foreach (var mail in destinatarios)
                        {
                            await this.emailSender.SendDefaultAsync(mail.Email, "Informe de Infracción nro." + entity.NroInforme +" - " + motivo, arrayforerrors.ToString());
                        }
                    }
                }
            }
        }

        public async Task<List<InformeConsulta>> ConsultaInformesUserDia ()
        {
            var user = authService.GetCurretUserId();

            var informes =  this.Context.InfInformes.Where(i => i.CodUsuinf == user && i.FecInforme.Value.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy")).OrderByDescending(f => f.FecInforme);

            List<InformeConsulta> informesConsultas = new List<InformeConsulta>();

            if (informes.ToList().Count() > 0)
            {
                HServiciosFilter hsf = new HServiciosFilter();
                var conductores = await this.hServicioService.RecuperarConductores(hsf);

                foreach (var i in informes.ToList())
                {
                    InformeConsulta infoConsul = new InformeConsulta();
                    infoConsul.fechaInforme = i.FecInforme.Value.ToString("dd/MM/yyyy HH:mm");
                    infoConsul.fechaInfraccion = i.FecInfraccion.Value.ToString("dd/MM/yyyy HH:mm");
                    infoConsul.fechaNotificado = i.FechaNotificado.HasValue ? i.FechaNotificado.Value.ToString("dd/MM/yyyy") : null;
                    infoConsul.notificado = i.Notificado == 0 ? "No" : "Si";
                    infoConsul.obsInforme = i.ObsInforme;
                    infoConsul.servicio = i.NumSer;
                    infoConsul.descLugar = i.DscLugar;
                    infoConsul.descMotivo = this.Context.MotivoInfra.Where(m => m.Id == i.CodMotivo).FirstOrDefault().DesMotivo;
                    infoConsul.descLinea = this.Context.Linea.Where(l => l.Id == i.CodLin).FirstOrDefault().DesLin;
                    var coche = this.Context.CCoches.Where(c => c.Id == i.NroInterno).FirstOrDefault();
                    infoConsul.ficha = coche.Ficha.ToString();
                    infoConsul.interno = coche.Interno;
                    infoConsul.dominio = coche.Dominio;
                    var conductor = conductores.Where(co => co.Id == i.CodEmp).FirstOrDefault();
                    infoConsul.naConductor = conductor.Description;
                    infoConsul.legConductor = conductor.Legajo;
                    infoConsul.numInforme = i.NroInforme;
                    informesConsultas.Add(infoConsul);
                    
                }
            }

            return informesConsultas;

        }        


    }
}
