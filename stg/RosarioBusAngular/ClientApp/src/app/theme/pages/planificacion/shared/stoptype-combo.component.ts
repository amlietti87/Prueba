import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';

import { ItemDto } from '../../../../shared/model/base.model';
import { ComboBoxYesNoAllComponent } from '../../../../shared/components/comboBase.component';



@Component({
    selector: 'stoptype-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => StopTypeComboComponent),
            multi: true
        }
    ]
})


export class StopTypeComboComponent extends ComboBoxYesNoAllComponent<ItemDto> implements OnInit {


    constructor(protected injector: Injector) {
        super(injector);
        var opc1 = new ItemDto();
        opc1.Id = 0;
        opc1.Description = 'Parada';
        var opc2 = new ItemDto();
        opc2.Id = 1;
        opc2.Description = 'Estacion';
        var opc3 = new ItemDto();
        opc3.Id = 2;
        opc3.Description = 'Entrada o Salida de una estacion';


        this.items.push(opc1);
        this.items.push(opc2);
        this.items.push(opc3);

    }


    onSearch(): void {
        var self = this;
        self.isLoading = false;
    }
}
