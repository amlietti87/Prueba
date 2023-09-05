import { Component, ComponentFactoryResolver, ViewContainerRef, OnInit, Output, ViewEncapsulation, EventEmitter, Type, ViewChild, ReflectiveInjector, Inject, Injector, OnDestroy, AfterViewInit, Input } from '@angular/core';
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

import { DetailComponent, IDetailComponent } from './detail.component';

import { DialogService, DialogComponent } from 'ng2-bootstrap-modal';
import { LocatorService } from '../../shared/common/services/locator.service';
import { retry } from 'rxjs/operator/retry';
import { elementAt } from 'rxjs/operator/elementAt';
import { BreadcrumbsService } from '../../theme/layouts/breadcrumbs/breadcrumbs.service';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { detachEmbeddedView } from '@angular/core/src/view';
import { Observable } from 'rxjs';



export abstract class AgregationListComponent<T extends ADto> extends AppComponentBase implements OnInit, OnDestroy, AfterViewInit {

    protected cfr: ComponentFactoryResolver;
    protected popupHeight: string = '';
    protected popupWidth: string = '80%';



    public isTableLoading = false;
    advancedFiltersAreShown = true;
    _list: T[] = [];

    @Output() listChange = new EventEmitter();
    @Input()
    get list(): T[] {

        return this._list;
    }

    set list(value: T[]) {

        this._list = value;
        this.listChange.emit(this._list);
    }


    allowAdd: boolean = false;
    allowDelete: boolean = false;
    allowModify: boolean = false;

    isFirstTime?: Boolean = true;

    active = true;
    protected detailElement: IDetailComponent;


    @ViewChild('dataTable') dataTable: DataTable;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();


    protected router: Router;
    protected route: ActivatedRoute;
    protected dialog: MatDialog;
    viewMode: ViewMode;
    constructor(

        protected detailComponentType: Type<IDetailComponent>,
        injector: Injector
    ) {
        super(injector);

        this.cfr = injector.get(ComponentFactoryResolver);

        // var s = LocatorService.injector.get(ViewContainerRef);
        // LocatorService.injector.get(CreateOrEditUserModalComponent)        
        this.router = injector.get(Router);
        this.route = injector.get(ActivatedRoute);
        this.dialog = injector.get(MatDialog);

    }

    SetAllowPermission() {
        //this.allowAdd = this.permission.isGranted(key + '.Agregar');
        //this.allowDelete = this.permission.isGranted(key + '.Eliminar');
        //this.allowModify = this.permission.isGranted(key + '.Modificar');
        this.allowAdd = true;
        this.allowDelete = true;
        this.allowModify = true;
    }


    GetEditComponentType(): Type<IDetailComponent> {
        return this.detailComponentType;
    }

    GetEditComponent(): IDetailComponent {
        return this.detailElement;
    }


    ngOnInit() {
        this.SetAllowPermission();

        this.viewMode = ViewMode.List;

    }

    ngAfterViewInit() {

    }

    ngOnDestroy(): void {

    }

    onView(row: any) {
        this.viewMode = ViewMode.View;
        this.Opendialog(row);

        //this.active = false;
    }

    onEdit(row: T) {

        if (!this.allowModify) {
            return;
        }
        //this.active = false;
        this.viewMode = ViewMode.Modify;
        this.Opendialog(row);

    }


    CloseChild(): void {
        //this.GetEditComponent().close();
    }



    onCreate() {
        if (!this.allowAdd) {
            return;
        }
        //this.active = false;
        this.viewMode = ViewMode.Add;
        let item = this.getNewItem(null);
        item.Id = Math.min.apply(Math, this.list.map(function(o) { return o.Id; })) - 1;
        if (item.Id > 0)
            item.Id = -1;

        this.Opendialog(item);
    }


    Opendialog(_detail) {
        const dialogConfig = new MatDialogConfig<T>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;

        //dialogConfig.id = "1";
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        dialogConfig.data = this.CompleteDataBeforeShow(JSON.parse(JSON.stringify(_detail)) as T);

        let dialogRef = this.dialog.open(this.GetEditComponentType(), dialogConfig);

        this.detailElement = dialogRef.componentInstance;

        dialogRef.componentInstance.viewMode = this.viewMode;

        dialogRef.afterClosed().subscribe(


            data => {
                //console.log("Dialog output:", data);
                this.active = true;
                this.viewMode = ViewMode.List;
                if (data) {
                    this.UpdateItem(data);
                }

            }
        );

        //dialogRef.updateSize(this.popupWidth, this.popupHeight);


    }

    OpenCustomizedWidthDialog(_detail, width) {
        const dialogConfig = new MatDialogConfig<T>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = true;
        //dialogConfig.id = "1";
        dialogConfig.width = width;
        dialogConfig.height = this.popupHeight;

        dialogConfig.data = this.CompleteDataBeforeShow(JSON.parse(JSON.stringify(_detail)) as T);

        let dialogRef = this.dialog.open(this.GetEditComponentType(), dialogConfig);

        this.detailElement = dialogRef.componentInstance;

        dialogRef.componentInstance.viewMode = this.viewMode;

        dialogRef.afterClosed().subscribe(


            data => {
                //console.log("Dialog output:", data);
                this.active = true;
                this.viewMode = ViewMode.List;
                if (data) {
                    this.UpdateItem(data);
                }

            }
        );

        //dialogRef.updateSize(this.popupWidth, this.popupHeight);


    }

    CompleteDataBeforeShow(_detail: T): T {
        return _detail;
    }

    onDelete(item: T, bAvoidConfirmation?: boolean) {

        if (!this.allowDelete) {
            return;
        }


        this.ValidateDelete(item).subscribe(valid => {

            if (valid) {

                var strindto = this.getDescription(item);
                //var aa = this.getNewItem(item);
                //var stringentity = aa.getDescription();

                if (!bAvoidConfirmation) {
                    this.message.confirm('¿Está seguro de que desea eliminar el registro?', strindto || 'Confirmación', (a) => {

                        //this.isshowalgo = !this.isshowalgo;
                        if (a.value) {
                            this.deleteItem(item);
                        }

                    });
                } else {
                    this.deleteItem(item);
                }
            }
        })



    }

    ValidateDelete(item: T): Observable<boolean> {
        return Observable.create(observer => {
            observer.next(true);
            observer.complete();
        });
    }

    deleteItem(item: T) {

        let _list = [...this.list];
        var index = this.list.findIndex(e => e == item);
        _list.splice(index, 1);
        this.list = _list;
        this.primengDatatableHelper.records = [..._list];
    }

    getDescription(item: T): string {
        return '';
    }

    getNewItem(item: T): T {
        return null;
    }

    exportToExcel() {
    }

    UpdateItem(item: T) {
        this.CompleteDataBeforeUpdateItem(item);
        var index = this.list.findIndex(e => e.Id == item.Id);
        let items = [...this.list];

        if (index == -1) {
            items.push(item);
        }
        else {
            items.splice(this.list.findIndex(e => e.Id == item.Id), 1, item);
        }

        this.list = items;


    }

    CompleteDataBeforeUpdateItem(item: T): any {

    }

    RefreshList(items: T[]) {

        this.isTableLoading = true;
        this.primengDatatableHelper.showLoadingIndicator();


        this.list = items;
        this.primengDatatableHelper.records = items;

        this.isTableLoading = false;
    }



    reloadTable() {

    }

}
