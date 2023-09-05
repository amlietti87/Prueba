import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent, ComboBoxBaseComponent } from '../../../../shared/components/comboBase.component';
import { ItemDto } from '../../../../shared/model/base.model';
import { SiniestroService } from '../siniestro/siniestro.service';
import { ResponsableDto } from '../model/responsable.model';

@Component({
    selector: 'responsable-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => ResponsableComboComponent),
            multi: true
        }
    ]
})


export class ResponsableComboComponent extends ComboBoxComponent<ResponsableDto> implements OnInit {



    constructor(protected service: SiniestroService,
        protected injector: Injector) {

        super(service, injector);
        var opc1 = new ResponsableDto();
        opc1.Id = 2;
        opc1.Description = 'No';
        var opc2 = new ResponsableDto();
        opc2.Id = 1;
        opc2.Description = 'Si';

        this.items.push(opc1);
        this.items.push(opc2);
    }

    ngOnInit(): void {
        this.onSearch();
    }



    onSearch(): void {
        var self = this;
        self.isLoading = false;
    }
}
