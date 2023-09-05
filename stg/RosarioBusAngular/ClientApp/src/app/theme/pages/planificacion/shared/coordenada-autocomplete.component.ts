import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { CoordenadasDto } from '../model/coordenadas.model';
import { CoordenadasService } from '../coordenadas/coordenadas.service';
declare let mApp: any;
declare let $: any;

@Component({
    selector: 'coordenada-autocomplete',
    templateUrl: './coordenada-autocomplete.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => CoordenadasAutoCompleteComponent),
            multi: true
        }
    ]
})
export class CoordenadasAutoCompleteComponent extends AutoCompleteComponent<CoordenadasDto> implements OnInit {


    constructor(service: CoordenadasService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    onSelect(): void {
        this.killTooltip();
    }

    onBlur(): void {
        // this.killTooltip();
    }

    onUnselect(): void {
        this.killTooltip();
    }

    onKeyUp(): void {
        // this.killTooltip();
    }

    onDropdownClick(): void {
        this.killTooltip();
    }

    private killTooltip() {
        //if (this.t) {
        //    $(this.t).tooltip('dispose');
        //}
    }


    protected GetFilter(query: any): any {
        var f = {
            FilterText: query,
            AnuladoCombo: 2
        };

        return f;
    }

    t: any;

    filterItems(event) {
        let query = null;
        if (event != null) { query = event.query; }
        this.service.requestAllByFilter(this.GetFilter(query)).subscribe(x => {
            this.items = [];
            for (var i in x.DataObject.Items) {
                var item = x.DataObject.Items[i];
                this.items.push(item);
            }
            setTimeout(() => {
                //this.killTooltip();
                if (this.items && this.items.length > 2) {
                    //this.t = $('[data-toggle="tooltip"]').tooltip();
                }
            }, 10);
        });
    }

}
