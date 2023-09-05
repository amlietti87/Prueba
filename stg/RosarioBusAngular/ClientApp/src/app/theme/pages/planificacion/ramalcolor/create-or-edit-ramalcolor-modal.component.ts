import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit, Input, ComponentFactoryResolver } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailModalComponent, DetailEmbeddedComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { RamalColorDto, RamalSubeDto } from '../model/ramalcolor.model';
import { RamalColorService } from './ramalcolor.service';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { FilterDTO, ResponseModel, PaginListResultDto, ViewMode } from '../../../../shared/model/base.model';
import { BanderaService } from '../bandera/bandera.service';
import { BanderaDto, BanderaFilter } from '../model/bandera.model';
import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { RouterModule, Routes, Router } from '@angular/router';
import { CreateOrEditBanderaModalComponent } from '../bandera/create-or-edit-bandera-modal.component';
import { EmpresaDto } from '../model/empresa.model';
import { EmpresaService } from '../empresa/empresa.service';
import { NgForm } from '@angular/forms';
import { CreateOrEditEmpresaModalComponent } from '../empresa/create-or-edit-empresa-modal.component';



@Component({
    selector: 'createOrEditramalcolorDtoModal',
    templateUrl: './create-or-edit-ramalcolor-modal.component.html',

})
export class CreateOrEditRamaColorModalComponent extends DetailEmbeddedComponent<RamalColorDto> implements IDetailComponent, OnInit {
    banderaList: BanderaDto[];
    Empresas: EmpresaDto[];
    NombreLinea: string = "";
    @ViewChild('dataTableBanderas') dataTableBanderas: DataTable;
    @ViewChild('paginatorBanderas') paginatorBanderas: Paginator;
    @Input() Sucursalid: number;
    @Input() Sucursal: string;
    @Input() PlaTipoLineaId: number;

    @ViewChild('createOrEditBanderaDtoModal') createOrEditBanderaDtoModal: CreateOrEditBanderaModalComponent;
    @ViewChild('createOrEditEmpresaDtoModal') createOrEditEmpresaDtoModal: CreateOrEditEmpresaModalComponent;

    allowBanderaAdd: boolean = false;
    allowBanderaDelete: boolean = false;
    allowBanderaModify: boolean = false;

    @Input() viewMainTab: boolean = true;
    @Input() viewChildTab: boolean = true;
    tabMode(m: boolean, c: boolean): void {
        this.viewChildTab = c
        this.viewMainTab = m


        if (this.viewChildTab && !this.viewMainTab) {

            $('#m_heder_portlet_tab_Banderas').click()
        }


        if (this.viewMainTab) {
            $('#m_heder_portlet_tab_RC').click()
        }
    }


    getDescription(item: RamalColorDto): string {
        return item.Nombre;
    }


    constructor(injector: Injector,
        protected service: RamalColorService,
        protected empresaservice: EmpresaService,
        protected serviceBandera: BanderaService,
        protected router: Router,
        private cfr: ComponentFactoryResolver) {

        super(service, injector);
        this.detail = new RamalColorDto();
        this.icon = "fa fa-sitemap"
        this.title = "Ramal/Color";

    }


    setSucursal(Sucursalid: number, Sucursal: string, PlaTipoLineaId: number): void {
        this.Sucursalid = Sucursalid;
        this.Sucursal = Sucursal;
        this.PlaTipoLineaId = PlaTipoLineaId;
    }


    ngAfterViewInit(): void {
        mApp.initPortlets()
    }


    initFirtTab(): void {
        $('#m_heder_portlet_tab_RC').click();
    }


    completedataBeforeShow(item: RamalColorDto): any {
        this.initFirtTab();
        this.banderaList = [];
        super.completedataBeforeShow(item);

        this.NombreLinea = "";
        this.NombreLinea = item.NombreLinea;
        if (this.viewMode == ViewMode.Add) {

            this.createOrEditBanderaDtoModal.active = false;
            setTimeout(() => {
                item.Activo = true;

            }, 0);


            setTimeout(() => {
                item.ColorTupid = null;

            }, 100);



        }


    }


    CloseChild(): void {
        this.createOrEditBanderaDtoModal.CloseChild();
        this.createOrEditBanderaDtoModal.close();
    }


    private SearchEmpresas(): void {
        this.empresaservice.requestAllByFilter().subscribe(result => {
            this.Empresas = result.DataObject.Items;
        });
    }


    ngOnInit() {

        super.ngOnInit();

        this.SearchEmpresas();

        this.allowBanderaAdd = this.permission.isGranted('Planificacion.Bandera.Agregar');
        this.allowBanderaDelete = this.permission.isGranted('Planificacion.Bandera.Eliminar');
        this.allowBanderaModify = this.permission.isGranted('Planificacion.Bandera.Modificar');

        this.createOrEditBanderaDtoModal.modalClose.subscribe(e => {
            this.active = true;

        });

        this.createOrEditBanderaDtoModal.modalSave.subscribe(e => {
            this.active = true;
            this.onSearchBanderas();
        });



        this.createOrEditEmpresaDtoModal.modalClose.subscribe(e => {

        });

        this.createOrEditEmpresaDtoModal.modalSave.subscribe(e => {

            this.SearchEmpresas();
        });


    }



    allowSearchBanderas: Boolean = false;

    onBanderaClick() {
        this.allowSearchBanderas = true;
        this.onSearchBanderas();
    }

    onDeleteBandera(row: BanderaDto) {


        //var aa = this.getNewItem(item);
        //var stringentity = aa.getDescription();

        this.message.confirm('Si elimina la bandera se borraran las Rutas asociadas. ¿Desea continuar?', "Eliminar Bandera", (a) => {

            //this.isshowalgo = !this.isshowalgo;
            if (a.value) {
                this.serviceBandera.delete(row.Id)
                    .subscribe(() => {
                        this.onSearchBanderas();
                        this.notify.success(this.l('Registro eliminado correctamente'));
                    });
            }

        });


    }


    onSearchBanderas(event?: LazyLoadEventData) {

        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages

        if (!this.allowSearchBanderas) {
            return;
        }
        if (!this.detail.Id) {
            return;
        }


        var filterBandera = new BanderaFilter();

        this.isTableLoading = true;
        this.primengDatatableHelper.showLoadingIndicator();

        filterBandera.RamaColorId = this.detail.Id;

        filterBandera.Sort = this.primengDatatableHelper.getSorting(this.dataTableBanderas);
        filterBandera.Page = this.primengDatatableHelper.getPageIndex(this.paginatorBanderas, event);
        filterBandera.PageSize = this.primengDatatableHelper.getPageSize(this.paginatorBanderas, event);

        this.serviceBandera.search(filterBandera)
            .finally(() => {
                this.isTableLoading = false;
                //this.datatable.reload()
            })
            .subscribe((result: ResponseModel<PaginListResultDto<BanderaDto>>) => {
                this.banderaList = result.DataObject.Items;
                this.primengDatatableHelper.records = result.DataObject.Items
                this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
                this.primengDatatableHelper.hideLoadingIndicator()
            });

    }


    onEditBanderas(item: BanderaDto): void {
        this.active = false;
        this.createOrEditBanderaDtoModal.setSucursal(this.Sucursalid, this.Sucursal);
        this.createOrEditBanderaDtoModal.show(item.Id);
    }
    onEditRutasEnBandera(item: BanderaDto) {
        if (!this.allowBanderaModify) {
            return;
        }
        this.active = false;
        this.createOrEditBanderaDtoModal.setSucursal(this.Sucursalid, this.Sucursal);
        this.createOrEditBanderaDtoModal.showInRutaMode(item);
    }



    onCreateBanderas(): void {
        this.active = false;
        var item = new BanderaDto();
        item.RamalColorId = this.detail.Id;

        item.RamalColorNombre = this.detail.Nombre;
        item.LineaNombre = this.detail.NombreLinea;
        item.TipoBanderaId = 1;
        item.tbn = "1"
        item.SucursalId = this.Sucursalid;
        this.createOrEditBanderaDtoModal.viewMode = ViewMode.Add;
        this.createOrEditBanderaDtoModal.showNew(item);
    }

    show(id: any) {
        this.tabMode(false, false);
        super.show(id);
    }



    showInBanderaMode(item: RamalColorDto): any {
        this.active = true;
        this.viewMode = ViewMode.Modify;
        this.showDto(item);
        this.tabMode(false, true);
        this.onBanderaClick();
    }


    onEmpresaChange(newValue, row): void {
        row.EmpresaNombre = this.Empresas.find(x => x.Id == newValue).Nombre;
    }


    //addNewRamalSube() {
    //    let lista = [...this.detail.RamalSube];

    //    lista.push(this.getNewRamalSubeItem(null, lista.length * -1));

    //    this.detail.RamalSube = lista;
    //}

    //getNewRamalSubeItem(item: RamalSubeDto, id: number): RamalSubeDto {
    //    var item = new RamalSubeDto(item)
    //    item.RamalColorId = this.detail.Id;
    //    item.Id = id;

    //    return item;
    //}

    //onRamalSubeDelete(row: RamalSubeDto): void {
    //    var index = this.detail.RamalSube.findIndex(x => x.Id == row.Id);

    //    if (index >= 0) {
    //        let lista = [...this.detail.RamalSube];
    //        lista.splice(index, 1);
    //        this.detail.RamalSube = lista;
    //    }
    //}


    save(form: NgForm) {

        var result = this.checkUniqueCondition();
        if (result) {
            this.notificationService.warn("Empresas duplicadas", result.EmpresaNombre);
            return;
        }

        super.save(form);
    }

    checkUniqueCondition(): any {


        //if (this.detail.RamalSube) {
        //    var sorted_arr = this.detail.RamalSube.sort((a, b) => { return b['EmpresaId'] - a['EmpresaId'] });


        //    for (var i = 0; i < sorted_arr.length - 1; i++) {
        //        if (sorted_arr[i + 1]['EmpresaId'] == sorted_arr[i]['EmpresaId']) {
        //            return sorted_arr[i];
        //        }
        //    }
        //}

        return false;
    }


    crearEmpresa() {
        this.createOrEditEmpresaDtoModal.showNew(new EmpresaDto());
    }


}
