import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit, Input, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;
import * as moment from 'moment';

import { DetailModalComponent, DetailEmbeddedComponent, IDetailComponent } from '../../../../../shared/manager/detail.component';


import { LazyLoadEventData } from '../../../../../shared/helpers/PrimengDatatableHelper';
import { FilterDTO, ResponseModel, PaginListResultDto, ViewMode } from '../../../../../shared/model/base.model';
import { BanderaService } from '../../bandera/bandera.service';
import { BanderaDto, BanderaFilter } from '../../model/bandera.model';
import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { RouterModule, Routes, Router } from '@angular/router';



import { NgForm } from '@angular/forms';
import { HFechasConfiService } from '../HFechasConfi.service';
import { HFechasConfiDto, PlaDistribucionDeCochesPorTipoDeDiaDto } from '../../model/HFechasConfi.model';

import { BanderaCartelDto, BanderaCartelFilter } from '../../model/banderacartel.model';
import { AsignacionBanderaHbasec } from './asignacion-bandera-hbasec.component';
import { BanderaCartelService } from '../../banderacartel/banderacartel.service';
import { DistribucionDeCochesAgregation } from '../distribucion/distribuciondecoches-agregation.component';
import { ImportarHorarioFechaComponent } from '../importador/importar-horariofecha.component';
import { MessageService } from '../../../../../shared/common/message.service';
import { PuntoDto, PuntoFilter } from '../../model/punto.model';
import { PuntoService } from '../../punto/punto.service';
import { MatGridTileHeaderCssMatStyler } from '@angular/material';






@Component({
    selector: 'createOrEdithorariofechaDtoModal',
    templateUrl: './create-or-edit-hfechasconfi-modal.component.html',

})
export class CreateOrEditHFechasConfiModalComponent extends DetailEmbeddedComponent<HFechasConfiDto> implements IDetailComponent, OnInit {


    EditEnabled: boolean = true;
    message: MessageService;
    PuntosDto: PuntoDto[];
    saving = false;
    hideDefaultSaveButton = false;
    shouldSkipValidations = false;
    @ViewChild('asignacionBanderaHbasec') asignacionBanderaHbasec: AsignacionBanderaHbasec;
    @ViewChild('distribuciondecochesAV') distribuciondecochesAV: DistribucionDeCochesAgregation;
    @ViewChild('importarhorariofecha', { read: ViewContainerRef }) importarhorariofecha: ViewContainerRef;


    getDescription(item: HFechasConfiDto): string {
        return moment(item.FechaDesde).format('DD/MM/YYYY');
    }


    constructor(
        injector: Injector,
        protected service: HFechasConfiService,
        protected banderaCartelService: BanderaCartelService,
        protected router: Router,
        private cfr: ComponentFactoryResolver,
        protected ptoservice: PuntoService

    ) {
        super(service, injector);
        this.detail = new HFechasConfiDto();
        this.icon = "flaticon-clock-2";
        this.title = "Horario";
    }

    onChangeScreen() {

        this.active = false;
        this.viewMode = ViewMode.Undefined;
        this.modalClose.emit({ detail: this.detail, event: "ChangeScreen" });
    }

    private importarHorarioInstance: ImportarHorarioFechaComponent;

    ngAfterViewInit(): void {
        mApp.initPortlets()



        this.distribuciondecochesAV.ImportarServicioEvent.subscribe(e => {
            var row = (e as PlaDistribucionDeCochesPorTipoDeDiaDto);

            if (!row.CodHfecha) {
                row.CodHfecha = this.detail.Id;
            }

            this.OnImportarServicio(row);

        });

        this.asignacionBanderaHbasec.reloadData.subscribe(e => {

            if (e) {
                this.asignacionBanderaHbasec.sectoresOriginal = null;
                this.show(this.detail.Id);
            }
        })



    }
    isLoadingCarteles: boolean;


    ngOnInit(): void {
        super.ngOnInit();
        this.modalClose.subscribe(e => {
            this.asignacionBanderaHbasec.sectoresOriginal = null;
        })
    }

    completedataBeforeShow(item: HFechasConfiDto): any {

        this.hideDefaultSaveButton = item.PlaEstadoHorarioFechaId === 2;
        this.shouldSkipValidations = item.PlaEstadoHorarioFechaId === 2;
        if (item.HBasec && item.HBasec.length > 0) {

            var filtro = new BanderaCartelFilter();
            filtro.CodHfecha = this.detail.Id;

            item.HBasec.forEach(e => {
                this.asignacionBanderaHbasec.selectedItemChangeBanderasColores({ value: e.CodBanderaColor }, e);
                this.asignacionBanderaHbasec.selectedItemChangeBanderaTup({ value: e.CodBanderaTup }, e);
            });


            this.isLoadingCarteles = true;
            this.banderaCartelService.requestAllByFilter(filtro)
                .finally(() => {
                    this.isLoadingCarteles = false;
                }).subscribe(e => {
                    if (e.DataObject.Items.length > 0) {
                        var volb = e.DataObject.Items[0];
                        item.HBasec.forEach(f => {
                            var cartel = volb.BolBanderasCartelDetalle.find(e => e.CodBan == f.CodBan)
                            if (cartel) {
                                f.NroSecuencia = cartel.NroSecuencia.toString();
                                f.TextoBandera = cartel.TextoBandera;
                                f.Movible = cartel.Movible;
                                f.ObsBandera = cartel.ObsBandera;
                            };
                        })
                    }
                });

            setTimeout(e => { $('.hBasec tbody tr')[0].click(); }, 100);


        }

        this.asignacionBanderaHbasec.sectores = [];

    }


    validateSave(): boolean {
        console.log(moment(this.detail.FechaDesde));
        console.log(moment(this.detail.FechaHasta));

        if (moment(this.detail.FechaDesde) > moment(this.detail.FechaHasta)) {
            this.message.error("La fecha Desde debe ser menor a la Fecha Hasta", "Error");
            return false;
        }

        return true;
    }

    save(form: NgForm) {
        this.saving = true;
        var puntofiltro = new PuntoFilter();
        var puntos: PuntoDto[] = [];
        var recorridos: string = "";
        console.log(puntofiltro);

        if(this.shouldSkipValidations) {
            this.service.GuardarBanderaPorSerctor(this.detail)
            .pipe(e => e.takeUntil(this.unsubscriber)) 
            .subscribe( e=> {
                this.saving = false;       
                this.message.success("Cambios Realizados");
                
            }, error => {
                this.saving = false;
            });
        }
        else {
            this.detail.HBasec.forEach(e => {
                puntofiltro.CodRecs.push(e.CodRec);
            })
    
            if (!puntofiltro.CodRecs || puntofiltro.CodRecs.length == 0) {
                super.save(form);
            }
            else {    
                this.ptoservice.requestAllByFilter(puntofiltro)
                    .subscribe(data => {
                        this.PuntosDto = data.DataObject.Items
                        var self = this
    
                        this.PuntosDto.map(punto => {
                            if (punto.PlaCoordenadaAnulada == true) {
                                puntos.push(punto)
                            }
                        })
                        if (this.detail.PlaEstadoHorarioFechaId == 2 && puntos.length > 0) {
                            puntos.map(punto => {
                                var nombre = this.detail.HBasec.find(f => f.CodRec == punto.RutaId).Recorido
                                if (!recorridos.includes(nombre)) {
                                    if (recorridos == "") {
                                        recorridos = nombre;
                                    }
                                    else {
                                        recorridos += " - " + nombre;
                                    }
                                }
                            })
    
                            var mensaje = `Estos recorridos 
                                        {{recorridos}}
                                        tienen coordenadas anuladas.
                                        Desea Continuar?`
    
                            self.message.confirmHtml("Estos recorridos <br /> " + recorridos + "<br /> tienen coordenadas anuladas. <br /> Desea continuar?", "Coordenadas anuladas.", (e) => {
                                if (e.value) {
                                    super.save(form)
                                } else {
                                    this.saving = false;
                                }
                            })
                        }
                        else {
                            super.save(form);
                        }
                    });
            }
        }
    }

    OnImportarServicio(row: PlaDistribucionDeCochesPorTipoDeDiaDto): any {

        if (row.IsNew) {
            this.notificationService.warn("Debe guardar la planificacion para poder importar.");
            return;
        }


        this.active = false;

        if (!this.importarHorarioInstance) {
            var factory = this.cfr.resolveComponentFactory(ImportarHorarioFechaComponent);
            const ref = this['importarhorariofecha'].createComponent(factory);
            ref.changeDetectorRef.detectChanges();

            this.importarHorarioInstance = (ref.instance as ImportarHorarioFechaComponent);

            this.importarHorarioInstance.modalClose.subscribe(e => {
                this.active = true;

            });

            this.importarHorarioInstance.modalSave.subscribe(e => {
                this.active = true;
                this.service.getById(this.detail.Id).subscribe(result => {
                    this.viewMode = ViewMode.Modify;
                    this.showDto(result.DataObject);
                });
            });

            this.importarHorarioInstance.detail = row;
            this.importarHorarioInstance.completedataBeforeShow(row);
            this.importarHorarioInstance.nroStep = 1;
        }
        else {


            this.importarHorarioInstance.ClearFile();




            this.importarHorarioInstance.viewMode = this.viewMode;
            this.importarHorarioInstance.detail = row;
            this.importarHorarioInstance.completedataBeforeShow(row);
            this.importarHorarioInstance.active = true;
        }
    }








}
