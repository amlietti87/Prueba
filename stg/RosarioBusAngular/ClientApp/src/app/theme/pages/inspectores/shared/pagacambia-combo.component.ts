import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxYesNoAllComponent } from '../../../../shared/components/comboBase.component';
import { ItemDto } from '../../../../shared/model/base.model';


@Component({
    selector: 'pagacambia-combo',
    template: `
    <button *ngIf="showAddButton"
            type="button"
            class="smallesttext btn btn-primary blue"
            (click)="onAddButtonClick()">
            <i class="la la-plus"></i>
    </button>
    <select #combobox
            [(ngModel)]="value"
            [disabled]="IsDisabled || isLoading"
            (ngModelChange)="selectedItemChange.emit($event)"
            name="combobox"
            title="{{emptyText}}"
            data-container="body"
            style="width:100%;">
            <option *ngIf="allowNullable==true" value="null">
                {{emptyText}}
            </option>
            <option *ngFor="let item of items" [value]="item.Id">{{item.Description}}</option>
    </select>
  `,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => PagaCambiaComboComponent),
            multi: true
        }
    ]
})


export class PagaCambiaComboComponent extends ComboBoxYesNoAllComponent<ItemDto> implements OnInit {


    constructor(protected injector: Injector) {
        super(injector);
        var opc1 = new ItemDto();
        opc1.Id = 0;
        opc1.Description = 'Cambia';
        var opc2 = new ItemDto();
        opc2.Id = 1;
        opc2.Description = 'Paga';

        this.items.push(opc1);
        this.items.push(opc2);

    }


    onSearch(): void {
        var self = this;
        self.isLoading = false;
    }
}
