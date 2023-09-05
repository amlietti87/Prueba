import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { SiniestrosDto } from '../../siniestros/model/siniestro.model';
import { SiniestroService } from '../../siniestros/siniestro/siniestro.service';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { ItemDto } from '../../../../shared/model/base.model';

@Component({
    selector: 'siniestro-combo',
    templateUrl: '../../../../shared/components/autocompleteBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SiniestrosComboComponent),
            multi: true
        }
    ]
})
export class SiniestrosComboComponent extends AutoCompleteComponent<SiniestrosDto> implements OnInit {


    constructor(service: SiniestroService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _SiniestroId: number;
    _SucursalId: number;
    _selectEmpleados: ItemDto;

    @Input()
    get SiniestroId(): number {

        return this._SiniestroId;
    }

    set SiniestroId(value: number) {
        this._SiniestroId = value;
    }

    @Input() ReclamosSearch: boolean = false;

    @Input()
    get SucursalId(): number {

        return this._SucursalId;
    }

    set SucursalId(value: number) {
        this._SucursalId = value;
    }

    @Input()
    get selectEmpleados(): ItemDto {

        return this._selectEmpleados;
    }

    set selectEmpleados(value: ItemDto) {
        this._selectEmpleados = value;
        if (value) {
            this.ConductorId = value.Id;
        }
    }

    ConductorId: number = null;

    protected GetFilter(query: any): any {
        var f = {
            SiniestroId: this.SiniestroId,
            ConductorId: this.ConductorId,
            Anulado: false,
            SucursalId: this.SucursalId,
            ReclamosSearch: this.ReclamosSearch,
            FilterText: query
        };
        return f;
    }

    filterItems(event) {
        let query = event.query;
        var filter = this.GetFilter(query);
        this.service.GetItemsAsync(filter).subscribe(x => {
            this.items = [];
            for (var i in x.DataObject) {
                var item = x.DataObject[i];
                this.items.push(item);
            }

        });
    }
}
