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
var ngx_bootstrap_1 = require("ngx-bootstrap");
var app_component_base_1 = require("../../../../shared/common/app-component-base");
var usuariolineas_model_1 = require("../model/usuariolineas.model");
var linea_service_1 = require("../../planificacion/linea/linea.service");
var linea_model_1 = require("../../planificacion/model/linea.model");
var user_service_1 = require("../../../../services/user.service");
var EditUserLineasModalComponent = /** @class */ (function (_super) {
    __extends(EditUserLineasModalComponent, _super);
    function EditUserLineasModalComponent(injector, _userService, _lineaService) {
        var _this = _super.call(this, injector) || this;
        _this._userService = _userService;
        _this._lineaService = _lineaService;
        _this.model = new usuariolineas_model_1.UpdateLineasForEdit();
        _this.saving = false;
        _this.SucursalId = 0;
        _this.allowModificarUsr = true;
        _this.modoInspector = false;
        return _this;
    }
    EditUserLineasModalComponent.prototype.show = function (_user) {
        var _this = this;
        this.user = _user;
        this.userId = this.user.Id;
        this.userName = this.user.NomUsuario;
        this.SucursalId = this.user.SucursalId;
        this._userService.GetUserLineasForEdit(this.userId).subscribe(function (result) {
            // this.permissionTree.editData = result.DataObject;
            _this.model = result.DataObject;
            if (_this.mode == 'inspector') {
                _this.modoInspector = true;
                _this.allowModificarUsr = _this.permission.isGranted("Inspectores.User.Modificar");
            }
            console.log("MODE::::::::", _this.user);
            _this.modal.show();
        });
    };
    EditUserLineasModalComponent.prototype.ngOnInit = function () {
    };
    EditUserLineasModalComponent.prototype.save = function () {
        var _this = this;
        var input = new usuariolineas_model_1.UpdateLineasForEdit();
        this.saving = true;
        this._userService.SetUserLineasForEdit(this.model)
            .finally(function () { _this.saving = false; })
            .subscribe(function () {
            _this.notify.info("Guardado exitosamente");
            _this.close();
        });
    };
    EditUserLineasModalComponent.prototype.addLineaSucursal = function () {
        var _this = this;
        var self = this;
        if (this.SucursalId) {
            var filtro = new linea_model_1.LineaFilter();
            filtro.SucursalId = this.SucursalId;
            this._lineaService.FindItemsAsync(filtro).subscribe(function (result) {
                var msg = [];
                result.DataObject.forEach(function (item) {
                    if (self.model.Lineas.some(function (x) { return x.Id == item.Id; })) {
                        msg.push(item.Description);
                    }
                    self.addAndValidateLinea(item, false);
                });
                if (msg.length > 0) {
                    _this.notificationService.warn(msg.join(', '), "Algunas Lineas ya fueron agregadas");
                }
            });
        }
    };
    EditUserLineasModalComponent.prototype.addLinea = function (item) {
        this.addAndValidateLinea(item, true);
    };
    EditUserLineasModalComponent.prototype.addAndValidateLinea = function (item, showwarn) {
        if (item.Id) {
            if (!this.model.Lineas.some(function (x) { return x.Id == item.Id; })) {
                item.IsSelected = true;
                this.model.Lineas.push(item);
            }
            else {
                var c = this.model.Lineas.find(function (x) { return x.Id == item.Id; });
                if (showwarn) {
                    this.notificationService.warn("La linea ya fue agregada", c.Description);
                }
                c.animate = true;
                setTimeout(function () {
                    c.animate = false;
                }, 1000);
            }
            this.selectItem = null;
        }
    };
    EditUserLineasModalComponent.prototype.removeLinea = function (item) {
        var index = this.model.Lineas.findIndex(function (x) { return x.Id == item.Id; });
        if (index >= 0) {
            this.model.Lineas.splice(index, 1);
        }
    };
    EditUserLineasModalComponent.prototype.close = function () {
        this.modal.hide();
    };
    __decorate([
        core_1.ViewChild('editModal'),
        __metadata("design:type", ngx_bootstrap_1.ModalDirective)
    ], EditUserLineasModalComponent.prototype, "modal", void 0);
    EditUserLineasModalComponent = __decorate([
        core_1.Component({
            selector: 'editUserLineasModal',
            templateUrl: './edit-user-lineas-modal.component.html'
        }),
        __metadata("design:paramtypes", [core_1.Injector,
            user_service_1.UserService,
            linea_service_1.LineaService])
    ], EditUserLineasModalComponent);
    return EditUserLineasModalComponent;
}(app_component_base_1.AppComponentBase));
exports.EditUserLineasModalComponent = EditUserLineasModalComponent;
//# sourceMappingURL=edit-user-lineas-modal.component.js.map