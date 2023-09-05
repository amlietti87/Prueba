"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
//import { FileDownloadService } from './file-download.service';
//import { EqualValidator } from './validation/equal-validator.directive';
//import { PasswordComplexityValidator } from './validation/password-complexity-validator.directive';
//import { MinValueValidator } from './validation/min-value-validator.directive';
var button_busy_directive_1 = require("./button-busy.directive");
//import { AutoFocusDirective } from './auto-focus.directive';
var busy_if_directive_1 = require("./busy-if.directive");
var local_storage_service_1 = require("./local-storage.service");
//import { FriendProfilePictureComponent } from './friend-profile-picture.component';
var moment_format_pipe_1 = require("./moment-format.pipe");
//import { CurrencyInputDirective } from './currency-input.directive';
//import { NormalizeDropdownPositionDirective } from './normalize-dropdown-position.directive';
var UtilsModule = /** @class */ (function () {
    function UtilsModule() {
    }
    UtilsModule = __decorate([
        core_1.NgModule({
            providers: [
                // FileDownloadService,
                local_storage_service_1.DBLocalStorageService
            ],
            declarations: [
                //EqualValidator,
                //PasswordComplexityValidator,
                //MinValueValidator,
                button_busy_directive_1.ButtonBusyDirective,
                //AutoFocusDirective,
                busy_if_directive_1.BusyIfDirective,
                //FriendProfilePictureComponent,
                moment_format_pipe_1.MomentFormatPipe,
            ],
            exports: [
                //EqualValidator,
                //PasswordComplexityValidator,
                //MinValueValidator,
                button_busy_directive_1.ButtonBusyDirective,
                //AutoFocusDirective,
                busy_if_directive_1.BusyIfDirective,
                //FriendProfilePictureComponent,
                moment_format_pipe_1.MomentFormatPipe,
            ]
        })
    ], UtilsModule);
    return UtilsModule;
}());
exports.UtilsModule = UtilsModule;
//# sourceMappingURL=utils.module.js.map