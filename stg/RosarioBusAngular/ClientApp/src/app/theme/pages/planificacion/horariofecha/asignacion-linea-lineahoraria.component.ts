import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit, Input, ComponentFactoryResolver, ViewContainerRef, Inject } from '@angular/core';

import * as _ from 'lodash';
declare let mApp: any;
import * as moment from 'moment';
import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';

import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
import { LineaDto } from '../model/linea.model';
import { PlaLineaLineaHorariaDto } from '../model/linea_lineahoraria.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { LineaService } from '../linea/linea.service';




@Component({
    selector: 'asignacion-linea-lineahoraria',
    templateUrl: './asignacion-linea-lineahoraria.component.html',

})
export class AsignacionLineaLineaHoraria extends DetailAgregationComponent<LineaDto> implements OnInit {

    @Input()
    detail: LineaDto;

    UrbInterInt: number;


    loadingsectores: boolean;
    selectItem: ItemDto;


    constructor(
        protected dialogRef: MatDialogRef<AsignacionLineaLineaHoraria>,
        injector: Injector,
        protected service: LineaService,
        @Inject(MAT_DIALOG_DATA) public data: LineaDto

    ) {
        super(dialogRef, service, injector, data);
        //this.cfr = injector.get(ComponentFactoryResolver);


        this.saveLocal = false;

    }

    close() {
        this.dialogRef.close(false);
    }

    ngOnInit(): void {

    }


    addLinea(item: ItemDto): void {

        this.addAndValidateLinea(item, true);
    }

    addAndValidateLinea(item: ItemDto, showwarn: boolean) {

        if (item.Id) {
            if (!this.detail.PlaLineaLineaHoraria.some(x => x.PlaLineaId == item.Id)) {


                var newItem = new PlaLineaLineaHorariaDto();
                newItem.IsSelected = true;
                newItem.PlaLineaId = item.Id;
                newItem.LineaId = this.detail.Id;
                newItem.Description = item.Description;//this.detail.DesLin;

                this.detail.PlaLineaLineaHoraria.push(newItem);

            }
            else {
                var c = this.detail.PlaLineaLineaHoraria.find(x => x.LineaId == item.Id);
                if (showwarn) {
                    this.notificationService.warn("La linea ya fue agregada", item.Description);
                }

                c.animate = true;

                setTimeout(() => {
                    c.animate = false;
                }, 1000);
            }

            this.selectItem = null;

        }
    }


    removeLinea(item: PlaLineaLineaHorariaDto) {

        var index = this.detail.PlaLineaLineaHoraria.findIndex(x => x.Id == item.Id);
        if (index >= 0) {
            this.detail.PlaLineaLineaHoraria.splice(index, 1);
        }

    }

    completedataBeforeShow(item: LineaDto): any {
        if (this.detail.UrbInter) {
            this.UrbInterInt = parseInt(this.detail.UrbInter);
        }
    }

    completedataBeforeSave(item: LineaDto): any {
        if (this.UrbInterInt) {
            this.detail.UrbInter = this.UrbInterInt.toString();
        }
    }

    SaveDetail(): any {

        this.saving = true;

        if (this.viewMode == ViewMode.Add) {
            this.service.createOrUpdate(this.detail, this.viewMode)
                .finally(() => { this.saving = false; })
                .subscribe((t) => {

                    this.notify.info('Guardado exitosamente');
                    this.affterSave(this.detail);
                    this.closeOnSave = true;
                    this.modalSave.emit(null);
                    this.saving = false;
                    this.dialogRef.close(this.detail);
                })
        }
        else {
            this.service.UpdateLineasAsociadas(this.detail)
                .finally(() => { this.saving = false; })
                .subscribe((t) => {

                    this.notify.info('Guardado exitosamente');
                    this.affterSave(this.detail);
                    this.closeOnSave = true;
                    this.modalSave.emit(null);
                    this.saving = false;
                    this.dialogRef.close(this.detail);
                })
        }




    }



    showDto(item: LineaDto) {

        this.detail = item;

        this.completedataBeforeShow(item);

        this.active = true;
    }


}
