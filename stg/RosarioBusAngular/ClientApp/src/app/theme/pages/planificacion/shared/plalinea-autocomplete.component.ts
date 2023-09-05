import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { LineaDto } from '../model/linea.model';
import { PlaLineaService } from '../linea/pla-linea.service';

@Component({
    selector: 'plalinea-autocomplete',
    templateUrl: '../../../../shared/components/autocompleteBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => PlaLineaAutoCompleteComponent),
            multi: true
        }
    ]
})
export class PlaLineaAutoCompleteComponent extends AutoCompleteComponent<LineaDto> implements OnInit {


    private _SucursalId: number;
    get SucursalId(): number {
        return this._SucursalId;
    }

    @Input()
    set SucursalId(SucursalId: number) {
        this._SucursalId = SucursalId;
    }


    constructor(service: PlaLineaService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    protected GetFilter(query: any): any {
        var f = {
            SucursalId: this._SucursalId,
            FilterText: query
        };

        return f;
    }

}
