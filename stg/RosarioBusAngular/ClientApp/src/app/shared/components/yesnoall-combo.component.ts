import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';

import { ComboBoxYesNoAllComponent, ComboBoxBaseComponent } from './comboBase.component';
import { ItemDto } from '../model/base.model';

@Component({
    selector: 'yesnoall-combo',
    templateUrl: './comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => YesNoAllComboComponent),
            multi: true
        }
    ]
})


export class YesNoAllComboComponent extends ComboBoxYesNoAllComponent<ItemDto> implements OnInit {


    constructor(protected injector: Injector) {
        super(injector);
        var opc1 = new ItemDto();
        opc1.Id = 2;
        opc1.Description = 'No';
        var opc2 = new ItemDto();
        opc2.Id = 1;
        opc2.Description = 'Si';

        this.items.push(opc1);
        this.items.push(opc2);

    }


    onSearch(): void {
        var self = this;
        self.isLoading = false;
    }
}

@Component({
    template : `
    <div class="col-md-12" style="padding:0px; display:inline-flex">
        <select #combobox
                class="form-control custom-combo"
                [(ngModel)]="value"
                (ngModelChange)="selectedItemChange.emit($event)"
                name="combobox"
                jq-plugin="selectpicker"
                data-container="body"
                [attr.data-live-search]="livesearch">
            <option value="">Todos</option>
            <option value="true">Si</option>
            <option value="false">No</option>
        </select>
    </div>`,
    selector : 'combo-yes-no-all-boolean',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => YesNoAllBooleanComboComponent),
            multi: true
        }
    ]

})

export class YesNoAllBooleanComboComponent extends ComboBoxBaseComponent {
    constructor(protected injector: Injector) {
        super(injector);
    }
}
