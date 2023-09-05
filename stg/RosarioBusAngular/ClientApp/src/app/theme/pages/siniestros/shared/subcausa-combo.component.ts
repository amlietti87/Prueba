import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { CausasService } from '../causas/causas.service';
import { CausasDto, SubCausasDto } from '../model/causas.model';
import { SubCausasService } from '../causas/subcausas.service';

@Component({
    selector: 'subcausas-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SubCausaComboComponent),
            multi: true
        }
    ]
})
export class SubCausaComboComponent extends ComboBoxComponent<SubCausasDto> implements OnInit {
    _causaid: number;
    _subcausaid: number;
    @Input()
    get CausaId(): number {

        return this._causaid;
    }

    set CausaId(value: number) {
        this._causaid = value;
        if (!(this.items.find(e => e.CausaId == value))) {
            super.onSearch();
        }
    }

    @Input()
    get SubCausaId(): number {

        return this._subcausaid;
    }

    set SubCausaId(value: number) {
        this._subcausaid = value;
        if (!(this.items.find(e => e.Id == value))) {
            super.onSearch();
        }
    }


    constructor(service: SubCausasService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    protected GetFilter(): any {
        var f = {
            CausaId: this.CausaId,
            SubCausaId: this.SubCausaId
        };

        return f;
    }
}
