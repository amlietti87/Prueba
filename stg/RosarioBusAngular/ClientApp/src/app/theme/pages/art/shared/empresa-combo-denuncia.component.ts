import { Component, OnInit, Injector, Input, forwardRef, SimpleChanges, OnChanges, SimpleChange, ViewEncapsulation } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';
import { EmpresaDto, EmpresaFilter } from '../../planificacion/model/empresa.model';
import { EmpresaService } from '../../planificacion/empresa/empresa.service';


@Component({
    selector: 'empresa-combo-denuncia',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => EmpresaComboDenunciaComponent),
            multi: true
        }
    ]
})
export class EmpresaComboDenunciaComponent extends ComboBoxAsync<EmpresaDto> implements OnInit {

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

    onSearch(): void {
        var self = this;
        this.isLoading = true;
        this.service.requestAllByFilter(this.GetFilter()).subscribe(result => {

            this.Items.next(result.DataObject.Items);
            this.data = result.DataObject.Items;
            self.isLoading = false;
            this.detectChanges();

        });
    }
}
