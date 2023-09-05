import { Component, OnInit, ViewEncapsulation, Type, ViewChild, ReflectiveInjector, Inject, Injector, OnDestroy, AfterViewInit, Output, EventEmitter, ElementRef } from '@angular/core';
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
import { FilterDTO, Dto, IDto, ResponseModel, PaginListResultDto, ADto, Data, ViewMode } from '../../shared/model/base.model';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEventData } from '../helpers/PrimengDatatableHelper';

import { CrudService } from '../../shared/common/services/crud.service';


import { ModalDirective, BsModalService } from 'ngx-bootstrap/modal';
import { DialogService, DialogComponent } from 'ng2-bootstrap-modal';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/delay';
import { NgForm, FormGroup, FormBuilder } from '@angular/forms';
import { BreadcrumbsService } from '../../theme/layouts/breadcrumbs/breadcrumbs.service';
import { MatDialogConfig, MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export abstract class DetailComponent<T extends ADto> extends AppComponentBase implements IDetailComponent, OnInit, OnDestroy, AfterViewInit {

    createDefaultDetail(): any {

    }

    viewMode: ViewMode = ViewMode.Undefined;
    static hide(): any {
        throw new Error("Method not implemented.");
    }


    @ViewChild('detailForm') detailForm: NgForm;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();



    IsInMaterialPopupMode: boolean;
    icon: string;
    title: string;
    protected detailFB: FormBuilder;
    mainForm: FormGroup;

    id: any;

    //displayedColumns: any;
    filter: FilterDTO = new FilterDTO();
    public isTableLoading = false;
    advancedFiltersAreShown = false;
    public detail: T;
    active = false;
    saving = false;
    removing = false;
    closeOnSave = true;
    protected element: ElementRef;

    public breadcrumbsService: BreadcrumbsService;

    constructor(
        protected service: CrudService<T>,
        injector: Injector
    ) {
        super(injector);
        this.breadcrumbsService = injector.get(BreadcrumbsService);
        this.element = injector.get(ElementRef);
        this.createDefaultDetail();
        this.detailFB = injector.get(FormBuilder);
        this.createForm();
    }

    createForm() {

    }

    protected setStatusOnCloseSave(value: boolean) {
        this.closeOnSave = value;
    }

    markFormGroupTouched(formGroup: FormGroup) {
        (<any>Object).values(formGroup.controls).forEach(control => {
            control.markAsTouched();

            if (control.controls) {
                this.markFormGroupTouched(control);
            }
        });
    }


    getSelector(): string {
        return this.element.nativeElement.tagName;
    }


    validateSave(): boolean {
        return true;
    }

    save(form: NgForm): void {
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
    }


    SaveDetail(): any {

        this.service.createOrUpdate(this.detail, this.viewMode)
            .finally(() => { this.saving = false; })
            .pipe((e) => e.takeUntil(this.unsubscriber))
            .subscribe((t) => {

                if (this.viewMode = ViewMode.Add) {
                    this.detail.Id = t.DataObject;
                }

                this.notify.info('Guardado exitosamente');

                if (t.Messages && t.Messages.length > 0) {
                    this.notify.info(t.Messages.join(','));
                }

                if (this.closeOnSave) {
                    this.close();
                };
                this.affterSave(this.detail);
                this.closeOnSave = true;
                this.modalSave.emit(null);
            })
    }

    onSaveAndContinue(): void {
        this.closeOnSave = false;
        this.save(this.detailForm);
    }



    onShown(): void {
        // $(this.nameInput.nativeElement).focus();
    }

    affterSave(detail: T): void {

        if (!this.closeOnSave) {
            this.breadcrumbsService.RemoveItem(this.getSelector());
            this.active = false;
            this.show(this.detail.Id);
        }
    }

    ngAfterViewInit(): void {

    }
    ngOnDestroy(): void {

    }
    ngOnInit(): void {

        this.modalClose
            .pipe((e) => e.takeUntil(this.unsubscriber))
            .subscribe(e => {
            this.breadcrumbsService.RemoveItem(this.getSelector());
        })

        //this.detailForm.form.addControl()
        // this.ReadDto(this.id);
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


    show(id: any) {
        this.service.getById(id)
            .pipe((e) => e.takeUntil(this.unsubscriber))
            .subscribe(result => {
            this.viewMode = ViewMode.Modify;
            this.showDto(result.DataObject);
        }, e => this.handleErros(e,id));
    }

    showNew(item: T) {
        if (this.detailForm) {
            //this.detailForm.reset()
            this.detailForm.resetForm();
        }

        this.viewMode = ViewMode.Add;
        this.showDto(item)
    }

    //Alan bruhns 04/01/2019
    showEdit(item: T) {
        if (this.detailForm) {
            //this.detailForm.reset()
            this.detailForm.resetForm();
        }

        this.viewMode = ViewMode.Modify;
        this.showDto(item)
    }



    showDto(item: T) {
        this.detail = item;

        this.AddItemBreadcrumbs(item);

        this.completedataBeforeShow(item)
        this.active = true;
        if (this.element && this.element.nativeElement)
            this.tabInicializeFirst(this.element.nativeElement.tagName);
        else
            this.tabInicializeFirst();
    }


    AddItemBreadcrumbs(item: T): void {


        var selft = this;
        var closeChild = function() {
            selft.CloseChild()
        }


        if (this.viewMode == ViewMode.Add) {
            this.breadcrumbsService.AddItem("Agregar " + this.title, this.icon, "", this.getSelector(), closeChild);
        }
        else {
            this.breadcrumbsService.AddItem((this.getDescription(item) || "Editar " + this.title), this.icon, "", this.getSelector(), closeChild);
        }
    }



    getDescription(item: T): string {

        if (item && item["Description"]) {
            return item["Description"];
        }
        return "";
    }


    CloseChild(): void {

    }



    completedataBeforeShow(item: T): any {

    }

    completedataBeforeSave(item: T): any {

    }

    close(): void {
        this.closeMax();
        this.active = false;
        this.viewMode = ViewMode.Undefined;
        this.modalClose.emit(true);

    }

    closeMax() {
        try {
            var maxMin = $("[m-portlet-tool=fullscreen]");
            for (var i = 0; i < maxMin.length; i++) {
                if (maxMin[i].offsetParent) {
                    if (maxMin[i].offsetParent.id != "") {
                        maxMin[i].click();
                    }
                }
            }
        } catch {
            console.log("Fallo closeMax");
        }
    }


    tabInicializeFirst(selector?: string) {
        if (selector) {
            (<any>$(selector + ' .nav-tabs li:first-child a')).tab('show');
        } else {
            (<any>$('.nav-tabs li:first-child a')).tab('show');
        }
    }

}

export abstract class DetailEmbeddedComponent<T extends ADto> extends DetailComponent<T> implements IDetailComponent, OnInit, OnDestroy, AfterViewInit {

    constructor(
        protected service: CrudService<T>,
        injector: Injector
    ) {
        super(service, injector);

    }

}

export abstract class DetailModalComponent<T extends ADto> extends DetailComponent<T> implements IDetailComponent, OnInit, OnDestroy, AfterViewInit {

    @ViewChild('createOrEditModal') modal: ModalDirective;

    showDto(item: T) {
        super.showDto(item);
        this.modal.show();
    }

    close(): void {
        super.close();
        this.modal.hide();
    }

}

export abstract class DetailAgregationComponent<T extends ADto> extends DetailComponent<T> implements IDetailComponent, OnInit, OnDestroy, AfterViewInit {

    saveLocal: Boolean = true;

    constructor(
        protected dialogRef: MatDialogRef<DetailAgregationComponent<T>>,
        protected service: CrudService<T>,
        injector: Injector,
        public data: T

    ) {
        super(service, injector);

        this.detail = {} as T;

        setTimeout(() => {
            this.InitializeDetail(data);
            this.completedataBeforeShow(this.detail);
        }, 200)

    }


    InitializeDetail(data: T) {
        if (data && data != null) {
            this.detail = data;
        }
        else {
            this.detail.Id = 0;
        }
    }



    showDto(item: T) {

        this.detail = item;

        this.completedataBeforeShow(item);

        this.active = true;
    }

    close(): void {
        super.close();
        this.dialogRef.close(false);

    }
    createDefaultDetail(): any {

    }
    SaveDetail(): any {
        this.saving = true;
        if (!this.saveLocal) {
            this.service.createOrUpdate(this.detail, this.viewMode)
                .finally(() => { this.saving = false; })
                .pipe((e) => e.takeUntil(this.unsubscriber))
                .subscribe((t) => {

                    if (this.viewMode = ViewMode.Add) {
                        this.detail.Id = t.DataObject;
                    }

                    this.notify.info('Guardado exitosamente');
                    this.affterSave(this.detail);
                    this.closeOnSave = true;
                    this.modalSave.emit(null);
                    this.saving = false;
                    this.dialogRef.close(this.detail);
                })
        }
        else {
            this.affterSave(this.detail);
            this.closeOnSave = true;
            this.modalSave.emit(null);
            this.saving = false;
            this.dialogRef.close(this.detail);
        }


    }
}

export interface IDetailComponent {
    //save();
    modalSave: EventEmitter<any>;
    modalClose: EventEmitter<any>;
    viewMode: ViewMode;
    show(id: any)
    showNew(item: any)
    active: boolean;
    detail: any;
    close()
}


