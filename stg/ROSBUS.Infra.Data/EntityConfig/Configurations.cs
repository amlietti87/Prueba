using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;


namespace ROSBUS.infra.Data.EntityConfig
{
    //public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    //{
   

    //    public void Configure(EntityTypeBuilder<Producto> builder)
    //    {

    //        builder.HasKey(p => p.Id);

    //        builder.Property(p => p.Nome)
    //            .IsRequired()
    //            .HasMaxLength(250);

    //        builder.Property(p => p.Valor)
    //            .IsRequired();

    //        //builder.HasRequired(p => p.Cliente)
    //        //    .WithMany()
    //        //    .HasForeignKey(p => p.ClienteId);
    //    }
    //}
    public static class Configuration
    {
        public static void ApplyConfigurations(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PlaParadas>(entity =>
            {
                entity.ToTable("pla_paradas");

                entity.Property(e => e.Calle)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Cruce)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Lat).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Long).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.DropOffType).HasColumnName("drop_off_type");

                entity.Property(e => e.LocationType).HasColumnName("location_type");

                entity.Property(e => e.ParentStationId).HasColumnName("parentStationID");

                entity.Property(e => e.PickUpType).HasColumnName("pick_up_type");

                entity.Property(e => e.Sentido)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.ParentStation)
                    .WithMany()
                    .HasForeignKey(d => d.ParentStationId)
                    .HasConstraintName("FK_pla_Paradas_pla_Paradas");
            });

            #region "Planificacion"

            modelBuilder.Entity<PlaSentidoBanderaSube>(entity =>
            {
                entity.ToTable("pla_SentidoBanderaSube");
            });


            modelBuilder.Entity<PlaCodigoSubeBandera>(entity =>
            {
                entity.ToTable("pla_CodigoSubeBandera");

                entity.Property(e => e.CodBan).HasColumnName("cod_ban");

                entity.Property(e => e.CodEmpresa)
                    .HasColumnName("cod_empresa")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.CodBanNavigation)
                    .WithMany(p => p.PlaCodigoSubeBandera)
                    .HasForeignKey(d => d.CodBan)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_CodigoSubeBandera_h_banderas");


                entity.HasOne(d => d.PlaSentidoBanderaSubeNavigation)
                    .WithMany(p => p.PlaCodigoSubeBandera)
                    .HasForeignKey(d => d.SentidoBanderaSubeId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_CodigoSubeBandera_pla_SentidoBanderaSub");

                entity.HasOne(d => d.CodEmpresaNavigation)
                    .WithMany(p => p.PlaCodigoSubeBandera)
                    .HasForeignKey(d => d.CodEmpresa)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_CodigoSubeBandera_empresa");
            });
            modelBuilder.Entity<PlaCoordenadas>(entity =>
            {
                entity.ToTable("pla_Coordenadas");

                entity.Property(e => e.Id);

                entity.Property(e => e.Abreviacion).HasMaxLength(100);

                entity.Property(e => e.CodigoNombre)
                    .IsRequired()
                    .HasMaxLength(100);


                entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Lat).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Long).HasColumnType("decimal(10, 6)");
            });
            modelBuilder.Entity<PlaEstadoRuta>(entity =>
            {
                entity.ToTable("pla_EstadoRuta");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre).HasMaxLength(50);
            });
            modelBuilder.Entity<PlaGrupoLineas>(entity =>
            {
                entity.ToTable("pla_GrupoLineas");

                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.HasOne(d => d.Sucursal)
                    .WithMany(p => p.PlaGrupoLineas)
                    .HasForeignKey(d => d.SucursalId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_GrupoLineas_sucursales");
            });
            modelBuilder.Entity<PlaRamalColor>(entity =>
            {
                entity.ToTable("pla_RamalColor");

                entity.Property(e => e.ColorTupid).HasColumnName("ColorTUPId");
                entity.Property(e => e.RouteLongName).HasColumnName("route_long_name");
                entity.Property(e => e.RouteShortName).HasColumnName("route_short_name");

                //entity.Property(e => e.LineaId).HasColumnName("LineaId");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.BanderaTup)
                    .WithMany(p => p.PlaRamalColor)
                    .HasForeignKey(d => d.ColorTupid)
                    .HasConstraintName("FK_pla_RamalColor_h_banderasTUP");

                entity.HasOne(d => d.PlaLinea)
                    .WithMany(p => p.RamalColor)
                    .HasForeignKey(d => d.LineaId);
                ////.OnDelete(DeleteBehavior.ClientSetNull)
                //.HasConstraintName("FK_pla_RamalColor_pla_Linea");
            });

            modelBuilder.Entity<MotivoInfra>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("motivo_infra");

                entity.HasIndex(e => e.DesMotivo)
                    .HasName("motivo_infra_uk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("cod_motivo")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CodMotivoBa)
                    .HasColumnName("cod_motivo_ba")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.DesMotivo)
                    .IsRequired()
                    .HasColumnName("des_motivo")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.PtosEvaluacion)
                    .HasColumnName("ptos_evaluacion")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Tipo)
                    .HasColumnName("tipo")
                    .HasColumnType("numeric(1, 0)");

                entity.Property(e => e.TpoEnfoque)
                    .HasColumnName("tpoEnfoque")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InfInformes>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("inf_informes");

                entity.HasIndex(e => new { e.NroInforme, e.TpoInforme })
                    .HasName("ix_01_inf_informes");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_informe")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Anulado)
                    .HasColumnName("anulado")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.CodEmp)
                    .HasColumnName("cod_emp")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CodEmpr).HasColumnName("cod_empr");

                entity.Property(e => e.CodGalpon).HasColumnName("cod_galpon");

                entity.Property(e => e.CodInformeBa)
                    .HasColumnName("cod_informe_ba")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.CodInspector)
                    .HasColumnName("cod_inspector")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CodLin).HasColumnName("cod_lin");

                entity.Property(e => e.CodModrespsancion)
                    .HasColumnName("cod_modrespsancion")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CodMotivo)
                    .HasColumnName("cod_motivo")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CodRespanulacion)
                    .HasColumnName("cod_respanulacion")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CodRespcarta)
                    .HasColumnName("cod_respcarta")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CodRespinf)
                    .HasColumnName("cod_respinf")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CodRespsancion)
                    .HasColumnName("cod_respsancion")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CodSucursalInspector).HasColumnName("cod_sucursal_inspector");

                entity.Property(e => e.CodUsuanulacion).HasColumnName("cod_usuanulacion");

                entity.Property(e => e.CodUsuarioAnulacion).HasColumnName("cod_usuarioAnulacion");

                entity.Property(e => e.CodUsucarta).HasColumnName("cod_usucarta");

                entity.Property(e => e.CodUsuinf).HasColumnName("cod_usuinf");

                entity.Property(e => e.CodUsumodsancion).HasColumnName("cod_usumodsancion");

                entity.Property(e => e.CodUsusancion).HasColumnName("cod_ususancion");

                entity.Property(e => e.ControlSancion).HasColumnName("control_sancion");

                entity.Property(e => e.DiasModsancion).HasColumnName("dias_modsancion");

                entity.Property(e => e.DiasSancion).HasColumnName("dias_sancion");

                entity.Property(e => e.DscLugar)
                    .HasColumnName("dsc_lugar")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FecAnulacion)
                    .HasColumnName("fec_anulacion")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FecCartadocumento)
                    .HasColumnName("fec_cartadocumento")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FecInforme)
                    .HasColumnName("fec_informe")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FecInfraccion)
                    .HasColumnName("fec_infraccion")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FecIniciosancion)
                    .HasColumnName("fec_iniciosancion")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FecModiniciosancion)
                    .HasColumnName("fec_modiniciosancion")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FecModsancion)
                    .HasColumnName("fec_modsancion")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FecSancion)
                    .HasColumnName("fec_sancion")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FechaNotificado)
                    .HasColumnName("fechaNotificado")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Imprimio).HasColumnName("imprimio");

                entity.Property(e => e.Latitud)
                    .HasColumnName("latitud")
                    .HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Longitud)
                    .HasColumnName("longitud")
                    .HasColumnType("decimal(10, 6)");

                entity.Property(e => e.LugarHecho).HasColumnName("lugarHecho");

                entity.Property(e => e.Notificado).HasColumnName("notificado");

                entity.Property(e => e.NroCartadocumento)
                    .HasColumnName("nro_cartadocumento")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NroInforme)
                    .HasColumnName("nro_informe")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.NroInterno)
                    .HasColumnName("nro_interno")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.NumSer)
                    .HasColumnName("num_ser")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.ObsAnulacion)
                    .HasColumnName("obs_anulacion")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ObsCartadocumento)
                    .HasColumnName("obs_cartadocumento")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ObsInforme)
                    .HasColumnName("obs_informe")
                    .HasMaxLength(700)
                    .IsUnicode(false);

                entity.Property(e => e.ObsModsancion)
                    .HasColumnName("obs_modsancion")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ObsSancion)
                    .HasColumnName("obs_sancion")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.OrigenInforme)
                    .HasColumnName("origen_informe")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('C')");

                entity.Property(e => e.Sancion)
                    .HasColumnName("sancion")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.TpoInforme)
                    .HasColumnName("tpo_informe")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TpoModsancion)
                    .HasColumnName("tpo_modsancion")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TpoSancion)
                    .HasColumnName("tpo_sancion")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Vencido).HasColumnName("vencido");
            });

            modelBuilder.Entity<InspGruposInspectores>(entity =>
            {
                entity.ToTable("insp_GruposInspectores");

                entity.Property(e => e.LineaId).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Linea)
                   .WithMany()
                   .HasForeignKey(d => d.LineaId)
                   ;
            });

            modelBuilder.Entity<PersTopesHorasExtras>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("pers_topesHorasExtras");

                entity.HasIndex(e => new { e.CodGalpon, e.CodArea })
                    .HasName("IX_pers_topesHorasExtras")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("cod_topeHorasExtras")
                    .ValueGeneratedNever();

                entity.Property(e => e.CodArea).HasColumnName("cod_area");

                entity.Property(e => e.CodGalpon).HasColumnName("cod_galpon");

                entity.Property(e => e.FeriadosPersona).HasColumnName("feriadosPersona");

                entity.Property(e => e.FeriadosTaller).HasColumnName("feriadosTaller");

                entity.Property(e => e.FrancosTrabajadosPersona).HasColumnName("francosTrabajadosPersona");

                entity.Property(e => e.FrancosTrabajadosTaller).HasColumnName("francosTrabajadosTaller");

                entity.Property(e => e.HoraFeriado)
                    .HasColumnName("horaFeriado")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraFranco)
                    .HasColumnName("horaFranco")
                    .HasColumnType("datetime");

                entity.Property(e => e.HorasComunes)
                    .HasColumnName("horasComunes")
                    .HasColumnType("datetime");

                entity.Property(e => e.Hs50Persona).HasColumnName("hs50Persona");

                entity.Property(e => e.Hs50Taller).HasColumnName("hs50Taller");

                entity.Property(e => e.IdGrupoInspectores).HasColumnName("id_grupoInspectores");

                entity.Property(e => e.MinutosNocturnosPersona).HasColumnName("minutosNocturnosPersona");

                entity.Property(e => e.MinutosNocturnosTaller).HasColumnName("minutosNocturnosTaller");

                entity.Property(e => e.PermiteFrancosTrabajadosFeriado).HasColumnName("permiteFrancosTrabajadosFeriado");

                entity.Property(e => e.PermiteHsExtrasFeriado).HasColumnName("permiteHsExtrasFeriado");
            });


            modelBuilder.Entity<InspDiagramasInspectores>(entity =>
            {
                entity.ToTable("insp_DiagramasInspectores");
                

                entity.HasOne(d => d.EstadoDiagrama)
                    .WithMany(p => p.InspDiagramasInspectores)
                    .HasForeignKey(d => d.EstadoDiagramaId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_DiagramasInspectores_insp_EstadosDiagrama2");

                entity.HasOne(d => d.GrupoInspectores)
                    .WithMany(p => p.InspDiagramasInspectores)
                    .HasForeignKey(d => d.GrupoInspectoresId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_DiagramasInspectores_insp_GruposInspectores1");
            });          

            modelBuilder.Entity<InspEstadosDiagramaInspectores>(entity =>
            {
                entity.ToTable("insp_EstadosDiagramaInspectores");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EsBorrador).HasColumnName("esBorrador");
            });

            modelBuilder.Entity<InspGrupoInspectoresRangosHorarios>(entity =>
            {
                entity.ToTable("insp_GrupoInspectoresRangosHorarios");

                entity.HasOne(d => d.GrupoInspectores)
                    .WithMany(p => p.InspGrupoInspectoresRangosHorarios)
                    .HasForeignKey(d => d.GrupoInspectoresId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_GrupoInspectoresTurnos_insp_GruposInspectores1");

                entity.HasOne(d => d.RangoHorario)
                    .WithMany(p => p.InspGrupoInspectoresRangosHorarios)
                    .HasForeignKey(d => d.RangoHorarioId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_GrupoInspectoresTurnos_insp_turnos1");
            });

            modelBuilder.Entity<InspGrupoInspectoresZona>(entity =>
            {
                entity.ToTable("insp_GrupoInspectoresZona");

                entity.HasOne(d => d.GrupoInspectores)
                    .WithMany(p => p.InspGrupoInspectoresZona)
                    .HasForeignKey(d => d.GrupoInspectoresId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_GrupoInspectoresZona_insp_GruposInspectores");

                entity.HasOne(d => d.Zona)
                    .WithMany(p => p.InspGrupoInspectoresZona)
                    .HasForeignKey(d => d.ZonaId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_GrupoInspectoresZona_insp_zonas");
            });

            modelBuilder.Entity<InspGrupoInspectoresTarea>(entity =>
            {
                entity.ToTable("insp_GrupoInspectoresTareas");
                entity.HasKey("Id");
                entity.Property("TareaId").IsRequired();
                entity.Property("GrupoInspectoresId").IsRequired();

                entity.HasOne(d => d.GrupoInspectores)
                    .WithMany(p => p.InspGrupoInspectoresTareas)
                    .HasForeignKey(d => d.GrupoInspectoresId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_GrupoInspectoresTareas_insp_GruposInspectores");

                entity.HasOne(d => d.Tarea)
                    .WithMany(p => p.GruposInspectoresTareas)
                    .HasForeignKey(d => d.TareaId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_GrupoInspectoresTareas_insp_Tareas");
            });


            //modelBuilder.Entity<InspTiposTarea>(entity =>
            //{
            //    entity.ToTable("insp_TiposTarea");

            //    entity.Property(e => e.Descripcion)
            //        .IsRequired()
            //        .HasMaxLength(100);
            //});

            modelBuilder.Entity<InspTopesFrancosGruposInspectores>(entity =>
            {
                entity.ToTable("insp_TopesFrancosGruposInspectores");

                entity.HasOne(d => d.GrupoInspectores)
                    .WithMany(p => p.InspTopesFrancosGruposInspectores)
                    .HasForeignKey(d => d.GrupoInspectoresId)
                    .HasConstraintName("FK_insp_TopesFrancosGruposInspectores_insp_GruposInspectores");
            });

            modelBuilder.Entity<InspTopesGruposInspectores>(entity =>
            {
                entity.ToTable("insp_TopesGruposInspectores");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.GrupoInspectores)
                    .WithMany(p => p.InspTopesGruposInspectores)
                    .HasForeignKey(d => d.GrupoInspectoresId)
                    .HasConstraintName("FK_insp_TopesGruposInspectores_insp_GruposInspectores");
            });
            modelBuilder.Entity<InspRangosHorarios>(entity =>
            {
                entity.ToTable("insp_RangosHorarios");

                entity.Property(e => e.Color).HasMaxLength(100);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EsFranco).HasColumnName("esFranco");

                entity.Property(e => e.EsFrancoTrabajado).HasColumnName("esFrancoTrabajado");
            });

            modelBuilder.Entity<InspZonas>(entity =>
            {
                entity.ToTable("insp_Zonas");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Detalle).HasMaxLength(100);
            });

            modelBuilder.Entity<PersTurnos>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("pers_turnos");

                entity.HasIndex(e => e.DscTurno)
                    .HasName("pers_Turnos_uk")
                    .IsUnique();

                entity.Property(e => e.Color)
                    .HasColumnName("color")
                    .HasMaxLength(50);

                entity.Property(e => e.Id)
                    .HasColumnName("cod_turno")
                    .ValueGeneratedNever();

                entity.Property(e => e.DscTurno)
                    .IsRequired()
                    .HasColumnName("dsc_turno")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Orden).HasColumnName("orden");
            });

            modelBuilder.Entity<InspDiagramasInspectoresTurnos>(entity =>
            {
                entity.ToTable("insp_DiagramaInspectoresTurnos");


                entity.HasOne(d => d.DiagramaInspectores)
                    .WithMany(p => p.InspDiagramaInspectoresTurnos)
                    .HasForeignKey(d => d.DiagramaInspectoresId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_DiagramaInspectoresTurnos_insp_DiagramasInspectores");

                entity.HasOne(d => d.Turno)
                    .WithMany(p => p.InspDiagramaInspectoresTurnos)
                    .HasForeignKey(d => d.TurnoId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_DiagramaInspectoresTurnos_insp_Turnos");
            });

            modelBuilder.Entity<InspGruposInspectoresTurnos>(entity =>
            {
                entity.ToTable("insp_GruposInspectoresTurnos");


                entity.HasOne(d => d.GrupoInspectores)
                    .WithMany(p => p.InspGruposInspectoresTurnos)
                    .HasForeignKey(d => d.GrupoInspectoresId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_GruposInspectoresTurnos_insp_GruposInspectores");

                entity.HasOne(d => d.Turno)
                    .WithMany(p => p.InspGruposInspectoresTurnos)
                    .HasForeignKey(d => d.TurnoId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_GruposInspectoresTurnos_pers_turnos");
            });

            modelBuilder.Entity<PersJornadasTrabajadas>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("pers_jornadasTrabajadas");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_jornadaTrabajada")
                    .ValueGeneratedNever();

                entity.Property(e => e.CodArea).HasColumnName("cod_area");

                entity.Property(e => e.CodEmpleado)
                    .HasColumnName("cod_empleado")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CodGalpon).HasColumnName("cod_galpon");

                entity.Property(e => e.CodJornadaTrabajadabsas).HasColumnName("cod_jornadaTrabajadabsas");

                entity.Property(e => e.CodTurno).HasColumnName("cod_turno");

                entity.Property(e => e.Pago).HasColumnName("pago");

                entity.Property(e => e.Duracion)
                    .HasColumnName("duracion")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraDesde)
                    .HasColumnName("horaDesde")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraDesdeModif)
                    .HasColumnName("horaDesdeModif")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraHasta)
                    .HasColumnName("horaHasta")
                    .HasColumnType("datetime");

                entity.Property(e => e.HoraHastaModif)
                    .HasColumnName("horaHastaModif")
                    .HasColumnType("datetime");

                entity.Property(e => e.HorasDescanso)
                    .HasColumnName("horasDescanso")
                    .HasColumnType("datetime");

                entity.Property(e => e.PasadaSueldos)
                    .HasColumnName("pasadaSueldos")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.TpoServicio)
                    .IsRequired()
                    .HasColumnName("tpo_servicio")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HFrancos>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Fecha });

                entity.ToTable("h_francos");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_emp")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.CodNov).HasColumnName("cod_nov");

                entity.Property(e => e.CodUsu)
                    .HasColumnName("cod_usu")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Modificable)
                    .HasColumnName("modificable")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasColumnName("observacion")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PasadoSueldos)
                    .HasColumnName("pasadoSueldos")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.JornadasTrabajada)
                    .WithMany(p => p.HFrancos)
                    .HasForeignKey(d => d.JornadasTrabajadaId)
                    .HasConstraintName("FK_h_francos_pers_jornadasTrabajadas");
            });

            modelBuilder.Entity<HNovxchof>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.CodNov, e.FecDesde });

                entity.ToTable("h_novxchof");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_emp")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CodNov).HasColumnName("cod_nov");

                entity.Property(e => e.FecDesde)
                    .HasColumnName("fec_desde")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Anio)
                    .HasColumnName("anio")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.Comenta)
                    .HasColumnName("comenta")
                    .HasColumnType("text");

                entity.Property(e => e.FecHasta)
                    .HasColumnName("fec_hasta")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Imprimio)
                    .HasColumnName("imprimio")
                    .HasColumnType("numeric(2, 0)");

                entity.Property(e => e.PasadaSueldos)
                    .HasColumnName("pasadaSueldos")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ProgAutomatica)
                    .HasColumnName("prog_automatica")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.VacPagas)
                    .HasColumnName("vac_pagas")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Novedades>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("novedades");

                entity.HasIndex(e => e.AbrNov)
                    .HasName("novedades_un1")
                    .IsUnique();

                entity.HasIndex(e => e.DesNov)
                    .HasName("novedades_un")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("cod_nov")
                    .ValueGeneratedNever();

                entity.Property(e => e.AbrNov)
                    .IsRequired()
                    .HasColumnName("abr_nov")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ClaseAusensia)
                    .HasColumnName("clase_ausensia")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DesNov)
                    .IsRequired()
                    .HasColumnName("des_nov")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FecHastaEditable)
                    .HasColumnName("fecHastaEditable")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FranNov)
                    .IsRequired()
                    .HasColumnName("fran_nov")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.HorSue)
                    .IsRequired()
                    .HasColumnName("hor_sue")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PorHora)
                    .HasColumnName("por_hora")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Variable)
                    .HasColumnName("variable")
                    .HasMaxLength(7)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InspZonas>(entity =>
            {
                entity.ToTable("insp_zonas");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Detalle).HasMaxLength(100);
            });

            modelBuilder.Entity<InspRespuestasIncognitos>(entity =>
            {
                entity.ToTable("insp_RespuestasIncognitos");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);
            });


            modelBuilder.Entity<InspPreguntasIncognitos>(entity =>
            {
                entity.ToTable("insp_PreguntasIncognitos");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<InspPreguntasIncognitosRespuestas>(entity =>
            {
                entity.ToTable("insp_PreguntasIncognitosRespuestas");

                entity.HasOne(d => d.PreguntaIncognito)
                    .WithMany(p => p.InspPreguntasIncognitosRespuestas)
                    .HasForeignKey(d => d.PreguntaIncognitoId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_PreguntasIncognitosRespuestas_insp_PreguntasIncognitos");

                entity.HasOne(d => d.Respuesta)
                    .WithMany(p => p.InspPreguntasIncognitosRespuestas)
                    .HasForeignKey(d => d.RespuestaId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_insp_PreguntasIncognitosRespuestas_insp_RespuestasIncognitos");
            });

            modelBuilder.Entity<InspPlanillaIncognitos>(entity =>
            {
                entity.ToTable("insp_PlanillaIncognitos");
                entity.Property(e => e.Fecha).IsRequired();
                //entity.Property(e => e.SucursalId).IsRequired();
                entity.Property(e => e.HoraAscenso).IsRequired();
                entity.Property(e => e.CocheId).IsRequired();
                entity.Property(e => e.CocheFicha).IsRequired();
                entity.Property(e => e.CocheInterno).IsRequired();
                entity.Property(e => e.Tarifa).HasColumnType("decimal(24, 2)").IsRequired();
                entity.Property(e => e.IsDeleted);
               
            });

            modelBuilder.Entity<InspPlanillaIncognitosDetalle>(entity =>
            {
                entity.ToTable("insp_PlanillaIncognitosDetalle");
                entity.Property(e => e.PlanillaIncognitoId).IsRequired();
                entity.Property(e => e.PreguntaIncognitoId).IsRequired();


                entity.HasOne(d => d.PlanillaIncognito)
                    .WithMany(p => p.InspPlanillaIncognitosDetalle)
                    .HasForeignKey(d => d.PlanillaIncognitoId)
                    .HasConstraintName("FK_insp_PlanillaIncognitosDetalle_insp_PlanillaIncognitos");

                entity.HasOne(d => d.PreguntaIncognitos)
                    .WithMany(e => e.InspPlanillaIncognitosDetalles)
                    .HasForeignKey(e => e.PreguntaIncognitoId)
                    .HasConstraintName("FK_insp_PlanillaIncognitosDetalle_insp_PreguntasIncognitos");

                entity.HasOne(e => e.RespuestaIncognitos)
                    .WithMany(e => e.InspPlanillaIncognitosDetalles)
                    .HasForeignKey(e => e.RespuestaIncognitoId)
                    .HasConstraintName("FK_insp_PlanillaIncognitosDetalle_insp_RespuestasIncognitos");
                    
            });




            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("sys_Notificaciones");

                //entity.HasIndex(e => e.Permiso)
                //    .HasName("UQ__sys_Noti__4DAF40C15884ACE1")
                //    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion).IsRequired();

                entity.Property(e => e.Permiso)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysUltimosNumeros>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Tipo1, e.Tipo2, e.Tipo3, e.Tipo4 });

                entity.ToTable("sys_ultimosNumeros");

                entity.Property(e => e.Id)
                    .HasColumnName("tabla")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo1)
                    .HasColumnName("tipo1")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo2)
                    .HasColumnName("tipo2")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo3)
                    .HasColumnName("tipo3")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo4)
                    .HasColumnName("tipo4")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.UltNumero).HasColumnName("ult_numero");
            });

            modelBuilder.Entity<PlaSector>(entity =>
            {
                entity.ToTable("pla_Sector");

                entity.Property(e => e.CodRec).HasColumnName("cod_rec");

                entity.Property(e => e.Color).HasMaxLength(100);

                entity.Property(e => e.Descripcion).HasMaxLength(100);

                entity.Property(e => e.DistanciaKm)
                    .HasColumnName("DistanciaKM")
                    .HasColumnType("decimal(20, 4)");

                entity.HasOne(d => d.Ruta)
                    .WithMany(p => p.Sectores)
                    .HasForeignKey(d => d.CodRec)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_Sector_gps_recorridos");

                entity.HasOne(d => d.PuntoFin)
                    .WithMany(p => p.SectorPuntoFin)
                    .HasPrincipalKey(p => p.Id)
                    .HasForeignKey(d => d.PuntoFinId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_Sector_gps_deta_reco1");

                entity.HasOne(d => d.PuntoInicio)
                    .WithMany(p => p.SectorPuntoInicio)
                    .HasPrincipalKey(p => p.Id)
                    .HasForeignKey(d => d.PuntoInicioId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_Sector_gps_deta_reco");
            });
            modelBuilder.Entity<PlaTiempoEsperadoDeCarga>(entity =>
            {
                entity.ToTable("pla_TiempoEsperadoDeCarga");

                entity.HasOne(d => d.TipoParada)
                    .WithMany(p => p.PlaTiempoEsperadoDeCarga)
                    .HasForeignKey(d => d.TipoParadaId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TiempoEsperadoDeCarga_TipoParada");

                entity.HasOne(d => d.TipodeDia)
                    .WithMany(p => p.PlaTiempoEsperadoDeCarga)
                    .HasForeignKey(d => d.TipodeDiaId)
                    .HasConstraintName("FK_pla_TiempoEsperadoDeCarga_h_tipodia");
            });
            modelBuilder.Entity<PlaTipoBandera>(entity =>
            {
                entity.ToTable("pla_TipoBandera");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150);
            });
            modelBuilder.Entity<PlaTipoLinea>(entity =>
            {
                entity.ToTable("pla_TipoLinea");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
            });
            modelBuilder.Entity<PlaTipoParada>(entity =>
            {
                entity.ToTable("pla_TipoParada");

                entity.Property(e => e.Abreviatura)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200);
            });
            modelBuilder.Entity<PlaLineasUsuarios>(entity =>
            {
                entity.ToTable("pla_LineasUsuarios");

                entity.Property(e => e.CodLin)
                    .HasColumnName("cod_lin")
                    .HasColumnType("numeric(4, 0)");

                entity.HasOne(d => d.CodLinNavigation)
                    .WithMany(p => p.PlaLineasUsuarios)
                    .HasForeignKey(d => d.CodLin)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_LineasUsuarios_linea");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PlaLineasUsuarios)
                    .HasForeignKey(d => d.UserId)
                    // ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_LineasUsuarios_sys_usersAD");
            });
            modelBuilder.Entity<PlaMinutosPorSector>(entity =>
            {
                entity.ToTable("pla_MinutosPorSector");

                entity.HasIndex(e => new { e.IdBandaHoraria, e.IdSector, e.Fecha })
                    .HasName("IX_pla_MinutosPorSector_IX")
                    .IsUnique();

                entity.Property(e => e.Demora)
                    .HasColumnType("time(0)")
                    .HasDefaultValueSql("('00:00:00')");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.HasOne(d => d.BandasHoraria)
                    .WithMany(p => p.PlaMinutosPorSector)
                    .HasForeignKey(d => d.IdBandaHoraria)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_MinutosPorSector_pla_BandasHorarias");

                entity.HasOne(d => d.Sector)
                    .WithMany(p => p.PlaMinutosPorSector)
                    .HasForeignKey(d => d.IdSector)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_MinutosPorSector_pla_Sector");
            });
            modelBuilder.Entity<PlaBandasHorarias>(entity =>
            {
                entity.ToTable("pla_BandasHorarias");

                entity.Property(e => e.HoraDesde).HasColumnType("time(0)");

                entity.Property(e => e.HoraHasta).HasColumnType("time(0)");
            });
            modelBuilder.Entity<PlaSentidoBandera>(entity =>
            {

                entity.ToTable("pla_SentidoBandera");


                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<PlaPuntos>(entity =>
            {
                entity.ToTable("pla_Puntos");

                entity.Property(e => e.PlaCoordenadaId).HasColumnName("Pla_CoordenadaId");

                entity.Property(e => e.PlaTipoViajeId).HasColumnName("Pla_TipoViajeId");

                entity.Property(e => e.PlaParadaId).HasColumnName("Pla_ParadaId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Abreviacion).HasMaxLength(100);

                entity.Property(e => e.CodigoNombre).HasMaxLength(100);

                entity.Property(e => e.Color).HasMaxLength(100);

                entity.Property(e => e.Data).IsRequired();

                entity.Property(e => e.Lat).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Long).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.MostrarEnReporte).HasDefaultValueSql("((1))");

                entity.Property(e => e.CodSectorTarifario).HasColumnName("cod_sectorTarifario");

                entity.HasOne(d => d.Ruta)
                    .WithMany(p => p.Puntos)
                    .HasForeignKey(d => d.CodRec)

                    .HasConstraintName("FK_Puntos_Rutas");

                entity.HasOne(d => d.PlaCoordenada)
                   .WithMany(p => p.PlaPuntos)
                   .HasForeignKey(d => d.PlaCoordenadaId)
                   .HasConstraintName("FK_pla_Puntos_pla_Coordenadas");

                entity.HasOne(d => d.BolSectoresTarifarios)
                    .WithMany(p => p.PlaPuntos)
                    .HasForeignKey(d => d.CodSectorTarifario)
                    .HasConstraintName("FK_pla_Puntos_bol_sectoresTarifarios");

                entity.HasOne(d => d.PlaParada)
                 .WithMany(p => p.PlaPuntos)
                 .HasForeignKey(d => d.PlaParadaId)
                 .HasConstraintName("FK_pla_Puntos_pla_parada");


                entity.HasOne(d => d.TipoParada)
                    .WithMany(p => p.PlaPuntos)
                    .HasForeignKey(d => d.TipoParadaId)
                    .HasConstraintName("FK_Puntos_TipoParada");

                entity.HasOne(d => d.PlaTipoViaje)
                    .WithMany(p => p.PlaPuntos)
                    .HasForeignKey(d => d.PlaTipoViajeId)
                    .HasConstraintName("FK_pla_Puntos_pla_TipoViaje");

            });
            modelBuilder.Entity<PlaLineaLineaHoraria>(entity =>
            {
                entity.ToTable("pla_Linea_LineaHoraria");

                entity.Property(e => e.LineaId).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.PlaLineaId).HasColumnName("pla_LineaId");

                entity.HasOne(d => d.Linea)
                    .WithMany(p => p.PlaLineaLineaHoraria)
                    .HasForeignKey(d => d.LineaId)
                    .HasConstraintName("FK_pla_Linea_LineaHoraria_linea");

                entity.HasOne(d => d.PlaLinea)
                    .WithMany(p => p.PlaLineaLineaHoraria)
                    .HasForeignKey(d => d.PlaLineaId)
                    .HasConstraintName("FK_pla_Linea_LineaHoraria_pla_Linea");
            });
            modelBuilder.Entity<PlaDistribucionDeCochesPorTipoDeDia>(entity =>
            {
                entity.ToTable("pla_DistribucionDeCochesPorTipoDeDia");

                entity.Property(e => e.CodHfecha).HasColumnName("cod_hfecha");

                entity.Property(e => e.CodTdia).HasColumnName("cod_tdia");

                entity.Property(e => e.Motivo).HasMaxLength(500);

                entity.HasOne(d => d.CodHfechaNavigation)
                    .WithMany(p => p.PlaDistribucionDeCochesPorTipoDeDia)
                    .HasForeignKey(d => d.CodHfecha)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_DistribucionDeCochesPorTipoDeDia_h_fechas_confi");

                entity.HasOne(d => d.CodTdiaNavigation)
                    .WithMany(p => p.PlaDistribucionDeCochesPorTipoDeDia)
                    .HasForeignKey(d => d.CodTdia)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pla_DistribucionDeCochesPorTipoDeDia_h_tipodia");
            });
            modelBuilder.Entity<PlaLinea>(entity =>
            {
                entity.ToTable("pla_Linea");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired();
                entity.Property(x => x.DesLin).HasColumnName(@"DesLin").HasColumnType("nvarchar").IsRequired().HasMaxLength(200);
                entity.Property(x => x.SucursalId).HasColumnName(@"SucursalId").HasColumnType("int").IsRequired();
                entity.Property(x => x.PlaTipoLineaId).HasColumnName(@"pla_TipoLineaId").HasColumnType("int").IsRequired();

                entity.Property(x => x.Activo).HasColumnName(@"Activo").HasColumnType("bit").IsRequired();
                entity.Property(x => x.CreatedDate).HasColumnName(@"CreatedDate").HasColumnType("datetime2");
                entity.Property(x => x.CreatedUserId).HasColumnName(@"CreatedUserId").HasColumnType("int");
                entity.Property(x => x.DeletedUserId).HasColumnName(@"DeletedUserId").HasColumnType("int");
                entity.Property(x => x.DeletedDate).HasColumnName(@"DeletedDate").HasColumnType("datetime2");
                entity.Property(x => x.LastUpdatedUserId).HasColumnName(@"LastUpdatedUserId").HasColumnType("int");
                entity.Property(x => x.LastUpdatedDate).HasColumnName(@"LastUpdatedDate").HasColumnType("datetime2");
                entity.Property(x => x.IsDeleted).HasColumnName(@"IsDeleted").HasColumnType("bit").IsRequired();


                // Foreign keys

                entity.HasOne(a => a.PlaTipoLinea).WithMany(b => b.PlaLineas).HasForeignKey(c => c.PlaTipoLineaId); // FK_pla_Linea_pla_TipoLinea
                entity.HasOne(a => a.Sucursal).WithMany(b => b.PlaLineas).HasForeignKey(c => c.SucursalId); // FK_pla_Linea_sucursales

            });
            modelBuilder.Entity<InfResponsables>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("inf_responsables");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_responsable")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CodResponsableBa)
                    .HasColumnName("cod_responsable_ba")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.DscResponsable)
                    .IsRequired()
                    .HasColumnName("dsc_responsable")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<PlaEstadoHorarioFecha>(entity =>
            {
                entity.ToTable("pla_EstadoHorarioFecha");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);
            });
            #endregion

            #region "Siniestros"
            modelBuilder.Entity<SinAbogados>(entity =>
            {
                entity.ToTable("sin_Abogados");

                entity.Property(e => e.ApellidoNombre)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Domicilio)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<SinBandaSiniestral>(entity =>
            {
                entity.ToTable("sin_BandaSiniestral");

                entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });
            modelBuilder.Entity<SinCategorias>(entity =>
            {
                entity.ToTable("sin_Categorias");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.InfoAdicional).IsUnicode(false);

                entity.HasOne(d => d.Consecuencia)
                    .WithMany(p => p.SinCategorias)
                    .HasForeignKey(d => d.ConsecuenciaId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Categorias_Consecuencias");
            });
            modelBuilder.Entity<SinCausas>(entity =>
            {
                entity.ToTable("sin_Causas");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Responsable)
    .HasColumnName("Responsable")
    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<SinCiaSeguros>(entity =>
            {
                entity.ToTable("sin_CiaSeguros");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Domicilio)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Encargado)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
    .HasMaxLength(250)
    .IsUnicode(false);
            });
            modelBuilder.Entity<SinConductasNormas>(entity =>
            {
                entity.ToTable("sin_ConductasNormas");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });
            modelBuilder.Entity<SinConductores>(entity =>
            {
                entity.ToTable("sin_Conductores");

                entity.Property(e => e.ApellidoNombre)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Domicilio)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.NroDoc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NroLicencia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.TipoDoc)
                    .WithMany(p => p.SinConductores)
                    .HasForeignKey(d => d.TipoDocId)
                    .HasConstraintName("FK_Conductores_TipoDni");
            });
            modelBuilder.Entity<SinConsecuencias>(entity =>
            {
                entity.ToTable("sin_Consecuencias");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Responsable)
.HasColumnName("Responsable")
.HasDefaultValueSql("((0))");


        });
            modelBuilder.Entity<SinDetalleLesion>(entity =>
            {
                entity.ToTable("sin_DetalleLesion");

                entity.Property(e => e.FechaFactura).HasColumnType("datetime");


                entity.Property(e => e.LugarAtencion).IsUnicode(false);

                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.HasOne(d => d.Involucrado)
                    .WithMany(p => p.SinDetalleLesion)
                    .HasForeignKey(d => d.InvolucradoId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetalleLesion_Involucrados");
            });
            modelBuilder.Entity<SinEstados>(entity =>
            {
                entity.ToTable("sin_Estados");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });
            modelBuilder.Entity<SinFactoresIntervinientes>(entity =>
            {
                entity.ToTable("sin_FactoresIntervinientes");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });
            modelBuilder.Entity<SinSancionSugerida>(entity =>
            {
                entity.ToTable("sin_SancionSugerida");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });
            modelBuilder.Entity<SinInvolucrados>(entity =>
            {
                entity.ToTable("sin_Involucrados");

                entity.Property(e => e.ApellidoNombre)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Detalle).IsUnicode(false);

                entity.Property(e => e.Domicilio)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.NroDoc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NroInvolucrado)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Conductor)
                   .WithMany(p => p.SinInvolucrados)
                   .HasForeignKey(d => d.ConductorId)
                   .HasConstraintName("FK_Involucrados_Conductores");


                entity.HasOne(d => d.Lesionado)
                    .WithMany(p => p.SinInvolucrados)
                    .HasForeignKey(d => d.LesionadoId)
                    .HasConstraintName("FK_Involucrados_Lesionados");

                entity.HasOne(d => d.MuebleInmueble)
                    .WithMany(p => p.SinInvolucrados)
                    .HasForeignKey(d => d.MuebleInmuebleId)
                    .HasConstraintName("FK_Involucrados_MuebleInmueble");

                entity.HasOne(d => d.Siniestro)
                    .WithMany(p => p.SinInvolucrados)
                    .HasForeignKey(d => d.SiniestroId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Involucrados_Siniestros");

                entity.HasOne(d => d.TipoDoc)
                    .WithMany(p => p.SinInvolucrados)
                    .HasForeignKey(d => d.TipoDocId)
                    .HasConstraintName("FK_Involucrados_TipoDoc");

                entity.HasOne(d => d.TipoInvolucrado)
                    .WithMany(p => p.SinInvolucrados)
                    .HasForeignKey(d => d.TipoInvolucradoId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Involucrados_TipoInvolucrado");

                entity.HasOne(d => d.Vehiculo)
                    .WithMany(p => p.SinInvolucrados)
                    .HasForeignKey(d => d.VehiculoId)
                    .HasConstraintName("FK_Involucrados_Vehiculos");
            });
            modelBuilder.Entity<SinInvolucradosAdjuntos>(entity =>
            {
                entity.HasKey(e => new { e.AdjuntoId, e.InvolucradoId });

                entity.ToTable("sin_InvolucradosAdjuntos");

                entity.HasOne(d => d.Involucrado)
                    .WithMany(p => p.SinInvolucradosAdjuntos)
                    .HasForeignKey(d => d.InvolucradoId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvolucradosAdjuntos_Involucrados");
            });
            modelBuilder.Entity<SinJuzgados>(entity =>
            {
                entity.ToTable("sin_Juzgados");

                entity.Property(e => e.Descripcion).IsUnicode(false);
            });
            modelBuilder.Entity<SinLesionados>(entity =>
            {
                entity.ToTable("sin_Lesionados");

                //entity.Property(e => e.FechaAtencion).HasColumnType("datetime");

                //entity.Property(e => e.LugarAtencion).IsUnicode(false);

                entity.HasOne(d => d.TipoLesionado)
                    .WithMany(p => p.SinLesionados)
                    .HasForeignKey(d => d.TipoLesionadoId)
                    .HasConstraintName("FK_Lesionados_TipoLesionado");
            });
            modelBuilder.Entity<SinMuebleInmueble>(entity =>
            {
                entity.ToTable("sin_MuebleInmueble");

                entity.Property(e => e.Lugar)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.TipoInmueble)
                    .WithMany(p => p.SinMuebleInmueble)
                    .HasForeignKey(d => d.TipoInmuebleId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MuebleInmueble_TipoInmueble");
            });
            modelBuilder.Entity<SinPracticantes>(entity =>
            {
                entity.ToTable("sin_Practicantes");

                entity.Property(e => e.ApellidoNombre)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Domicilio)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.NroDoc)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.TipoDoc)
                    .WithMany(p => p.SinPracticantes)
                    .HasForeignKey(d => d.TipoDocId)
                    .HasConstraintName("FK_Practicantes_TipoDni");
            });
            modelBuilder.Entity<SinReclamoAdjuntos>(entity =>
            {
                entity.HasKey(e => new { e.ReclamoId, e.AdjuntoId });

                entity.ToTable("sin_ReclamoAdjuntos");

                entity.HasOne(d => d.Reclamo)
                    .WithMany(p => p.SinReclamoAdjuntos)
                    .HasForeignKey(d => d.ReclamoId)
                    // ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReclamosAdjuntos_Reclamos");
            });
            modelBuilder.Entity<SinReclamoCuotas>(entity =>
            {
                entity.ToTable("sin_ReclamoCuotas");

                entity.Property(e => e.Concepto).IsUnicode(false);

                entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");

                entity.Property(e => e.Monto).HasColumnType("decimal(24, 2)");

                entity.HasOne(d => d.Reclamo)
                    .WithMany(p => p.ReclamoCuotas)
                    .HasForeignKey(d => d.ReclamoId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReclamosCuotas_Reclamos");
            });
            modelBuilder.Entity<SinReclamos>(entity =>
            {
                entity.ToTable("sin_Reclamos");

                entity.Property(e => e.Autos)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.FechaOfrecimiento).HasColumnType("date");

                entity.Property(e => e.FechaPago).HasColumnType("date");

                entity.Property(e => e.MontoDemandado).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoFranquicia).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoHonorariosAbogado).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoHonorariosMediador).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoHonorariosPerito).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoOfrecido).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoPagado).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoReconsideracion).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoTasasJudiciales).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.NroExpediente)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmpleadoAntiguedad).HasColumnType("datetime");

                entity.Property(e => e.EmpleadoArea).HasMaxLength(150);


                entity.Property(e => e.EmpleadoEmpresaId).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.EmpleadoFechaIngreso).HasColumnType("datetime");

                entity.Property(e => e.EmpleadoLegajo).HasMaxLength(150);

                entity.Property(e => e.EmpresaId).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.ObsConvenioExtrajudicial).IsUnicode(false);

                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.Property(e => e.PorcentajeIncapacidad).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.EmpleadoGrilla)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasComputedColumnSql("([dbo].[GetEmpleadoGrillaReclamos]([Id]))");

                entity.Property(e => e.NombreEmpleado)
.HasMaxLength(250)
.IsUnicode(false)
.HasComputedColumnSql("([dbo].[GetNombreEmpleadoReclamo]([Id]))");

                entity.HasOne(d => d.EmpleadoEmpresa)
                .WithMany(p => p.ArtReclamosEmpleadoEmpresa)
                .HasForeignKey(d => d.EmpleadoEmpresaId)
                .HasConstraintName("FK_Reclamos_EmpresaEmpleado");

                entity.HasOne(d => d.AbogadoEmpresa)
                    .WithMany(p => p.SinReclamosAbogadoEmpresa)
                    .HasForeignKey(d => d.AbogadoEmpresaId)
                    .HasConstraintName("FK_Reclamos_AbogadosEmpresa");

                entity.HasOne(d => d.Abogado)
                    .WithMany(p => p.SinReclamosAbogado)
                    .HasForeignKey(d => d.AbogadoId)
                    .HasConstraintName("FK_Reclamos_Abogados");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.SinReclamos)
                    .HasForeignKey(d => d.EstadoId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reclamos_Estados");

                entity.HasOne(d => d.Involucrado)
                    .WithMany(p => p.SinReclamos)
                    .HasForeignKey(d => d.InvolucradoId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reclamos_Involucrados");

                entity.HasOne(d => d.Juzgado)
                    .WithMany(p => p.SinReclamos)
                    .HasForeignKey(d => d.JuzgadoId)
                    .HasConstraintName("FK_Reclamos_Juzgados");

                entity.HasOne(d => d.SubEstado)
                    .WithMany(p => p.SinReclamos)
                    .HasForeignKey(d => d.SubEstadoId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reclamos_SubEstados");


                entity.HasOne(d => d.TipoReclamo)
                    .WithMany(p => p.Reclamos)
                    .HasForeignKey(d => d.TipoReclamoId)
                    .HasConstraintName("FK_Reclamos_TipoReclamo");


                entity.HasOne(d => d.Sucursal)
                    .WithMany(p => p.Reclamos)
                    .HasForeignKey(d => d.SucursalId)
                    .HasConstraintName("FK_Reclamos_Sucursal");


                entity.HasOne(d => d.Empresa)
                    .WithMany(p => p.Reclamos)
                    .HasForeignKey(d => d.EmpresaId)
                    .HasConstraintName("FK_Reclamos_Empresa");

                entity.HasOne(d => d.Denuncia)
                .WithMany(p => p.Reclamos)
                .HasForeignKey(d => d.DenunciaId)
                .HasConstraintName("FK_Reclamos_Denuncia");

                entity.HasOne(d => d.Siniestro)
                .WithMany(p => p.Reclamos)
                .HasForeignKey(d => d.SiniestroId)
                .HasConstraintName("FK_Reclamos_Siniestro");

                entity.HasOne(d => d.TipoAcuerdo)
                .WithMany(p => p.Reclamos)
                .HasForeignKey(d => d.TipoAcuerdoId)
                .HasConstraintName("FK_Reclamos_TiposAcuerdo");

                entity.HasOne(d => d.RubroSalarial)
.WithMany(p => p.Reclamos)
.HasForeignKey(d => d.RubroSalarialId)
.HasConstraintName("FK_Reclamos_RubroSalarial");

                entity.HasOne(d => d.Causa)
.WithMany(p => p.Reclamos)
.HasForeignKey(d => d.CausaId)
.HasConstraintName("FK_Reclamos_Causas");

            });
            modelBuilder.Entity<SinReclamosHistoricos>(entity =>
            {
                entity.ToTable("sin_ReclamosHistorico");

                entity.Property(e => e.Autos)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.FechaOfrecimiento).HasColumnType("date");

                entity.Property(e => e.FechaPago).HasColumnType("date");

                entity.Property(e => e.MontoDemandado).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoFranquicia).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoHonorariosAbogado).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoHonorariosMediador).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoHonorariosPerito).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoOfrecido).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoPagado).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoReconsideracion).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.MontoTasasJudiciales).HasColumnType("decimal(24, 2)");

                entity.Property(e => e.NroExpediente)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmpleadoAntiguedad).HasColumnType("datetime");

                entity.Property(e => e.EmpleadoArea).HasMaxLength(150);


                entity.Property(e => e.EmpleadoEmpresaId).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.EmpleadoFechaIngreso).HasColumnType("datetime");

                entity.Property(e => e.EmpleadoLegajo).HasMaxLength(150);

                entity.Property(e => e.EmpresaId).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.ObsConvenioExtrajudicial).IsUnicode(false);

                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.Property(e => e.PorcentajeIncapacidad).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.NombreEmpleado)
.HasMaxLength(250)
.IsUnicode(false)
.HasComputedColumnSql("([dbo].[GetNombreEmpleadoReclamoHistorico]([Id]))");

                entity.HasOne(d => d.Reclamo)
                    .WithMany(p => p.ReclamosHistoricos)
                    .HasForeignKey(d => d.ReclamoId)
                    .HasConstraintName("FK_ReclamosHistorico_Reclamos");


                entity.HasOne(d => d.AbogadoEmpresa)
                    .WithMany(p => p.SinReclamosHistoricoAbogadoEmpresa)
                    .HasForeignKey(d => d.AbogadoEmpresaId)
                    .HasConstraintName("FK_ReclamosHistorico_AbogadosEmpresa");

                entity.HasOne(d => d.Abogado)
                    .WithMany(p => p.SinReclamosHistoricoAbogado)
                    .HasForeignKey(d => d.AbogadoId)
                    .HasConstraintName("FK_ReclamosHistorico_Abogados");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.SinReclamosHistoricos)
                    .HasForeignKey(d => d.EstadoId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReclamosHistorico_Estados");

                entity.HasOne(d => d.Involucrado)
                    .WithMany(p => p.SinReclamosHistoricos)
                    .HasForeignKey(d => d.InvolucradoId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReclamosHistorico_Involucrados");

                entity.HasOne(d => d.Juzgado)
                    .WithMany(p => p.SinReclamosHistoricos)
                    .HasForeignKey(d => d.JuzgadoId)
                    .HasConstraintName("FK_ReclamosHistorico_Juzgados");

                entity.HasOne(d => d.SubEstado)
                    .WithMany(p => p.SinReclamosHistoricos)
                    .HasForeignKey(d => d.SubEstadoId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReclamosHistorico_SubEstados");

                entity.HasOne(d => d.TipoReclamo)
                 .WithMany(p => p.ReclamosHistoricos)
                 .HasForeignKey(d => d.TipoReclamoId)
                 .HasConstraintName("FK_ReclamosHistorico_TipoReclamo");


                entity.HasOne(d => d.Sucursal)
                 .WithMany(p => p.ReclamosHistoricos)
                 .HasForeignKey(d => d.SucursalId)
                 .HasConstraintName("FK_ReclamosHistorico_Sucursal");


                entity.HasOne(d => d.Empresa)
                 .WithMany(p => p.ReclamosHistoricos)
                 .HasForeignKey(d => d.EmpresaId)
                 .HasConstraintName("FK_ReclamosHistorico_Empresa");

                entity.HasOne(d => d.Denuncia)
                .WithMany(p => p.ReclamosHistoricos)
                .HasForeignKey(d => d.DenunciaId)
                .HasConstraintName("FK_ReclamosHistorico_Denuncia");

                entity.HasOne(d => d.Siniestro)
                .WithMany(p => p.ReclamosHistoricos)
                .HasForeignKey(d => d.SiniestroId)
                .HasConstraintName("FK_ReclamosHistorico_Siniestro");

                entity.HasOne(d => d.TipoAcuerdo)
                .WithMany(p => p.ReclamosHistoricos)
                .HasForeignKey(d => d.TipoAcuerdoId)
                .HasConstraintName("FK_ReclamosHistorico_TiposAcuerdo");

                entity.HasOne(d => d.RubroSalarial)
                .WithMany(p => p.ReclamosHistoricos)
                .HasForeignKey(d => d.RubroSalarialId)
                .HasConstraintName("FK_ReclamosHistorico_RubroSalarial");

                entity.HasOne(d => d.Causa)
                .WithMany(p => p.ReclamosHistoricos)
                .HasForeignKey(d => d.CausaId)
                .HasConstraintName("FK_ReclamosHistorico_Causas");

                entity.HasOne(d => d.EmpleadoEmpresa)
                .WithMany(p => p.ReclamosHistoricosEmpleadoEmpresa)
                .HasForeignKey(d => d.EmpleadoEmpresaId)
                .HasConstraintName("FK_ReclamosHistorico_EmpresaEmpleado");

            });
            modelBuilder.Entity<SinSiniestroAdjuntos>(entity =>
            {
                entity.HasKey(e => new { e.SiniestroId, e.AdjuntoId });

                entity.ToTable("sin_SiniestroAdjuntos");

                entity.HasOne(d => d.Siniestro)
                    .WithMany(p => p.SinSiniestroAdjuntos)
                    .HasForeignKey(d => d.SiniestroId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_sin_SiniestrosAdjuntos");
            });
            modelBuilder.Entity<SinSiniestros>(entity =>
            {
                entity.ToTable("sin_Siniestros");

                entity.Property(e => e.CocheDominio)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CocheId)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.CocheInterno)
    .HasMaxLength(6)
    .IsUnicode(false);

                entity.Property(e => e.CocheLineaId).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CodInforme)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InformaTaller).HasDefaultValueSql("((0))");

                entity.Property(e => e.Comentario).IsUnicode(false);
                entity.Property(e => e.EmpresaId).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.ConductorEmpresaId).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.ConductorLegajo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Dia)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.EmpPract)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.FechaDenuncia).HasColumnType("datetime");

                entity.Property(e => e.Latitud)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Longitud)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Localidad)
    .HasMaxLength(100)
    .IsUnicode(false);

                entity.Property(e => e.Lugar).IsUnicode(false);

                entity.Property(e => e.NroSiniestro)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.NroSiniestroSeguro)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ObsDanios).IsUnicode(false);

                entity.Property(e => e.CostoReparacion).IsUnicode(false);

                entity.Property(e => e.ObsInterna).IsUnicode(false);

                entity.Property(e => e.NombreConductor)
    .HasMaxLength(250)
    .IsUnicode(false)
    .HasComputedColumnSql("([dbo].[GetNombreConductor]([Id]))");

                entity.Property(e => e.DniConductor)
.HasMaxLength(250)
.IsUnicode(false)
.HasComputedColumnSql("([dbo].[GetDniConductor]([Id]))");

                //entity.HasOne(d => d.BandaSiniestral)
                //    .WithMany(p => p.SinSiniestros)
                //    .HasForeignKey(d => d.BandaSiniestralId)
                //    .HasConstraintName("FK_Siniestros_BandaSiniestral");

                entity.HasOne(d => d.Causa)
                    .WithMany(p => p.SinSiniestros)
                    .HasForeignKey(d => d.CausaId)
                    .HasConstraintName("FK_Siniestros_Causas");

                //entity.HasOne(d => d.Coche)
                //    .WithMany(p => p.SinSiniestros)
                //    .HasForeignKey(d => d.CocheId)
                //    ////.OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Siniestros_Coches");

                entity.HasOne(d => d.CocheLinea)
                    .WithMany(p => p.SinSiniestros)
                    .HasForeignKey(d => d.CocheLineaId);

                entity.HasOne(d => d.Conducta)
                    .WithMany(p => p.SinSiniestros)
                    .HasForeignKey(d => d.ConductaId)
                    .HasConstraintName("FK_Siniestros_Conductas");

                entity.HasOne(d => d.ConductorEmpresa)
                    .WithMany(p => p.SinSiniestrosConductorEmpresa)
                    .HasForeignKey(d => d.ConductorEmpresaId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Siniestros_ConductorEmpresa");

                entity.HasOne(d => d.Empresa)
                    .WithMany(p => p.SinSiniestrosEmpresaEmpresa)
                    .HasForeignKey(d => d.EmpresaId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Siniestros_Empresas");


                entity.HasOne(d => d.Factores)
                    .WithMany(p => p.SinSiniestros)
                    .HasForeignKey(d => d.FactoresId)
                    .HasConstraintName("FK_Siniestros_Factores");

                entity.HasOne(d => d.SancionSugerida)
                    .WithMany(p => p.SinSiniestros)
                    .HasForeignKey(d => d.SancionSugeridaId)
                    .HasConstraintName("FK_Siniestros_Sanciones");

                entity.HasOne(d => d.Practicante)
                    .WithMany(p => p.SinSiniestros)
                    .HasForeignKey(d => d.PracticanteId)
                    .HasConstraintName("FK_Siniestros_Practicantes");

                entity.HasOne(d => d.Seguro)
                    .WithMany(p => p.SinSiniestros)
                    .HasForeignKey(d => d.SeguroId)
                    .HasConstraintName("FK_Siniestros_CiaSeguros");

                entity.HasOne(d => d.SubCausa)
                    .WithMany(p => p.SinSiniestros)
                    .HasForeignKey(d => d.SubCausaId)
                    .HasConstraintName("FK_Siniestros_SubCausa");

                entity.HasOne(d => d.Sucursal)
                    .WithMany(p => p.SinSiniestros)
                    .HasForeignKey(d => d.SucursalId)
                    //  ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Siniestros_Sucursales");

                entity.HasOne(d => d.TipoDanio)
                    .WithMany(p => p.SinSiniestros)
                    .HasForeignKey(d => d.TipoDanioId)
                    .HasConstraintName("FK_Siniestros_TipoDanio");

                entity.HasOne(d => d.TipoColision)
    .WithMany(p => p.SinSiniestros)
    .HasForeignKey(d => d.TipoColisionId)
    .HasConstraintName("FK_Siniestros_TipoColision");
            });
            modelBuilder.Entity<SinSiniestrosConsecuencias>(entity =>
            {
                entity.ToTable("sin_SiniestrosConsecuencias");

                entity.Property(e => e.Observaciones).IsUnicode(false);

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.SinSiniestrosConsecuencias)
                    .HasForeignKey(d => d.CategoriaId)
                    .HasConstraintName("FK_SiniestrosConsecuencias_Categorias");

                entity.HasOne(d => d.Consecuencia)
                    .WithMany(p => p.SinSiniestrosConsecuencias)
                    .HasForeignKey(d => d.ConsecuenciaId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SiniestrosConsecuencias_Consecuencias");

                entity.HasOne(d => d.Siniestro)
                    .WithMany(p => p.SinSiniestrosConsecuencias)
                    .HasForeignKey(d => d.SiniestroId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SiniestrosConsecuencias_Siniestros");

            });
            modelBuilder.Entity<SinSubCausas>(entity =>
            {
                entity.ToTable("sin_SubCausas");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Causa)
                    .WithMany(p => p.SinSubCausas)
                    .HasForeignKey(d => d.CausaId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SubCausas_Causas");
            });
            modelBuilder.Entity<SinSubEstados>(entity =>
            {
                entity.ToTable("sin_SubEstados");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.SinSubEstados)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SubEstados_Estados");
            });
            modelBuilder.Entity<SinTipoDanio>(entity =>
            {
                entity.ToTable("sin_TipoDanio");

                entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SinTipoDanio>(entity =>
            {
                entity.ToTable("sin_TipoDanio");

                entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SinTipoColision>(entity =>
            {
                entity.ToTable("sin_TipoColision");

                entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SinTipoInvolucrado>(entity =>
            {
                entity.ToTable("sin_TipoInvolucrado");

                entity.Property(e => e.Descripcion).IsUnicode(false);
            });
            modelBuilder.Entity<SinTipoLesionado>(entity =>
            {
                entity.ToTable("sin_TipoLesionado");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });
            modelBuilder.Entity<SinTipoMueble>(entity =>
            {
                entity.ToTable("sin_TipoMueble");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .IsUnicode(false);
            });
            modelBuilder.Entity<SinVehiculos>(entity =>
            {
                entity.ToTable("sin_Vehiculos");

                entity.Property(e => e.Dominio)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Marca)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Modelo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Poliza)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Seguro)
                    .WithMany(p => p.SinVehiculos)
                    .HasForeignKey(d => d.SeguroId)
                    .HasConstraintName("FK_Siniestros_Seguros");
            });
            #region "Croquis"
            modelBuilder.Entity<CroCroquis>(entity =>
            {
                entity.ToTable("Cro_Croquis");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Svg)
                    .IsRequired()
                    .HasColumnName("svg");
            });
            modelBuilder.Entity<CroElemeneto>(entity =>
            {
                entity.ToTable("Cro_Elemeneto");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion).HasMaxLength(500);

                entity.Property(e => e.Nombre).HasMaxLength(200);

                entity.HasOne(d => d.TipoElemento)
                    .WithMany(p => p.CroElemeneto)
                    .HasForeignKey(d => d.TipoElementoId)
                    .HasConstraintName("FK_Cro_Elemeneto_Cro_TipoElemento");

                entity.HasOne(d => d.Tipo)
                    .WithMany(p => p.CroElemeneto)
                    .HasForeignKey(d => d.TipoId)
                    .HasConstraintName("FK_Cro_Elemeneto_Cro_Tipo");
            });
            modelBuilder.Entity<CroTipo>(entity =>
            {
                entity.ToTable("Cro_Tipo");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre).HasMaxLength(100);

            });
            modelBuilder.Entity<CroTipoElemento>(entity =>
            {
                entity.ToTable("Cro_TipoElemento");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200);

            });
            #endregion
            #endregion

            #region "Permissions"
            modelBuilder.Entity<SysAreas>(entity =>
            {
                entity.HasKey(e => e.Area);

                entity.ToTable("sys_areas");

                entity.Property(e => e.Area)
                    .HasMaxLength(100)
                    .ValueGeneratedNever();

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);
            });
            modelBuilder.Entity<SysPages>(entity =>
            {
                entity.HasKey(e => e.Page);

                entity.ToTable("sys_pages");

                entity.Property(e => e.Page)
                    .HasMaxLength(100)
                    .ValueGeneratedNever();

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);
            });
            modelBuilder.Entity<SysPermissions>(entity =>
            {
                entity.ToTable("sys_permissions");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Page)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(302)
                    .HasComputedColumnSql("(((([Area]+'.')+[Page])+'.')+[Action])");

                entity.HasOne(d => d.Areas)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.Area)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permissions_Areas");

                entity.HasOne(d => d.Pages)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.Page)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permissions_Pages");
            });
            modelBuilder.Entity<SysPermissionsRoles>(entity =>
            {
                entity.ToTable("sys_permissionsRoles");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.PermissionRols)
                    .HasForeignKey(d => d.PermissionId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PermissionRols_Permissions");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.PermissionRols)
                    .HasForeignKey(d => d.RolId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PermissionRols_Roles");
            });
            modelBuilder.Entity<SysPermissionsUsers>(entity =>
            {
                entity.ToTable("sys_permissionsUsers");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.PermissionsUsers)
                    .HasForeignKey(d => d.PermissionId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PermissionsUsers_Permissions");
            });
            modelBuilder.Entity<SysRoles>(entity =>
            {
                entity.ToTable("sys_roles");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);
            });
            modelBuilder.Entity<SysUsersAd>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("sys_usersAD");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_usuario")
                    .ValueGeneratedNever();

                entity.Property(e => e.Area)
                    .HasColumnName("area")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Baja)
                    .HasColumnName("baja")
                    .HasColumnType("char(1)");

                entity.Property(e => e.CanonicalName)
                    .HasColumnName("canonicalName")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CodEmp)
                    .HasColumnName("cod_emp")
                    .HasColumnType("char(7)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .HasColumnName("displayName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DistinguishedName)
                    .HasColumnName("distinguishedName")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.LogicalLogon)
                    .IsRequired()
                    .HasColumnName("logicalLogon")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LogonName)
                    .IsRequired()
                    .HasColumnName("logonName")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.Mail)
                    .HasColumnName("mail")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NomUsuario)
                    .IsRequired()
                    .HasColumnName("nom_usuario")
                    .HasColumnType("char(35)");

                entity.Property(e => e.NroDoc)
                    .HasColumnName("nro_doc")
                    .HasColumnType("char(15)");

                entity.Property(e => e.TelPersonal)
                    .HasColumnName("telPersonal")
                    .HasColumnType("char(40)");

                entity.Property(e => e.TelTrabajo)
                    .HasColumnName("telTrabajo")
                    .HasColumnType("char(30)");

                entity.Property(e => e.TpoDoc)
                    .HasColumnName("tpo_doc")
                    .HasColumnType("char(6)");

                entity.Property(e => e.TpoNroDoc)
                    .HasColumnName("tpo_nro_doc")
                    .HasColumnType("char(20)");

                entity.Property(e => e.UserPrincipalName)
                    .HasColumnName("userPrincipalName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SucursalId)
                         .HasColumnName("SucursalId");

                entity.Property(e => e.GruposInspectoresId)
                        .HasColumnName("GruposInspectoresId");

                entity.Property(e => e.EmpleadoId)
                       .HasColumnName("EmpleadoId");

                entity.Property(e => e.TurnoId)
                       .HasColumnName("TurnoId");

                entity.Property(e => e.DescEmpleado)
                   .HasColumnName("DescEmpleado")
                   .HasMaxLength(100);
            });
            modelBuilder.Entity<SysUsersRoles>(entity =>
            {
                entity.ToTable("sys_usersRoles");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRoles_Roles");
            });
            modelBuilder.Entity<SysDataTypes>(entity =>
            {
                entity.ToTable("sys_dataTypes");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
            modelBuilder.Entity<SysParameters>(entity =>
            {
                entity.ToTable("sys_parameters");
                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Description)
                    .HasMaxLength(200);
                entity.Property(e => e.DataTypeId).HasColumnName("DataTypeId");
                entity.HasOne(e => e.SysDataType)
                    .WithMany(e => e.SysParameters)
                    .HasForeignKey(e => e.DataTypeId)
                    .HasConstraintName("FK_sys_parameters_sys_dataTypes1");
            });
            #endregion

            #region SinClasificar
            modelBuilder.Entity<Linea>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("linea");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_lin")
                    .HasColumnType("numeric(4, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AsocBan)
                    .IsRequired()
                    .HasColumnName("asoc_ban")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.BandaHoraria)
                    .HasColumnName("bandaHoraria")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ClaEmpr1)
                    .HasColumnName("cla_empr1")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.ClaEmpr2)
                    .HasColumnName("cla_empr2")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.ClaEmpr3)
                    .HasColumnName("cla_empr3")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.ClaEmpr4)
                    .HasColumnName("cla_empr4")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.CodLinCaudalimetro).HasColumnName("cod_linCaudalimetro");
                entity.Property(e => e.CodRespInformes).HasColumnName("cod_respInformes").HasMaxLength(7).IsUnicode(false);

                entity.Property(e => e.DesLin)
                    .IsRequired()
                    .HasColumnName("des_lin")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.DiasBaj)
                    .HasColumnName("dias_baj")
                    .HasColumnType("numeric(1, 0)");

                entity.Property(e => e.FecBaja)
                    .HasColumnName("fec_baja")
                    .HasColumnType("datetime");

                entity.Property(e => e.GrupoQv)
                    .HasColumnName("grupo_QV")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GrupoSap)
                    .HasColumnName("grupo_sap")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.HoraCambioTurno)
                    .HasColumnName("horaCambioTurno")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.IdSap)
                    .HasColumnName("id_sap")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PlaGrupoLineaId).HasColumnName("pla_GrupoLineaId");

                entity.Property(e => e.SociedadFi)
                    .HasColumnName("sociedad_fi")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.UrbInter)
                    .IsRequired()
                    .HasColumnName("urb_inter")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(a => a.RespInformes).WithMany(b => b.Lineas).HasForeignKey(c => c.CodRespInformes);


            });

            modelBuilder.Entity<InfResponsables>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("inf_responsables");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_responsable")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CodResponsableBa)
                    .HasColumnName("cod_responsable_ba")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.DscResponsable)
                    .IsRequired()
                    .HasColumnName("dsc_responsable")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InfResponsables>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("inf_responsables");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_responsable")
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CodResponsableBa)
                    .HasColumnName("cod_responsable_ba")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.DscResponsable)
                    .IsRequired()
                    .HasColumnName("dsc_responsable")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<PlaTalleresIvu>(entity =>
            {
                entity.ToTable("pla_talleresIVU");

                entity.Property(e => e.CodGal)
                    .HasColumnName("Cod_gal")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CodGalIvu).HasColumnName("Cod_galIVU");

                entity.HasOne(d => d.CodGalNavigation)
                    .WithMany(p => p.PlaTalleresIvu)
                    .HasForeignKey(d => d.CodGal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Galpones_talleresIVU");
            });

            modelBuilder.Entity<Galpon>(entity =>
            {
                entity.ToTable("galpones");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_gal")
                    .HasColumnType("numeric(4, 0)");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.DesGal)
                    .IsRequired()
                    .HasColumnName("des_gal")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.DomGal)
                    .HasColumnName("dom_gal")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.EncGal)
                    .HasColumnName("enc_gal")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DeletedDate)
                   .HasColumnName("fec_baja")
                   .HasColumnType("datetime");

                entity.Property(e => e.HoraCorte)
                    .HasColumnName("horaCorte")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdSap)
                    .HasColumnName("id_sap")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.IdSapRepuestos)
                    .HasColumnName("id_sapRepuestos")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Latitud).HasColumnName("latitud");

                entity.Property(e => e.Longitud).HasColumnName("longitud");

                entity.Property(e => e.PosGal)
                    .IsRequired()
                    .HasColumnName("pos_gal")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PtoPedido)
                    .HasColumnName("pto_pedido")
                    .HasColumnType("decimal(12, 2)");

                entity.Property(e => e.Radio).HasColumnName("radio");

                entity.Property(e => e.TelGal)
                    .HasColumnName("tel_gal")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<SubGalpon>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("sub_galpon");



                entity.Property(e => e.Id)
                    .HasColumnName("cod_subg")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.Balanceo)
                    .HasColumnName("balanceo")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DesSubg)
                    .IsRequired()
                    .HasColumnName("des_subg")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.FecBaja)
                    .HasColumnName("fec_baja")
                    .HasColumnType("datetime");

                entity.Property(e => e.FltComodines)
                    .HasColumnName("flt_comodines")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HBanderasColores>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("h_banderasColores_pk");

                entity.ToTable("h_banderasColores");

                entity.HasIndex(e => e.DscBanderaColor)
                    .HasName("h_banderasColores_uk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("cod_banderaColor")
                    .ValueGeneratedNever();

                entity.Property(e => e.CodBanderaColorbsas).HasColumnName("cod_banderaColorbsas");

                entity.Property(e => e.DscBanderaColor)
                    .IsRequired()
                    .HasColumnName("dsc_banderaColor")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HDesignar>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("h_designar");

                entity.HasIndex(e => e.Llega)
                    .HasName("_dta_index_h_designar_7_1019228223__K7");

                entity.HasIndex(e => new { e.CodEmp, e.Fecha })
                    .HasName("IX_h_designar_2");

                entity.HasIndex(e => new { e.CodServicio, e.Fecha })
                    .HasName("IX_02_h_horarios_confi");

                entity.HasIndex(e => new { e.Fecha, e.CodServicio, e.CodEmp })
                    .HasName("IX1_h_designar");

                entity.HasIndex(e => new { e.Fecha, e.CodServicio, e.CodUni, e.CodEmp })
                    .HasName("h_designar_un")
                    .IsUnique();

                entity.HasIndex(e => new { e.CodUni, e.Sale, e.Fecha, e.Id, e.CodEmp, e.CodServicio })
                    .HasName("_dta_index_h_designar_7_1019228223__K2_K1_K9_K3_4_5");

                entity.HasIndex(e => new { e.Sale, e.SaleOriginal, e.Llega, e.LlegaOriginal, e.HoraDesc, e.TipoServ, e.CodUsu, e.Duracion, e.PasadaSueldos, e.Observacion, e.Anular, e.Fecha, e.CodUni, e.CodServicio, e.Id, e.CodEmp })
                    .HasName("_dta_index_h_designar_7_1019228223__K2_K4_K3_K1_K9_5_6_7_8_11_12_13_14_15_16_17");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_desig")
                    .ValueGeneratedNever();

                entity.Property(e => e.Anular)
                    .HasColumnName("anular")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CodDesigbsas).HasColumnName("cod_desigbsas");

                entity.Property(e => e.CodEmp)
                    .IsRequired()
                    .HasColumnName("cod_emp")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CodServicio).HasColumnName("cod_servicio");

                entity.Property(e => e.CodUni).HasColumnName("cod_uni");

                entity.Property(e => e.CodUsu)
                    .HasColumnName("cod_usu")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Duracion)
                    .HasColumnName("duracion")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.HoraDesc)
                    .HasColumnName("hora_desc")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.HorasModificadas)
                    .HasColumnName("horasModificadas")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Llega)
                    .HasColumnName("llega")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.LlegaOriginal)
                    .HasColumnName("llegaOriginal")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Observacion)
                    .HasColumnName("observacion")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PasadaSueldos)
                    .HasColumnName("pasadaSueldos")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Sale)
                    .HasColumnName("sale")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.SaleOriginal)
                    .HasColumnName("saleOriginal")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.TipoServ)
                    .HasColumnName("tipo_serv")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BolBanderasCartel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("bol_banderasCartel");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_banderaCartel");
                //  .ValueGeneratedNever();

                entity.Property(e => e.CodBanderaCartelbsas).HasColumnName("cod_banderaCartelbsas");

                entity.Property(e => e.CodHfecha).HasColumnName("cod_hfecha");

                entity.Property(e => e.CodLinea).HasColumnName("cod_linea");
            });
            modelBuilder.Entity<BolBanderasCartelDetalle>(entity =>
            {
                entity.HasKey(e => new { e.CodBanderaCartel, e.CodBan });

                entity.ToTable("bol_banderasCartelDetalle");

                entity.Property(e => e.CodBanderaCartel).HasColumnName("cod_banderaCartel");

                entity.Property(e => e.CodBan).HasColumnName("cod_ban");

                entity.Property(e => e.Movible)
                    .IsRequired()
                    .HasColumnName("movible")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.NroSecuencia).HasColumnName("nro_secuencia");

                entity.Property(e => e.ObsBandera)
                    .HasColumnName("obs_bandera")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.TextoBandera)
                    .HasColumnName("texto_bandera")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodBanNavigation)
                    .WithMany(p => p.BolBanderasCartelDetalle)
                    .HasForeignKey(d => d.CodBan);
                // //.OnDelete(DeleteBehavior.ClientSetNull)
                //                    .HasConstraintName("FK_bol_banderasCartelDetalle_h_banderas");


                entity.HasOne(d => d.CodBanderaCartelNavigation)
                    .WithMany(p => p.BolBanderasCartelDetalle)
                    .HasForeignKey(d => d.CodBanderaCartel);
                // //.OnDelete(DeleteBehavior.ClientSetNull)
                // .HasConstraintName("FK__bol_bande__cod_b__35C7EB02");
            });
            modelBuilder.Entity<BolSectoresTarifarios>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(m => m.Id).ValueGeneratedOnAdd();

                entity.ToTable("bol_sectoresTarifarios");

                entity.HasIndex(e => e.DscSectorTarifario)
                    .HasName("bol_sectoresTarifarios_uk")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("cod_sectorTarifario")
                    .ValueGeneratedNever();

                entity.Property(e => e.CodManualSectorTarifario).HasColumnName("cod_manualSectorTarifario");

                entity.Property(e => e.CodSectorTarifariobsas).HasColumnName("cod_sectorTarifariobsas");

                entity.Property(e => e.DscCompleta)
                    .HasColumnName("dsc_completa")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.DscSectorTarifario)
                    .IsRequired()
                    .HasColumnName("dsc_sectorTarifario")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Nacional).HasColumnName("nacional");
            });

            modelBuilder.Entity<DshDashboard>(entity =>
            {
                entity.ToTable("Dsh_Dashboard");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.TipoDashboard)
                    .WithMany(p => p.DshDashboard)
                    .HasForeignKey(d => d.TipoDashboardId)
                    .HasConstraintName("FK_Dsh_Dashboard_Dsh_TipoDasboard");
            });
            modelBuilder.Entity<DshTipoDasboard>(entity =>
            {
                entity.ToTable("Dsh_TipoDasboard");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);
            });
            modelBuilder.Entity<DshUsuarioDashboardItem>(entity =>
            {
                entity.ToTable("Dsh_UsuarioDashboardItem");

                entity.Property(e => e.CodUsuario).HasColumnName("Cod_usuario");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.DshUsuarioDashboardItem)
                    .HasForeignKey(d => d.CodUsuario)
                    .HasConstraintName("FK_Dsh_UsuarioDashboardItem_sys_usersAD");

                entity.HasOne(d => d.Dashboard)
                    .WithMany(p => p.DshUsuarioDashboardItem)
                    .HasForeignKey(d => d.DashboardId)
                    .HasConstraintName("FK_Dsh_UsuarioDashboardItem_Dsh_Dashboard");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {

                entity.HasKey(e => e.Id);
                //entity.HasKey(e => e.CodEmpr);

                entity.ToTable("empresa");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_empr")
                    .HasColumnType("numeric(4, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ActEmpr)
                    .HasColumnName("act_empr")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.AlicuotaLRT)
                    .HasColumnName("alicuota_l_r_t")
                    .HasColumnType("numeric(4, 3)");

                entity.Property(e => e.Anses)
                    .HasColumnName("anses")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Art).HasColumnName("art");

                entity.Property(e => e.CodActi).HasColumnName("cod_acti");

                entity.Property(e => e.CodIva)
                    .HasColumnName("cod_iva")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.CtaDeposito)
                    .HasColumnName("cta_deposito")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Cuenta)
                    .HasColumnName("cuenta")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CuentaSap)
                    .HasColumnName("cuenta_sap")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Cuit)
                    .HasColumnName("cuit")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.CuotaLRT)
                    .HasColumnName("cuota_l_r_t")
                    .HasColumnType("numeric(12, 2)");

                entity.Property(e => e.DepEmpr)
                    .HasColumnName("dep_empr")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DesEmpr)
                    .IsRequired()
                    .HasColumnName("des_empr")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.DomEmpr)
                    .HasColumnName("dom_empr")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.FecBaja)
                    .HasColumnName("fec_baja")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdSap)
                    .HasColumnName("id_sap")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.NroAgenteIb)
                    .HasColumnName("nro_agenteIb")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.NroIsib)
                    .HasColumnName("nro_isib")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.NroSap)
                    .HasColumnName("nro_sap")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PosEmpr)
                    .IsRequired()
                    .HasColumnName("pos_empr")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Reducc)
                    .HasColumnName("reducc")
                    .HasColumnType("numeric(4, 2)");

                entity.Property(e => e.Siglas)
                    .HasColumnName("siglas")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.TelEmpr)
                    .HasColumnName("tel_empr")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Zona).HasColumnName("zona");
            });

            modelBuilder.Entity<GpsMensajesCoches>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("gps_mensajesCoches");

                entity.HasIndex(e => e.Dia)
                    .HasName("IX_dia_dsc");

                entity.Property(e => e.Id).HasColumnName("idMensaje");

                entity.Property(e => e.CodLin).HasColumnName("cod_lin");

                entity.Property(e => e.CodTdia).HasColumnName("cod_tdia");

                entity.Property(e => e.CodUsuario).HasColumnName("cod_usuario");

                entity.Property(e => e.Codigo).HasColumnName("codigo");

                entity.Property(e => e.Dia)
                    .HasColumnName("dia")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Enviado)
                    .HasColumnName("enviado")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha)
                    .IsRequired()
                    .HasColumnName("fecha")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Ficha).HasColumnName("ficha");

                entity.Property(e => e.Hora)
                    .IsRequired()
                    .HasColumnName("hora")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Id2).HasColumnName("id");

                entity.Property(e => e.Legajo).HasColumnName("legajo");

                entity.Property(e => e.Maquina)
                    .HasColumnName("maquina")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Origen).HasColumnName("origen");

                entity.Property(e => e.Servicio).HasColumnName("servicio");

                entity.Property(e => e.Texto)
                    .IsRequired()
                    .HasColumnName("texto")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasColumnName("usuario")
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

        modelBuilder.Entity<GpsEstadosActualesRepli>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.TpoPc });

                entity.ToTable("gps_estadosActuales_repli");

                entity.HasIndex(e => new { e.Codlin, e.Servi })
                    .HasName("IX_gps_estadosActualesRepli");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TpoPc).HasColumnName("tpo_pc");

                entity.Property(e => e.Activo)
                    .HasColumnName("activo")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Alerta)
                    .HasColumnName("alerta")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Atraso)
                    .HasColumnName("atraso")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Cdesde).HasColumnName("cdesde");

                entity.Property(e => e.Celular).HasColumnName("celular");

                entity.Property(e => e.Chasta).HasColumnName("chasta");

                entity.Property(e => e.CodBan).HasColumnName("cod_ban");

                entity.Property(e => e.CodEmp)
                    .HasColumnName("cod_emp")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CodEstado).HasColumnName("cod_estado");

                entity.Property(e => e.CodHfecha).HasColumnName("cod_hfecha");

                entity.Property(e => e.CodMediaVuelta).HasColumnName("cod_media_vuelta");

                entity.Property(e => e.Codlin).HasColumnName("codlin");

                entity.Property(e => e.Codtip).HasColumnName("codtip");

                entity.Property(e => e.ComienzoEstado)
                    .HasColumnName("comienzoEstado")
                    .HasColumnType("datetime");

                entity.Property(e => e.Cuenta).HasColumnName("cuenta");

                entity.Property(e => e.Desban)
                    .HasColumnName("desban")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Deslin)
                    .HasColumnName("deslin")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Destip)
                    .HasColumnName("destip")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Diagramado)
                    .HasColumnName("diagramado")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Esta)
                    .HasColumnName("esta")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FechaHora)
                    .HasColumnName("fecha_hora")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Fechai)
                    .HasColumnName("fechai")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Ficha).HasColumnName("ficha");

                entity.Property(e => e.Hdesde)
                    .HasColumnName("hdesde")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Hhasta)
                    .HasColumnName("hhasta")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Hora)
                    .HasColumnName("hora")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Horai)
                    .HasColumnName("horai")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Indliscon).HasColumnName("indliscon");

                entity.Property(e => e.Interno)
                    .HasColumnName("interno")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Ip)
                    .HasColumnName("ip")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Kdesde).HasColumnName("kdesde");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Late).HasColumnName("late");

                entity.Property(e => e.Legajo)
                    .HasColumnName("legajo")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Lon).HasColumnName("lon");

                entity.Property(e => e.Lone).HasColumnName("lone");

                entity.Property(e => e.NroServ).HasColumnName("nro_serv");

                entity.Property(e => e.Observaciones)
                    .HasColumnName("observaciones")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Pedido).HasColumnName("pedido");

                entity.Property(e => e.Pendiente).HasColumnName("pendiente");

                entity.Property(e => e.Proceso).HasColumnName("proceso");

                entity.Property(e => e.Sdesde)
                    .HasColumnName("sdesde")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Sectores)
                    .HasColumnName("sectores")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Sent).HasColumnName("sent");

                entity.Property(e => e.Sentido)
                    .HasColumnName("sentido")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Servi).HasColumnName("servi");

                entity.Property(e => e.Shasta)
                    .HasColumnName("shasta")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Ubicacion)
                    .HasColumnName("ubicacion")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Veloc).HasColumnName("veloc");
            });


            modelBuilder.Entity<GpsDetaReco>(entity =>
            {
                entity.HasKey(e => new { e.CodRec, e.Cuenta });

                entity.ToTable("gps_deta_reco");

                entity.Property(e => e.CodRec).HasColumnName("cod_rec");

                entity.Property(e => e.Cuenta).HasColumnName("cuenta");

                entity.Property(e => e.DscSector)
                    .HasColumnName("dsc_sector")
                    .HasColumnType("char(75)");

                entity.Property(e => e.Lat)
                    .HasColumnName("lat")
                    .HasColumnType("numeric(9, 6)");

                entity.Property(e => e.Lon)
                    .HasColumnName("lon")
                    .HasColumnType("numeric(9, 6)");

                entity.Property(e => e.Metro)
                    .HasColumnName("metro")
                    .HasColumnType("numeric(9, 6)");

                entity.Property(e => e.Sector)
                    .IsRequired()
                    .HasColumnName("sector")
                    .HasColumnType("char(1)");

                entity.Property(e => e.Sent1)
                    .IsRequired()
                    .HasColumnName("sent1")
                    .HasColumnType("char(2)");

                entity.Property(e => e.Sent2)
                    .IsRequired()
                    .HasColumnName("sent2")
                    .HasColumnType("char(2)");
            });
            modelBuilder.Entity<GpsRecorridos>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("gps_recorridos");

                entity.Property(e => e.Id).HasColumnName("cod_rec");

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasColumnName("activo")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CodBan).HasColumnName("cod_ban");

                entity.Property(e => e.CodLin).HasColumnName("cod_lin");

                entity.Property(e => e.CodMap).HasColumnName("cod_map");

                entity.Property(e => e.CodSec).HasColumnName("cod_sec");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaVigenciaHasta).HasColumnType("datetime");

                entity.Property(e => e.Nombre).HasMaxLength(100);

                //Ref custom
                entity.HasOne(d => d.Bandera)
                  .WithMany(p => p.Rutas)
                  .HasForeignKey(d => d.CodBan);

                //Ref custom esta relacion no me deja hacerla porque cod_linea es entero y el id de la linea es decimal
                //entity.HasOne(d => d.Linea)
                //  .WithMany(p => p.Recorridos)
                //  .HasForeignKey(d => d.CodLin);

                entity.HasOne(d => d.EstadoRuta)
                    .WithMany(p => p.Rutas)
                    .HasForeignKey(d => d.EstadoRutaId)
                    .HasConstraintName("FK_gps_recorridos_pla_EstadoRuta");

            });
            modelBuilder.Entity<Grupos>(entity =>
            {

                entity.HasKey(e => e.Id);

                entity.ToTable("grupos");

                entity.HasIndex(e => e.DesGru)
                    .HasName("grupos_in1")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("cod_gru")
                    .HasColumnType("numeric(4, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DesGru)
                    .IsRequired()
                    .HasColumnName("des_gru")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.FecBaja)
                    .HasColumnName("fec_baja")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<HBanderas>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("h_banderas");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("cod_ban");

                entity.Property(e => e.AbrBan)
                    .IsRequired()
                    .HasColumnName("abr_ban")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Activo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ClaveWay)
                    .HasColumnName("clave_way")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoVarianteLinea).HasMaxLength(50);

                entity.Property(e => e.DesBan)
                    .IsRequired()
                    .HasColumnName("des_ban")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Km)
                    .HasColumnName("km")
                    .HasColumnType("numeric(5, 2)");

                entity.Property(e => e.Kmr)
                    .HasColumnName("kmr")
                    .HasColumnType("numeric(5, 2)");

                entity.Property(e => e.RamalColorId).HasColumnName("RamalColorID");

                entity.Property(e => e.Ramalero).HasMaxLength(100);

                entity.Property(e => e.SenBan)
                    .IsRequired()
                    .HasColumnName("sen_ban")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoBanderaId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Velocidad)
                    .HasColumnName("velocidad")
                    .HasColumnType("numeric(5, 2)");

                entity.Property(e => e.SucursalId)
                    .HasColumnName("SucursalId");


                entity.HasOne(d => d.RamalColor)
                    .WithMany(p => p.HBanderas)
                    .HasForeignKey(d => d.RamalColorId)
                    .HasConstraintName("FK_h_banderas_pla_RamalColor");


                entity.HasOne(d => d.BanderasEspeciales)
                    .WithOne(p => p.HBanderas)
                    .HasForeignKey<HBanderas>(d => d.Id);
                ////.OnDelete(DeleteBehavior.ClientSetNull);


                entity.HasOne(d => d.SentidoBandera)
                    .WithMany(p => p.HBanderas)
                    .HasForeignKey(d => d.SentidoBanderaId)
                    .HasConstraintName("FK_h_banderas_pla_SentidoBandera");
            });

            modelBuilder.Entity<HBanderasEspeciales>(entity =>
            {
                entity.HasKey(e => e.CodBan);

                entity.ToTable("h_banderasEspeciales");

                entity.Property(e => e.CodBan)
                    .HasColumnName("cod_ban")
                    .ValueGeneratedNever();
            });
            modelBuilder.Entity<HBanderasTup>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("h_banderasTUP");

                entity.Property(e => e.Id)
                    .HasColumnName("codigo")
                    .ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasColumnType("char(50)");
            });
            modelBuilder.Entity<HBasec>(entity =>
            {
                entity.HasKey(e => new { e.CodHfecha, e.CodBan });

                entity.ToTable("h_basec");


                entity.Property(e => e.CodHfecha).HasColumnName("cod_hfecha");

                entity.Property(e => e.CodBan).HasColumnName("cod_ban");

                entity.Property(e => e.CodGal)
                    .HasColumnName("cod_gal")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CodRec).HasColumnName("cod_rec");

                entity.Property(e => e.CodSec).HasColumnName("cod_sec");

                entity.Property(e => e.CodSecbsas).HasColumnName("cod_secbsas");

                entity.Property(e => e.VendeBoletos).HasColumnName("vendeBoletos");

                entity.HasOne(d => d.CodBanNavigation)
                    .WithMany(p => p.HBasec)
                    .HasForeignKey(d => d.CodBan)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_basec_h_banderas");

                entity.HasOne(d => d.CodHfechaNavigation)
                    .WithMany(p => p.HBasec)
                    .HasForeignKey(d => d.CodHfecha)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_basec_h_fechas_confi");

                entity.HasOne(d => d.CodRecNavigation)
                    .WithMany(p => p.HBasec)
                    .HasForeignKey(d => d.CodRec)
                    .HasConstraintName("FK_h_basec_gps_recorridos");

                 

            });


            modelBuilder.Entity<HKilometros>(entity =>
            {
                entity.HasKey(e => new { e.CodBan, e.CodSec })
                    .HasName("h_kilometros_pk");

                entity.ToTable("h_kilometros");

                entity.Property(e => e.CodBan).HasColumnName("cod_ban");

                entity.Property(e => e.CodSec).HasColumnName("cod_sec");

                entity.Property(e => e.CodBanderaColor).HasColumnName("cod_banderaColor");

                entity.Property(e => e.CodBanderaTup).HasColumnName("cod_banderaTUP");

                entity.Property(e => e.Km)
                    .HasColumnName("km")
                    .HasColumnType("numeric(5, 2)");

                entity.Property(e => e.Kmr)
                    .HasColumnName("kmr")
                    .HasColumnType("numeric(5, 2)"); 
            });

            modelBuilder.Entity<HFechas>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Fecha });

                entity.ToTable("h_fechas");

                entity.Property(e => e.Id).HasColumnName("cod_lin");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.CodTdia).HasColumnName("cod_tdia");

                entity.Property(e => e.CompensatorioPago)
                    .HasColumnName("compensatorioPago")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Feriado)
                    .HasColumnName("feriado")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<HFechasConfi>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("h_fechas_confi");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_hfecha")
                    .ValueGeneratedNever();

                entity.Property(e => e.Activo)
                    .HasColumnName("activo")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CantCoches)
                    .HasColumnName("cant_coches")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CodLin)
                    .HasColumnName("cod_lin")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CodSabanaHoraria).HasColumnName("cod_sabanaHoraria");

                entity.Property(e => e.FecDesde)
                    .HasColumnName("fec_desde")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FecHasta)
                    .HasColumnName("fec_hasta")
                    .HasColumnType("smalldatetime");

                entity.HasOne(d => d.CodLinNavigation)
                    .WithMany(p => p.HFechasConfi)
                    .HasForeignKey(d => d.CodLin)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_fechas_confi_linea");

                entity.HasOne(d => d.PlaEstadoHorarioFecha)
                    .WithMany(p => p.HFechasConfi)
                    .HasForeignKey(d => d.PlaEstadoHorarioFechaId)
                    .HasConstraintName("FK_h_fechas_confi_pla_EstadoHorarioFecha");
            });
            modelBuilder.Entity<HServicios>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("h_servicios");


                entity.Property(e => e.Id)
                    .HasColumnName("cod_servicio");

                entity.Property(e => e.CodHconfi).HasColumnName("cod_hconfi");


                entity.Property(e => e.Duracion).HasColumnName("duracion");

                entity.Property(e => e.NroInterno)
                    .HasColumnName("nro_interno")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.NumSer)
                    .IsRequired()
                    .HasColumnName("num_ser")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodHconfiNavigation)
                    .WithMany(p => p.HServicios)
                    .HasForeignKey(d => d.CodHconfi)
                    ////.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_servicios_h_horarios_confi");
            });
            modelBuilder.Entity<HTipodia>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("h_tipodia");

                entity.HasIndex(e => e.DesTdia)
                    .HasName("h_tipodia_un")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("cod_tdia");

                entity.Property(e => e.DesTdia)
                    .IsRequired()
                    .HasColumnName("des_tdia")
                    .HasMaxLength(15);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });
            modelBuilder.Entity<HChoxser>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.CodUni });

                entity.ToTable("h_choxser");

                entity.Property(e => e.Id).HasColumnName("cod_servicio");

                entity.Property(e => e.CodUni).HasColumnName("cod_uni");

                entity.Property(e => e.CodEmp)
                    .IsRequired()
                    .HasColumnName("cod_emp")
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.Llega)
                    .HasColumnName("llega")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Sale)
                    .HasColumnName("sale")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.TipoMultiple).HasColumnName("tipo_multiple");
            });
            modelBuilder.Entity<HSectores>(entity =>
            {
                entity.HasKey(e => new { e.CodSec, e.CodHsector });

                entity.ToTable("h_sectores");

                entity.HasIndex(e => new { e.CodSec, e.CodHsector, e.Orden })
                    .HasName("h_sectores_un")
                    .IsUnique();

                entity.Property(e => e.CodSec).HasColumnName("cod_sec");

                entity.Property(e => e.CodHsector).HasColumnName("cod_hsector");

                entity.Property(e => e.CodSectorTarifario).HasColumnName("cod_sectorTarifario");

                entity.Property(e => e.LlegaA).HasColumnName("llega_a");

                entity.Property(e => e.Orden).HasColumnName("orden");

                entity.Property(e => e.SaleDe).HasColumnName("sale_de");

                entity.Property(e => e.VerEnResumen)
                    .HasColumnName("verEnResumen")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.HasOne(d => d.CodHsectorNavigation)
                    .WithMany(p => p.HSectores)
                    .HasForeignKey(d => d.CodHsector);

                entity.HasOne(d => d.CodSectorTarifarioNavigation)
                    .WithMany(p => p.HSectores)
                    .HasForeignKey(d => d.CodSectorTarifario);

            });
            modelBuilder.Entity<HDetaminxtipo>(entity =>
            {
                entity.HasKey(e => new { e.CodMinxtipo, e.CodHsector });

                entity.ToTable("h_detaminxtipo");

                entity.Property(e => e.CodMinxtipo).HasColumnName("cod_minxtipo");

                entity.Property(e => e.CodHsector).HasColumnName("cod_hsector");

                entity.Property(e => e.Minuto)
                    .HasColumnName("minuto")
                    .HasColumnType("numeric(5, 2)");

                entity.HasOne(d => d.CodHsectorNavigation)
                    .WithMany(p => p.HDetaminxtipo)
                    .HasForeignKey(d => d.CodHsector)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_detaminxtipo_pla_Coordenadas");

                entity.HasOne(d => d.CodMinxtipoNavigation)
                    .WithMany(p => p.HDetaminxtipo)
                    .HasForeignKey(d => d.CodMinxtipo)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_detaminxtipo_h_minxtipo");
            });
            modelBuilder.Entity<HHorariosConfi>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("h_horarios_confi");


                entity.Property(e => e.Id)
                    .HasColumnName("cod_hconfi");

                entity.Property(e => e.CantCoches)
                    .HasColumnName("cant_coches")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CantConductores).HasColumnName("cant_Conductores");

                entity.Property(e => e.CodHconfibsas).HasColumnName("cod_hconfibsas");

                entity.Property(e => e.CodHfecha).HasColumnName("cod_hfecha");

                entity.Property(e => e.CodSubg)
                    .HasColumnName("cod_subg")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CodTdia).HasColumnName("cod_tdia");

                entity.HasOne(d => d.CodHfechaNavigation)
                    .WithMany(p => p.HHorariosConfi)
                    .HasForeignKey(d => d.CodHfecha)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_horarios_confi_h_fechas_confi");

                entity.HasOne(d => d.CodSubgNavigation)
                    .WithMany(p => p.HHorariosConfi)
                    .HasForeignKey(d => d.CodSubg)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_horarios_confi_sub_galpon");

                entity.HasOne(d => d.CodTdiaNavigation)
                    .WithMany(p => p.HHorariosConfi)
                    .HasForeignKey(d => d.CodTdia)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_horarios_confi_h_tipodia");
            });
            modelBuilder.Entity<HMediasVueltas>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("h_medias_vueltas");



                entity.Property(e => e.Id)
                    .HasColumnName("cod_mvuelta");

                entity.Property(e => e.CodBan).HasColumnName("cod_ban");

                entity.Property(e => e.CodMvueltabsas).HasColumnName("cod_mvueltabsas");

                entity.Property(e => e.CodServicio).HasColumnName("cod_servicio");

                entity.Property(e => e.CodTpoHora)
                    .IsRequired()
                    .HasColumnName("cod_tpoHora")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.DifMin)
                    .HasColumnName("dif_min")
                    .HasColumnType("decimal(4, 0)");

                entity.Property(e => e.Llega)
                    .HasColumnName("llega")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Orden)
                    .HasColumnName("orden")
                    .HasColumnType("decimal(2, 0)");

                entity.Property(e => e.Sale)
                    .HasColumnName("sale")
                    .HasColumnType("smalldatetime");


                entity.HasOne(d => d.CodBanNavigation)
                   .WithMany(p => p.HMediasVueltas)
                   .HasForeignKey(d => d.CodBan);
                ////.OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CodServicioNavigation)
                    .WithMany(p => p.HMediasVueltas)
                    .HasForeignKey(d => d.CodServicio);
                ////.OnDelete(DeleteBehavior.ClientSetNull)
                //.HasConstraintName("FK_h_medias_vueltas_h_servicios");

                entity.HasOne(d => d.CodTpoHoraNavigation)
                    .WithMany(p => p.HMediasVueltas)
                    .HasForeignKey(d => d.CodTpoHora);
                ////.OnDelete(DeleteBehavior.ClientSetNull)
                //.HasConstraintName("FK_h_medias_vueltas_h_tposHoras");
            });
            modelBuilder.Entity<HMinxtipo>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("h_minxtipo");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_minxtipo");

                entity.Property(e => e.CodBan).HasColumnName("cod_ban");

                entity.Property(e => e.CodHfecha).HasColumnName("cod_hfecha");

                entity.Property(e => e.CodMinxtipobsas).HasColumnName("cod_minxtipobsas");

                entity.Property(e => e.CodTdia).HasColumnName("cod_tdia");

                entity.Property(e => e.TipoHora)
                    .IsRequired()
                    .HasColumnName("tipo_hora")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.TotalMin)
                    .HasColumnName("total_min")
                    .HasColumnType("numeric(3, 0)");

                entity.HasOne(d => d.CodBanNavigation)
                    .WithMany(p => p.HMinxtipo)
                    .HasForeignKey(d => d.CodBan)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_minxtipo_h_banderas");

                entity.HasOne(d => d.CodHfechaNavigation)
                    .WithMany(p => p.HMinxtipo)
                    .HasForeignKey(d => d.CodHfecha)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_minxtipo_h_fechas_confi");

                entity.HasOne(d => d.CodTdiaNavigation)
                    .WithMany(p => p.HMinxtipo)
                    .HasForeignKey(d => d.CodTdia)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_minxtipo_h_tipodia");

                entity.HasOne(d => d.TipoHoraNavigation)
                    .WithMany(p => p.HMinxtipo)
                    .HasForeignKey(d => d.TipoHora)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_minxtipo_h_tposHoras");
            });
            modelBuilder.Entity<HProcMin>(entity =>
            {
                entity.HasKey(e => new { e.CodMvuelta, e.CodHsector });

                entity.ToTable("h_proc_min");

                entity.Property(e => e.CodMvuelta).HasColumnName("cod_mvuelta");

                entity.Property(e => e.CodHsector).HasColumnName("cod_hsector");

                entity.Property(e => e.Minuto)
                    .HasColumnName("minuto")
                    .HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.CodHsectorNavigation)
                    .WithMany(p => p.HProcMin)
                    .HasForeignKey(d => d.CodHsector)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_proc_min_pla_Coordenadas");

                entity.HasOne(d => d.CodMvueltaNavigation)
                    .WithMany(p => p.HProcMin)
                    .HasForeignKey(d => d.CodMvuelta)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_h_proc_min_h_medias_vueltas");
            });
            modelBuilder.Entity<HTposHoras>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("h_tposHoras");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_tpoHora")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CodTpoHorabsas)
                    .HasColumnName("cod_tpoHorabsas")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.DscTpoHora)
                    .IsRequired()
                    .HasColumnName("dsc_tpoHora")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Orden).HasColumnName("orden");
            });

            modelBuilder.Entity<CCoches>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("c_coches");

                entity.Property(e => e.Id)
                    .HasColumnName("nro_interno")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AireAcondicionado)
                    .HasColumnName("aireAcondicionado")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Anio)
                    .HasColumnName("anio")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.AsientosHab).HasColumnName("asientos_hab");

                entity.Property(e => e.CantAsientos).HasColumnName("cant_asientos");

                entity.Property(e => e.Carroceria)
                    .IsRequired()
                    .HasColumnName("carroceria")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodEmpr).HasColumnName("cod_empr");

                entity.Property(e => e.CodEmpresaSube).HasColumnName("cod_empresaSube");

                entity.Property(e => e.CodGruTar).HasColumnName("cod_gru_tar");

                entity.Property(e => e.CodTpoAsiento).HasColumnName("cod_tpoAsiento");

                entity.Property(e => e.Cortinas)
                    .HasColumnName("cortinas")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Dominio)
                    .HasColumnName("dominio")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FecHab)
                    .HasColumnName("fec_hab")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecIng)
                    .HasColumnName("fec_ing")
                    .HasColumnType("datetime");

                entity.Property(e => e.Ficha).HasColumnName("ficha");

                entity.Property(e => e.Gps)
                    .HasColumnName("gps")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Interno)
                    .IsRequired()
                    .HasColumnName("interno")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.InternoAnterior)
                    .HasColumnName("internoAnterior")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.InternoSap)
                    .HasColumnName("internoSap")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Kilometraje)
                    .HasColumnName("kilometraje")
                    .HasColumnType("numeric(10, 2)");

                entity.Property(e => e.Marca)
                    .IsRequired()
                    .HasColumnName("marca")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NroChasis)
                    .IsRequired()
                    .HasColumnName("nro_chasis")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.NroHab)
                    .HasColumnName("nro_hab")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NroInternoBa)
                    .HasColumnName("nro_interno_ba")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.NroMotor)
                    .IsRequired()
                    .HasColumnName("nro_motor")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Proveedor)
                    .HasColumnName("proveedor")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RampaDiscapacitados)
                    .HasColumnName("rampa_discapacitados")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Titular)
                    .IsRequired()
                    .HasColumnName("titular")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Visible)
                    .HasColumnName("visible")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                //Ref custom
                entity.HasOne(d => d.Empresa)
                .WithMany(p => p.Coches)
                 .HasForeignKey(d => d.CodEmpr);
            });
            modelBuilder.Entity<Sucursales>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("sucursales");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_sucursal")
                    .ValueGeneratedNever();

                entity.Property(e => e.DscSucursal)
                    .IsRequired()
                    .HasColumnName("dsc_sucursal")
                    .HasColumnType("char(20)");

                entity.Property(e => e.EntornoActivo)
                    .HasColumnName("entorno_activo")
                    .HasColumnType("char(1)");

                entity.Property(e => e.NomServidor)
                    .HasColumnName("nom_servidor")
                    .HasColumnType("char(100)");
            });

            modelBuilder.Entity<SucursalesxEmpresas>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.CodEmpr });

                entity.ToTable("sucursalesxEmpresas");

                entity.Property(e => e.Id).HasColumnName("cod_sucursal");

                entity.Property(e => e.CodEmpr)
                    .HasColumnName("cod_empr")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.FecBaja)
                    .HasColumnName("fec_baja")
                    .HasColumnType("datetime");

                //entity.HasOne(d => d.CodSucursalNavigation)
                //    .WithMany(p => p.Sucursales)
                //    .HasForeignKey(d => d.Id)
                //    //.OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK__sucursale__cod_s__6EA1A496");
            });

            modelBuilder.Entity<SucursalesxLineas>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.CodLinea });

                entity.ToTable("sucursalesxLineas");

                entity.Property(e => e.Id).HasColumnName("cod_sucursal");

                entity.Property(e => e.CodLinea)
                    .HasColumnName("cod_linea")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.FecBaja)
                    .HasColumnName("fec_baja")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Lineas)
                    .WithMany(p => p.SucursalesxLineas)
                    .HasForeignKey(d => d.CodLinea);
                ////.OnDelete(DeleteBehavior.ClientSetNull)
                //.HasConstraintName("fk_bol_banderasRamales_linea");


            });
            modelBuilder.Entity<TipoDni>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<PlanCam>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("plan_cam");

                entity.HasIndex(e => e.Depot)
                    .HasName("plan_cam_in3")
                    .IsUnique();

                entity.HasIndex(e => e.DesPlan)
                    .HasName("plan_cam_in1")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("plan_cam")
                    .HasColumnType("numeric(4, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Depot)
                    .IsRequired()
                    .HasColumnName("depot")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DesPlan)
                    .IsRequired()
                    .HasColumnName("des_plan")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasColumnType("numeric(20, 0)");
            });

            modelBuilder.Entity<Configu>(entity =>
            {
                entity.HasKey(e => new { e.CodGru, e.CodEmpr, e.CodSuc, e.CodLin, e.CodGal, e.CodSubg, e.PlanCam });

                entity.ToTable("configu");

                entity.HasIndex(e => new { e.CodGru, e.CodEmpr, e.CodSuc, e.CodLin, e.CodGal, e.CodSubg, e.PlanCam })
                    .HasName("configu_in1")
                    .IsUnique();

                entity.Property(e => e.CodGru)
                    .HasColumnName("cod_gru")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CodEmpr)
                    .HasColumnName("cod_empr")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CodSuc)
                    .HasColumnName("cod_suc")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CodSucCast)
                    .HasColumnName("cod_sucCast")
                    .HasComputedColumnSql("(CONVERT([int],[cod_suc]))");

                entity.Property(e => e.CodLin)
                    .HasColumnName("cod_lin")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CodGal)
                    .HasColumnName("cod_gal")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.CodSubg)
                    .HasColumnName("cod_subg")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.PlanCam)
                    .HasColumnName("plan_cam")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.FecBaja)
                    .HasColumnName("fec_baja")
                    .HasColumnType("datetime");

                //Ref custom
                entity.HasOne(d => d.Grupo)
                  .WithMany(p => p.Configu)
                  .HasForeignKey(d => d.CodGru);

                //Ref custom
                entity.HasOne(d => d.Empresa)
                  .WithMany(p => p.Configu)
                  .HasForeignKey(d => d.CodEmpr);


                //Ref custom
                entity.HasOne(d => d.Sucursal)
                  .WithMany(p => p.Configu)
                  .HasForeignKey(d => d.CodSucCast);

                //Ref custom
                entity.HasOne(d => d.Linea)
                  .WithMany(p => p.Configu)
                  .HasForeignKey(d => d.CodLin);

                //Ref custom
                entity.HasOne(d => d.Galpon)
                  .WithMany(p => p.Configu)
                  .HasForeignKey(d => d.CodGal);


                //Ref custom
                entity.HasOne(d => d.SubGalpon)
                  .WithMany(p => p.Configu)
                  .HasForeignKey(d => d.CodSubg);


                //Ref custom
                entity.HasOne(d => d.PlanCamNav)
                  .WithMany(p => p.Configu)
                  .HasForeignKey(d => d.PlanCam);
            });

            #endregion

            #region ART

            // ART Denuncias y Reclamos ↓ 
            modelBuilder.Entity<ArtContingencias>(entity =>
            {
                entity.ToTable("ART_Contingencias");

                entity.Property(e => e.Descripcion).IsRequired();
            });
            modelBuilder.Entity<ArtDenunciaAdjuntos>(entity =>
            {
                entity.HasKey(e => new { e.DenunciaId, e.AdjuntoId });

                entity.ToTable("ART_DenunciaAdjuntos");

                //entity.HasOne(d => d.Denuncia)
                //    .WithMany(p => p.ArtDenunciaAdjuntos)
                //    .HasForeignKey(d => d.DenunciaId)
                //    //.OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_ART_Denuncias");
            });
            modelBuilder.Entity<ArtDenuncias>(entity =>
            {
                entity.ToTable("ART_Denuncias");

                entity.Property(e => e.AltaLaboral).HasDefaultValueSql("((0))");

                entity.Property(e => e.AltaMedica).HasDefaultValueSql("((0))");

                entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");

                entity.Property(e => e.BajaServicio).HasDefaultValueSql("((0))");

                entity.Property(e => e.EmpleadoAntiguedad).HasColumnType("datetime");

                entity.Property(e => e.EmpleadoArea).HasMaxLength(150);


                entity.Property(e => e.EmpleadoEmpresaId).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.EmpleadoFechaIngreso).HasColumnType("datetime");

                entity.Property(e => e.EmpleadoLegajo).HasMaxLength(150);

                entity.Property(e => e.EmpresaId).HasColumnType("numeric(4, 0)");

                entity.Property(e => e.FechaAltaLaboral).HasColumnType("datetime");

                entity.Property(e => e.FechaAltaMedica).HasColumnType("datetime");

                entity.Property(e => e.FechaAudienciaMedica).HasColumnType("datetime");

                entity.Property(e => e.FechaBajaServicio).HasColumnType("datetime");

                entity.Property(e => e.FechaOcurrencia).HasColumnType("datetime");

                entity.Property(e => e.FechaProximaConsulta).HasColumnType("datetime");

                entity.Property(e => e.FechaRecepcionDenuncia).HasColumnType("datetime");

                entity.Property(e => e.FechaUltimoControl).HasColumnType("datetime");

                entity.Property(e => e.FechaProbableAlta).HasColumnType("datetime");

                entity.Property(e => e.MotivoAnulado).IsUnicode(false);

                entity.Property(e => e.Juicio).HasComputedColumnSql("([dbo].[GetJuicio]([Id]))");

                entity.Property(e => e.DniEmpleado)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasComputedColumnSql("([dbo].[GetDniEmpleado]([Id]))");

                entity.Property(e => e.NombreEmpleado)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComputedColumnSql("([dbo].[GetNombreEmpleado]([Id]))");

                entity.Property(e => e.NroDenuncia).HasMaxLength(100);

                entity.Property(e => e.PorcentajeIncapacidad).HasColumnType("decimal(20, 2)");

                entity.Property(e => e.TieneReingresos).HasComputedColumnSql("([dbo].[GetART_ReingresosBoolean]([NroDenuncia]))");


                entity.HasOne(d => d.Contingencia)
                    .WithMany(p => p.ArtDenuncias)
                    .HasForeignKey(d => d.ContingenciaId)
                    .HasConstraintName("FK_ART_Denuncias_ART_Contingencias");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.ArtDenuncias)
                    .HasForeignKey(d => d.EstadoId)
                    .HasConstraintName("FK_ART_Denuncias_ART_Estados");

                entity.HasOne(d => d.MotivoAudiencia)
                    .WithMany(p => p.ArtDenuncias)
                    .HasForeignKey(d => d.MotivoAudienciaId)
                    .HasConstraintName("FK_ART_Denuncias_ART_MotivosAudiencias");

                entity.HasOne(d => d.Patologia)
                    .WithMany(p => p.ArtDenuncias)
                    .HasForeignKey(d => d.PatologiaId)
                    .HasConstraintName("FK_ART_Denuncias_ART_Patologias");

                entity.HasOne(d => d.PrestadorMedico)
                    .WithMany(p => p.ArtDenuncias)
                    .HasForeignKey(d => d.PrestadorMedicoId)
                    .HasConstraintName("FK_ART_Denuncias_ART_PrestadoresMedicos");

                entity.HasOne(d => d.Siniestro)
                    .WithMany(p => p.ArtDenuncias)
                    .HasForeignKey(d => d.SiniestroId)
                    .HasConstraintName("FK_ART_Denuncias_sin_Siniestros");

                entity.HasOne(d => d.Sucursal)
                    .WithMany(p => p.ArtDenuncias)
                    .HasForeignKey(d => d.SucursalId)
                    .HasConstraintName("FK_ART_Denuncias_sucursales");

                entity.HasOne(d => d.Tratamiento)
                    .WithMany(p => p.ArtDenuncias)
                    .HasForeignKey(d => d.TratamientoId)
                    .HasConstraintName("FK_ART_Denuncias_ART_Tratamientos");


                entity.HasOne(d => d.EmpleadoEmpresa)
                    .WithMany(p => p.ArtDenunciasEmpleadoEmpresa)
                    .HasForeignKey(d => d.EmpleadoEmpresaId)
                    .HasConstraintName("FK_Denuncias_EmpresaEmpleado");

                entity.HasOne(d => d.Empresa)
                    .WithMany(p => p.ArtDenunciasEmpresa)
                    .HasForeignKey(d => d.EmpresaId)
                    .HasConstraintName("FK_Denuncias_Empresa");


                entity.HasOne(d => d.DenunciaOrigen)
                    .WithMany(p => p.ArtDenunciaReingresos)
                    .HasForeignKey(d => d.DenunciaIdOrigen)
                    .HasConstraintName("FK_ART_Denuncias_ART_DenunciaReingresos");
            });
            modelBuilder.Entity<ArtDenunciasNotificaciones>(entity =>
            {
                entity.ToTable("ART_DenunciasNotificaciones");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.HasOne(d => d.Denuncia)
                    .WithMany(p => p.ArtDenunciasNotificaciones)
                    .HasForeignKey(d => d.DenunciaId)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DenunciasNotificaciones-Denuncia");

                entity.HasOne(d => d.Motivo)
                    .WithMany(p => p.ArtDenunciasNotificaciones)
                    .HasForeignKey(d => d.MotivoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DenunciasNotificaciones-Motivo");
            });
            modelBuilder.Entity<ArtEstados>(entity =>
            {
                entity.ToTable("ART_Estados");

                entity.Property(e => e.Descripcion).HasMaxLength(150);
            });
            modelBuilder.Entity<ArtMotivosAudiencias>(entity =>
            {
                entity.ToTable("ART_MotivosAudiencias");

                entity.Property(e => e.Descripcion).IsRequired();
            });
            modelBuilder.Entity<ArtMotivosNotificaciones>(entity =>
            {
                entity.ToTable("ART_MotivosNotificaciones");

                entity.Property(e => e.Descripcion).IsRequired();
            });
            modelBuilder.Entity<ArtPatologias>(entity =>
            {
                entity.ToTable("ART_Patologias");

                entity.Property(e => e.Descripcion).IsRequired();
            });
            modelBuilder.Entity<ArtPrestadoresMedicos>(entity =>
            {
                entity.ToTable("ART_PrestadoresMedicos");

                entity.Property(e => e.Descripcion).IsRequired();
            });


            modelBuilder.Entity<ArtTratamientos>(entity =>
            {
                entity.ToTable("ART_Tratamientos");

                entity.Property(e => e.Descripcion).IsRequired();
            });

            modelBuilder.Entity<TiposReclamo>(entity =>
            {
                entity.ToTable("ART_TiposReclamo");
                entity.Property(e => e.Descripcion).IsRequired();
            });
            modelBuilder.Entity<TiposAcuerdo>(entity =>
            {
                entity.ToTable("ART_TiposAcuerdo");
                entity.Property(e => e.Descripcion).IsRequired();
            });
            modelBuilder.Entity<CausasReclamo>(entity =>
            {
                entity.ToTable("ART_CausasReclamo");
                entity.Property(e => e.Descripcion).IsRequired();
            });
            modelBuilder.Entity<RubrosSalariales>(entity =>
            {
                entity.ToTable("ART_RubrosSalariales");
                entity.Property(e => e.Descripcion).IsRequired();
            });
            // ART Denuncias y Reclamos↑


            #endregion

            #region Inspectores

            modelBuilder.Entity<InsDesvios>(entity =>
            {
                entity.ToTable("Ins_Desvios");

                entity.Property(e => e.Id);

                entity.Property(e => e.Descripcion)
                                .IsRequired()
                                .HasMaxLength(500);


                entity.Property(e => e.FechaHora).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            });


            modelBuilder.Entity<InspGeo>(entity =>
            {
                entity.ToTable("insp_geolocalizacion");

                entity.Property(e => e.Id);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CurrentTime).HasColumnType("datetime");

                entity.Property(e => e.Latitud).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Longitud).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Accion).HasMaxLength(150);

            });

            modelBuilder.Entity<InspGeo_Hist>(entity =>
            {
                entity.ToTable("insp_geolocalizacion_hist");

                entity.Property(e => e.Id);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CurrentTime).HasColumnType("datetime");

                entity.Property(e => e.Latitud).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Longitud).HasColumnType("decimal(10, 6)");

                entity.Property(e => e.Accion).HasMaxLength(150);

            });

            modelBuilder.Entity<InspTarea>(entity =>
            {
                entity.ToTable("insp_Tareas");
                entity.HasKey("Id");
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Anulado).IsRequired();
                entity.Property(e => e.CreatedDate).HasColumnType("datetime2").IsRequired();
                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime2");
                entity.Property(x => x.CreatedUserId).HasColumnName(@"CreatedUserId").HasColumnType("int").IsRequired();
                entity.Property(x => x.LastUpdatedUserId).HasColumnName(@"LastUpdatedUserId").HasColumnType("int");
            });

            modelBuilder.Entity<InspTareaCampo>(entity =>
            {
                entity.ToTable("insp_TareasCampos");
                entity.HasKey("Id");
                entity.Property(e => e.TareaId).HasColumnType("int").IsRequired();
                entity.Property(e => e.Requerido).HasColumnType("bit").IsRequired();
                entity.Property(e => e.TareaCampoConfigId).HasColumnType("int").IsRequired();
                entity.Property(e => e.Etiqueta).IsRequired();
                entity.Property(x => x.Orden).HasColumnType("int").IsRequired();
                entity.HasOne(e=> e.Tarea)
                    .WithMany(p => p.TareasCampos)
                    .HasForeignKey(d => d.TareaId)                    
                    .HasConstraintName("FK_insp_TareasCampos_insp_Tareas");
                entity.HasOne(e => e.TareaCampoConfig)
                    .WithMany(e => e.TareasCampos)
                    .HasForeignKey(e => e.TareaCampoConfigId)                    
                    .HasConstraintName("FK_insp_TareasCampos_insp_TareasCamposConfig");
                
            });

            modelBuilder.Entity<InspTareaCampoConfig>(entity =>
            {
                entity.ToTable("Insp_TareasCamposConfig");
                entity.HasKey("Id");
                entity.Property(e => e.Descripcion).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Campo).HasMaxLength(100).IsRequired();              
            });




            modelBuilder.Entity<InspTareasRealizadas>(entity =>
            {
                entity.ToTable("Insp_TareasRealizadas");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DeletedUserId).HasColumnName("DeletedUserID");

                entity.Property(e => e.Fecha)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Tarea)
                    .WithMany()
                    .HasForeignKey(d => d.TareaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Insp_TareasRealizadas_insp_Tareas");
            });

            modelBuilder.Entity<InspTareasRealizadasDetalle>(entity =>
            {
                entity.ToTable("Insp_TareasRealizadasDetalle");

                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.TareaCampo)
                    .WithMany()
                    .HasForeignKey(d => d.TareaCampoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Insp_TareasRealizadasDetalle_insp_TareasCampos");

                entity.HasOne(d => d.TareaRealizada)
                    .WithMany(p => p.InspTareasRealizadasDetalle)
                    .HasForeignKey(d => d.TareaRealizadaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Insp_TareasRealizadasDetalle_Insp_TareasRealizadas");
            });


            #endregion

            modelBuilder.Entity<PlaTipoViaje>(entity =>
            {
                entity.ToTable("pla_TipoViaje");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TravelMode)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PlaTipoViaje>(entity =>
            {
                entity.ToTable("pla_TipoViaje");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TravelMode)
                    .IsRequired()
                    .HasMaxLength(50);
            });


            #region FirmaDigital

            modelBuilder.Entity<FdAcciones>(entity =>
            {
                entity.ToTable("FD_Acciones");

                entity.Property(e => e.AccionBdempleador).HasColumnName("AccionBDEmpleador");

                entity.Property(e => e.AccionPermitidaId).HasColumnName("AccionPermitidaID");

                entity.Property(e => e.EstadoActualId).HasColumnName("EstadoActualID");

                entity.Property(e => e.EstadoNuevoId).HasColumnName("EstadoNuevoID");

                entity.Property(e => e.MostrarBdempleado).HasColumnName("MostrarBDEmpleado");

                entity.Property(e => e.MostrarBdempleador).HasColumnName("MostrarBDEmpleador");

                entity.Property(e => e.NotificacionId).HasColumnName("NotificacionID");

                entity.Property(e => e.TipoDocumentoId).HasColumnName("TipoDocumentoID");

                entity.HasOne(d => d.AccionPermitida)
                    .WithMany(p => p.FdAcciones)
                    .HasForeignKey(d => d.AccionPermitidaId)
                    .HasConstraintName("FK_FD_Acciones_FD_AccionesPermitidas");

                entity.HasOne(d => d.EstadoActual)
                    .WithMany(p => p.FdAccionesEstadoActual)
                    .HasForeignKey(d => d.EstadoActualId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_Acciones_FD_Estados");

                entity.HasOne(d => d.EstadoNuevo)
                    .WithMany(p => p.FdAccionesEstadoNuevo)
                    .HasForeignKey(d => d.EstadoNuevoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_Acciones_FD_Estados1");

                entity.HasOne(d => d.TipoDocumento)
                    .WithMany(p => p.FdAcciones)
                    .HasForeignKey(d => d.TipoDocumentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_Flujos_FD_TiposDocumentos");
            });

            modelBuilder.Entity<FdAccionesPermitidas>(entity =>
            {
                entity.ToTable("FD_AccionesPermitidas");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Permiso)
    .WithMany(p => p.AccionesPermitidas)
    .HasForeignKey(d => d.PermissionId)
    .HasConstraintName("PK_FD_AccionesPermitidas_Sys-Permissions");
            });

            modelBuilder.Entity<FdDocumentosError>(entity =>
            {
                entity.ToTable("FD_DocumentosError");

                entity.Property(e => e.Cuilempleado)
                    .HasColumnName("CUILEmpleado")
                    .HasMaxLength(250);

                entity.Property(e => e.DetalleError).IsRequired();

                entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");

                entity.Property(e => e.EmpresaEmpleadoId)
                    .HasColumnName("EmpresaEmpleadoID")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.FechaProcesado).HasColumnType("datetime");

                entity.Property(e => e.LegajoEmpleado).HasMaxLength(250);

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SucursalEmpleadoId).HasColumnName("SucursalEmpleadoID");

                entity.Property(e => e.TipoDocumentoId).HasColumnName("TipoDocumentoID");

                entity.Property(e => e.NombreEmpleado)
    .HasMaxLength(250)
    .IsUnicode(false)
    .HasComputedColumnSql("([dbo].[GetNombreEmpleadoWithCodEmp]([EmpleadoId]))");

                entity.HasOne(d => d.TipoDocumento)
                    .WithMany(p => p.FdDocumentosError)
                    .HasForeignKey(d => d.TipoDocumentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_DocumentosError_FD_TiposDocumentos");

                entity.HasOne(d => d.Sucursal)
                    .WithMany(p => p.FdDocumentosError)
                    .HasForeignKey(d => d.SucursalEmpleadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_DocumentosError_FD_Sucursal");

                entity.HasOne(d => d.Empresa)
                    .WithMany(p => p.DocumentosError)
                    .HasForeignKey(d => d.EmpresaEmpleadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_DocumentosError_FD_Empresa");

            });

            modelBuilder.Entity<FdDocumentosProcesados>(entity =>
            {
                entity.ToTable("FD_DocumentosProcesados");

                entity.Property(e => e.ArchivoId).HasColumnName("ArchivoID");

                //entity.Property(e => e.CodMinisterio).HasMaxLength(100);

                entity.Property(e => e.Cuilempleado)
                    .IsRequired()
                    .HasColumnName("CUILEmpleado")
                    .HasMaxLength(250);

                entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");

                entity.Property(e => e.EmpresaEmpleadoId)
                    .HasColumnName("EmpresaEmpleadoID")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.NombreEmpleado)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasComputedColumnSql("([dbo].[GetNombreEmpleadoWithCodEmp]([EmpleadoId]))");

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.FechaProcesado).HasColumnType("datetime");

                entity.Property(e => e.LegajoEmpleado).HasMaxLength(250);

                entity.Property(e => e.MotivoRechazo).HasMaxLength(500);

                entity.Property(e => e.SucursalEmpleadoId).HasColumnName("SucursalEmpleadoID");

                entity.Property(e => e.TipoDocumentoId).HasColumnName("TipoDocumentoID");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.FdDocumentosProcesados)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_DocumentosProcesados_FD_Estados");

                entity.HasOne(d => d.TipoDocumento)
                    .WithMany(p => p.FdDocumentosProcesados)
                    .HasForeignKey(d => d.TipoDocumentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_DocumentosProcesados_FD_TiposDocumentos");

                entity.HasOne(d => d.Sucursal)
                    .WithMany(p => p.FdDocumentosProcesados)
                    .HasForeignKey(d => d.SucursalEmpleadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_DocumentosProcesados_FD_Sucursal");

                entity.HasOne(d => d.Empresa)
                    .WithMany(p => p.DocumentosProcesados)
                    .HasForeignKey(d => d.EmpresaEmpleadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_DocumentosProcesados_FD_Empresa");
            });

            modelBuilder.Entity<FdDocumentosProcesadosHistorico>(entity =>
            {
                entity.ToTable("FD_DocumentosProcesadosHistorico");

                entity.Property(e => e.ArchivoId).HasColumnName("ArchivoID");

                //entity.Property(e => e.CodMinisterio).HasMaxLength(100);

                entity.Property(e => e.DocumentoProcesadoId).HasColumnName("DocumentoProcesadoID");

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.HasOne(d => d.DocumentoProcesado)
                    .WithMany(p => p.FdDocumentosProcesadosHistorico)
                    .HasForeignKey(d => d.DocumentoProcesadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_DocumentosProcesadosHistorico_FD_DocumentosProcesados");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.FdDocumentosProcesadosHistorico)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_DocumentosProcesadosHistorico_FD_Estados");



                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.FdDocumentosProcesadosHistorico)
                    .HasForeignKey(d => d.CreatedUserId);
            });

            modelBuilder.Entity<FdEstados>(entity =>
            {
                entity.ToTable("FD_Estados");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ImportarDocumentoOk).HasColumnName("ImportarDocumentoOK");
            });

            modelBuilder.Entity<FdTiposDocumentos>(entity =>
            {
                entity.ToTable("FD_TiposDocumentos");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Prefijo)
                    .IsRequired()
                    .HasMaxLength(3);
            });
            modelBuilder.Entity<FdCertificados>(entity =>
            {
                entity.ToTable("FD_Certificados");

                entity.Property(e => e.ArchivoId).HasColumnName("ArchivoID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FechaActivacion).HasColumnType("datetime");

                entity.Property(e => e.FechaRevocacion).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.FdCertificadosCreatedUser)
                    .HasForeignKey(d => d.CreatedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_Certificados_sys_usersAD1");

                entity.HasOne(d => d.LastUpdatedUser)
                    .WithMany(p => p.FdCertificadosLastUpdatedUser)
                    .HasForeignKey(d => d.LastUpdatedUserId)
                    .HasConstraintName("FK_FD_Certificados_sys_usersAD2");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.FdCertificadosUsuario)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_Certificados_sys_usersAD");
            });


            modelBuilder.Entity<FdFirmador>(entity =>
            {
                entity.ToTable("FD_Firmador");

                entity.Property(e => e.CoordenadasEmpleado)
                    .IsRequired()
                    .HasColumnName("Coordenadas_Empleado")
                    .HasMaxLength(100);

                entity.Property(e => e.CoordenadasEmpleador)
                    .IsRequired()
                    .HasColumnName("Coordenadas_Empleador")
                    .HasMaxLength(100);

                entity.Property(e => e.PathGetDescarga)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.PathPostSubida)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.SessionId).IsRequired();

                entity.Property(e => e.UsuarioApellido)
                    .IsRequired()
                    .HasColumnName("Usuario_Apellido")
                    .HasMaxLength(200);

                entity.Property(e => e.UsuarioId)
                    .IsRequired()
                    .HasColumnName("Usuario_Id")
                    .HasMaxLength(11);

                entity.Property(e => e.UsuarioNombre)
                    .IsRequired()
                    .HasColumnName("Usuario_Nombre")
                    .HasMaxLength(200);

                entity.Property(e => e.UsuarioRol)
                    .IsRequired()
                    .HasColumnName("Usuario_rol")
                    .HasMaxLength(20);

                entity.Property(e => e.UsuarioUserName)
                    .IsRequired()
                    .HasColumnName("Usuario_UserName")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<FdFirmadorDetalle>(entity =>
            {
                entity.ToTable("FD_FirmadorDetalle");

                entity.HasOne(d => d.DocumentoProcesado)
                    .WithMany()
                    .HasForeignKey(d => d.DocumentoProcesadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_DetalleFirmador_FD_DocumentosProcesados");

                entity.HasOne(d => d.Firmador)
                    .WithMany(p => p.FdFirmadorDetalle)
                    .HasForeignKey(d => d.FirmadorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_DetalleFirmador_FD_Firmador");
            });

            modelBuilder.Entity<FdFirmadorLog>(entity =>
            {
                entity.ToTable("FD_FirmadorLog");

                entity.Property(e => e.DetalleLog).IsRequired();

                entity.Property(e => e.FechaHora).HasColumnType("datetime");

                entity.Property(e => e.FirmadorId).HasColumnName("Firmador_Id");

                entity.HasOne(d => d.Firmador)
                    .WithMany(p => p.FdFirmadorLog)
                    .HasForeignKey(d => d.FirmadorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FD_FirmadorLog_FD_Firmador");
            });

            #endregion

        }

        public static void ApplyConfigurationsOperacionesRB(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Empleados>(entity =>
            {
                entity.ToTable("empleados");

                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Cuil)
                    .HasName("UK_CuilEmpleados")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("cod_empleado");

                entity.Property(e => e.Apellido)
                    .HasColumnName("apellido")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AportesAntPrivilegiados)
                    .HasColumnName("aportes_antPrivilegiados");

                entity.Property(e => e.AportesAntSimples)
                    .HasColumnName("aportes_antSimples");

                entity.Property(e => e.Area)
                    .HasColumnName("area")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BlockDomicilio)
                    .HasColumnName("block_domicilio")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CalleDomicilio)
                    .HasColumnName("calle_domicilio")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Categoria)
                    .HasColumnName("categoria")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.CodLinea)
                    .HasColumnName("cod_linea");

                entity.Property(e => e.CodLocalidad)
                    .HasColumnName("cod_localidad");

                entity.Property(e => e.CodObrasocial)
                    .HasColumnName("cod_obrasocial")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ConvColectivo)
                    .HasColumnName("convColectivo")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Convenio)
                    .HasColumnName("convenio")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Cuil)
                    .HasColumnName("cuil")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.DeptoDomicilio)
                    .HasColumnName("depto_domicilio")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Dni)
                    .HasColumnName("dni")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.FecAntiguedad)
                    .HasColumnName("fec_antiguedad")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecNacimiento)
                    .HasColumnName("fec_nacimiento")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecProbJubilacion)
                    .HasColumnName("fec_probJubilacion")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.FecProcesado)
                    .HasColumnName("fec_procesado")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecVacaciones)
                    .HasColumnName("fec_vacaciones")
                    .HasColumnType("datetime");

                entity.Property(e => e.GestionTiempoReal)
                    .HasColumnName("gestionTiempoReal");

                entity.Property(e => e.IdLector)
                    .HasColumnName("idLector")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.IntimadoJubilarse)
                    .HasColumnName("intimadoJubilarse")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Jubilado)
                    .HasColumnName("jubilado");

                entity.Property(e => e.LatDomicilio)
                    .HasColumnName("lat_domicilio");

                entity.Property(e => e.LonDomicilio)
                    .HasColumnName("lon_domicilio");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NroDomicilio)
                    .HasColumnName("nro_domicilio")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Obrasocial)
                    .HasColumnName("obrasocial")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ObsJubilacion)
                    .HasColumnName("obs_jubilacion")
                    .HasMaxLength(600)
                    .IsUnicode(false);

                entity.Property(e => e.Pin)
                    .HasColumnName("pin")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.PisoDomicilio)
                    .HasColumnName("piso_domicilio")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Sexo)
                    .HasColumnName("sexo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Un)
                    .HasColumnName("un")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                //Ref custom
                entity.HasOne(d => d.UnidadNegocio)
                  .WithMany(p => p.Empleados)
                  .HasForeignKey(d => d.Un);
            });

            modelBuilder.Entity<LegajosEmpleado>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.FecIngreso });

                entity.ToTable("legajos_empleado");

                entity.Property(e => e.Id).HasColumnName("cod_empleado");

                entity.Property(e => e.FecIngreso)
                    .HasColumnName("fec_ingreso")
                    .HasColumnType("datetime");

                entity.Property(e => e.CodEmpresa).HasColumnName("cod_empresa");

                entity.Property(e => e.FecBaja)
                    .HasColumnName("fec_baja")
                    .HasColumnType("datetime");

                entity.Property(e => e.FecProcesado)
                    .HasColumnName("fec_procesado")
                    .HasColumnType("datetime");

                entity.Property(e => e.LegajoSap)
                    .HasColumnName("legajoSap")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                //Ref custom
                //entity.HasOne(d => d.Empleado)
                // .WithMany(p => p.LegajosEmpleado);
                //  .HasForeignKey(d => d.Id);

            });

            modelBuilder.Entity<Localidades>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("localidades");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_localidad")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CodPostal)
                    .IsRequired()
                    .HasColumnName("cod_postal")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CodProvincia)
                    .HasColumnName("cod_provincia");

                entity.Property(e => e.DscLocalidad)
                    .IsRequired()
                    .HasColumnName("dsc_localidad")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                //Ref custom
                entity.HasOne(d => d.Provincia)
                  .WithMany(p => p.Localidades)
                  .HasForeignKey(d => d.CodProvincia);
            });

            modelBuilder.Entity<Provincias>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("provincias");

                entity.Property(e => e.Id)
                    .HasColumnName("cod_provincia")
                    .ValueGeneratedNever();

                entity.Property(e => e.DscProvincia)
                    .HasColumnName("dsc_provincia")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UnidadesNegocio>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("unidadesNegocio");

                entity.Property(e => e.Id)
                    .HasColumnName("UN")
                    .ValueGeneratedNever();

                entity.Property(e => e.descripcion)
                    .HasColumnName("descripcion")
                    .HasMaxLength(255);

                entity.Property(e => e.cod_sucursal)
                    .HasColumnName("cod_sucursal")
                    .IsUnicode(false);


            });



        }

        public static void ApplyConfigurationsAdjuntos(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Adjuntos>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });

        }
    }

}
