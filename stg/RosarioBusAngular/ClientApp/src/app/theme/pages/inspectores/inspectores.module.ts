import { UserService } from './../admin/users/user.service';
import { UsersComponent } from './../admin/users/users.component';
import { PreguntasIncognitosService } from './preguntasincognitos/preguntasincognitos.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalModule, TabsModule } from 'ngx-bootstrap';
import { Routes, RouterModule } from '@angular/router';
import { LayoutModule } from '../../layouts/layout.module';
import { DefaultComponent } from '../default/default.component';
import { UtilsModule } from '../../../shared/utils/utils.module';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { AuthGuard } from "../../../auth/guards/auth.guard";
import { SharedModule } from '../../../shared/shared.module';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { SortablejsModule } from 'angular-sortablejs/dist';
import { PickListModule } from 'primeng/picklist';
import { NgxMaskModule } from 'ngx-mask'
import { MatDialogModule, MatTabsModule, MatExpansionModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { GruposInspectoresComponent } from './gruposinspectores/gruposinspectores.component';
import { CreateOrEditGruposInspectoresModalComponent } from './gruposinspectores/create-or-edit-gruposinspectores-modal.component';
import { NotificationComboComponent } from '../admin/shared/notification-combo.component';
import { GruposInspectoresService } from './gruposinspectores/gruposinspectores.service';
import { NotificationService } from '../../../shared/notification/notification.service';
import { YesNoAllComboComponent, YesNoAllBooleanComboComponent } from '../../../shared/components/yesnoall-combo.component';
import { ZonasComponent } from './zonas/zonas.component';
import { ZonasService } from './zonas/zonas.service';
import { ZonasComboComponent } from './shared/zonas-combo.component';
import { RangosHorariosService } from './rangoshorarios/rangoshorarios.service';
import { RangosHorariosComponent } from './rangoshorarios/rangoshorarios.component';
import { GrupoInspectoresRangosHorariosService } from './gruposinspectores/grupoinspectoresrangoshorarios..service';
import { GrupoInspectoresZonaService } from './gruposinspectores/grupoinspectoreszona.service';
import { NovedadesService } from './novedades/novedades.service';
import { NovedadesComboComponent } from './shared/novedades-combo.component';
import { CreateOrEditZonasModalComponent } from './zonas/create-or-edit-zonas-modal.component';
import { TareasComboComponent } from './shared/tareas-combo.component';
import { TiposTareaService } from './tipostarea/tipostarea.service';
import { DiagramasInspectoresComponent } from './diagramasinspectores/diagramasinspectores.component';
import { EstadosDiagramaInspectoresService } from './estadosdiagramainspectores/estadosdiagramainspectores.service';
import { DiagramasInspectoresService } from './diagramasinspectores/diagramasinspectores.service';
import { CreateOrEditDiagramasInspectoresModalComponent } from './diagramasinspectores/create-or-edit-diagramasinspectores-modal.component';
import { MonthComboComponent } from '../../../shared/components/month-combo.component';
import { GruposInspectoresComboComponent } from './shared/gruposinspectores-combo.component';
import { EstadosDiagramaInspectoresComboComponent } from './shared/estadosdiagramainspectores-combo.component';
import { DiagramacionComponent } from './diagramasinspectores/diagramacion.component';
import { TemplateJornadaTrabajadaComponent } from './diagramasinspectores/templates/template-jornada-trabajada.component';
import { TemplateFrancoComponent } from './diagramasinspectores/templates/template-franco.component';
import { TemplateNovedadComponent } from './diagramasinspectores/templates/template-novedad.component';
import { TemplateVacioComponent } from './diagramasinspectores/templates/template-vacio.component';
import { EditJornadaTrabajadaComponent } from './diagramasinspectores/templates/edit-jornada-trabajada.component';
import { EditFrancoComponent } from './diagramasinspectores/templates/edit-franco.component';
import { CreateOrEditRangosHorariosModalComponent } from './rangoshorarios/create-or-edit-rangoshorarios-modal.component';
import { RangosHorariosComboComponent } from './shared/rangoshorarios-combo.component';
import { PersTurnosService } from './turnos/persturnos.service';
import { PersTurnosComboComponent } from './shared/persturnos-combo.component';
import { EditDiagramacionComponent } from './diagramasinspectores/edit-diagramacion.component';
import { DiagramasInspectoresValidatorService } from './diagramasinspectores/diagramas-inspectores-validator.service';
import { AgregarFrancoComponent } from './diagramasinspectores/templates/agregar-franco.component';
import { PersTopesHorasExtrasService } from './diagramasinspectores/pers-topes-horas-extras.service';
import { AdminDefaultModule } from '../admin/admin-default.module';
import { CanDeactivateGuard } from '../../../app.component';
import { PagaCambiaComboComponent } from './shared/pagacambia-combo.component';
import { RespuestasIncognitosComponent } from './respuestasincognitos/respuestasIncognitos.component';
import { RespuestasIncognitosService } from './respuestasincognitos/respuestasIncognitos.service';
import { CreateOrEditRespuestasModalComponent } from './respuestasincognitos/create-or-edit-respuestas-modal.component';
import { PreguntasIncognitosComponent } from './preguntasincognitos/preguntasincognitos.component';
import { CreateOrEditPreguntasIncognitosModalComponent } from './preguntasincognitos/create-or-edit-preguntasincognitos-modal.component';
import { PreguntasIncognitosRespuestasService } from './preguntasincognitos/preguntasincognitosrespuestas.service';
import { RespuestasComboComponent } from './shared/respuestas-combo.component';
import { TareasComponent } from './tareas/tareas.component';
import { TareaService } from './tareas/tarea.service';
import { CreateOrEditTareaModalComponent } from './tareas/create-or-edit-tarea-modal.component';
import { TareasCamposConfigComboComponent } from './shared/tareas-campos-config-combo.component';
import { TareaCampoConfigService } from './tareas/tarea-campo-config.service';
import { TiposTareaComboComponent } from './shared/tipostarea-combo.component';
const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        canActivate: [AuthGuard],
        children: [


            {
                path: "usuarios/:mode",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: UsersComponent,
                data: {
                    permissions: {
                        only: 'Inspectores.User.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "gruposinspectores",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: GruposInspectoresComponent,
                data: {
                    permissions: {
                        only: 'Inspectores.GruposInspectores.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "zonas",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: ZonasComponent,
                data: {
                    permissions: {
                        only: 'Inspectores.Zonas.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "rangoshorarios",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: RangosHorariosComponent,
                data: {
                    permissions: {
                        only: 'Inspectores.RangosHorarios.Administracion',
                        redirectTo: '/401'
                    }
                }

            },

            {
                path: "diagramasinspectores",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: DiagramasInspectoresComponent,
                data: {
                    permissions: {
                        only: 'Inspectores.Diagramacion.Administracion',
                        redirectTo: '/401'
                    }
                }

            },

            {
                path: "diagramacionEdit/:DiagramaInspectorId",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: DiagramacionComponent,
                canDeactivate: [CanDeactivateGuard],
                data: {
                    permissions: {
                        //only: 'Inspectores.Diagramacion.Administracion',
                        //redirectTo: '/401'
                    }
                }

            },

            {
                path: "respuestasincognitos",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: RespuestasIncognitosComponent,
                data: {
                    permissions: {
                        only: 'Inspectores.RespuestasIncognitos.Administracion',
                        redirectTo: '/401'
                    }
                }
            },
            {
                path: "tareas",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TareasComponent,                
                data: {
                    permissions: {
                        //only: 'Inspectores.Diagramacion.Administracion',
                        //redirectTo: '/401'
                    }
                } 

            },

            {
                path: "preguntasincognitos",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: PreguntasIncognitosComponent,
                data: {
                    permissions: {
                        only: 'Inspectores.PreguntasIncognitos.Administracion',
                        redirectTo: '/401'
                    }
                }
            }
        ]
    }
];
@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        RouterModule.forChild(routes),
        ModalModule.forRoot(),
        TabsModule.forRoot(),
        LayoutModule,
        UtilsModule,
        SharedModule,
        AutoCompleteModule,
        SortablejsModule,
        PickListModule,
        NgxMaskModule.forRoot(),
        MatDialogModule,
        MatTabsModule,
        MatExpansionModule,
        //AdminDefaultModule

    ], exports: [
        RouterModule,
        PagaCambiaComboComponent,
    ], declarations: [
        GruposInspectoresComponent,
        CreateOrEditGruposInspectoresModalComponent,
        NotificationComboComponent,
        CreateOrEditZonasModalComponent,
        ZonasComponent,
        ZonasComboComponent,
        RangosHorariosComboComponent,
        RangosHorariosComponent,
        CreateOrEditRangosHorariosModalComponent,
        NovedadesComboComponent,
        TareasComboComponent,
        TiposTareaComboComponent,
        DiagramasInspectoresComponent,
        CreateOrEditDiagramasInspectoresModalComponent,
        EstadosDiagramaInspectoresComboComponent,
        DiagramacionComponent,
        TemplateJornadaTrabajadaComponent,
        TemplateFrancoComponent,
        TemplateNovedadComponent,
        TemplateVacioComponent,
        EditJornadaTrabajadaComponent,
        RespuestasIncognitosComponent,
        CreateOrEditRespuestasModalComponent,
        PreguntasIncognitosComponent,
        CreateOrEditPreguntasIncognitosModalComponent,
        RespuestasComboComponent,
        EditFrancoComponent,
        EditDiagramacionComponent,
        AgregarFrancoComponent,
        PagaCambiaComboComponent,
        TareasComponent,
        CreateOrEditTareaModalComponent,
        TareasCamposConfigComboComponent,
        YesNoAllBooleanComboComponent
    ],
    entryComponents: [
        CreateOrEditGruposInspectoresModalComponent,
        NotificationComboComponent,
        YesNoAllComboComponent,
        CreateOrEditZonasModalComponent,
        ZonasComboComponent,
        RangosHorariosComboComponent,
        CreateOrEditRangosHorariosModalComponent,
        NovedadesComboComponent,
        TareasComboComponent,
        TiposTareaComboComponent,
        DiagramasInspectoresComponent,
        CreateOrEditDiagramasInspectoresModalComponent,
        MonthComboComponent,
        EstadosDiagramaInspectoresComboComponent,
        EditJornadaTrabajadaComponent,
        EditFrancoComponent,
        EditDiagramacionComponent,
        AgregarFrancoComponent,
        YesNoAllBooleanComboComponent,
        RespuestasIncognitosComponent,
        CreateOrEditRespuestasModalComponent,
        PreguntasIncognitosComponent,
        CreateOrEditPreguntasIncognitosModalComponent,
        RespuestasComboComponent,
        CreateOrEditTareaModalComponent
    ],
    providers: [
        PersTopesHorasExtrasService,
        GruposInspectoresService,
        NotificationService,
        ZonasService,
        RangosHorariosService,
        GrupoInspectoresRangosHorariosService,
        GrupoInspectoresZonaService,
        NovedadesService,
        { provide: MatDialogRef, useValue: {} },
        { provide: MAT_DIALOG_DATA, useValue: [] },
        TiposTareaService,
        PersTurnosService,
        EstadosDiagramaInspectoresService,
        DiagramasInspectoresService,
        DiagramasInspectoresValidatorService, 
        RespuestasIncognitosService,
        PreguntasIncognitosRespuestasService,
        PreguntasIncognitosService,
        UserService,
        TareaService,
        TareaCampoConfigService
    ],
})
export class InspectoresModule {


}