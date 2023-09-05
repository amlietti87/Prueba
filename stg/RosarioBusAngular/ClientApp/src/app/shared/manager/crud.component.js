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
var router_1 = require("@angular/router");
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
var datatable_1 = require("primeng/components/datatable/datatable");
var paginator_1 = require("primeng/components/paginator/paginator");
var breadcrumbs_service_1 = require("../../theme/layouts/breadcrumbs/breadcrumbs.service");
var material_1 = require("@angular/material");
var fromEvent_1 = require("rxjs/observable/fromEvent");
var BaseCrudComponent = /** @class */ (function (_super) {
    __extends(BaseCrudComponent, _super);
    function BaseCrudComponent(service, detailComponentType, injector) {
        var _this = _super.call(this, injector) || this;
        _this.service = service;
        _this.detailComponentType = detailComponentType;
        _this.injector = injector;
        _this.isTableLoading = false;
        _this.advancedFiltersAreShown = false;
        _this.list = [];
        _this.moduleWithinTitle = false;
        _this.showbreadcum = true;
        _this.showDefaultBreadcum = true;
        _this.allowAdd = false;
        _this.allowDelete = false;
        _this.allowModify = false;
        _this.isFirstTime = true;
        _this.active = true;
        _this.modalSave = new core_1.EventEmitter();
        _this.breadcrumbsService = injector.get(breadcrumbs_service_1.BreadcrumbsService);
        _this.cfr = injector.get(core_1.ComponentFactoryResolver);
        // var s = LocatorService.injector.get(ViewContainerRef);
        // LocatorService.injector.get(CreateOrEditUserModalComponent)        
        _this.filter = _this.getNewfilter();
        _this.router = injector.get(router_1.Router);
        _this.route = injector.get(router_1.ActivatedRoute);
        return _this;
    }
    BaseCrudComponent.prototype.SetAllowPermission = function () {
        if (this.route.snapshot && this.route.snapshot.data && this.route.snapshot.data.permissions && this.route.snapshot.data.permissions["only"]) {
            var b = this.route.snapshot.data.permissions["only"].split(".");
            var key = b[0] + '.' + b[1];
            this.allowAdd = this.permission.isGranted(key + '.Agregar');
            this.allowDelete = this.permission.isGranted(key + '.Eliminar');
            this.allowModify = this.permission.isGranted(key + '.Modificar');
        }
    };
    BaseCrudComponent.prototype.GetEditComponentType = function () {
        return this.detailComponentType;
    };
    BaseCrudComponent.prototype.GetEditComponent = function () {
        var _this = this;
        if (!this.loadInMaterialPopup && !this.detailElement) {
            this.detailComponentType = this.GetEditComponentType();
            var factory = this.cfr.resolveComponentFactory(this.detailComponentType);
            var ref = this['createOrEdit'].createComponent(factory);
            ref.changeDetectorRef.detectChanges();
            this.detailElement = ref.instance;
            this.detailElement.modalClose
                .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
                .subscribe(function (e) {
                _this.active = true;
            });
            this.detailElement.modalSave
                .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
                .subscribe(function (e) {
                _this.active = true;
                _this.Search(null);
            });
        }
        return this.detailElement;
    };
    BaseCrudComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.SetAllowPermission();
        this.GetEditComponent();
        if (this.filterText) {
            fromEvent_1.fromEvent(this.filterText.nativeElement, 'keyup')
                .debounceTime(300)
                .distinctUntilChanged()
                .map(function () {
                _this.paginator.changePage(0);
                _this.onSearch();
            })
                .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
                .subscribe();
        }
    };
    BaseCrudComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        setTimeout(function (e) {
            if (_this.showDefaultBreadcum) {
                _this.breadcrumbsService.defaultBreadcrumbs(_this.title);
            }
            if (_this.showbreadcum) {
                var title = _this.title;
                if (_this.moduleName) {
                    if (_this.moduleWithinTitle) {
                        title = _this.moduleName;
                    }
                    else {
                        title = _this.moduleName + " - " + _this.title;
                    }
                }
                _this.breadcrumbsService.AddItem(title, _this.icon, '', null, null);
            }
        }, 10);
    };
    BaseCrudComponent.prototype.ngOnDestroy = function () {
        this.unsubscriber.next();
        this.unsubscriber.complete();
    };
    BaseCrudComponent.prototype.onView = function (row) {
        this.active = false;
    };
    BaseCrudComponent.prototype.onEdit = function (row) {
        this.onEditID(row.Id);
    };
    BaseCrudComponent.prototype.onEditID = function (id) {
        var _this = this;
        if (!this.allowModify) {
            return;
        }
        this.active = false;
        if (this.loadInMaterialPopup) {
            this.service.getById(id)
                .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
                .subscribe(function (e) { return _this.Opendialog(e.DataObject, base_model_1.ViewMode.Modify); }, function (e) { return _this.handleErros(e, id); });
        }
        else {
            this.GetEditComponent().show(id);
        }
    };
    BaseCrudComponent.prototype.handleErros = function (err, id) {
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
    BaseCrudComponent.prototype.CloseChild = function () {
        this.GetEditComponent().close();
    };
    BaseCrudComponent.prototype.onCreate = function () {
        if (!this.allowAdd) {
            return;
        }
        if (this.loadInMaterialPopup) {
            this.Opendialog(this.getNewItem(null), base_model_1.ViewMode.Add);
        }
        else {
            this.active = false;
            this.GetEditComponent().showNew(this.getNewItem(null));
        }
    };
    BaseCrudComponent.prototype.onDelete = function (item) {
        var _this = this;
        if (!this.allowDelete) {
            return;
        }
        var strindto = this.getDescription(item);
        //var aa = this.getNewItem(item);
        //var stringentity = aa.getDescription();
        this.message.confirm('¿Está seguro de que desea eliminar el registro?', strindto || 'Confirmación', function (a) {
            //this.isshowalgo = !this.isshowalgo;
            if (a.value) {
                _this.active = false;
                _this.service.delete(item.Id)
                    .finally(function () { _this.active = true; })
                    .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
                    .subscribe(function () {
                    _this.Search();
                    _this.notify.success(_this.l('Registro eliminado correctamente'));
                });
            }
        });
    };
    BaseCrudComponent.prototype.getDescription = function (item) {
        return '';
    };
    BaseCrudComponent.prototype.getNewItem = function (item) {
        return null;
    };
    BaseCrudComponent.prototype.exportToExcel = function () {
    };
    BaseCrudComponent.prototype.onSearch = function (event) {
        if (event == null) {
            if (this.paginator) { //ojo que al paginator.changePage se vuelve a ejecutar la busqueda.
                this.paginator.changePage(0);
            }
            else {
                this.Search(null);
            }
        }
        else {
            this.Search(event);
        }
    };
    BaseCrudComponent.prototype.Search = function (event) {
        var _this = this;
        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages
        if (this.isFirstTime == false) {
            this.isFirstTime = true;
            return;
        }
        if (!this.filter) {
            this.filter = this.getNewfilter();
        }
        this.completeFilter(this.filter);
        this.isTableLoading = true;
        this.primengDatatableHelper.showLoadingIndicator();
        this.filter.Sort = this.primengDatatableHelper.getSorting(this.dataTable);
        this.filter.Page = this.primengDatatableHelper.getPageIndex(this.paginator, event);
        this.filter.PageSize = this.primengDatatableHelper.getPageSize(this.paginator, event);
        this.service.search(this.filter)
            .finally(function () {
            _this.isTableLoading = false;
            //this.datatable.reload()
        })
            .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
            .subscribe(function (result) {
            _this.list = result.DataObject.Items;
            _this.primengDatatableHelper.records = result.DataObject.Items;
            _this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
            _this.primengDatatableHelper.hideLoadingIndicator();
        });
    };
    BaseCrudComponent.prototype.completeFilter = function (filter) {
    };
    BaseCrudComponent.prototype.getNewfilter = function () {
        return null;
    };
    BaseCrudComponent.prototype.reloadTable = function () {
    };
    BaseCrudComponent.prototype.Opendialog = function (_detail, viewMode) {
        var _this = this;
        var popupHeight = '';
        var popupWidth = '80%';
        var dialog = this.injector.get(material_1.MatDialog);
        var dialogConfig = new material_1.MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.width = popupWidth;
        dialogConfig.height = popupHeight;
        dialogConfig.data = _detail;
        var dialogRef = dialog.open(this.GetEditComponentType(), dialogConfig);
        this.detailElement = dialogRef.componentInstance;
        dialogRef.componentInstance.viewMode = viewMode;
        dialogRef.componentInstance.saveLocal = false;
        dialogRef.afterOpen()
            .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
            .subscribe(function (data) { _this.afterOpenDialog(data); });
        dialogRef.afterClosed()
            .pipe(function (e) { return e.takeUntil(_this.unsubscriber); })
            .subscribe(function (data) {
            //console.log("Dialog output:", data);
            _this.active = true;
            if (data) {
                _this.onSearch();
            }
        });
        //dialogRef.updateSize(this.popupWidth, this.popupHeight);
    };
    BaseCrudComponent.prototype.afterOpenDialog = function (data) {
        this.active = true;
    };
    __decorate([
        core_1.ViewChild('dataTable'),
        __metadata("design:type", datatable_1.DataTable)
    ], BaseCrudComponent.prototype, "dataTable", void 0);
    __decorate([
        core_1.ViewChild('paginator'),
        __metadata("design:type", paginator_1.Paginator)
    ], BaseCrudComponent.prototype, "paginator", void 0);
    __decorate([
        core_1.ViewChild('createOrEdit', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], BaseCrudComponent.prototype, "createOrEdit", void 0);
    __decorate([
        core_1.ViewChild('filterText'),
        __metadata("design:type", core_1.ElementRef)
    ], BaseCrudComponent.prototype, "filterText", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], BaseCrudComponent.prototype, "modalSave", void 0);
    return BaseCrudComponent;
}(app_component_base_1.AppComponentBase));
exports.BaseCrudComponent = BaseCrudComponent;
var CrudComponent = /** @class */ (function (_super) {
    __extends(CrudComponent, _super);
    function CrudComponent(service, detailComponentType, injector) {
        var _this = _super.call(this, service, detailComponentType, injector) || this;
        _this.service = service;
        _this.detailComponentType = detailComponentType;
        return _this;
    }
    CrudComponent.prototype.getNewfilter = function () {
        return new base_model_1.FilterDTO();
    };
    return CrudComponent;
}(BaseCrudComponent));
exports.CrudComponent = CrudComponent;
//# sourceMappingURL=crud.component.js.map