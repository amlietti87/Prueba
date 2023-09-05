"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var app_menu_item_1 = require("./app-menu-item");
var app_menu_1 = require("./app-menu");
var permission_checker_service_1 = require("../../../shared/common/permission-checker.service");
var environment_1 = require("../../../../environments/environment");
var http_1 = require("@angular/common/http");
var rxjs_1 = require("rxjs");
var router_1 = require("@angular/router");
var storage_service_1 = require("../../../shared/common/services/storage.service");
var auth_service_1 = require("../../../auth/auth.service");
var AppNavigationService = /** @class */ (function () {
    function AppNavigationService(_router, _permissionService, http, authService, _storageService) {
        this._router = _router;
        this._permissionService = _permissionService;
        this.http = http;
        this.authService = authService;
        this._storageService = _storageService;
        this.identityUrl = '';
        this.subject = new rxjs_1.Subject();
        this.identityUrl = environment_1.environment.identityUrl + '/MenuItem';
        //_router.events.subscribe(event => {
        //    if (event instanceof NavigationStart) {
        //        this.subject.next();
        //    }
        //});
    }
    AppNavigationService.prototype.getMenu = function () {
        return this.subject.asObservable();
    };
    AppNavigationService.prototype.getLoadMenuAsyc = function () {
        var _this = this;
        if (!this.authService.isAuthenticated()) {
            return;
        }
        var m = this._storageService.retrieve("GetUnidadDeNegocio");
        if (!m) {
            var url = this.identityUrl;
            this.http.get(url).subscribe(function (r) {
                m = new app_menu_1.AppMenu('MainMenu', 'MainMenu', [
                    new app_menu_item_1.AppMenuItem('Dashboard', '', 'flaticon-line-graph', '/index'),
                    new app_menu_item_1.AppMenuItem('Administración General', '', 'flaticon-interface-8', '', [
                        new app_menu_item_1.AppMenuItem('Usuarios', 'Admin.Rol.Administracion', 'flaticon-users', '/admin/users'),
                        new app_menu_item_1.AppMenuItem('Roles', 'Admin.User.Administracion', 'flaticon-settings', '/admin/roles'),
                        new app_menu_item_1.AppMenuItem('Unidades de negocio', 'Admin.UnidadNegocio.Administracion', 'flaticon-settings', '/planificacion/unidadnegocio'),
                        new app_menu_item_1.AppMenuItem('Empresas', 'Admin.Empresa.Administracion', 'flaticon-settings', '/planificacion/empresa'),
                        new app_menu_item_1.AppMenuItem('Tipos de línea', 'Admin.TipoDeLinea.Administracion', 'flaticon-settings', '/planificacion/tipolinea'),
                        new app_menu_item_1.AppMenuItem('Sentidos de bandera', 'Admin.SentidoBandera.Administracion', 'flaticon-settings', '/planificacion/sentidobandera'),
                        new app_menu_item_1.AppMenuItem('Tipos de parada', 'Admin.TipoParada.Administracion', 'flaticon-placeholder-2', '/planificacion/tipoparada'),
                        new app_menu_item_1.AppMenuItem('Tipos de días', 'Admin.TipoDeDias.Administracion', 'flaticon-settings', '/planificacion/tipodia'),
                        new app_menu_item_1.AppMenuItem('Coordenadas', 'Admin.TipoDeDias.Administracion', 'flaticon-settings', '/planificacion/coordenadas'),
                        new app_menu_item_1.AppMenuItem('Paradas', 'Admin.Paradas.Administracion', 'flaticon-settings', '/planificacion/paradas'),
                        new app_menu_item_1.AppMenuItem('Sub-Galpones', 'Admin.TipoDeDias.Administracion', 'flaticon-settings', '/planificacion/subgalpon'),
                        new app_menu_item_1.AppMenuItem('Parámetros', 'Admin.Parametros.Administracion', 'flaticon-settings', '/admin/parametros'),
                        new app_menu_item_1.AppMenuItem('Tipos de documento', 'Admin.TipoDni.Administracion', 'flaticon-settings', '/admin/tipodni'),
                        new app_menu_item_1.AppMenuItem('Localidades', 'Admin.Localidad.Administracion', 'flaticon-settings', '/admin/localidades'),
                        new app_menu_item_1.AppMenuItem('Sectores Tarifarios', 'Admin.SectoresTarifarios.Administracion', 'flaticon-settings', '/admin/sectores-tarifarios'),
                        new app_menu_item_1.AppMenuItem('Talleres IVU', 'Admin.TalleresIVU.Administracion', 'flaticon-settings', '/admin/talleresivu'),
                    ]),
                    new app_menu_item_1.AppMenuItem("Siniestros", '', 'fa fa-car', '', [
                        new app_menu_item_1.AppMenuItem('Siniestros', 'Siniestro.Siniestro.Visualizar', 'fa fa-car', '/siniestros/siniestro'),
                        new app_menu_item_1.AppMenuItem("Croquis", '', 'flaticon-settings', '', [
                            new app_menu_item_1.AppMenuItem('Tipos de Elementos', 'Siniestro.TipoElemento.Administracion', 'flaticon-settings', '/siniestros/tipoelemento'),
                            new app_menu_item_1.AppMenuItem('Elementos', 'Siniestro.Elemento.Administracion', 'flaticon-settings', '/siniestros/elemento'),
                        ]),
                        new app_menu_item_1.AppMenuItem("Configuración", '', 'flaticon-settings', '', [
                            new app_menu_item_1.AppMenuItem('Consecuencias', 'Siniestro.Consecuencia.Administracion', 'flaticon-settings', '/siniestros/consecuencias'),
                            new app_menu_item_1.AppMenuItem('Factores Intervinientes', 'Siniestro.FactoresIntervinientes.Administracion', 'flaticon-settings', '/siniestros/factores-intervinientes'),
                            new app_menu_item_1.AppMenuItem('Cía. de Seguros', 'Siniestro.Seguro.Administracion', 'flaticon-settings', '/siniestros/seguros'),
                            new app_menu_item_1.AppMenuItem('Abogados', 'Siniestro.Abogado.Administracion', 'flaticon-settings', '/siniestros/abogados'),
                            new app_menu_item_1.AppMenuItem('Juzgados', 'Siniestro.Juzgado.Administracion', 'flaticon-settings', '/siniestros/juzgados'),
                            new app_menu_item_1.AppMenuItem('Tipos de Lesionados', 'Siniestro.TipoLesionado.Administracion', 'flaticon-settings', '/siniestros/tipolesionado'),
                            new app_menu_item_1.AppMenuItem('Tipos de Muebles/Inmuebles', 'Siniestro.TipoMuebleInmueble.Administracion', 'flaticon-settings', '/siniestros/tipoMuebleInmueble'),
                            new app_menu_item_1.AppMenuItem('Tipos de Involucrados', 'Siniestro.TipoInvolucrado.Administracion', 'flaticon-settings', '/siniestros/tipoInvolucrado'),
                            new app_menu_item_1.AppMenuItem('Practicantes', 'Siniestro.Practicante.Administracion', 'flaticon-settings', '/siniestros/practicantes'),
                            new app_menu_item_1.AppMenuItem('Causas', 'Siniestro.Causa.Administracion', 'flaticon-settings', '/siniestros/causas'),
                            new app_menu_item_1.AppMenuItem('Estados', 'Siniestro.Estado.Administracion', 'flaticon-settings', '/siniestros/estados'),
                            new app_menu_item_1.AppMenuItem('Normas/Conductas incumplidas', 'Siniestro.Norma.Administracion', 'flaticon-settings', '/siniestros/normas'),
                            new app_menu_item_1.AppMenuItem('Tipos de Daño', 'Siniestro.TipoDanio.Administracion', 'flaticon-settings', '/siniestros/tipodedanio'),
                            new app_menu_item_1.AppMenuItem('Sanción Sugerida', 'Siniestro.SancionSugerida.Administracion', 'flaticon-settings', '/siniestros/sancionsugerida'),
                        ]),
                    ]),
                ]);
                r.DataObject.items.forEach(function (item) {
                    m.items.push(item);
                });
                //new AppMenuItem('DemoUno', 'Pages.DemoUno', 'flaticon-settings', '/demo/demouno')
                _this._storageService.store("GetUnidadDeNegocio", m);
                _this.subject.next(m);
            });
        }
        else {
            this.subject.next(m);
        }
    };
    AppNavigationService.prototype.clrear = function () {
        this._storageService.removeItem("GetUnidadDeNegocio");
    };
    AppNavigationService.prototype.checkChildMenuItemPermission = function (menuItem) {
        for (var i = 0; i < menuItem.items.length; i++) {
            var subMenuItem = menuItem.items[i];
            if (subMenuItem.permissionName && this._permissionService.isGranted(subMenuItem.permissionName)) {
                return true;
            }
            if (subMenuItem.items && subMenuItem.items.length) {
                return this.checkChildMenuItemPermission(subMenuItem);
            }
            else if (!subMenuItem.permissionName) {
                return true;
            }
        }
        return false;
    };
    AppNavigationService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [router_1.Router,
            permission_checker_service_1.PermissionCheckerService,
            http_1.HttpClient,
            auth_service_1.AuthService,
            storage_service_1.LocalStorageService])
    ], AppNavigationService);
    return AppNavigationService;
}());
exports.AppNavigationService = AppNavigationService;
//# sourceMappingURL=app-navigation.service.js.map