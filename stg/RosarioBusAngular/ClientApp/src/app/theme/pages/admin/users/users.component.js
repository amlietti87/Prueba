"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var base_model_1 = require("./../../../../shared/model/base.model");
var core_1 = require("@angular/core");
var crud_component_1 = require("../../../../shared/manager/crud.component");
var user_service_1 = require("./user.service");
var create_or_edit_user_modal_component_1 = require("./create-or-edit-user-modal.component");
var edit_user_permissions_modal_component_1 = require("./edit-user-permissions-modal.component");
var user_model_1 = require("../model/user.model");
var edit_user_lineas_modal_component_1 = require("./edit-user-lineas-modal.component");
var router_1 = require("@angular/router");
var UsersComponent = /** @class */ (function (_super) {
    __extends(UsersComponent, _super);
    function UsersComponent(injector, _userService, _activatedRoute) {
        var _this = _super.call(this, _userService, create_or_edit_user_modal_component_1.CreateOrEditUserModalComponent, injector) || this;
        _this._userService = _userService;
        _this._activatedRoute = _activatedRoute;
        _this.cambiarModo = false;
        _this.allowEditPermisos = false;
        _this.title = "Usuarios";
        _this.moduleName = "Administración General";
        _this.icon = "flaticon-users";
        _this.showbreadcum = false;
        return _this;
    }
    UsersComponent.prototype.GetEditComponentType = function () {
        return create_or_edit_user_modal_component_1.CreateOrEditUserModalComponent;
    };
    UsersComponent.prototype.SetAllowPermission = function () {
        _super.prototype.SetAllowPermission.call(this);
        this.allowEditPermisos = this.permission.isGranted('Admin.User.Permisos');
    };
    UsersComponent.prototype.getNewfilter = function () {
        return new user_model_1.UserFilter();
    };
    UsersComponent.prototype.ngOnInit = function () {
        var _this = this;
        _super.prototype.ngOnInit.call(this);
        this.sub = this._activatedRoute.params.subscribe(function (params) {
            _this.paramsSubscribe(params);
        });
    };
    UsersComponent.prototype.paramsSubscribe = function (params) {
        this.breadcrumbsService.defaultBreadcrumbs(this.title);
        if (params.mode) {
            this.mode = params.mode;
            if (this.mode == 'inspector') {
                this.cambiarModo = true;
            }
        }
    };
    UsersComponent.prototype.onEditModoInspector = function (user) {
        if (user.EsInspector) {
            this.GetEditComponent().mode = this.mode;
            // super.onEdit(user);
            this.onEdit(user);
        }
        else {
            this.notify.error('Usuario no Inspector', '');
        }
    };
    UsersComponent.prototype.getNewItem = function (item) {
        return new user_model_1.UserDto(item);
    };
    UsersComponent.prototype.getDescription = function (item) {
        return item.NomUsuario;
    };
    UsersComponent.prototype.onEditUserPermissions = function (user) {
        this.editUserPermissionsModal.show(user.Id, user.NomUsuario);
    };
    UsersComponent.prototype.onEditUserLineasModoInsp = function (user) {
        if (user.EsInspector) {
            this.editUserlineasModal.show(user);
            (this.editUserlineasModal).mode = this.mode;
        }
        else {
            this.notify.error('Usuario no Inspector', '');
        }
    };
    UsersComponent.prototype.onEditUserLineas = function (user) {
        this.editUserlineasModal.show(user);
    };
    UsersComponent.prototype.onEdit = function (row) {
        this.onEditID(row.Id);
    };
    UsersComponent.prototype.onEditID = function (id) {
        var _this = this;
        this.active = false;
        if (this.loadInMaterialPopup) {
            this.service.getById(id).subscribe(function (e) { return _this.Opendialog(e.DataObject, base_model_1.ViewMode.Modify); });
        }
        else {
            this.GetEditComponent().show(id);
        }
    };
    __decorate([
        core_1.ViewChild('editUserPermissionsModal'),
        __metadata("design:type", edit_user_permissions_modal_component_1.EditUserPermissionsModalComponent)
    ], UsersComponent.prototype, "editUserPermissionsModal", void 0);
    __decorate([
        core_1.ViewChild('editUserLineasModal'),
        __metadata("design:type", edit_user_lineas_modal_component_1.EditUserLineasModalComponent)
    ], UsersComponent.prototype, "editUserlineasModal", void 0);
    UsersComponent = __decorate([
        core_1.Component({
            templateUrl: "./users.component.html",
            encapsulation: core_1.ViewEncapsulation.None,
        }),
        __metadata("design:paramtypes", [core_1.Injector, user_service_1.UserService, router_1.ActivatedRoute])
    ], UsersComponent);
    return UsersComponent;
}(crud_component_1.BaseCrudComponent));
exports.UsersComponent = UsersComponent;
//# sourceMappingURL=users.component.js.map