import { Component, Input, Type, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef, DebugElement, Injector, Output, EventEmitter } from '@angular/core';

import { IDetailComponent } from '../../../../../shared/manager/detail.component';
import { forEach } from '@angular/router/src/utils/collection';
import { NgModel } from '@angular/forms';

import { InvolucradosService } from '../../involucrados/involucrados.service';
import { InvolucradosDto, DetalleLesionDto } from '../../model/involucrados.model';

import { AgregationListComponent } from '../../../../../shared/manager/agregationlist.component';
import { InvolucradoDetailComponent } from './involucrado-detail-modal.component';
import { DetalleLesionDetailComponent } from './detallelesion-detail-modal.component';


@Component({
    selector: 'detallelesion',
    templateUrl: './detallelesion.component.html'
})
export class DetalleLesionAgregationComponent extends AgregationListComponent<DetalleLesionDto> {
    constructor(injector: Injector) {
        super(DetalleLesionDetailComponent, injector);

        //this.popupWidth = "800px";
        //this.popupHeight = "";
    }


    _detail: InvolucradosDto;

    @Input()
    get detail(): InvolucradosDto {

        return this._detail;
    }

    set detail(value: InvolucradosDto) {

        this._detail = value;

    }

    GetEditComponentType(): Type<IDetailComponent> {
        return DetalleLesionDetailComponent;
    }

    getNewItem(item: any): DetalleLesionDto {
        item = new DetalleLesionDto(item);
        item.InvolucradoId = this.detail.Id;
        return item;
    }

    AgregarLesion() {

        if (this.list) {
            if (this.list.length) {
                if (!this.list[this.list.length - 1]) {
                    return;
                }
            }
        }

        var item = new DetalleLesionDto();

        if (!this.list) {
            let list: Array<DetalleLesionDto> = [];
        }
        this.list = [...this.list, this.getNewItem(item)];
    }


}