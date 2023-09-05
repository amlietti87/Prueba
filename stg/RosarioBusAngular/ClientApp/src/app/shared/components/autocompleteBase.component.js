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
var material_1 = require("@angular/material");
//@Component({
//    template: ComboBoxComponentTemplete._TEMPLATE,
//    providers: [
//        {
//            provide: NG_VALUE_ACCESSOR,
//            useExisting: forwardRef(() => ComboBoxComponent),
//            multi: true
//        }
//    ]
//})
var AutoCompleteComponent = /** @class */ (function () {
    function AutoCompleteComponent(service, injector) {
        this.service = service;
        this.injector = injector;
        this.items = [];
        this.placeHolder = '';
        this.field = 'Description';
        this.minLength = 1;
        this.showAddButton = false;
        this.selectedItemChange = new core_1.EventEmitter();
        this.IsDisabled = false;
        this.livesearch = true;
        this.onChange = function (rating) { };
        this.onTouched = function () {
        };
        this.isLoading = false;
        this.innerValue = '';
        this._renderer = injector.get(core_1.Renderer);
    }
    AutoCompleteComponent.prototype.ngAfterViewInit = function () {
    };
    Object.defineProperty(AutoCompleteComponent.prototype, "value", {
        //get accessor
        get: function () {
            return this.innerValue;
        },
        //set accessor including call the onchange callback
        set: function (v) {
            if (v !== this.innerValue) {
                this.innerValue = v;
                this.onChange(v);
            }
        },
        enumerable: true,
        configurable: true
    });
    ;
    AutoCompleteComponent.prototype.writeValue = function (value) {
        var self = this;
        if (value != this.innerValue) {
            this.innerValue = value;
            this.onChange(this.value);
        }
    };
    AutoCompleteComponent.prototype.registerOnChange = function (fn) {
        // throw new Error("Method not implemented.");
        this.onChange = fn;
    };
    AutoCompleteComponent.prototype.registerOnTouched = function (fn) {
        //throw new Error("Method not implemented.");
        this.onTouched = fn;
    };
    AutoCompleteComponent.prototype.setDisabledState = function (isDisabled) {
        //throw new Error("Method not implemented.");
        this.IsDisabled = isDisabled;
    };
    AutoCompleteComponent.prototype.ngOnInit = function () {
    };
    //sobrescrivir para filtro custom
    AutoCompleteComponent.prototype.GetFilter = function (query) {
        var f = {
            FilterText: query
        };
        return f;
    };
    AutoCompleteComponent.prototype.filterItems = function (event) {
        var _this = this;
        var query = null;
        if (event != null) {
            query = event.query;
        }
        this.service.GetItemsAsync(this.GetFilter(query)).subscribe(function (x) {
            _this.items = [];
            for (var i in x.DataObject) {
                var item = x.DataObject[i];
                _this.items.push(item);
            }
        });
    };
    AutoCompleteComponent.prototype.Unselect = function (event) {
    };
    AutoCompleteComponent.prototype.Clear = function (event) {
        //Hack. limpiar valor cuando se deseleciona el autocomplete
        this.value = null;
    };
    AutoCompleteComponent.prototype.onAddButtonClick = function () {
        var _this = this;
        var dialog = this.injector.get(material_1.MatDialog);
        var dialogConfig = new material_1.MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        var dialogRef = dialog.open(this.getIDetailComponent(), dialogConfig);
        dialogRef.componentInstance.showNew(this.getNewDto());
        dialogRef.afterClosed().subscribe(function (data) {
            if (data) {
                _this.filterItems(null);
            }
        });
    };
    AutoCompleteComponent.prototype.getIDetailComponent = function () {
        return null;
    };
    AutoCompleteComponent.prototype.getNewDto = function () {
        return null;
    };
    __decorate([
        core_1.ViewChild('autocomplete'),
        __metadata("design:type", core_1.ElementRef)
    ], AutoCompleteComponent.prototype, "autocompleteElement", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], AutoCompleteComponent.prototype, "placeHolder", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], AutoCompleteComponent.prototype, "field", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], AutoCompleteComponent.prototype, "minLength", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], AutoCompleteComponent.prototype, "showAddButton", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], AutoCompleteComponent.prototype, "selectedItemChange", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], AutoCompleteComponent.prototype, "livesearch", void 0);
    return AutoCompleteComponent;
}());
exports.AutoCompleteComponent = AutoCompleteComponent;
//# sourceMappingURL=autocompleteBase.component.js.map