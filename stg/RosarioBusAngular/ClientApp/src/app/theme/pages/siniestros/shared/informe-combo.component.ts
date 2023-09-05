import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { TipoDniDto } from '../model/tipodni.model';
import { TipoDniService } from '../tipodni/tipodni.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent, ComboBoxBaseComponent } from '../../../../shared/components/comboBase.component';
import { ItemDto } from '../../../../shared/model/base.model';
import { SiniestroService } from '../siniestro/siniestro.service';
import { EmpPractDto } from '../model/EmpPract.model';
import { InformeDto } from '../model/informe.model';

@Component({
    selector: 'informe-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => InformeComboComponent),
            multi: true
        }
    ]
})


export class InformeComboComponent extends ComboBoxComponent<InformeDto> implements OnInit {



    constructor(protected service: SiniestroService,
        protected injector: Injector) {

        super(service, injector);
        var opc1 = new InformeDto();
        opc1.Id = 2;
        opc1.Description = 'No';
        var opc2 = new InformeDto();
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
