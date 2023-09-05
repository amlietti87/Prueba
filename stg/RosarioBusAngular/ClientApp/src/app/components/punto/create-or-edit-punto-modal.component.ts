import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, AfterViewInit, OnInit, Input } from '@angular/core';

import * as _ from 'lodash';
declare let mApp: any;
declare let mOffcanvas: any;

import { NgForm } from '@angular/forms';

import { MatDialogConfig, MatDialog } from '@angular/material';

import { DetailEmbeddedComponent } from '../../shared/manager/detail.component';
import { CreateOrEditCoordenadaModalComponent } from '../../theme/pages/planificacion/coordenadas/create-or-edit-coordenadas-modal.component';

import { PuntoDto } from '../../theme/pages/planificacion/model/punto.model';
import { TipoParadaDto } from '../../theme/pages/planificacion/model/tipoParada.model';
import { SectorDto } from '../../theme/pages/planificacion/model/sector.model';
import { ItemDto, ViewMode } from '../../shared/model/base.model';

import { PuntoService } from '../../theme/pages/planificacion/punto/punto.service';
import { TipoParadaService } from '../../theme/pages/planificacion/tipoParada/tipoparada.service';
import { CoordenadasService } from '../../theme/pages/planificacion/coordenadas/coordenadas.service';
import { SectoresTarifariosService } from '../../theme/pages/planificacion/sectoresTarifarios/sectoresTarifarios.service';

import { CoordenadasFilter, CoordenadasDto } from '../../theme/pages/planificacion/model/coordenadas.model';
import { SectoresTarifariosDto, SectoresTarifariosFilter } from '../../theme/pages/planificacion/model/sectoresTarifarios.model';
import { CoordenadasComponent } from '../../theme/pages/planificacion/coordenadas/coordenadas.component';
import { ParadaComponent } from '../../theme/pages/planificacion/parada/parada.component';
import { CreateOrEditParadaModalComponent } from '../../theme/pages/planificacion/parada/create-or-edit-parada-modal.component';
import { ParadaDto } from '../../theme/pages/planificacion/model/parada.model';

@Component({
    selector: 'createOrEditPuntoDtoModal',
    templateUrl: './create-or-edit-punto-modal.component.html',
    styleUrls: ['./create-or-edit-punto-modal.component.css']

})
export class CreateOrEditPuntoModalComponent extends DetailEmbeddedComponent<PuntoDto> implements AfterViewInit, OnInit {


    TiposParadas: TipoParadaDto[];
    SectoresTarifarios: SectoresTarifariosDto[];

    sectoresdto: SectorDto[];
    isSectormode: boolean = false

    puntoOriginal: PuntoDto;
    estadoEnabled: boolean;

    public dialog: MatDialog;

    constructor(
        injector: Injector,
        protected service: PuntoService,
        protected _servicetp: TipoParadaService,
        protected _coordenadasService: CoordenadasService,
        protected _sectoresTarifariosService: SectoresTarifariosService,
    ) {
        super(service, injector);
        this.dialog = injector.get(MatDialog);
        //this.active = true;
        this.active = false;
        this.title = "Punto"
        this.detail = new PuntoDto();
    }



    //GetEditCoordenadaComponent() {
    //    if (!this.detailElement) { 

    //        var factory = this.cfr.resolveComponentFactory(CreateOrEditCoordenadaModalComponent);
    //        const ref = this['CreateOrEditCoordenadaModal'].createComponent(factory);
    //        ref.changeDetectorRef.detectChanges();
    //        this.detailElement = ref.instance;

    //        this.detailElement.modalClose.subscribe(e => {
    //            this.active = true;
    //        });

    //        this.detailElement.modalSave.subscribe(e => {
    //            this.active = true;
    //            this.Search(null);
    //        });
    //    }
    //    return this.detailElement;
    //}



    ngOnInit(): void {

        super.ngOnInit();
        this._servicetp.requestAllByFilter().subscribe(result => {

            this.TiposParadas = result.DataObject.Items;
        });

        // this._sectoresTarifariosService.requestAllByFilter().subscribe(result => {

        //     this.SectoresTarifarios = result.DataObject.Items;
        // });
    }


    topbarAsideObj: any

    getDescription(item: PuntoDto): string {


        return "Punto";
    }

    showDto(item: PuntoDto) {
        this.isSectormode = false;

        if (item.EsPuntoInicio) {
            item.EsCambioSectorTarifario = true;
        }
        super.showDto(item);
        this.topbarAsideObj.show();
        $('#m_h_quick_sidebar_tabs_Puntos').click();
    }


    showSectores(items: SectorDto[]) {
        this.isSectormode = true;
        this.sectoresdto = items;

        this.topbarAsideObj.show();

        $('#m_h_quick_sidebar_tabs_Sector').click();
    }


    close(): void {
        super.close();
        this.topbarAsideObj.hide();
    }
    ngAfterViewInit(): void {
        mApp.initPortlets()
        this.initpuntosDetailsidebar();
    }


    save(form: NgForm): void {
        super.save(form);
    }

    @Output() modalSaveRuta: EventEmitter<any> = new EventEmitter<any>();
    @Output() ApplyPunto: EventEmitter<any> = new EventEmitter<PuntoDto>();
    @Output() ApplySectores: EventEmitter<SectorDto[]> = new EventEmitter<SectorDto[]>();

    saveRuta(form: NgForm): void {
        this.modalSaveRuta.emit({});
    }

    applyPunto(): void {


        //if (this.detail.CodigoNombre && this.detail.Abreviacion && (this.puntoOriginal.CodigoNombre != this.detail.CodigoNombre
        //    || this.puntoOriginal.Abreviacion != this.detail.Abreviacion)) {

        //    var filter = new CoordenadasFilter();
        //    filter.CodigoNombre = this.detail.CodigoNombre;
        //    filter.Abreviacion = this.detail.Abreviacion;

        //    this._coordenadasService.requestAllByFilter(filter)
        //        .subscribe(result => {
        //            console.log(result);


        //            if (result.DataObject.Items && result.DataObject.Items.length > 0) {
        //                var coordenadadto = result.DataObject.Items[0];

        //                if (this.detail.Lat != coordenadadto.Lat || this.detail.Long != coordenadadto.Long) {

        //                    this.message.confirm('Cambio de Coordenadas?', 'Confirmación', (a) => {

        //                        //this.isshowalgo = !this.isshowalgo;
        //                        if (a.value) {
        //                            this.detail.Lat = coordenadadto.Lat;
        //                            this.detail.Long = coordenadadto.Long;

        //                            this.ApplyPunto.emit(this.detail);
        //                            this.close();

        //                        }

        //                    });
        //                }
        //                else {
        //                    //si existe diferencia
        //                    this.ApplyPunto.emit(this.detail);
        //                    this.close();
        //                }
        //            }
        //            else {
        //                //si no existe el punto
        //                this.ApplyPunto.emit(this.detail);
        //                this.close();
        //            }
        //        });
        //}
        //else {
        //    //si no se completaron los datos
        //    this.ApplyPunto.emit(this.detail);
        //    this.close();
        //}
        if (this.detail.PlaCoordenadaAnulada == true) {
            this.message.error("La coordenada de este punto esta anulada. Por favor cambiarla.", 'Coordenada Anulada');
        }
        else {
            this.ApplyPunto.emit(this.detail);
            this.close();
        }

    }

    applySectores(): void {
        console.log("sectoresdto", this.sectoresdto);
        this.ApplySectores.emit(this.sectoresdto);
        this.close();
    }



    initpuntosDetailsidebar(): void {
        var topbarAside = $('#puntosDetailsidebar');
        var topbarAsideContent = topbarAside.find('.puntosDetailsidebar_content');

        this.topbarAsideObj = new mOffcanvas('puntosDetailsidebar', {
            overlay: false,
            baseClass: 'm-quick-sidebar',
            //  closeBy: 'm_quick_sidebar_close',
            // toggleBy: 'm_quick_sidebar_toggle'
        });

        // run once on first time dropdown shown
        this.topbarAsideObj.one('afterShow', function() {
            mApp.block(topbarAside);

            setTimeout(function() {
                mApp.unblock(topbarAside);

                topbarAsideContent.removeClass('m--hide');

                //                initOffcanvasTabs();
            }, 1000);
        });



    }

    openSearchDialog() {

        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;


        let dialogRef = this.dialog.open(CoordenadasComponent, dialogConfig);
        dialogRef.componentInstance.showbreadcum = false;
        dialogRef.componentInstance.showDefaultBreadcum = false;
        dialogRef.componentInstance.allowSelect = true;
        dialogRef.componentInstance.showAnulado = true;

        dialogRef.componentInstance.onCreated.subscribe(() => {

            dialogRef.close(true);

            let dialogRef2 = this.dialog.open(CreateOrEditCoordenadaModalComponent, dialogConfig);
            dialogRef2.componentInstance.IsInMaterialPopupMode = true;
            dialogRef2.componentInstance.saveLocal = false;
            //this.topbarAsideObj.hide();

            var dto = new CoordenadasDto();

            dialogRef2.componentInstance.showNew(dto);
            dialogRef2.afterClosed().subscribe(
                data => {
                    this.topbarAsideObj.show();
                    if (data) {
                        console.log(data);
                        this.active = true;
                        this.detail.PlaCoordenadaId = data.Id;
                        var item = new ItemDto();
                        item.Description = data.Calle1 + " " + data.Calle2 + "";
                        item.Id = data.Id;
                        this.detail.PlaCoordenadaItem = item;
                        this.detail.PlaCoordenadaAnulada = data.Anulado;
                        this.detail.NumeroExterno = data.NumeroExternoIVU;

                    }

                }
            );
        });

        dialogRef.componentInstance.onSelected.subscribe((data) => {

            dialogRef.close();

            //this.topbarAsideObj.show();

            if (data) {
                console.log(data);
                this.active = true;
                this.detail.PlaCoordenadaId = data.Id;
                var item = new ItemDto();
                item.Description = data.Calle1 + " " + data.Calle2 + "";
                item.Id = data.Id;
                this.detail.PlaCoordenadaItem = item;
                this.detail.NumeroExterno = data.NumeroExternoIVU;
            }
        });

        dialogRef.afterClosed().subscribe(
            data => {
                if (!data) {
                    this.topbarAsideObj.show();
                }


            });

        this.topbarAsideObj.hide();
    }

    OnChangeAbreviacion(event: any) {
        if (event) {
            if (event.NumeroExternoIVU)
                this.detail.NumeroExterno = event.NumeroExternoIVU;
        }
        else {
            this.detail.NumeroExterno = null;
        }
    }

    openSearchDialogParada() {

        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;


        let dialogRef = this.dialog.open(ParadaComponent, dialogConfig);
        dialogRef.componentInstance.showbreadcum = false;
        dialogRef.componentInstance.showDefaultBreadcum = false;
        dialogRef.componentInstance.allowSelect = true;
        dialogRef.componentInstance.inMapa = true;

        dialogRef.componentInstance.filter.Lat = this.detail.Lat;
        dialogRef.componentInstance.filter.Long = this.detail.Long;

        dialogRef.componentInstance.onCreated.subscribe(() => {

            dialogRef.close(true);


            const dialogConfigModel = new MatDialogConfig();
            dialogConfigModel.disableClose = false;
            dialogConfigModel.autoFocus = true;

            var dto = new ParadaDto();
            if (this.detail) {
                dto.Lat = this.detail.Lat;
                dto.Long = this.detail.Long;
            }
            dialogConfigModel.data = dto;


            let dialogRef2 = this.dialog.open(CreateOrEditParadaModalComponent, dialogConfigModel);
            dialogRef2.componentInstance.viewMode = ViewMode.Add;
            dialogRef2.componentInstance.IsInMaterialPopupMode = true;
            dialogRef2.componentInstance.saveLocal = false;

            dialogRef2.afterClosed().subscribe(
                data => {
                    this.topbarAsideObj.show();
                    if (data) {
                        console.log(data);
                        this.active = true;
                        this.detail.PlaParadaId = data.Id;
                        var item = new ItemDto();
                        item.Description = data.Codigo;
                        item.Id = data.Id;

                        this.detail.PlaParadaItem = item;

                    }

                }
            );
        });

        dialogRef.componentInstance.onSelected.subscribe((data) => {

            dialogRef.close();

            //this.topbarAsideObj.show();

            if (data) {
                console.log(data);
                this.active = true;
                this.detail.PlaParadaId = data.Id;
                var item = new ItemDto();
                item.Description = data.Codigo;
                item.Id = data.Id;
                this.detail.PlaParadaItem = item;
            }
        });

        dialogRef.afterClosed().subscribe(
            data => {
                if (!data) {
                    this.topbarAsideObj.show();
                }


            });

        this.topbarAsideObj.hide();
    }


}
