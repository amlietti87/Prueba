import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { SectoresTarifariosDto } from '../model/sectoresTarifarios.model';
import { SectoresTarifariosService } from '../sectoresTarifarios/sectoresTarifarios.service';


@Component({
    selector: 'sectoresTarifarios-autocomplete',
    templateUrl: '../../../../shared/components/autocompleteBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SectoresTarifariosAutoCompleteComponent),
            multi: true
        }
    ]
})


export class SectoresTarifariosAutoCompleteComponent extends AutoCompleteComponent<SectoresTarifariosDto> implements OnInit {


    constructor(service: SectoresTarifariosService, injector: Injector) {
        super(service, injector);
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

}
