import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ZonasDto, ZonasFilter } from '../model/zonas.model';
import { ZonasService } from '../zonas/zonas.service';
import { CreateOrEditGruposInspectoresModalComponent } from '../gruposinspectores/create-or-edit-gruposinspectores-modal.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditZonasModalComponent } from '../zonas/create-or-edit-zonas-modal.component';
import { TareaCampoConfigDto } from '../model/tarea-campo-config.model';
import { TareaCampoConfigService } from '../tareas/tarea-campo-config.service';


@Component({
    selector: 'tareas-campos-config-combo',
    template: `   
        <select #combobox
                [(ngModel)]="value"
                [disabled]="IsDisabled || isLoading"
                (ngModelChange)="selectedItemChange.emit($event)"
                name="combobox"
                title="{{emptyText}}"
                data-container="body"
                style="width:100%;">
                <option *ngIf="allowNullable==true" value="null">
                    {{emptyText}}
                </option>
                <option *ngFor="let item of items" [value]="item.Id">{{item.Description}}</option>
        </select>
    `,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TareasCamposConfigComboComponent),
            multi: true
        }
    ]
})
export class TareasCamposConfigComboComponent extends ComboBoxComponent<TareaCampoConfigDto> implements OnInit {
    constructor(service: TareaCampoConfigService, injector: Injector) {
        super(service, injector);
    }   
}
