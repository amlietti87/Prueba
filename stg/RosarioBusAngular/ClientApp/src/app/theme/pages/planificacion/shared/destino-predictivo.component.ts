import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { BanderaDto } from '../model/bandera.model';
import { BanderaService } from '../bandera/bandera.service';

@Component({
    selector: 'destino-predictivo',
    templateUrl: '../../../../shared/components/predictiveBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => DestinoPredictivoComponent),
            multi: true
        }
    ]
})
export class DestinoPredictivoComponent extends AutoCompleteComponent<BanderaDto> implements OnInit {


    items: string[] = [];

    constructor(protected serviceBase: BanderaService, injector: Injector) {
        super(serviceBase, injector);
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

    Unselect(event) {
    }


    Clear(event) {
    }

    filterItems(event): void {
        let query = event.query;

        var filter = this.GetFilter(query);

        this.serviceBase.DestinoPredictivo(filter).subscribe(x => {
            this.items = x.DataObject;

        });
    }


}
