import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit, Input, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';



import * as _ from 'lodash';
declare let mApp: any;
import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { RouterModule, Routes, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { IDetailComponent, DetailEmbeddedComponent } from '../../../../../shared/manager/detail.component';
import { HServiciosFilter, HServiciosDto } from '../../model/hServicios.model';
import { HFechasConfiDto } from '../../model/HFechasConfi.model';
import { HFechasConfiService } from '../HFechasConfi.service';
import { MinutosPorSectorComponent } from '../tabsitem/minutos-por-sector.component';
import { CreateOrEditHFechasConfiServiciosComponent } from '../tabsitem/create-or-edit-hfechasconfi-servicios.component';
import { HChoxserComponent } from '../tabsitem/hChoxser.component';
import { ViewMode } from '../../../../../shared/model/base.model';
import { HHorariosConfiDto } from '../../model/hhorariosconfi.model';
import { filter } from 'rxjs/operator/filter';
import { MatDialog, MatDialogConfig } from '@angular/material';




@Component({
    selector: 'tabsdetalle-horariofecha',
    templateUrl: './tabsdetalle-horariofecha.component.html',

})
export class TabsDetalleHorariofechaComponent extends DetailEmbeddedComponent<HFechasConfiDto> implements IDetailComponent, OnInit {


    @ViewChild('createOrEditHFechasConfiServiciosView', { read: ViewContainerRef }) createOrEditHFechasConfiServiciosView: ViewContainerRef;
    @ViewChild('minutosPorSectorView', { read: ViewContainerRef }) minutosPorSectorView: ViewContainerRef;
    @ViewChild('duracionView', { read: ViewContainerRef }) duracionView: ViewContainerRef;

    minutosPorSectorViewInstance: MinutosPorSectorComponent;
    createOrEditHFechasConfiServicios: CreateOrEditHFechasConfiServiciosComponent;
    duracionViewInstance: HChoxserComponent;
    confirm: string = '¿Está seguro de que desea salir existen datos sin guardar?';
    EditEnabled: boolean = true;

    isLoadingMediaVueltas: boolean;
    isLoading: boolean;
    public dialog: MatDialog;
    filter: HServiciosFilter = new HServiciosFilter();


    getDescription(item: HFechasConfiDto): string {
        return item.Description;
    }


    constructor(
        injector: Injector,
        protected service: HFechasConfiService,
        protected router: Router,
        private cfr: ComponentFactoryResolver

    ) {
        super(service, injector);
        this.detail = new HFechasConfiDto();
        this.icon = "flaticon-clock-2";
        this.title = "Servicios";
        this.dialog = injector.get(MatDialog);
        this.filter = new HServiciosFilter();

    }

    ngOnInit(): void {

        super.ngOnInit();
    }
    crearcreateOrEditHFechasConfiServicios(): void {


        if (!this.createOrEditHFechasConfiServicios) {
            let cType = CreateOrEditHFechasConfiServiciosComponent;
            var factory = this.cfr.resolveComponentFactory(cType);
            const ref = this['createOrEditHFechasConfiServiciosView'].createComponent(factory);

            ref.changeDetectorRef.detectChanges();
            this.createOrEditHFechasConfiServicios = (ref.instance as CreateOrEditHFechasConfiServiciosComponent);

            this.createOrEditHFechasConfiServicios.active = true;
            mApp.init();
            this.createOrEditHFechasConfiServicios.modalClose.subscribe(s => {

                this.emitClose(true);
            });

            this.createOrEditHFechasConfiServicios.IrDuracion.subscribe(s => {
                const dialogConfig = new MatDialogConfig();
                dialogConfig.disableClose = false;
                dialogConfig.autoFocus = true;
                dialogConfig.data = new HHorariosConfiDto(
                    {
                        CodHfecha: this.filter.CodHfecha,
                        CodSubg: this.filter.CodSubg,
                        CodTdia: this.filter.CodTdia,
                        CurrentServicio: s.CurrentServicio
                    });


                dialogConfig.width = '80%';

                let dialogRef = this.dialog.open(HChoxserComponent, dialogConfig);

                dialogRef.componentInstance.modalSave.subscribe(s => {
                    $('#m_h_quick_sidebar_tabs_DetalleSYR')[0].click()
                    this.createOrEditHFechasConfiServicios.BuscarServicios(this.filter.ServicioId, this.filter.DescripcionServicio);
                    this.filter.ServicioId = null;
                    dialogRef.componentInstance.cleanData();
                });
                dialogRef.componentInstance.completedataBeforeShow(dialogRef.componentInstance.detail, s.CurrentServicio)

                dialogRef.componentInstance.parentEntity = this.detail;
                dialogRef.componentInstance.viewMode = this.viewMode;
                dialogRef.componentInstance.filter = this.filter;
                dialogRef.componentInstance.filter.CodHfecha = this.createOrEditHFechasConfiServicios.filter.CodHfecha
                dialogRef.componentInstance.filter.CodTdia = this.createOrEditHFechasConfiServicios.filter.CodTdia
                dialogRef.componentInstance.filter.ServicioId = s.CurrentServicio.Id;
                dialogRef.componentInstance.filter.DescripcionServicio = s.CurrentServicio.NumSer;
                dialogRef.componentInstance.currentServicio = s.CurrentServicio;
                this.setActiveView(dialogRef.componentInstance, true);
                dialogRef.componentInstance.disableFilter = true;
                dialogRef.componentInstance.isPopup = true;
                dialogRef.componentInstance.BuscarDuraciones();

                dialogRef.afterClosed().subscribe(
                    data => {
                        this.filter.ServicioId = null;
                        if (data) {
                            this.dialog.closeAll();
                        }
                    }
                );
            });
        }
    }

    onFilterChange(filter: any) {


        this.filter = filter;

        if (this.createOrEditHFechasConfiServicios && this.createOrEditHFechasConfiServicios.filter != filter) {
            this.createOrEditHFechasConfiServicios.filter = filter;
        }

        if (this.minutosPorSectorViewInstance && this.minutosPorSectorViewInstance.filter != filter) {
            this.minutosPorSectorViewInstance.filter = filter;
        }

        if (this.duracionViewInstance && this.duracionViewInstance.filter != filter) {
            this.duracionViewInstance.filter = filter;
        }
    }



    onChangeScreen() {
        this.confirmClose({ detail: this.detail, event: "ChangeScreen" });
    }




    canDeactivate(): boolean {
        return !(this.createOrEditHFechasConfiServicios && this.createOrEditHFechasConfiServicios.existUnsavedData());
    }



    ngAfterViewInit(): void {
        mApp.initPortlets();
    }

    completedataBeforeShow(item: HFechasConfiDto): any {
        //this.filter = new HServiciosFilter();
        //this.filter.CodHfecha = item.Id;

        super.completedataBeforeShow(item);

    }

    show(id: any) {
        this.setActiveView(this.createOrEditHFechasConfiServicios, true);
        this.isLoading = true
        this.crearcreateOrEditHFechasConfiServicios();
        super.show(id);
    }

    showDto(item: HFechasConfiDto) {

        super.showDto(item);
        this.createOrEditHFechasConfiServicios.filter = this.filter;
        this.createOrEditHFechasConfiServicios.viewMode = this.viewMode;
        this.createOrEditHFechasConfiServicios.showDto(item);
        this.isLoading = false
    }

    //AddItemBreadcrumbs(HFechasConfiDto) {

    //}

    emitClose(paramClose: any): void {
        this.active = false;
        this.viewMode = ViewMode.Undefined;
        this.modalClose.emit(paramClose);

    }

    CleanChilds(): any {

        if (this.createOrEditHFechasConfiServicios) {
            this.createOrEditHFechasConfiServicios.cleanData();
        }

        if (this.minutosPorSectorViewInstance) {
            this.minutosPorSectorViewInstance.cleanData();
        }

        if (this.duracionViewInstance) {
            this.duracionViewInstance.cleanData();
        }

    }


    confirmClose(paramClose: any) {
        if (!this.createOrEditHFechasConfiServicios) {
            this.CleanChilds();
            this.emitClose(paramClose);
        }
        else if (!this.createOrEditHFechasConfiServicios.existUnsavedData()) {
            this.CleanChilds();
            this.createOrEditHFechasConfiServicios.close();
            this.emitClose(paramClose);
        }
        else {
            this.message.confirm(this.createOrEditHFechasConfiServicios.confirm, 'Confirmación', (a) => {
                if (a.value) {
                    this.CleanChilds();
                    this.createOrEditHFechasConfiServicios.clearUnsavedData();
                    this.createOrEditHFechasConfiServicios.close();
                    this.emitClose(paramClose);
                }

            });
        }


    }

    close(): void {
        this.confirmClose(true);
    }

    ServiciosSelected(): void {
        if (!this.createOrEditHFechasConfiServicios.active) {
            this.setActiveView(this.createOrEditHFechasConfiServicios, true);
            this.setActiveView(this.minutosPorSectorViewInstance, false);
            this.setActiveView(this.duracionViewInstance, false);
            this.createOrEditHFechasConfiServicios.BuscarServicios();
        }
    }

    MinutosPorSectorSelected(): void {
        let self = this;
        //armo una funcion para invocarla en varios lugares
        let internalCallback = function() {
            if (!self.minutosPorSectorViewInstance) {
                let cType = MinutosPorSectorComponent;
                var factory = self.cfr.resolveComponentFactory(cType);
                const ref = self['minutosPorSectorView'].createComponent(factory);
                ref.changeDetectorRef.detectChanges();
                self.minutosPorSectorViewInstance = (ref.instance as MinutosPorSectorComponent);
                self.minutosPorSectorViewInstanceInicializar();
                mApp.init();
                self.minutosPorSectorViewInstance.modalClose.subscribe(s => {
                    self.close();
                })

            }
            else {
                self.minutosPorSectorViewInstanceInicializar();
            }
        }



        if (self.createOrEditHFechasConfiServicios.active) {

            if (self.createOrEditHFechasConfiServicios.existUnsavedData()) {
                self.message.confirm(self.confirm, 'Confirmación', (a) => {
                    if (a.value) {
                        self.createOrEditHFechasConfiServicios.clearUnsavedData();
                        internalCallback();
                    }
                    else {
                        $('#m_h_quick_sidebar_tabs_DetalleSYR')[0].click()
                    }
                });

                return;
            }
        }

        internalCallback();
    }

    private minutosPorSectorViewInstanceInicializar(): void {

        if (!this.minutosPorSectorViewInstance.active) {
            this.minutosPorSectorViewInstance.viewMode = this.viewMode;
            this.minutosPorSectorViewInstance.filter = this.filter;
            this.minutosPorSectorViewInstance.detail = this.detail;
            this.minutosPorSectorViewInstance.detail.CodTDia = this.createOrEditHFechasConfiServicios.filter.CodTdia;
            this.minutosPorSectorViewInstance.completedataBeforeShow(this.detail)

            this.minutosPorSectorViewInstance.filterChange.subscribe(e => this.onFilterChange(e));

            this.setActiveView(this.createOrEditHFechasConfiServicios, false);
            this.setActiveView(this.minutosPorSectorViewInstance, true);
            this.setActiveView(this.duracionViewInstance, false);
        }


    }


    DuracionSelected(servicio?: HHorariosConfiDto): void {
        let self = this;
        let internalCallback = function() {
            if (!self.duracionViewInstance) {
                let cType = HChoxserComponent;
                var factory = self.cfr.resolveComponentFactory(cType);
                const ref = self['duracionView'].createComponent(factory);
                ref.changeDetectorRef.detectChanges();
                self.duracionViewInstance = (ref.instance as HChoxserComponent);
                self.duracionViewInstanceInicializar(servicio);
                mApp.init();
                self.duracionViewInstance.modalClose.subscribe(s => {
                    self.close();
                })

                self.duracionViewInstance.modalSave.subscribe(s => {
                    $('#m_h_quick_sidebar_tabs_DetalleSYR')[0].click()
                    self.createOrEditHFechasConfiServicios.BuscarServicios();
                    self.duracionViewInstance.cleanData();
                });

                self.duracionViewInstance.parentEntity = self.detail;
            }
            else {
                self.duracionViewInstanceInicializar(servicio);
            }
        }


        if (self.createOrEditHFechasConfiServicios.active) {

            if (self.createOrEditHFechasConfiServicios.existUnsavedData()) {
                self.message.confirm(self.confirm, 'Confirmación', (a) => {
                    if (a.value) {
                        self.createOrEditHFechasConfiServicios.clearUnsavedData();
                        internalCallback();
                    }
                    else {
                        $('#m_h_quick_sidebar_tabs_DetalleSYR')[0].click()
                    }
                });

                return;
            }
        }

        internalCallback();
    }

    duracionViewInstanceInicializar(servicio: HHorariosConfiDto): any {

        if (!this.duracionViewInstance.active) {
            this.duracionViewInstance.viewMode = this.viewMode;
            this.duracionViewInstance.filter = this.filter;
            this.duracionViewInstance.filter.CodHfecha = this.createOrEditHFechasConfiServicios.filter.CodHfecha
            this.duracionViewInstance.filter.CodTdia = this.createOrEditHFechasConfiServicios.filter.CodTdia
            this.duracionViewInstance.completedataBeforeShow(this.duracionViewInstance.detail, servicio)
            this.duracionViewInstance.parentEntity = this.detail;
            this.duracionViewInstance.filterChange.subscribe(e => this.onFilterChange(e));

            this.setActiveView(this.createOrEditHFechasConfiServicios, false);
            this.setActiveView(this.minutosPorSectorViewInstance, false);
            this.setActiveView(this.duracionViewInstance, true);
        }

    }

    setActiveView(view, active) {
        if (view) {
            view.active = active;
        }
    }

}
