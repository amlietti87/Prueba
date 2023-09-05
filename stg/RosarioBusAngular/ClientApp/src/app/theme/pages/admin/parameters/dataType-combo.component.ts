import { ComboBoxComponent } from "../../../../shared/components/comboBase.component";
import { DataTypeDto } from "../model/dataType.model";
import { Component, forwardRef, OnInit, Injector } from "@angular/core";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { DataTypeService } from "./dataType.service";

@Component({
    selector: 'dataTypes-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => DataTypesComboComponent),
            multi: true
        }
    ]
})
export class DataTypesComboComponent extends ComboBoxComponent<DataTypeDto> implements OnInit {
    constructor(service: DataTypeService, injector: Injector) {
        super(service, injector);
    }
    ngOnInit(): void {
        super.ngOnInit();
    }
}