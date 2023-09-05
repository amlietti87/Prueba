import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { ParadaDto } from '../model/parada.model';
import { ParadaService } from '../parada/parada.service';
declare let mApp: any;
declare let $: any;

@Component({
    selector: 'parada-autocomplete',
    templateUrl: './parada-autocomplete.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => ParadaAutoCompleteComponent),
            multi: true
        }
    ]
})
export class ParadaAutoCompleteComponent extends AutoCompleteComponent<ParadaDto> implements OnInit {


    constructor(service: ParadaService, injector: Injector) {
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



    private _lat: number[];
    get Lat(): number[] {
        return this._lat;
    }

    @Input()
    set Lat(lat: number[]) {
        this._lat = lat;
    }

    private _long: number[];
    get Long(): number[] {
        return this._long;
    }

    @Input()
    set Long(long: number[]) {
        this._long = long;
    }

    private _LocationType: number;
    get LocationType(): number {
        return this._LocationType;
    }

    @Input()
    set LocationType(locationtype: number) {
        this._LocationType = locationtype;
    }



    protected GetFilter(query: any): any {
        var f = {
            FilterText: query,
            Anulado: false,
            Lat: this.Lat,
            Long: this.Long,
            LocationType: this.value ? null : this.LocationType
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
