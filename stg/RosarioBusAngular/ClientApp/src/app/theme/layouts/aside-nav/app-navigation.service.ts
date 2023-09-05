import { Injectable } from '@angular/core';
import { AppMenuItem } from './app-menu-item';
import { AppMenu } from './app-menu';
import { PermissionCheckerService } from '../../../shared/common/permission-checker.service';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs';
import { NavigationStart, Router } from '@angular/router';
import { ResponseModel } from '../../../shared/model/base.model';
import { LocalStorageService } from '../../../shared/common/services/storage.service';
import { AuthService } from '../../../auth/auth.service';

@Injectable()
export class AppNavigationService {

    private identityUrl: string = '';
    constructor(private _router: Router,
        private _permissionService: PermissionCheckerService,
        protected http: HttpClient,
        private authService: AuthService,
        protected _storageService: LocalStorageService) {
        this.identityUrl = environment.identityUrl + '/MenuItem';

        //_router.events.subscribe(event => {
        //    if (event instanceof NavigationStart) {
        //        this.subject.next();
        //    }
        //});
    }

    private subject = new Subject<AppMenu>();

    getMenu(): Observable<AppMenu> {
        return this.subject.asObservable();
    }

    getLoadMenuAsyc() {
        if (!this.authService.isAuthenticated()) {
            return;
        }

        var m = this._storageService.retrieve("GetUnidadDeNegocio") as AppMenu;

        if (!m) {

            let url = this.identityUrl;
            this.http.get<ResponseModel<AppMenu>>(url).subscribe(r => {
                m = new AppMenu('MainMenu', 'MainMenu', [
                    new AppMenuItem('Dashboard', '', 'flaticon-line-graph', '/index'),
                    new AppMenuItem('Administración General', '', 'flaticon-interface-8', '', [
                        new AppMenuItem('Usuarios', 'Admin.Rol.Administracion', 'flaticon-users', '/admin/users'),
                        new AppMenuItem('Roles', 'Admin.User.Administracion', 'flaticon-settings', '/admin/roles'),
                        new AppMenuItem('Unidades de negocio', 'Admin.UnidadNegocio.Administracion', 'flaticon-settings', '/planificacion/unidadnegocio'),
                        new AppMenuItem('Empresas', 'Admin.Empresa.Administracion', 'flaticon-settings', '/planificacion/empresa'),
                        new AppMenuItem('Tipos de línea', 'Admin.TipoDeLinea.Administracion', 'flaticon-settings', '/planificacion/tipolinea'),
                        new AppMenuItem('Sentidos de bandera', 'Admin.SentidoBandera.Administracion', 'flaticon-settings', '/planificacion/sentidobandera'),
                        new AppMenuItem('Tipos de parada', 'Admin.TipoParada.Administracion', 'flaticon-placeholder-2', '/planificacion/tipoparada'),
                        new AppMenuItem('Tipos de días', 'Admin.TipoDeDias.Administracion', 'flaticon-settings', '/planificacion/tipodia'),
                        new AppMenuItem('Coordenadas', 'Admin.Coordenadas.Administracion', 'flaticon-settings', '/planificacion/coordenadas'),
                        new AppMenuItem('Paradas', 'Admin.Paradas.Administracion', 'flaticon-settings', '/planificacion/paradas'),

                        new AppMenuItem('Sub-Galpones', 'Admin.SubGalpones.Administracion', 'flaticon-settings', '/planificacion/subgalpon'),
                        new AppMenuItem('Parámetros', 'Admin.Parametros.Administracion', 'flaticon-settings', '/admin/parametros'),
                        new AppMenuItem('Tipos de documento', 'Admin.TipoDni.Administracion', 'flaticon-settings', '/admin/tipodni'),
                        new AppMenuItem('Localidades', 'Admin.Localidad.Administracion', 'flaticon-settings', '/admin/localidades'),
                        new AppMenuItem('Sectores Tarifarios', 'Admin.SectoresTarifarios.Administracion', 'flaticon-settings', '/admin/sectores-tarifarios'),
                        new AppMenuItem('Talleres IVU', 'Admin.TalleresIVU.Administracion', 'flaticon-settings', '/admin/talleresivu'),
                    ]),
                    new AppMenuItem("Siniestros", '', 'fa fa-car', '', [
                        new AppMenuItem('Siniestros', 'Siniestro.Siniestro.Visualizar', 'fa fa-car', '/siniestros/siniestro'),
                        new AppMenuItem("Configuración general", '', 'flaticon-settings', '', [
                            new AppMenuItem('Consecuencias', 'Siniestro.Consecuencia.Administracion', 'flaticon-settings', '/siniestros/consecuencias'),
                            new AppMenuItem('Factores Intervinientes', 'Siniestro.FactoresIntervinientes.Administracion', 'flaticon-settings', '/siniestros/factores-intervinientes'),
                            new AppMenuItem('Cía. de Seguros', 'Siniestro.Seguro.Administracion', 'flaticon-settings', '/siniestros/seguros'),
                            new AppMenuItem('Practicantes', 'Siniestro.Practicante.Administracion', 'flaticon-settings', '/siniestros/practicantes'),
                            new AppMenuItem('Causas', 'Siniestro.Causa.Administracion', 'flaticon-settings', '/siniestros/causas'),
                            new AppMenuItem('Normas/Conductas incumplidas', 'Siniestro.Norma.Administracion', 'flaticon-settings', '/siniestros/normas'),
                            new AppMenuItem('Tipos de Daño', 'Siniestro.TipoDanio.Administracion', 'flaticon-settings', '/siniestros/tipodedanio'),
                            new AppMenuItem('Tipos de Colisión', 'Siniestro.TipoColision.Administracion', 'flaticon-settings', '/siniestros/tipocolision'),
                            new AppMenuItem('Sanción Sugerida', 'Siniestro.SancionSugerida.Administracion', 'flaticon-settings', '/siniestros/sancionsugerida'),
                        ]),
                        new AppMenuItem("Configuración involucrados", '', 'flaticon-settings', '', [
                            new AppMenuItem('Tipos de Lesionados', 'Siniestro.TipoLesionado.Administracion', 'flaticon-settings', '/siniestros/tipolesionado'),
                            new AppMenuItem('Tipos de Muebles/Inmuebles', 'Siniestro.TipoMuebleInmueble.Administracion', 'flaticon-settings', '/siniestros/tipoMuebleInmueble'),
                            new AppMenuItem('Tipos de Involucrados', 'Siniestro.TipoInvolucrado.Administracion', 'flaticon-settings', '/siniestros/tipoInvolucrado'),
                        ]),
                        new AppMenuItem("Croquis", '', 'flaticon-settings', '', [
                            new AppMenuItem('Tipos de Elementos', 'Siniestro.TipoElemento.Administracion', 'flaticon-settings', '/siniestros/tipoelemento'),
                            new AppMenuItem('Elementos', 'Siniestro.Elemento.Administracion', 'flaticon-settings', '/siniestros/elemento'),
                        ]),

                    ]),
                    new AppMenuItem("ART", '', 'fa fa-user-secret', '', [
                        new AppMenuItem('Denuncias', 'ART.Denuncia.Visualizar', 'fa fa-user-secret', '/art/denuncias'),
                        new AppMenuItem("Configuración", '', 'flaticon-settings', '', [
                            new AppMenuItem('Contingencias', 'ART.Contingencia.Administracion', 'flaticon-settings', '/art/contingencias'),
                            new AppMenuItem('Estados', 'ART.EstadoDenuncia.Administracion', 'flaticon-settings', '/art/estados'),
                            new AppMenuItem('Motivos Audiencias', 'ART.MotivoAudiencia.Administracion', 'flaticon-settings', '/art/motivosaudiencias'),
                            new AppMenuItem('Motivos Notificaciones', 'ART.MotivoNotificacion.Administracion', 'flaticon-settings', '/art/motivosnotificaciones'),
                            new AppMenuItem('Patologías', 'ART.Patologia.Administracion', 'flaticon-settings', '/art/patologias'),
                            new AppMenuItem('Prestadores Medicos', 'ART.PrestadorMedico.Administracion', 'flaticon-settings', '/art/prestadores'),
                            new AppMenuItem('Tratamientos', 'ART.Tratamiento.Administracion', 'flaticon-settings', '/art/tratamientos'),
                        ]),

                    ]),
                    new AppMenuItem("Reclamos", '', 'fa fa-gavel', '', [
                        new AppMenuItem('Reclamos', 'Reclamo.Reclamo.Visualizar', 'fa fa-gavel', '/reclamos/reclamos'),
                        new AppMenuItem("Configuración", '', 'flaticon-settings', '', [
                            new AppMenuItem('Tipos de Reclamo', 'Reclamo.TipoReclamo.Administracion', 'flaticon-settings', '/reclamos/tiposreclamo'),
                            new AppMenuItem('Causas de Reclamo', 'Reclamo.CausaReclamo.Administracion', 'flaticon-settings', '/reclamos/causasreclamo'),
                            new AppMenuItem('Tipos de Acuerdo', 'Reclamo.TipoAcuerdo.Administracion', 'flaticon-settings', '/reclamos/tiposacuerdo'),
                            new AppMenuItem('Rubros Salariales', 'Reclamo.RubroSalarial.Administracion', 'flaticon-settings', '/reclamos/rubrossalariales'),
                            new AppMenuItem('Abogados', 'Siniestro.Abogado.Administracion', 'flaticon-settings', '/siniestros/abogados'),
                            new AppMenuItem('Juzgados', 'Siniestro.Juzgado.Administracion', 'flaticon-settings', '/siniestros/juzgados'),
                            new AppMenuItem('Estados', 'Siniestro.Estado.Administracion', 'flaticon-settings', '/siniestros/estados'),
                        ]),

                    ]),
                    new AppMenuItem("Inspectores", '', 'flaticon-search', '', [
                        new AppMenuItem('Diagramación', 'Inspectores.Diagramacion.Administracion', 'flaticon-map', '/inspectores/diagramasinspectores'),
                        new AppMenuItem("Configuración", '', 'flaticon-settings', '', [
                            new AppMenuItem('Grupos de Inspectores', 'Inspectores.GruposInspectores.Administracion', 'flaticon-settings', '/inspectores/gruposinspectores'),
                            new AppMenuItem('Zonas', 'Inspectores.Zonas.Administracion', 'flaticon-settings', '/inspectores/zonas'),
                            new AppMenuItem('Rangos Horarios', 'Inspectores.RangosHorarios.Administracion', 'flaticon-settings', '/inspectores/rangoshorarios'),
                            new AppMenuItem('Usuarios', 'Inspectores.User.Administracion', 'flaticon-settings', '/inspectores/usuarios/inspector'),
                            new AppMenuItem('Respuestas Incógnitos', 'Inspectores.RespuestasIncognitos.Administracion', 'flaticon-settings', '/inspectores/respuestasincognitos'),
                            new AppMenuItem('Preguntas Incógnitos', 'Inspectores.PreguntasIncognitos.Administracion', 'flaticon-settings', '/inspectores/preguntasincognitos'),
                            new AppMenuItem('Tareas', 'Inspectores.Tareas.Visualizar', 'flaticon-settings', '/inspectores/tareas'),
                        ]),
                    ]),
                    new AppMenuItem("Firma Digital", '', 'fa fa-check-square-o', '', [
                        new AppMenuItem('Archivos', '', 'flaticon-settings', '', [
                            new AppMenuItem('Importar Documentos', 'FirmaDigital.Archivos.ImportarDocumentos', 'flaticon-settings', '/firmadigital/importardocumentos'),
                            new AppMenuItem('Revisar Errores', 'FirmaDigital.Archivos.RevisarErrores', 'flaticon-settings', '/firmadigital/revisarerrores'),
                        ]),
                        new AppMenuItem('BD-Empleador', 'FirmaDigital.BD-Empleador.Visualizar', 'fa fa-user ', '/firmadigital/bdempleador'),
                        new AppMenuItem('BD-Empleado', 'FirmaDigital.BD-Empleado.Visualizar', 'fa fa-users ', '/firmadigital/bdempleado'),
                        new AppMenuItem('Administrar Certificados', 'FirmaDigital.AdministrarCertificados.Visualizar', 'fa fa-users ', '/firmadigital/admincertificados'),
                        new AppMenuItem('Mi Certificado', 'FirmaDigital.MiCertificado.Visualizar', 'fa fa-users ', '/firmadigital/micertificado'),
                        new AppMenuItem('Configuración', '', 'flaticon-settings', '', [
                            new AppMenuItem('Tipo de Documentos', 'FirmaDigital.TipoDocumento.Administracion', 'flaticon-settings', '/firmadigital/fdtipodocumento'),
                            new AppMenuItem('Turnos', 'Inspectores.Turnos.Administracion', 'flaticon-settings', '/inspectores/turnos'),
                        ]),
                    ]),
                ]);

                r.DataObject.items.forEach(item => {
                    m.items.push(item);
                });

                //new AppMenuItem('DemoUno', 'Pages.DemoUno', 'flaticon-settings', '/demo/demouno')


                this._storageService.store("GetUnidadDeNegocio", m);

                this.subject.next(m);
            });
        }
        else {
            this.subject.next(m);
        }
    }

    clrear() {
        this._storageService.removeItem("GetUnidadDeNegocio");
    }


    checkChildMenuItemPermission(menuItem): boolean {

        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName && this._permissionService.isGranted(subMenuItem.permissionName)) {
                return true;
            }

            if (subMenuItem.items && subMenuItem.items.length) {
                return this.checkChildMenuItemPermission(subMenuItem);
            } else if (!subMenuItem.permissionName) {
                return true;
            }
        }

        return false;
    }
}
