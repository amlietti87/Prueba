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
//import { DialogService, DialogComponent } from 'ng2-bootstrap-modal';
//import { MatPaginator, MatSort } from '@angular/material';
//import { Page } from 'app/shared/models/pagination';
//import { DataSource } from '@angular/cdk/collections';
//import { AlertService } from 'app/shared/components/alert/alert.service';
//import { LocatorService } from 'app/shared/services/locator.service';
//import { ICRUDComponent, DTO, Data, CrudDataSource, FilterDTO } from 'app/shared/components/crud/crud.model';
//import { CrudService } from 'app/shared/components/crud/crud.service';
//import { ITEMS_PER_PAGE } from 'app/shared/constants/constants';
//import { ConfirmComponent } from 'app/shared/components/confirm/confirm.component';
var app_component_base_1 = require("../../shared/common/app-component-base");
var base_model_1 = require("../../shared/model/base.model");
var modal_1 = require("ngx-bootstrap/modal");
require("rxjs/add/observable/of");
require("rxjs/add/operator/delay");
var forms_1 = require("@angular/forms");
var breadcrumbs_service_1 = require("../../theme/layouts/breadcrumbs/breadcrumbs.service");
var DetailComponent = /** @class */ (function (_super) {
    __extends(DetailComponent, _super);
    function DetailComponent(service, injector) {
        var _this = _super.call(this, injector) || this;
        _this.service = service;
        _this.viewMode = base_model_1.ViewMode.Undefined;
        _this.modalSave = new core_1.EventEmitter();
        _this.modalClose = new core_1.EventEmitter();
        //displayedColumns: any;
        _this.filter = new base_model_1.FilterDTO();
        _this.isTableLoading = false;
        _this.advancedFiltersAreShown = false;
        _this.active = false;
        _this.saving = false;
        _this.removing = false;
        _this.closeOnSave = true;
        _this.breadcrumbsService = injector.get(breadcrumbs_service_1.BreadcrumbsService);
        _this.element = injector.get(core_1.ElementRef);
        _this.createDefaultDetail();
        _this.detailFB = injector.get(forms_1.FormBuilder);
        _this.createForm();
        return _this;
    }
    DetailComponent.prototype.createDefaultDetail = function () {
    };
    DetailComponent.hide = function () {
        throw new Error("Method not implemented.");
    };
    DetailComponent.prototype.createForm = function () {
    };
    DetailComponent.prototype.setStatusOnCloseSave = function (value) {
        this.closeOnSave = value;
    };
    DetailComponent.prototype.markFormGroupTouched = function (formGroup) {
        var _this = this;
        Object.values(formGroup.controls).forEach(function (control) {
            control.markAsTouched();
            if (control.controls) {
                _this.markFormGroupTouched(control);
            }
        });
    };
    DetailComponent.prototype.getSelector = function () {
        return this.element.nativeElement.tagName;
    };
    DetailComponent.prototype.validateSave = function () {
        return true;
    };
    DetailComponent.prototype.save = function (form) {
        if (this.detailForm && this.detailForm.form.invalid) {
            //(<any>Object).values(this.detailForm.controls).forEach(control => {
            //    if (control.status != "VALID") {
            //        console.log(control);
            //    }
            //});
            //(<any>Object).values(this.detailForm.form.controls).forEach(control => {
            //    if (control.status != "VALID") {
            //    
            //    }
            //});
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
    DetailComponent.prototype.SaveDetail = function () {
        var _this = this;
        this.service.createOrUpdate(this.detail, this.viewMode)
            .finally(function () { _this.saving = false; })
            .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
            .subscribe(function (t) {
            if (_this.viewMode = base_model_1.ViewMode.Add) {
                _this.detail.Id = t.DataObject;
            }
            _this.notify.info('Guardado exitosamente');
            if (t.Messages && t.Messages.length > 0) {
                _this.notify.info(t.Messages.join(','));
            }
            if (_this.closeOnSave) {
                _this.close();
            }
            ;
            _this.affterSave(_this.detail);
            _this.closeOnSave = true;
            _this.modalSave.emit(null);
        });
    };
    DetailComponent.prototype.onSaveAndContinue = function () {
        this.closeOnSave = false;
        this.save(this.detailForm);
    };
    DetailComponent.prototype.onShown = function () {
        // $(this.nameInput.nativeElement).focus();
    };
    DetailComponent.prototype.affterSave = function (detail) {
        if (!this.closeOnSave) {
            this.breadcrumbsService.RemoveItem(this.getSelector());
            this.active = false;
            this.show(this.detail.Id);
        }
    };
    DetailComponent.prototype.ngAfterViewInit = function () {
    };
    DetailComponent.prototype.ngOnDestroy = function () {
    };
    DetailComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.modalClose
            .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
            .subscribe(function (e) {
            _this.breadcrumbsService.RemoveItem(_this.getSelector());
        });
        //this.detailForm.form.addControl()
        // this.ReadDto(this.id);
    };
    DetailComponent.prototype.handleErros = function (err, id) {
        var _this = this;
        console.log('sever error:', err); // debug
        if (err.error.Status == "ConcurrencyValidator") {
            this.message.confirm(err.error.Messages.toString(), "Entidad Bloqueada", function (result) {
                if (result.value) {
                    _this.service.unBlockEntity(id)
                        .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
                        .subscribe(function (e) { return _this.notify.success("La entidad fue desbloqueada, por favor reintente nuevamente."); });
                }
            });
        }
        else if (err.error && err.error.Status == "ValidationError") {
            this.notify.error(err.error.Messages.toString());
        }
        else {
            this.notify.error("A ocurrido un erro por favor contactese con el administrador");
        }
    };
    DetailComponent.prototype.show = function (id) {
        var _this = this;
        this.service.getById(id)
            .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
            .subscribe(function (result) {
            _this.viewMode = base_model_1.ViewMode.Modify;
            _this.showDto(result.DataObject);
        }, function (e) { return _this.handleErros(e, id); });
    };
    DetailComponent.prototype.showNew = function (item) {
        if (this.detailForm) {
            //this.detailForm.reset()
            this.detailForm.resetForm();
        }
        this.viewMode = base_model_1.ViewMode.Add;
        this.showDto(item);
    };
    //Alan bruhns 04/01/2019
    DetailComponent.prototype.showEdit = function (item) {
        if (this.detailForm) {
            //this.detailForm.reset()
            this.detailForm.resetForm();
        }
        this.viewMode = base_model_1.ViewMode.Modify;
        this.showDto(item);
    };
    DetailComponent.prototype.showDto = function (item) {
        this.detail = item;
        this.AddItemBreadcrumbs(item);
        this.completedataBeforeShow(item);
        this.active = true;
        if (this.element && this.element.nativeElement)
            this.tabInicializeFirst(this.element.nativeElement.tagName);
        else
            this.tabInicializeFirst();
    };
    DetailComponent.prototype.AddItemBreadcrumbs = function (item) {
        var selft = this;
        var closeChild = function () {
            selft.CloseChild();
        };
        if (this.viewMode == base_model_1.ViewMode.Add) {
            this.breadcrumbsService.AddItem("Agregar " + this.title, this.icon, "", this.getSelector(), closeChild);
        }
        else {
            this.breadcrumbsService.AddItem((this.getDescription(item) || "Editar " + this.title), this.icon, "", this.getSelector(), closeChild);
        }
    };
    DetailComponent.prototype.getDescription = function (item) {
        if (item && item["Description"]) {
            return item["Description"];
        }
        return "";
    };
    DetailComponent.prototype.CloseChild = function () {
    };
    DetailComponent.prototype.completedataBeforeShow = function (item) {
    };
    DetailComponent.prototype.completedataBeforeSave = function (item) {
    };
    DetailComponent.prototype.close = function () {
        this.closeMax();
        this.active = false;
        this.viewMode = base_model_1.ViewMode.Undefined;
        this.modalClose.emit(true);
    };
    DetailComponent.prototype.closeMax = function () {
        try {
            var maxMin = $("[m-portlet-tool=fullscreen]");
            for (var i = 0; i < maxMin.length; i++) {
                if (maxMin[i].offsetParent) {
                    if (maxMin[i].offsetParent.id != "") {
                        maxMin[i].click();
                    }
                }
            }
        }
        catch (_a) {
            console.log("Fallo closeMax");
        }
    };
    DetailComponent.prototype.tabInicializeFirst = function (selector) {
        if (selector) {
            $(selector + ' .nav-tabs li:first-child a').tab('show');
        }
        else {
            $('.nav-tabs li:first-child a').tab('show');
        }
    };
    __decorate([
        core_1.ViewChild('detailForm'),
        __metadata("design:type", forms_1.NgForm)
    ], DetailComponent.prototype, "detailForm", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], DetailComponent.prototype, "modalSave", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], DetailComponent.prototype, "modalClose", void 0);
    return DetailComponent;
}(app_component_base_1.AppComponentBase));
exports.DetailComponent = DetailComponent;
var DetailEmbeddedComponent = /** @class */ (function (_super) {
    __extends(DetailEmbeddedComponent, _super);
    function DetailEmbeddedComponent(service, injector) {
        var _this = _super.call(this, service, injector) || this;
        _this.service = service;
        return _this;
    }
    return DetailEmbeddedComponent;
}(DetailComponent));
exports.DetailEmbeddedComponent = DetailEmbeddedComponent;
var DetailModalComponent = /** @class */ (function (_super) {
    __extends(DetailModalComponent, _super);
    function DetailModalComponent() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    DetailModalComponent.prototype.showDto = function (item) {
        _super.prototype.showDto.call(this, item);
        this.modal.show();
    };
    DetailModalComponent.prototype.close = function () {
        _super.prototype.close.call(this);
        this.modal.hide();
    };
    __decorate([
        core_1.ViewChild('createOrEditModal'),
        __metadata("design:type", modal_1.ModalDirective)
    ], DetailModalComponent.prototype, "modal", void 0);
    return DetailModalComponent;
}(DetailComponent));
exports.DetailModalComponent = DetailModalComponent;
var DetailAgregationComponent = /** @class */ (function (_super) {
    __extends(DetailAgregationComponent, _super);
    function DetailAgregationComponent(dialogRef, service, injector, data) {
        var _this = _super.call(this, service, injector) || this;
        _this.dialogRef = dialogRef;
        _this.service = service;
        _this.data = data;
        _this.saveLocal = true;
        _this.detail = {};
        setTimeout(function () {
            _this.InitializeDetail(data);
            _this.completedataBeforeShow(_this.detail);
        }, 200);
        return _this;
    }
    DetailAgregationComponent.prototype.InitializeDetail = function (data) {
        if (data && data != null) {
            this.detail = data;
        }
        else {
            this.detail.Id = 0;
        }
    };
    DetailAgregationComponent.prototype.showDto = function (item) {
        this.detail = item;
        this.completedataBeforeShow(item);
        this.active = true;
    };
    DetailAgregationComponent.prototype.close = function () {
        _super.prototype.close.call(this);
        this.dialogRef.close(false);
    };
    DetailAgregationComponent.prototype.createDefaultDetail = function () {
    };
    DetailAgregationComponent.prototype.SaveDetail = function () {
        var _this = this;
        this.saving = true;
        if (!this.saveLocal) {
            this.service.createOrUpdate(this.detail, this.viewMode)
                .finally(function () { _this.saving = false; })
                .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
                .subscribe(function (t) {
                if (_this.viewMode = base_model_1.ViewMode.Add) {
                    _this.detail.Id = t.DataObject;
                }
                _this.notify.info('Guardado exitosamente');
                _this.affterSave(_this.detail);
                _this.closeOnSave = true;
                _this.modalSave.emit(null);
                _this.saving = false;
                _this.dialogRef.close(_this.detail);
            });
        }
        else {
            this.affterSave(this.detail);
            this.closeOnSave = true;
            this.modalSave.emit(null);
            this.saving = false;
            this.dialogRef.close(this.detail);
        }
    };
    return DetailAgregationComponent;
}(DetailComponent));
exports.DetailAgregationComponent = DetailAgregationComponent;
//# sourceMappingURL=detail.component.js.map