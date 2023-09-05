import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ZonasDto, ZonasFilter } from '../model/zonas.model';
import { ZonasService } from '../zonas/zonas.service';
import { CreateOrEditGruposInspectoresModalComponent } from '../gruposinspectores/create-or-edit-gruposinspectores-modal.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditZonasModalComponent } from '../zonas/create-or-edit-zonas-modal.component';
import { TiposTareaDto } from '../model/tipostarea.model';
import { TiposTareaService } from '../tipostarea/tipostarea.service';


@Component({
    selector: 'tipostarea-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TiposTareaComboComponent),
            multi: true
        }
    ]
})
export class TiposTareaComboComponent extends ComboBoxComponent<TiposTareaDto> implements OnInit {


    constructor(service: TiposTareaService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    //private _Anulado: number;
    //get Anulado(): number {
    //    return this._Anulado;
    //}

    //@Input()
    //set Anulado(anulado: number) {
    //    this._Anulado = anulado;
    //    if (anulado) {
    //        this.onSearch();
    //    }
    //}

    //protected GetFilter(): any {
    //    var f = { Anulado: this.Anulado };

    //    return f;
    //}

    //getIDetailComponent(): ComponentType<DetailComponent<TiposTareaDto>> {
    //    return CreateOrEditTiposTareaModalComponent;
    //}

    //getNewDto(): TiposTareaDto {
    //    return new TiposTareaDto();
    //}
}
