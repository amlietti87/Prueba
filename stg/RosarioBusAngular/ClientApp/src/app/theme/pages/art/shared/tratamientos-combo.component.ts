import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { TratamientosDto } from '../model/tratamientos.model';
import { TratamientosService } from '../tratamientos/tratamientos.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditTratamientosModalComponent } from '../tratamientos/create-or-edit-tratamientos-modal.component';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';


@Component({
    selector: 'tratamientos-combo',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TratamientosComboComponent),
            multi: true
        }
    ]
})
export class TratamientosComboComponent extends ComboBoxAsync<TratamientosDto> implements OnInit {

    Anulado: boolean = false;

    constructor(service: TratamientosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _TratamientoId: number;

    @Input()
    get TratamientoId(): number {
        return this._TratamientoId;
    }

    set TratamientoId(value: number) {
        this._TratamientoId = value;
        if (value) {
            super.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            TratamientoId: this.TratamientoId
        };
        return f;
    }


    getIDetailComponent(): ComponentType<DetailComponent<TratamientosDto>> {
        return CreateOrEditTratamientosModalComponent;
    }

    getNewDto(): TratamientosDto {
        return new TratamientosDto();
    }

}
