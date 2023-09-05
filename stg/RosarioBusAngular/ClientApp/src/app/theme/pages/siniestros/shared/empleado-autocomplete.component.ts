import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChanges, SimpleChange } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { EmpleadosService } from '../empleados/empleados.service';
import { EmpleadosDto } from '../model/empleado.model';

@Component({
    selector: 'empleado-autocomplete',
    templateUrl: '../../../../shared/components/autocompleteBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => EmpleadosAutoCompleteComponent),
            multi: true
        }
    ]
})
export class EmpleadosAutoCompleteComponent extends AutoCompleteComponent<EmpleadosDto> implements OnInit {
    _UnidadNegocio: number;

    @Input()
    get UnidadNegocio(): number {

        return this._UnidadNegocio;
    }

    set UnidadNegocio(value: number) {
        this._UnidadNegocio = value;
    }


    constructor(service: EmpleadosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    protected GetFilter(query: any): any {
        var f = {
            FilterText: query,
            UnidadNegocio: this.UnidadNegocio
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
