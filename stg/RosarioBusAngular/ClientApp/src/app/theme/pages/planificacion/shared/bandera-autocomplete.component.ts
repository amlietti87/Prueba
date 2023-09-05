import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { BanderaDto } from '../model/bandera.model';
import { BanderaService } from '../bandera/bandera.service';
import { BanderaItemLongDto } from '../model/linea.model';

@Component({
    selector: 'bandera-autocomplete',
    templateUrl: '../../../../shared/components/autocompleteBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => BanderaAutoCompleteComponent),
            multi: true
        }
    ]
})
export class BanderaAutoCompleteComponent extends AutoCompleteComponent<BanderaDto> implements OnInit {


    private _SucursalId: number;
    get SucursalId(): number {
        return this._SucursalId;
    }

    @Input()
    set SucursalId(SucursalId: number) {
        this._SucursalId = SucursalId;
    }



    private _RamalesID: number[];
    get RamalesID(): number[] {
        return this._RamalesID;
    }

    @Input()
    set RamalesID(ramalesID: number[]) {
        this._RamalesID = ramalesID;
    }


    private _CodHfecha: number[];
    get CodHfecha(): number[] {
        return this._CodHfecha;
    }

    @Input()
    set CodHfecha(CodHfecha: number[]) {
        this._CodHfecha = CodHfecha;
    }

    @Input()
    public MostrarLinea: boolean = false;


    @Input()
    public RamalesEsRequerido: boolean = true;


    private _TipoBanderaId: number;
    get TipoBanderaId(): number {
        return this._TipoBanderaId;
    }

    @Input()
    set TipoBanderaId(tipoBanderaId: number) {
        this._TipoBanderaId = tipoBanderaId;
    }


    constructor(service: BanderaService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    protected GetFilter(query: any): any {
        var f = {
            SucursalId: this._SucursalId,
            FilterText: query,
            RamalesID: this.RamalesID,
            TipoBanderaId: this.TipoBanderaId,
            CodHfecha: this.CodHfecha
        };

        return f;
    }


    filterItems(event): void {
        let query = event.query;


        if (this.RamalesEsRequerido) {
            var isValid = (this.RamalesID && this.RamalesID.length > 0);
            if (!isValid) {
                return;
            }
        }

        this.service.FindItemsAsync(this.GetFilter(query)).subscribe(x => {
            this.items = [];
            var data = x.DataObject as BanderaItemLongDto[];
            for (var i in data) {
                var item = data[i];
                if (this.MostrarLinea && item.LineaNombre) {
                    item.Description = item.Description + " (" + item.LineaNombre + ")";
                }
                this.items.push(item);
            }

        });
    }


}
