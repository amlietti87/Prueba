import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
declare let mApp: any;


import * as _ from 'lodash';

import { DetailEmbeddedComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { LineaService } from './linea.service';
import { LineaDto } from '../model/linea.model';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { RamalColorDto, RamalColorFilter } from '../model/ramalcolor.model';
import { RamalColorService } from '../ramalcolor/ramalcolor.service';
import { ResponseModel, PaginListResultDto, ViewMode } from '../../../../shared/model/base.model';
import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { CreateOrEditRamaColorModalComponent } from '../ramalcolor/create-or-edit-ramalcolor-modal.component';
import { PlaLineaService } from './pla-linea.service';


@Component({
    selector: 'createOrEditLineaDtoModal',
    templateUrl: './create-or-edit-linea-modal.component.html',

})
export class CreateOrEditLineaModalComponent extends DetailEmbeddedComponent<LineaDto> implements OnInit, IDetailComponent {


    ramalesList: RamalColorDto[];

    @Input() Sucursalid: number;
    @Input() Sucursal: string;

    getDescription(item: LineaDto): string {
        return item.DesLin;
    }


    allowRamaColorAdd: boolean = false;
    allowRamaColorDelete: boolean = false;
    allowRamaColorModify: boolean = false;

    @Input() viewMainTab: boolean = true;
    @Input() viewChildTab: boolean = true;
    tabMode(m: boolean, c: boolean): void {
        this.viewChildTab = c
        this.viewMainTab = m


        if (this.viewChildTab && !this.viewMainTab) {
            $('#m_heder_portlet_tab_Ramal').click()
        }


        if (this.viewMainTab) {
            $('#m_heder_portlet_tab_Linea').click()
        }
    }


    constructor(
        injector: Injector,
        protected service: PlaLineaService,
        protected serviceRamalColor: RamalColorService
    ) {
        super(service, injector);
        this.detail = new LineaDto();

        this.icon = "fa fa-bus"
        this.title = "Línea";

        var el = injector.get(ElementRef);
        var selector = el.nativeElement.tagName;


    }

    @ViewChild('createOrEditramalcolor') CreateOrEditRamaColorModal: CreateOrEditRamaColorModalComponent;
    @ViewChild('dataTableRamales') dataTableRamales: DataTable;
    @ViewChild('paginatorRamales') paginatorRamales: Paginator;


    ngAfterViewInit(): void {
        mApp.initPortlets()

        this.initFirtTab();

    }

    allowSearchRamal: Boolean = false;

    setSucursal(Sucursalid: number, Sucursal: string): void {
        this.Sucursalid = Sucursalid;
        this.Sucursal = Sucursal;

    }

    initFirtTab(): void {

        $('#m_heder_portlet_tab_Linea').click()
    }



    completedataBeforeShow(item: LineaDto): any {

        this.initFirtTab();

        if (this.viewMode == ViewMode.Add) {

            item.SucursalId = this.Sucursalid;
            this.ramalesList = [];

            setTimeout(() => {
                item.Activo = true;

            }, 0);
        }
        else {
            this.ramalesList = [];
        }
    }


    CloseChild(): void {
        this.CreateOrEditRamaColorModal.CloseChild();
        this.CreateOrEditRamaColorModal.close();
    }


    ngOnInit() {

        super.ngOnInit();


        this.allowRamaColorAdd = this.permission.isGranted('Planificacion.RamaColor.Agregar');
        this.allowRamaColorDelete = this.permission.isGranted('Planificacion.RamaColor.Eliminar');
        this.allowRamaColorModify = this.permission.isGranted('Planificacion.RamaColor.Modificar');



        this.CreateOrEditRamaColorModal.modalClose.subscribe(e => {
            this.active = true;
            this.CreateOrEditRamaColorModal.paginatorBanderas.changePage(0);
            this.CreateOrEditRamaColorModal.dataTableBanderas.page = 0;
            //this.CreateOrEditRamaColorModal.createOrEditBanderaDtoModal.paginatorRutas.changePage(0);
            //this.CreateOrEditRamaColorModal.createOrEditBanderaDtoModal.dataTableRutas.page = 0; 
        });

        this.CreateOrEditRamaColorModal.modalSave.subscribe(e => {
            this.active = true;
            this.onSearchRamales();
        });
    }


    onRamalColorClick() {
        this.allowSearchRamal = true;
        this.onSearchRamales();
    }



    onSearchRamales(event?: LazyLoadEventData) {

        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages

        if (!this.allowSearchRamal) {
            return;
        }
        if (!this.detail.Id) {
            return;
        }
        this.ramalesList = [];

        var filterRamal = new RamalColorFilter();

        this.isTableLoading = true;
        this.primengDatatableHelper.showLoadingIndicator();

        filterRamal.LineaId = this.detail.Id;

        filterRamal.Sort = this.primengDatatableHelper.getSorting(this.dataTableRamales);
        filterRamal.Page = this.primengDatatableHelper.getPageIndex(this.paginatorRamales, event);
        filterRamal.PageSize = this.primengDatatableHelper.getPageSize(this.paginatorRamales, event);

        this.serviceRamalColor.search(filterRamal)
            .finally(() => {
                this.isTableLoading = false;
                //this.datatable.reload()
            })
            .subscribe((result: ResponseModel<PaginListResultDto<RamalColorDto>>) => {

                this.ramalesList = result.DataObject.Items;
                this.primengDatatableHelper.records = result.DataObject.Items
                this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
                this.primengDatatableHelper.hideLoadingIndicator()
            });

    }


    onDeleteRamales(row: RamalColorDto) {

        var mje = 'Si elimina el Ramal y se borraran las banderas asociadas. ¿ Desea continuar?';

        this.serviceRamalColor.TieneMapasEnBorrador(row.Id).subscribe(e => {
            if (e.DataObject) {
                mje = 'Existen mapas en Borrador. Si elimina el Ramal y se borraran las banderas asociadas. ¿Está seguro de que desea eliminar el registro ? ';
            }

            this.message.confirm(mje, "Eliminar Bandera", (a) => {

                //this.isshowalgo = !this.isshowalgo;
                if (a.value) {
                    this.serviceRamalColor.delete(row.Id)
                        .subscribe(() => {
                            this.onSearchRamales();
                            this.notify.success(this.l('Registro eliminado correctamente'));
                        });
                }

            });


        })









    }


    onEditRamales(ruta: RamalColorDto): void {
        this.active = false;
        this.CreateOrEditRamaColorModal.setSucursal(this.Sucursalid, this.Sucursal, this.detail.PlaTipoLineaId);
        this.CreateOrEditRamaColorModal.show(ruta.Id);
    }

    onEditBanderasEnRamal(item: RamalColorDto) {
        if (!this.allowRamaColorModify) {
            return;
        }
        this.active = false;
        this.CreateOrEditRamaColorModal.setSucursal(this.Sucursalid, this.Sucursal, this.detail.PlaTipoLineaId);
        this.CreateOrEditRamaColorModal.showInBanderaMode(item);
    }

    onCreateRamal(): void {
        this.CreateOrEditRamaColorModal.setSucursal(this.Sucursalid, this.Sucursal, this.detail.PlaTipoLineaId);
        this.active = false;
        var r = new RamalColorDto();
        r.LineaId = this.detail.Id;
        r.NombreLinea = this.detail.DesLin;
        r.NombreUN = this.Sucursal;
        r.PlaTipoLineaId = this.detail.PlaTipoLineaId;

        r.ColorTupid = 0;
        this.CreateOrEditRamaColorModal.showNew(r);
    }

    show(id: any) {
        this.tabMode(false, false);
        super.show(id);
    }


    showInRamalMode(linea: LineaDto): any {
        this.viewMode = ViewMode.Modify;
        this.showDto(linea);
        this.onRamalColorClick();
        this.tabMode(false, true);
    }






}
