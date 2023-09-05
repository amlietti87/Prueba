import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { NotificationDto } from '../model/notification.model';
import { NotificationServiceProxy } from '../../../layouts/header-nav/notifications/notification-service';
import { NotificationService } from '../../../../shared/notification/notification.service';


@Component({
    selector: 'notification-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => NotificationComboComponent),
            multi: true
        }
    ]
})
export class NotificationComboComponent extends ComboBoxComponent<NotificationDto> implements OnInit {


    constructor(service: NotificationService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}
