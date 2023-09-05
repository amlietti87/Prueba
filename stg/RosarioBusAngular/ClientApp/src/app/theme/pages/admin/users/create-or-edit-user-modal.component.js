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
var core_1 = require("@angular/core");
var base_model_1 = require("../../../../shared/model/base.model");
var detail_component_1 = require("../../../../shared/manager/detail.component");
var user_service_1 = require("../../../../services/user.service");
var _ = require("lodash");
var user_model_1 = require("../model/user.model");
var empleado_autocomplete_component_1 = require("../../siniestros/shared/empleado-autocomplete.component");
var empleados_service_1 = require("../../siniestros/empleados/empleados.service");
var CreateOrEditUserModalComponent = /** @class */ (function (_super) {
    __extends(CreateOrEditUserModalComponent, _super);
    function CreateOrEditUserModalComponent(injector, service, empleadoService) {
        var _this = _super.call(this, service, injector) || this;
        _this.service = service;
        _this.empleadoService = empleadoService;
        _this.allowInspector = false;
        _this.allowEmpleado = false;
        _this.allowTurno = false;
        _this.allowBlanquearPassword = false;
        _this.modoInspector = false;
        _this.allowModificarUsr = true;
        _this.detail = new user_model_1.UserDto();
        _this.active = true;
        _this.profilePicture = "";
        _this.allowBlanquearPassword = _this.permission.isGranted("Admin.User.BlanqueoPassword");
        return _this;
    }
    CreateOrEditUserModalComponent.prototype.ngAfterViewChecked = function () {
        //Temporary fix for: https://github.com/valor-software/ngx-bootstrap/issues/1508
        $('tabset ul.nav').addClass('m-tabs-line');
        $('tabset ul.nav li a.nav-link').addClass('m-tabs__link');
    };
    CreateOrEditUserModalComponent.prototype.showNew = function (item) {
        var _this = this;
        this.viewMode = base_model_1.ViewMode.Add;
        this.service.getById(0).subscribe(function (result) {
            _this.viewMode = base_model_1.ViewMode.Add;
            _this.showDto(result.DataObject);
        });
    };
    CreateOrEditUserModalComponent.prototype.completedataBeforeShow = function (item) {
        if (this.mode == 'inspector') {
            this.modoInspector = true;
            this.allowModificarUsr = this.permission.isGranted("Inspectores.User.Modificar");
        }
        if (item.EmpleadoId != null) {
            var itemdto = new base_model_1.ItemDto();
            itemdto.Id = item.EmpleadoId;
            itemdto.Description = item.DescEmpleado;
            this.selecEmpleado = itemdto;
        }
        if (this.viewMode == base_model_1.ViewMode.Add) {
            this.roles = item.UserRoles || [];
            item.PermiteLoginManual = true;
        }
        else {
            if (item.UserRoles) {
                this.roles = item.UserRoles;
            }
        }
        this.grupoId = item.GruposInspectoresId;
        this.turnoId = item.TurnoId;
    };
    CreateOrEditUserModalComponent.prototype.completedataBeforeSave = function (item) {
        this.detail.UserRoles = this.roles;
        if (this.selecEmpleado != null) {
            this.detail.EmpleadoId = this.selecEmpleado.Id;
            this.detail.DescEmpleado = this.selecEmpleado.Description.toString();
        }
        else {
            this.detail.EmpleadoId = undefined;
            this.detail.DescEmpleado = undefined;
        }
        if (this.detail.GruposInspectoresId == 0) {
            this.detail.GruposInspectoresId = undefined;
        }
    };
    CreateOrEditUserModalComponent.prototype.getProfilePicture = function (profilePictureId) {
        if (!profilePictureId) {
            this.profilePicture = '/assets/app/media/img/users/default-profile-picture.jpg';
        }
    };
    CreateOrEditUserModalComponent.prototype.save = function (form) {
        debugger;
        var rolInspector = this.detail.UserRoles.find(function (r) { return r.RoleName == "Inspector"; });
        this.allowInspector = rolInspector.IsAssigned && (this.detail.GruposInspectoresId == null || this.detail.GruposInspectoresId.toString() == "");
        this.allowEmpleado = rolInspector.IsAssigned && this.selecEmpleado == null;
        this.allowTurno = rolInspector.IsAssigned && (this.detail.TurnoId == null || this.detail.TurnoId.toString() == "");
        if (this.allowInspector || this.allowEmpleado || this.allowTurno) {
            return;
        }
        if (this.detailForm && this.detailForm.form.invalid) {
            return;
        }
        this.saving = true;
        this.completedataBeforeSave(this.detail);
        if (!this.validateSave()) {
            this.saving = false;
            return;
        }
        this.SaveDetail();
    };
    CreateOrEditUserModalComponent.prototype.OnChangeRole = function (event, role) {
        if (role.RoleName == 'Inspector' && !event) {
            this.allowInspector = false;
            this.allowEmpleado = false;
            this.allowTurno = false;
        }
    };
    CreateOrEditUserModalComponent.prototype.OnChangeEmpleado = function (event, selecEmpl) {
        if (this.selecEmpleado == null) {
            this.allowEmpleado = true;
        }
    };
    CreateOrEditUserModalComponent.prototype.getAssignedRoleCount = function () {
        return _.filter(this.roles, { IsAssigned: true }).length;
    };
    CreateOrEditUserModalComponent.prototype.resetPassword = function () {
        var _this = this;
        this.saving = true;
        this.service.resetPassword(this.detail.Id)
            .finally(function () { _this.saving = false; })
            .subscribe(function (resp) {
            if (resp.Status == base_model_1.StatusResponse.Ok) {
                _this.message.success("Se ha enviado un mail al Usuario.", "Blanqueo de contraseña");
            }
            else {
                _this.message.error("Ha ocurrido un error.", "Blanqueo de contraseña");
            }
        });
    };
    CreateOrEditUserModalComponent.prototype.findUser = function () {
        var _this = this;
        this.service.findUser(this.detail.LogonName)
            .finally(function () { })
            .subscribe(function (resp) {
            if (resp.Status == base_model_1.StatusResponse.Ok) {
                _this.detail.DisplayName = resp.DataObject.DisplayName;
                _this.detail.Mail = resp.DataObject.Mail;
                _this.detail.UserPrincipalName = resp.DataObject.UserPrincipalName;
                _this.detail.NomUsuario = resp.DataObject.NomUsuario;
                _this.detail.LogonName = resp.DataObject.LogonName;
            }
            else {
            }
        });
    };
    CreateOrEditUserModalComponent.prototype.OnChangeGrupos = function (event) {
        //Al cambiar grupo, limpia combo turnos.
        if (this.grupoId == parseInt(event)) {
            this.detail.TurnoId = this.turnoId;
        }
        else {
            this.detail.TurnoId = null;
        }
    };
    CreateOrEditUserModalComponent.prototype.close = function () {
        this.detailForm.controls['EmpleadoId'].reset();
        this.allowEmpleado = false;
        this.allowInspector = false;
        this.allowTurno = false;
        _super.prototype.close.call(this);
        this.modal.hide();
    };
    __decorate([
        core_1.ViewChild('EmpleadoId'),
        __metadata("design:type", empleado_autocomplete_component_1.EmpleadosAutoCompleteComponent)
    ], CreateOrEditUserModalComponent.prototype, "EmpleadoId", void 0);
    CreateOrEditUserModalComponent = __decorate([
        core_1.Component({
            selector: 'createOrEditUserModal',
            templateUrl: './create-or-edit-user-modal.component.html',
        }),
        __metadata("design:paramtypes", [core_1.Injector,
            user_service_1.UserService,
            empleados_service_1.EmpleadosService])
    ], CreateOrEditUserModalComponent);
    return CreateOrEditUserModalComponent;
}(detail_component_1.DetailModalComponent));
exports.CreateOrEditUserModalComponent = CreateOrEditUserModalComponent;
//# sourceMappingURL=create-or-edit-user-modal.component.js.map