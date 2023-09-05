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
var permission_tree_component_1 = require("../shared/permission-tree.component");
var user_service_1 = require("../../../../services/user.service");
var app_component_base_1 = require("../../../../shared/common/app-component-base");
var permission_model_1 = require("../model/permission.model");
var EditUserPermissionsModalComponent = /** @class */ (function (_super) {
    __extends(EditUserPermissionsModalComponent, _super);
    function EditUserPermissionsModalComponent(injector, _userService) {
        var _this = _super.call(this, injector) || this;
        _this._userService = _userService;
        _this.saving = false;
        _this.resettingPermissions = false;
        return _this;
    }
    EditUserPermissionsModalComponent.prototype.show = function (userId, userName) {
        var _this = this;
        this.userId = userId;
        this.userName = userName;
        this._userService.getUserPermissionsForEdit(userId).subscribe(function (result) {
            _this.permissionTree.editData = result.DataObject;
            _this.modal.show();
        });
    };
    EditUserPermissionsModalComponent.prototype.save = function () {
        var _this = this;
        var input = new permission_model_1.UpdatePermissionsInput();
        input.Id = this.userId;
        input.GrantedPermissionNames = this.permissionTree.getGrantedPermissionNames();
        this.saving = true;
        this._userService.updateUserPermissions(input)
            .finally(function () { _this.saving = false; })
            .subscribe(function () {
            _this.notify.info("Guardado exitosamente");
            _this.close();
        });
    };
    EditUserPermissionsModalComponent.prototype.resetPermissions = function () {
        //let input = new EntityDtoOfInt64();
        //input.id = this.userId;
        //this.resettingPermissions = true;
        //this._userService.resetUserSpecificPermissions(input).subscribe(() => {
        //    this.notify.info(this.l('ResetSuccessfully'));
        //    this._userService.getUserPermissionsForEdit(this.userId).subscribe(result => {
        //        this.permissionTree.editData = result;
        //    });
        //}, undefined, () => {
        //    this.resettingPermissions = false;
        //});
    };
    EditUserPermissionsModalComponent.prototype.close = function () {
        this.modal.hide();
    };
    __decorate([
        core_1.ViewChild('editModal'),
        __metadata("design:type", ngx_bootstrap_1.ModalDirective)
    ], EditUserPermissionsModalComponent.prototype, "modal", void 0);
    __decorate([
        core_1.ViewChild('permissionTree'),
        __metadata("design:type", permission_tree_component_1.PermissionTreeComponent)
    ], EditUserPermissionsModalComponent.prototype, "permissionTree", void 0);
    EditUserPermissionsModalComponent = __decorate([
        core_1.Component({
            selector: 'editUserPermissionsModal',
            templateUrl: './edit-user-permissions-modal.component.html'
        }),
        __metadata("design:paramtypes", [core_1.Injector,
            user_service_1.UserService])
    ], EditUserPermissionsModalComponent);
    return EditUserPermissionsModalComponent;
}(app_component_base_1.AppComponentBase));
exports.EditUserPermissionsModalComponent = EditUserPermissionsModalComponent;
//# sourceMappingURL=edit-user-permissions-modal.component.js.map