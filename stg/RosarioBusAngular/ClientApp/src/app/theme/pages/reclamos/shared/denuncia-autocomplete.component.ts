import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { ItemDto } from '../../../../shared/model/base.model';
import { DenunciasService } from '../../art/denuncias/denuncias.service';
import { DenunciasDto } from '../../art/model/denuncias.model';

@Component({
    selector: 'denuncia-autocomplete',
    templateUrl: '../../../../shared/components/autocompleteBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => DenunciaAutocompleteComponent),
            multi: true
        }
    ]
})
export class DenunciaAutocompleteComponent extends AutoCompleteComponent<DenunciasDto> implements OnInit {


    constructor(service: DenunciasService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _selectEmpleados: ItemDto;
    _selectDenuncia: ItemDto;

    @Input()
    get selectEmpleados(): ItemDto {

        return this._selectEmpleados;
    }

    set selectEmpleados(value: ItemDto) {
        this._selectEmpleados = value;
    }

    @Input()
    get selectDenuncia(): ItemDto {

        return this._selectDenuncia;
    }

    set selectDenuncia(value: ItemDto) {
        this._selectDenuncia = value;
    }

    protected GetFilter(query: any): any {
        var f = {
            selectEmpleados: this._selectEmpleados,
            EmpleadoId: (this._selectEmpleados != null || this._selectEmpleados != undefined) ? this._selectEmpleados.Id : null,
            Anulado: 2,
            FilterText: query
        };
        return f;
    }

    filterItems(event) {
        let query = event.query;
        this.service.GetItemsAsync(this.GetFilter(query)).subscribe(x => {
            this.items = [];
            for (var i in x.DataObject) {
                var item = x.DataObject[i];
                this.items.push(item);
            }

        });
    }
}
