import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChanges, OnChanges, SimpleChange } from '@angular/core';


import { EmpresaDto } from '../model/empresa.model';
import { TipoLineaService } from '../tipoLinea/tipoLinea.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { PlaSentidoBanderaSubeDto } from '../model/banderacartel.model';
import { PlaSentidoBanderaSubeService } from '../PlaSentidoBanderaSube/PlaSentidoBanderaSube.service';

@Component({
    selector: 'sentidobanderasube-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SentidoBanderaSubeComboComponent),
            multi: true
        }
    ]
})
export class SentidoBanderaSubeComboComponent extends ComboBoxComponent<PlaSentidoBanderaSubeDto> implements OnInit, OnChanges {

    @Input() ItemsModel: PlaSentidoBanderaSubeDto[];
    @Input() AutomaticSearch: boolean = true;

    constructor(service: PlaSentidoBanderaSubeService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    protected GetFilter(): any {
        var f = {

        };

        return f;
    }

    onSearch(): void {
        if (this.AutomaticSearch) {
            super.onSearch();
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
