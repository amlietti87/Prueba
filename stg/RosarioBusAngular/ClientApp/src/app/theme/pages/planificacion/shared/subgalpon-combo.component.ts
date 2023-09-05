import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';


import { SucursalDto } from '../model/sucursal.model';
import { SucursalService } from '../sucursal/sucursal.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { SubGalponDto } from '../model/subgalpon.model';
import { SubGalponService } from '../subgalpon/subgalpon.service';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';

@Component({
    selector: 'subgalpon-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SubgalponComboComponent),
            multi: true
        }
    ]
})
export class SubgalponComboComponent extends ComboBoxComponent<SubGalponDto> implements OnInit {



    constructor(service: SubGalponService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        //super.ngOnInit();
    }

    _CodHfecha: number;

    @Input()
    get CodHfecha(): number {

        return this._CodHfecha;
    }

    set CodHfecha(value: number) {

        this._CodHfecha = value;
        if (value) {
            super.onSearch();
        }


    }

    onSearch(): void {

        super.onSearch();
    }

    protected GetFilter(): any {

        var f = {
            CodHfecha: this.CodHfecha
        };

        return f;
    }

}
