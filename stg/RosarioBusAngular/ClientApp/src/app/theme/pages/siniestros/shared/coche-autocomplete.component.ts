import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChanges, SimpleChange } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { CochesService } from '../coches/coches.service';
import { CochesDto } from '../model/coche.model';
import { moment } from 'ngx-bootstrap/chronos/test/chain';
import { HOUR } from 'ngx-bootstrap/chronos/units/constants';

@Component({
    selector: 'coches-autocomplete',
    templateUrl: '../../../../shared/components/autocompleteBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => CochesAutoCompleteComponent),
            multi: true
        }
    ]
})
export class CochesAutoCompleteComponent extends AutoCompleteComponent<CochesDto> implements OnInit {


    constructor(private serviceCoche: CochesService, injector: Injector) {
        super(serviceCoche, injector);

    }

    ngOnInit(): void {
        super.ngOnInit();
    }



    protected GetFilter(query: any): any {
        var f = {
            FilterText: query
        };

        return f;
    }


    formatDate(date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear(),
            hour = '' + d.getHours(),
            minute = '' + d.getMinutes();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;
        if (hour.length < 2) hour = '0' + hour;
        if (minute.length < 2) minute = '0' + minute;

        var datetotal = [day, month, year].join('/');
        datetotal = datetotal + ' ' + hour + ':' + minute;

        return datetotal;
    }
}
