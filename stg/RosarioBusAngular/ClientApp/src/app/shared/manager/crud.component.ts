import { Component, ComponentFactoryResolver, ViewContainerRef, OnInit, Output, ViewEncapsulation, EventEmitter, Type, ViewChild, ReflectiveInjector, Inject, Injector, OnDestroy, AfterViewInit, ElementRef } from '@angular/core';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
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
import { AppComponentBase } from '../../shared/common/app-component-base';
import { FilterDTO, Dto, IDto, ResponseModel, PaginListResultDto, ADto, ViewMode } from '../../shared/model/base.model';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEventData } from '../helpers/PrimengDatatableHelper';

import { CrudService } from '../../shared/common/services/crud.service';

import { ModalDirective, BsModalService } from 'ngx-bootstrap/modal';

import { DetailAgregationComponent, IDetailComponent } from './detail.component';

import { DialogService, DialogComponent } from 'ng2-bootstrap-modal';
import { LocatorService } from '../../shared/common/services/locator.service';
import { retry } from 'rxjs/operator/retry';
import { elementAt } from 'rxjs/operator/elementAt';
import { BreadcrumbsService } from '../../theme/layouts/breadcrumbs/breadcrumbs.service';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { fromEvent } from 'rxjs/observable/fromEvent';
import { Subject } from 'rxjs';



export abstract class BaseCrudComponent<T extends ADto, F extends FilterDTO> extends AppComponentBase implements ICRUDComponent, OnInit, OnDestroy, AfterViewInit {

    protected cfr: ComponentFactoryResolver;
    //displayedColumns: any;
    filter: F;
    public isTableLoading = false;
    advancedFiltersAreShown = false;
    list: T[] = [];

    icon: string;
    title: string;
    moduleName: string;

    moduleWithinTitle: boolean = false;
    showbreadcum: boolean = true;
    showDefaultBreadcum: boolean = true;
    allowAdd: boolean = false;
    allowDelete: boolean = false;
    allowModify: boolean = false;

    isFirstTime?: Boolean = true;

    active = true;
    protected detailElement: IDetailComponent;
    
    //protected detailComponentType: Type<IDetailComponent>

    @ViewChild('dataTable') dataTable: DataTable;
    @ViewChild('paginator') paginator: Paginator;
    @ViewChild('createOrEdit', { read: ViewContainerRef }) createOrEdit: ViewContainerRef;
    @ViewChild('filterText') filterText: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    public breadcrumbsService: BreadcrumbsService;
    protected router: Router;
    protected route: ActivatedRoute;
    loadInMaterialPopup: boolean

    constructor(protected service: CrudService<T>,
        protected detailComponentType: Type<IDetailComponent>,
        protected injector: Injector) {

        super(injector);

        this.breadcrumbsService = injector.get(BreadcrumbsService);

        this.cfr = injector.get(ComponentFactoryResolver);

        // var s = LocatorService.injector.get(ViewContainerRef);
        // LocatorService.injector.get(CreateOrEditUserModalComponent)        
        this.filter = this.getNewfilter();

        this.router = injector.get(Router);

        this.route = injector.get(ActivatedRoute);
        

    }

    SetAllowPermission() {
        if (this.route.snapshot && this.route.snapshot.data && this.route.snapshot.data.permissions && this.route.snapshot.data.permissions["only"]) {

            var b = this.route.snapshot.data.permissions["only"].split(".");
            var key = b[0] + '.' + b[1];
            this.allowAdd = this.permission.isGranted(key + '.Agregar');
            this.allowDelete = this.permission.isGranted(key + '.Eliminar');
            this.allowModify = this.permission.isGranted(key + '.Modificar');
        }
    }


    GetEditComponentType(): Type<IDetailComponent> {
        return this.detailComponentType;
    }

    GetEditComponent(): IDetailComponent {
        if (!this.loadInMaterialPopup && !this.detailElement) {
            this.detailComponentType = this.GetEditComponentType();
            var factory = this.cfr.resolveComponentFactory(this.detailComponentType);
            const ref = this['createOrEdit'].createComponent(factory);
            ref.changeDetectorRef.detectChanges();
            this.detailElement = ref.instance;
            this.detailElement.modalClose
                .pipe((e) => e.takeUntil(this.unsubscriber))  
                .subscribe(e => {
                this.active = true;
            });

            this.detailElement.modalSave
                .pipe((e) => e.takeUntil(this.unsubscriber))  
                .subscribe(e => {
                this.active = true;
                this.Search(null);
            });
        }
        return this.detailElement;
    }


    ngOnInit() {
        this.SetAllowPermission();
        this.GetEditComponent();

        if (this.filterText) {
            fromEvent(this.filterText.nativeElement, 'keyup')
                .debounceTime(300)
                .distinctUntilChanged()
                .map(() => {
                    this.paginator.changePage(0);
                    this.onSearch();
                })
                .pipe((e) => e.takeUntil(this.unsubscriber))  
                .subscribe();
        }


    }

    ngAfterViewInit() {
        setTimeout(e => {
            if (this.showDefaultBreadcum) {
                this.breadcrumbsService.defaultBreadcrumbs(this.title);
            }

            if (this.showbreadcum) {
                var title = this.title;
                if (this.moduleName) {
                    if (this.moduleWithinTitle) {
                        title = this.moduleName;
                    }
                    else {
                        title = this.moduleName + " - " + this.title;
                    }
                }
                this.breadcrumbsService.AddItem(title, this.icon, '', null, null);
            }
        }
            , 10);

    }

    ngOnDestroy(): void {
        this.unsubscriber.next();
        this.unsubscriber.complete();
    }

    onView(row: any) {

        this.active = false;
    }

    onEdit(row: T) {
        this.onEditID(row.Id);
    }

    onEditID(id: any) {
        if (!this.allowModify) {
            return;
        }
        this.active = false;

        if (this.loadInMaterialPopup) {
            this.service.getById(id)
                .pipe((e) => e.takeUntil(this.unsubscriber))  
                .subscribe(e => this.Opendialog(e.DataObject, ViewMode.Modify), e => this.handleErros(e, id));

        }
        else {
            this.GetEditComponent().show(id);
        }

    }

    protected handleErros(err: any, id: any) {
        console.log('sever error:', err);  // debug

        if (err.error.Status == "ConcurrencyValidator") {
            this.message.confirm(err.error.Messages.toString(), "Entidad Bloqueada", (result) => {

                if (result.value) {

                    this.service.unBlockEntity(id)
                        .pipe((e) => e.takeUntil(this.unsubscriber))  
                        .subscribe(e => this.notify.success("La entidad fue desbloqueada, por favor reintente nuevamente."));

                }
            })

        }
        else if (err.error && err.error.Status == "ValidationError") {
            this.notify.error(err.error.Messages.toString())
        }
        else {
            this.notify.error("A ocurrido un erro por favor contactese con el administrador")
        }
    }

    

    CloseChild(): void {
        this.GetEditComponent().close();
    }



    onCreate() {
        debugger;
        if (!this.allowAdd) {
            return;
        }
        if (this.loadInMaterialPopup) {
            this.Opendialog(this.getNewItem(null), ViewMode.Add);
        }
        else {
            this.active = false;
            this.GetEditComponent().showNew(this.getNewItem(null));
        }

    }

    onDelete(item: T) {


        if (!this.allowDelete) {
            return;
        }

        var strindto = this.getDescription(item);
        //var aa = this.getNewItem(item);
        //var stringentity = aa.getDescription();

        this.message.confirm('¿Está seguro de que desea eliminar el registro?', strindto || 'Confirmación', (a) => {

            //this.isshowalgo = !this.isshowalgo;
            if (a.value) {
                this.active = false;
                this.service.delete(item.Id)
                    .finally(() => { this.active = true; })
                    .pipe((e) => e.takeUntil(this.unsubscriber))  
                    .subscribe(() => {
                        this.Search();
                        this.notify.success(this.l('Registro eliminado correctamente'));
                    });
            }

        });

    }

    getDescription(item: T): string {
        return '';
    }

    getNewItem(item: T): T {
        return null;
    }

    exportToExcel() {
    }

    onSearch(event?: LazyLoadEventData) {
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

    }

    Search(event?: LazyLoadEventData) {

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
            .finally(() => {
                this.isTableLoading = false;
                //this.datatable.reload()
            })
            .pipe((e) => e.takeUntil(this.unsubscriber))  
            .subscribe((result: ResponseModel<PaginListResultDto<T>>) => {
                this.list = result.DataObject.Items;
                this.primengDatatableHelper.records = result.DataObject.Items
                this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
                this.primengDatatableHelper.hideLoadingIndicator()
            });

    }
    completeFilter(filter: F) {

    }

    getNewfilter(): F {
        return null;
    }

    reloadTable() {

    }

    Opendialog(_detail: T, viewMode: ViewMode) {

        var popupHeight: string = '';
        var popupWidth: string = '80%';

        var dialog = this.injector.get(MatDialog);

        const dialogConfig = new MatDialogConfig<T>();

        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.width = popupWidth;
        dialogConfig.height = popupHeight;




        dialogConfig.data = _detail;

        let dialogRef = dialog.open(this.GetEditComponentType(), dialogConfig);

        this.detailElement = dialogRef.componentInstance;

        dialogRef.componentInstance.viewMode = viewMode;

        (dialogRef.componentInstance as DetailAgregationComponent<T>).saveLocal = false;
        dialogRef.afterOpen()
            .pipe((e) => e.takeUntil(this.unsubscriber))  
            .subscribe(
            data => { this.afterOpenDialog(data) }
        );
        dialogRef.afterClosed()
            .pipe((e) => e.takeUntil(this.unsubscriber))  
            .subscribe(
            data => {
                //console.log("Dialog output:", data);
                this.active = true;

                if (data) {
                    this.onSearch();
                }

            }
        );

        //dialogRef.updateSize(this.popupWidth, this.popupHeight);


    }

    public afterOpenDialog(data: any) {
        this.active = true;
    }

}

export abstract class CrudComponent<T extends ADto> extends BaseCrudComponent<T, FilterDTO>
{
    constructor(
        protected service: CrudService<T>,
        protected detailComponentType: Type<IDetailComponent>,
        injector: Injector
    ) {
        super(service, detailComponentType, injector)
    }

    getNewfilter(): FilterDTO {
        return new FilterDTO();
    }

}

export interface ICRUDComponent {
    onView(row: any);
    onCreate(row: any);
    onEdit(row: any);
    onDelete(row: any);
    onSearch();
    reloadTable();
}