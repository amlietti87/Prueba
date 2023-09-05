import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { RolDto } from '../model/rol.model';
import { RolesService } from '../roles/roles.service';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';


@Component({
    selector: 'role-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => RoleComboComponent),
            multi: true
        }
    ]
})
export class RoleComboComponent extends ComboBoxComponent<RolDto> implements OnInit {


    constructor(service: RolesService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}
