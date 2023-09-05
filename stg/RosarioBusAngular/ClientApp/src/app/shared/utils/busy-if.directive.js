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
var BusyIfDirective = /** @class */ (function () {
    function BusyIfDirective(_element) {
        this._element = _element;
    }
    Object.defineProperty(BusyIfDirective.prototype, "busyIf", {
        set: function (isBusy) {
            this._isBusy = isBusy;
            this.refreshState(isBusy);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusyIfDirective.prototype, "busyText", {
        set: function (busyText) {
            debugger;
            this._busyText = busyText;
            if (this._isBusy) {
                this.refreshState(this._isBusy);
            }
        },
        enumerable: true,
        configurable: true
    });
    BusyIfDirective.prototype.refreshState = function (isBusy) {
        if (isBusy === undefined) {
            return;
        }
        if (isBusy) {
            mApp.block($(this._element.nativeElement), {
                overlayColor: '#000000',
                opacity: 0.2,
                type: 'loader',
                state: 'primary',
                message: this._busyText || 'Cargando...'
            });
        }
        else {
            mApp.unblock($(this._element.nativeElement));
        }
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean),
        __metadata("design:paramtypes", [Boolean])
    ], BusyIfDirective.prototype, "busyIf", null);
    __decorate([
        core_1.Input('busyText'),
        __metadata("design:type", String),
        __metadata("design:paramtypes", [String])
    ], BusyIfDirective.prototype, "busyText", null);
    BusyIfDirective = __decorate([
        core_1.Directive({
            selector: '[busyIf]'
        }),
        __metadata("design:paramtypes", [core_1.ElementRef])
    ], BusyIfDirective);
    return BusyIfDirective;
}());
exports.BusyIfDirective = BusyIfDirective;
//# sourceMappingURL=busy-if.directive.js.map