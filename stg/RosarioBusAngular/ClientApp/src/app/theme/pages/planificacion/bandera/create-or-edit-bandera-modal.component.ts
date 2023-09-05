import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit, Input, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { ModalDirective, BsModalRef, BsModalService } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
declare let mApp: any;


import * as _ from 'lodash';

import { DetailEmbeddedComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { BanderaService } from './bandera.service';
import { BanderaDto, PlaCodigoSubeBanderaDto } from '../model/bandera.model';

import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { RouterModule, Routes, Router } from '@angular/router';

import { CreateOrEditRutaModalComponent } from '../ruta/create-or-edit-ruta-modal.component';
import { SelectSectoresHorariosModalComponent } from '../bandera/select-sectores-horarios-modal.component';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { RutaFilter, RutaDto } from '../model/ruta.model';
import { RutaService } from '../ruta/ruta.service';
import { ResponseModel, PaginListResultDto, ViewMode, StatusResponse } from '../../../../shared/model/base.model';
import { ESTADOS_RUTAS } from '../../../../shared/constants/constants';
import { SectorViewComponent } from '../sector/sector-view.component';
import { elementAt } from 'rxjs/operators/elementAt';
import { PuntoService } from '../punto/punto.service';
import { PuntoFilter } from '../model/punto.model';
import { SentidoBanderaComboComponent } from '../shared/sentidoBandera-combo.component';
import { EmpresaDto } from '../model/empresa.model';
import { EmpresaService } from '../empresa/empresa.service';
import { EstadoRutaDto } from '../model/estadoruta.model';
import { NgForm } from '@angular/forms';
import { Helpers } from '../../../../helpers';
import { PlaSentidoBanderaSubeDto } from '../model/banderacartel.model';
import { PlaSentidoBanderaSubeService } from '../PlaSentidoBanderaSube/PlaSentidoBanderaSube.service';
import { FileService } from '../../../../shared/common/file.service';
import { environment } from '../../../../../environments/environment';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { ExportarKmlModalComponent } from '../exportarkml/exportarkml-modal.component';



@Component({
    selector: 'createOrEditBanderaDtoModal',
    templateUrl: './create-or-edit-bandera-modal.component.html',

})
export class CreateOrEditBanderaModalComponent extends DetailEmbeddedComponent<BanderaDto> implements OnInit, IDetailComponent {

    //material dialog
    protected popupHeight: string = '';
    protected popupWidth: string = '65%';
    protected dialog: MatDialog;

    getDescription(item: BanderaDto): string {
        return item.Nombre;
    }

    Empresas: EmpresaDto[];
    SentidosBandera: PlaSentidoBanderaSubeDto[];

    @ViewChild('createOrEditRutaDtoModal') createOrEditRutaModalComponent: CreateOrEditRutaModalComponent;
    @ViewChild('selectSectoresHorarios') selectSectoresHorarios: SelectSectoresHorariosModalComponent;

    @ViewChild('dataTableRutas') dataTableRutas: DataTable;
    @ViewChild('paginatorRutas') paginatorRutas: Paginator;
    @ViewChild('sentidoBanderaCombo') sentidoBanderaCombo: SentidoBanderaComboComponent;

    @Input() Sucursalid: number;
    @Input() Sucursal: string;


    rutasList: RutaDto[];


    @Input() viewMainTab: boolean = true;
    @Input() viewChildTab: boolean = true;
    tabMode(m: boolean, c: boolean): void {
        this.viewChildTab = c
        this.viewMainTab = m


        if (this.viewChildTab && !this.viewMainTab) {
            $('#m_portlet_heder_tab_Recororidos').click()
        }


        if (this.viewMainTab) {
            $('#m_portlet_heder_tab_generalbandera').click()
        }
    }


    close(): void {
        $('#detailFormBandera').removeClass("smallsize");
        super.close();

    }


    showNew(item: BanderaDto) {
        if (this.detailForm) {
            //this.detailForm.reset();
            //this.detailForm.resetForm();
            this.detailForm.form.markAsPristine();
            this.detailForm.form.markAsUntouched();
            this.detailForm.form.updateValueAndValidity();
        }

        this.viewMode = ViewMode.Add;
        this.showDto(item)
    }


    constructor(
        injector: Injector,
        protected service: BanderaService,
        protected serviceRuta: RutaService,
        private modalService: BsModalService,
        private componentFactoryResolver: ComponentFactoryResolver,
        private viewContainerRef: ViewContainerRef,
        protected router: Router,
        protected puntoService: PuntoService,
        protected empresaservice: EmpresaService,
        private fileService: FileService,
        protected sentidoBanderaSubeServ: PlaSentidoBanderaSubeService,
    ) {
        super(service, injector);
        this.detail = new BanderaDto();
        this.icon = "fa fa-flag-o";
        this.title = "Bandera";
        this.dialog = injector.get(MatDialog);
    }


    completedataBeforeSave(item: BanderaDto): any {
        var s = this.sentidoBanderaCombo.items.find(en => en.Id == item.SentidoBanderaId);
        if (s)
            item.Sentido = s.Descripcion;
        else
            item.Sentido = "";
        if (this.viewMode == ViewMode.Add && !item.SucursalId) {
            item.SucursalId = this.Sucursalid;

        }
    }

    setSucursal(Sucursalid: number, Sucursal: string): void {
        this.Sucursalid = Sucursalid;
        this.Sucursal = Sucursal;
        //this.detail.Sucursal = Sucursal;
        //this.detail.SucursalId = Sucursalid;
    }


    ngAfterViewInit(): void {
        mApp.initPortlets()
    }


    initFirtTab(): void {
        $("#m_portlet_heder_tab_generalbandera").first().click();

    }

    ngOnInit() {
        super.ngOnInit();

        this.empresaservice.requestAllByFilter().subscribe(result => {
            this.Empresas = result.DataObject.Items;
        });
        this.sentidoBanderaSubeServ.requestAllByFilter().subscribe(r => {
            this.SentidosBandera = r.DataObject.Items;
        })



        this.createOrEditRutaModalComponent.modalClose.subscribe(e => {
            this.active = true;
        });

        this.createOrEditRutaModalComponent.modalSave.subscribe(e => {
            // this.active = true;
            this.onSearchRutas();
        });

        this.sentidoBanderaCombo.selectedItemChange.subscribe(e => {
            this.selectedSentidoChange(e);
        });
    }




    onEditRuta(ruta: RutaDto): void {
        this.active = false;

        this.createOrEditRutaModalComponent.show(ruta.Id);
    }


    onCreateRuta(): void {
        this.active = false;
        var r = new RutaDto();
        r.EstadoRutaId = ESTADOS_RUTAS.Borrador;
        r.BanderaId = this.detail.Id;
        this.createOrEditRutaModalComponent.showNew(r);
    }

    allowSearchRutas: Boolean = false;

    onRutasClick() {
        this.allowSearchRutas = true;
        this.onSearchRutas();
    }

    completedataBeforeShow(item: BanderaDto): any {

        $('#detailFormBandera').addClass("smallsize");
        this.initFirtTab();

        this.rutasList = [];

        if (this.viewMode == ViewMode.Add) {
            this.createOrEditRutaModalComponent.active = false;

            setTimeout(() => {
                item.Activo = true;

            }, 0);
        }
        else {
            this.recuperarCartelBandera();
        }

    }

    private recuperarCartelBandera(): void {

        var txt: string;
        this.service.recuperarCartelBandera(this.detail.Id).subscribe(e => {
            this.detail.Cartel = e.DataObject;
        });
    }

    CloseChild(): void {
        this.createOrEditRutaModalComponent.CloseChild();
        this.createOrEditRutaModalComponent.close();
    }

    onSearchRutas(event?: LazyLoadEventData) {


        if (this.allowSearchRutas == false) {
            return;
        }

        var rutaFilter = new RutaFilter();

        this.isTableLoading = true;
        this.primengDatatableHelper.showLoadingIndicator();

        rutaFilter.BanderaId = this.detail.Id;

        rutaFilter.Sort = this.primengDatatableHelper.getSorting(this.dataTableRutas);
        rutaFilter.Page = this.primengDatatableHelper.getPageIndex(this.paginatorRutas, event);
        rutaFilter.PageSize = this.primengDatatableHelper.getPageSize(this.paginatorRutas, event);

        this.serviceRuta.search(rutaFilter)
            .finally(() => {
                this.isTableLoading = false;
                //this.datatable.reload()
            })
            .subscribe((result: ResponseModel<PaginListResultDto<RutaDto>>) => {
                this.rutasList = result.DataObject.Items;
                this.primengDatatableHelper.records = result.DataObject.Items
                this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
                this.primengDatatableHelper.hideLoadingIndicator()
            });

    }

    aprobarRecorrido(row: RutaDto) {
        Helpers.blockUi('Validando recorrido');
        this.serviceRuta.validateAprobarRuta(row.Id)
            .finally(() => { Helpers.unBlockUi(); })
            .subscribe((e) => {
                setTimeout(() => {
                    if (e.Status == StatusResponse.Ok) {
                        this.aprobarRecorridoConfirmado(row);
                    }
                    else {

                        this.message.confirm(e.DataObject, "", (a) => {
                            if (a.value) {
                                this.aprobarRecorridoConfirmado(row);
                            }
                        });
                    }
                }, 100);
            });
    }


    aprobarRecorridoConfirmado(row: RutaDto) {
        Helpers.blockUi('Aprobando recorrido');
        this.serviceRuta.aprobarRuta(row.Id)
            .finally(() => { Helpers.unBlockUi(); })
            .subscribe(() => {
                this.onSearchRutas();
                this.notify.success('Aprobado con éxito');
            });
    }


    copiarRecorrido(row: RutaDto) {
        this.active = false;
        var r = new RutaDto();
        r.BanderaId = this.detail.Id;
        r.CopyFromRutaId = row.Id;
        this.createOrEditRutaModalComponent.showNew(r);
        //this.createOrEditRutaModalComponent.GetInstructions();
    }

    onDeleteRecorido(row: RutaDto) {
        this.message.confirm('Si elimina la Ruta asociada. ¿ Desea continuar?', "Eliminar Ruta", (a) => {

            if (a.value) {
                this.serviceRuta.delete(row.Id)
                    .subscribe(() => {
                        this.onSearchRutas();
                        this.notify.success('Eliminado con éxito');
                    });
            }

        });


    }

    show(id: any) {
        this.tabMode(false, false);
        super.show(id);
    }


    showInRutaMode(item: BanderaDto, RutaId?: any): any {
        this.viewMode = ViewMode.Modify;
        this.showDto(item);
        this.tabMode(false, true);
        this.onRutasClick();
    }

    ShowViewSectores(row: RutaDto) {
        this.router.navigate(['/planificacion/sectorView', { RutaId: row.Id }]);
    }

    ExportarCoordenadas(row: RutaDto) {
        var filter = new PuntoFilter();
        filter.RutaId = row.Id;
        this.puntoService.GetPuntosReporte(filter);
    }

    ExpotarKml(row: RutaDto) {

        var filterpunto = new PuntoFilter();
        filterpunto.CodRec = row.Id;
        filterpunto.CambioSectoresMapa = false;
        filterpunto.ParadasMapa = false;

        const dialogConfig = new MatDialogConfig<PuntoFilter>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        dialogConfig.data = filterpunto;

        let dialogRef = this.dialog.open(ExportarKmlModalComponent, dialogConfig);


        dialogRef.afterClosed().subscribe(
            data => {
                if (data == true) {
                    this.fileService.dowloadAuthenticatedByPost(environment.identityUrl + '/Puntos/GetKML', filterpunto)
                }
            }
        );


    }

    ExportarDemoras(row: RutaDto): void {
        this.selectSectoresHorarios.show(row);
    }

    selectedSentidoChange(e: any): void {
        var item = this.sentidoBanderaCombo.items.find(en => en.Id == e);
        if (item)
            this.detail.Sentido = item.Descripcion;
        else
            this.detail.Sentido = "";

    }


    onEmpresaChange(newValue, row): void {
        row.EmpresaNombre = this.Empresas.find(x => x.Id == newValue).DesEmpr;

        setTimeout(e => {
            $("#PlaCodigoSubeBanderaTable td")[1].click();
        }, 100);
    }


    onSentidoBanderaChange(newValue, row): void {
        row.SentidoBanderaSubeNombre = this.SentidosBandera.find(x => x.Id == newValue).Descripcion;

    }


    addNewBanderaSube() {

        if (!this.detail.PlaCodigoSubeBandera) {
            this.detail.PlaCodigoSubeBandera = [];
        }

        let lista = [...this.detail.PlaCodigoSubeBandera];

        lista.push(this.getNewBanderaSubeItem(null, lista.length * -1));

        this.detail.PlaCodigoSubeBandera = lista;

        setTimeout(e => {
            try {
                $("#PlaCodigoSubeBanderaTable  tr:last td:first")[0].click();
            } catch (e) {

            }
        }, 100)



    }


    save(form: NgForm): void {
        if (this.validateBeforeSave()) {
            super.save(form);
        }
    }

    validateBeforeSave(): boolean {
        if (this.detail.PlaCodigoSubeBandera && this.detail.PlaCodigoSubeBandera.length > 0) {

            this.detail.PlaCodigoSubeBandera.forEach((e, i) => {
                if (!(e.CodBan && e.CodigoSube && e.CodBan)) {
                    this.notificationService.warn("Completar datos Sube", "");

                    setTimeout(e => {
                        try {
                            $($("#PlaCodigoSubeBanderaTable tbody  tr")[i]).find("td")[0].click();
                        } catch (e) {

                        }
                    }, 100)

                    return false;
                }
            });

            this.detail.PlaCodigoSubeBandera.forEach((e, i) => {
                if (e.SentidoBanderaSubeId == null) {
                    this.notificationService.warn("Completar Sentido");
                    return false;
                }
            });

        }
        return true;
    }


    getNewBanderaSubeItem(item: PlaCodigoSubeBanderaDto, id: number): PlaCodigoSubeBanderaDto {
        var item = new PlaCodigoSubeBanderaDto(item)
        item.CodBan = this.detail.Id;
        item.Id = id;

        return item;
    }

    onBanderaSubeDelete(row: PlaCodigoSubeBanderaDto): void {
        var index = this.detail.PlaCodigoSubeBandera.findIndex(x => x.Id == row.Id);

        if (index >= 0) {
            let lista = [...this.detail.PlaCodigoSubeBandera];
            lista.splice(index, 1);
            this.detail.PlaCodigoSubeBandera = lista;
        }
    }
}
