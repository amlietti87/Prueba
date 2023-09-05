import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, AfterViewInit, OnInit, Input } from '@angular/core';

import * as _ from 'lodash';
declare let mApp: any;
declare let mOffcanvas: any;

import { NgForm } from '@angular/forms';
import { TallerDto, TallerFilter } from '../model/taller.model';
import { DetailEmbeddedComponent } from '../../../../shared/manager/detail.component';
import { TallerService } from './taller.service';
import { PuntoDto } from '../model/punto.model';
import { RutaDto } from '../model/ruta.model';
import { SucursalDto } from '../model/sucursal.model';
import { SucursalService } from '../sucursal/sucursal.service';
import { ItemDto, ResponseModel, ViewMode } from '../../../../shared/model/base.model';


@Component({
    selector: 'createOrEditTallerDtoModal',
    templateUrl: './create-or-edit-taller-modal.component.html',
    styleUrls: ['./create-or-edit-taller-modal.component.css']

})
export class CreateOrEditTallerModalComponent extends DetailEmbeddedComponent<TallerDto> implements AfterViewInit, OnInit {

    @Input() Sucursalid: number;
    @Input() Sucursal: string;
    @Output() modalSaveTalleres: EventEmitter<TallerDto> = new EventEmitter<TallerDto>();
    @Output() ApplyTaller: EventEmitter<TallerDto> = new EventEmitter<TallerDto>();
    @Output() CancelTaller: EventEmitter<TallerDto> = new EventEmitter<TallerDto>();
    @Output() OnMapSelected: EventEmitter<RutaDto> = new EventEmitter<RutaDto>();

    @Output() OnTabMapSelected: EventEmitter<SucursalDto> = new EventEmitter<SucursalDto>();



    sucursales: SucursalDto[] = [];
    isSectormode: boolean;


    constructor(
        injector: Injector,
        protected service: TallerService,
        protected sucService: SucursalService
    ) {
        super(service, injector);
        this.detail = new TallerDto();
    }

    ngOnInit(): void {
        super.ngOnInit();

    }


    topbarAsideObj: any


    showDto(item: TallerDto) {

        super.showDto(item);

        this.sucursales.forEach(s => { s.Rutas = null; });


        this.topbarAsideObj.show();


        setTimeout(() => {
            //We have access to the context values
            if ($('#accordionTab_suc_' + this.Sucursalid + ' .ui-state-active').length == 0) {
                $('#accordionTab_suc_' + this.Sucursalid).find('span').click();
            }
            else {

                var suc = this.sucursales.find(e => e.Id == this.Sucursalid);

                this.GetRutasByGalpon(suc);
            }

        }, 30);




    }


    getDescription(item: TallerDto): string {
        return item.Nombre;
    }

    onSelectedMap(item: RutaDto) {

        //var tallerPuntoDto = new TallerPuntoDto();
        //tallerPuntoDto.Id = this.detail.Id + item.Id;
        ////tallerPuntoDto.rutas = item;
        //tallerPuntoDto.taller = this.detail;



        this.OnMapSelected.emit(item)
    }


    close(): void {
        super.close();
        this.topbarAsideObj.hide();
    }

    cancelar(): void {
        this.close();
        this.CancelTaller.emit(this.detail);
    }

    ngAfterViewInit(): void {
        mApp.initPortlets()
        this.initTallerDetailsidebar();
        this.sucService.requestAllByFilter({}).subscribe(data => {
            this.sucursales = data.DataObject.Items;
        }


        );
    }




    save(form: NgForm): void {

        super.save(form);
    }



    setSucursal(Sucursalid: number, Sucursal: string): void {
        this.Sucursalid = Sucursalid;
        this.Sucursal = Sucursal;
    }


    saveTaller(form: NgForm): void {


        if (this.detailForm.form.invalid) {
            return;
        }

        this.saving = true;
        this.completedataBeforeSave(this.detail);

        if (!this.validateSave()) {
            this.saving = false;
            return;
        }


        //  this.sucursales[index]

        this.sucursales.forEach(s => {
            if (s.Rutas) {
                for (var i = 0; i < s.Rutas.length; i++) {


                    s.Rutas[i].SucursalId = s.Id;
                    s.Rutas[i].Puntos.forEach(item => {
                        item.Orden = item.EsPuntoInicio ? 1 : 2;
                        if (!item.Steps) item.Steps = [];
                        if (!item.Instructions) item.Instructions = [];

                        item.Data = JSON.stringify({ steps: item.Steps, instructions: item.Instructions });
                        item.Steps = null;
                        item.Polylines = null;

                    })
                }

            }

        });


        //this.modalSaveTalleres.emit(this.detail);

        var taller = this.detail;

        var rutasEliminadas = [];
        var rutasDto = [];

        this.sucursales.forEach(s => {

            if (s.Rutas) {
                s.Rutas.forEach(r => {
                    if (!r.Selected && r.BanderaId != 0) {
                        rutasEliminadas.push(r.BanderaId);

                    }
                    if (r.Selected) {
                        rutasDto.push(r);
                    }
                });

            }

        })



        var saveEntity: TallerDto = new TallerDto(taller);
        saveEntity.Rutas = rutasDto;
        saveEntity.BanderasAEliminar = rutasEliminadas;


        this.service.UpdateRutasPorGalpon(saveEntity)
            .finally(() => { this.saving = false; })
            .subscribe((t) => {

                this.notify.info('Guardado exitosamente');

                this.close();
                this.affterSave(this.detail);
                this.closeOnSave = true;
                this.modalSave.emit(null);
            });

    }

    applyTaller(): void {
        if (this.detailForm.form.invalid) {
            return;
        }

        this.detail.Id = "0";

        this.service.createOrUpdate(this.detail, ViewMode.Add).subscribe(e => {
            var ret = e.DataObject as TallerDto;
            this.ApplyTaller.emit(this.detail);
            this.close();

        });



    }


    onTabOpen(e) {

        var index = e.index;


        var suc = this.sucursales[index];

        //limpiar rutas y puntos de inicio fin de otra sucursal; 
        this.OnTabMapSelected.emit(suc);


        if (!suc.Rutas) {
            this.GetRutasByGalpon(suc);
        }
        else {

            suc.Rutas.forEach(r => {

                var f = r.Selected;
                r.Selected = null;
                r.Selected = f;
                var b = r.BanderaId != 0;
                this.OnMapSelected.emit(r);
            });

        }
    }



    GetRutasByGalpon(suc: SucursalDto) {

        var filter: TallerFilter = new TallerFilter();

        filter.Lat = this.detail.Lat;
        filter.Nombre = this.detail.Nombre;
        filter.SucursalId = suc.Id;
        filter.Long = this.detail.Long;
        this.isTableLoading = true;
        mApp.block('#m_quick_sidebar_tabs_Talleres');
        // Se puede mejorar para performance
        this.service.GetRutasByGalpon(filter)
            .finally(() => {
                this.isTableLoading = false;
                mApp.unblock('#m_quick_sidebar_tabs_Talleres');
            })
            .subscribe((result: ResponseModel<RutaDto[]>) => {


                result.DataObject.forEach(r => {
                    r.Selected = r.BanderaId != 0;
                    this.OnMapSelected.emit(r);

                });

                suc.Rutas = result.DataObject;

            });
    }




    initTallerDetailsidebar(): void {
        var topbarAside = $('#TallerDetailsidebar');
        var topbarAsideContent = topbarAside.find('.TallerDetailsidebar_content');

        this.topbarAsideObj = new mOffcanvas('TallerDetailsidebar', {
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



}
