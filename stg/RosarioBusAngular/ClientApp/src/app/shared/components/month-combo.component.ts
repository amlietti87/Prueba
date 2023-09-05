import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';

import { ComboBoxYesNoAllComponent } from './comboBase.component';
import { ItemDto } from '../model/base.model';

@Component({
    selector: 'month-combo',
    templateUrl: './comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => MonthComboComponent),
            multi: true
        }
    ]
})


export class MonthComboComponent extends ComboBoxYesNoAllComponent<ItemDto> implements OnInit {


    constructor(protected injector: Injector) {
        super(injector);
        var opc1 = new ItemDto();
        opc1.Id = 1;
        opc1.Description = 'Enero';
        var opc2 = new ItemDto();
        opc2.Id = 2;
        opc2.Description = 'Febrero';
        var opc3 = new ItemDto();
        opc3.Id = 3;
        opc3.Description = 'Marzo';
        var opc4 = new ItemDto();
        opc4.Id = 4;
        opc4.Description = 'Abril';
        var opc5 = new ItemDto();
        opc5.Id = 5;
        opc5.Description = 'Mayo';
        var opc6 = new ItemDto();
        opc6.Id = 6;
        opc6.Description = 'Junio';
        var opc7 = new ItemDto();
        opc7.Id = 7;
        opc7.Description = 'Julio';
        var opc8 = new ItemDto();
        opc8.Id = 8;
        opc8.Description = 'Agosto';
        var opc9 = new ItemDto();
        opc9.Id = 9;
        opc9.Description = 'Septiembre';
        var opc10 = new ItemDto();
        opc10.Id = 10;
        opc10.Description = 'Octubre';
        var opc11 = new ItemDto();
        opc11.Id = 11;
        opc11.Description = 'Noviembre';
        var opc12 = new ItemDto();
        opc12.Id = 12;
        opc12.Description = 'Diciembre';

        this.items.push(opc1);
        this.items.push(opc2);
        this.items.push(opc3);
        this.items.push(opc4);
        this.items.push(opc5);
        this.items.push(opc6);
        this.items.push(opc7);
        this.items.push(opc8);
        this.items.push(opc9);
        this.items.push(opc10);
        this.items.push(opc11);
        this.items.push(opc12);

    }


    onSearch(): void {
        var self = this;
        self.isLoading = false;
    }
}
