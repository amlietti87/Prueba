import { Component, OnInit, Input, Output, forwardRef, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { NotificationService } from '../notification/notification.service';
import { MAT_DATE_LOCALE } from '@angular/material';
import { CustomDateAdapter } from '../constants/constants';


export const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => DatePickerWithKeyboard),
    multi: true
};

const noop = () => {
};

/** @title Basic datepicker */
@Component({
    selector: 'datepicker-keyboard',
    templateUrl: 'datepickerWithKeyboard.component.html',
    providers: [
        CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR
    ],
})

export class DatePickerWithKeyboard implements ControlValueAccessor {
    public mask = {
        guide: true,
        showMask: false,
        // keepCharPositions : true,
        mask: [/\d/, /\d/, '/', /\d/, /\d/, '/', /\d/, /\d/, /\d/, /\d/]
    };

    constructor(private noti: NotificationService,
        private cdr: ChangeDetectorRef

    ) {

    }
    validateFechaPago: boolean = true;
    innerValue: Date = null
    fecha: string;

    @Input() labelError: string = 'Fecha inválida';
    //Placeholders for the callbacks which are later provided
    //by the Control Value Accessor
    private onTouchedCallback: () => void = noop;
    private onChangeCallback: (_: any) => void = noop;


    //get accessor
    get value(): Date {
        return this.innerValue;
    };

    //set accessor including call the onchange callback
    set value(v: Date) {
        if (v !== this.innerValue) {
            this.innerValue = v;
        }
    }
    //Occured value changed from module
    writeValue(value: any): void {
        if (value !== this.innerValue) {
            this.innerValue = value;
        }
    }

    registerOnChange(fn: any): void {
        this.onChangeCallback = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouchedCallback = fn;
    }

    onChange(event) {
        // console.log(event);
        this.value = event;
        this.onBlur();
    }

    isValidDate(d: string) {

        var timestamp = Date.parse(d);

        if (isNaN(timestamp) == false) {
            return true;
        }
        else {
            return false;
        }
    }

    todate(value) {
        // debugger;
        var fecha = this.parse(value);
        if (!this.isValidDate(fecha)) {
            this.writeValue(null);
            this.value = null;
            this.innerValue = null;
            this.validateFechaPago = false;
            this.noti.warn('Ingresó una fecha inválida, no se dejará guardar hasta su corrección', this.labelError);
            this.cdr.detectChanges();
        }
        else {
            this.value = new Date(fecha);
            this.validateFechaPago = true;
        }
    }
    parse(value: any): string {

        if ((typeof value === 'string') && (value.indexOf('/') > -1)) {
            const str = value.split('/');

            const year = Number(str[2]);
            const month = Number(str[1]);
            const date = Number(str[0]);

            return this.fecha = month + "/" + date + "/" + year;
        }

    }

    onBlur() {
        this.onChangeCallback(this.innerValue);
    }
}


/**  Copyright 2018 Google Inc. All Rights Reserved.
    Use of this source code is governed by an MIT-style license that
    can be found in the LICENSE file at http://angular.io/license */