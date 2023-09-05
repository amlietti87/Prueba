import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChanges, OnChanges, SimpleChange } from '@angular/core';


import { EmpresaDto, EmpresaFilter } from '../model/empresa.model';
import { TipoLineaService } from '../tipoLinea/tipoLinea.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { EmpresaService } from '../empresa/empresa.service';

@Component({
    selector: 'empresa-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => EmpresaComboComponent),
            multi: true
        }
    ]
})
export class EmpresaComboComponent extends ComboBoxComponent<EmpresaDto> implements OnInit, OnChanges {

    @Input() ItemsModel: EmpresaDto[];
    @Input() AutomaticSearch: boolean = true;

    constructor(service: EmpresaService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    protected GetFilter(): EmpresaFilter {

        var item = new EmpresaFilter();
        item.FecBaja = true;
        item.EmpresaId = this.EmpresaId;
        return item;
    }

    onSearch(): void {
        if (this.AutomaticSearch) {
            var self = this;
            this.isLoading = true;
            this.service.requestAllByFilter(this.GetFilter()).subscribe(result => {
                this.items = result.DataObject.Items;
                self.isLoading = false;
                setTimeout(() => {
                    $(self.comboboxElement.nativeElement).selectpicker('refresh');
                }, 200);
            });
        }
    }
    _EmpresaId: number;

    @Input()
    get EmpresaId(): number {

        return this._EmpresaId;
    }

    set EmpresaId(value: number) {
        this._EmpresaId = value;
        if (value) {
            this.onSearch();
        }
    }

    ngOnChanges(changes: SimpleChanges) {

        const itemmodel: SimpleChange = changes.ItemsModel;
        var self = this;

        if (itemmodel && itemmodel.previousValue != itemmodel.currentValue) {

            this.items = this.ItemsModel;
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 0);
        }
    }

}
