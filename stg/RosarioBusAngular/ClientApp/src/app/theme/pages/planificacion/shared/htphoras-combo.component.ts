import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { TipoLineaService } from '../tipoLinea/tipoLinea.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { HTposHorasDto } from '../model/htposhoras.model';
import { HTposHorasService } from '../htposhoras/htposhoras.service';


@Component({
    selector: 'htposhoras-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => HTposHorasComboComponent),
            multi: true
        }
    ]
})
export class HTposHorasComboComponent extends ComboBoxComponent<HTposHorasDto> implements OnInit {
    @Input() autoLoad: boolean = false;


    @Input()
    get items(): HTposHorasDto[] {
        return this._items;
    }
    set items(val: HTposHorasDto[]) {
        this._items = val;
        if (this.itemstChange)
            this.itemstChange.emit(val);
        this.refreshCombo();
    }





    constructor(service: HTposHorasService, injector: Injector) {
        super(service, injector);

        //this.itemstChange.subscribe(e => {
        //    
        //    this.refreshCombo();
        //})

    }

    ngOnInit(): void {
        if (this.autoLoad) {

            super.ngOnInit();
        }
    }

    refreshCombo(): void {
        var self = this;
        setTimeout(() => {
            $(self.comboboxElement.nativeElement).selectpicker('refresh');
        }, 0);
    }



}
