import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { GruposInspectoresDto, GruposInspectoresFilter } from '../model/gruposinspectores.model';
import { GruposInspectoresService } from '../gruposinspectores/gruposinspectores.service';


@Component({
    selector: 'gruposinspectores-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => GruposInspectoresComboComponent),
            multi: true
        }
    ]
})
export class GruposInspectoresComboComponent extends ComboBoxComponent<GruposInspectoresDto> implements OnInit {


    constructor(service: GruposInspectoresService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    private _Anulado: number;
    get Anulado(): number {
        return this._Anulado;
    }

    @Input()
    set Anulado(anulado: number) {
        this._Anulado = anulado;
        if (anulado) {
            this.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = { Anulado: this.Anulado };

        return f;
    }
}
