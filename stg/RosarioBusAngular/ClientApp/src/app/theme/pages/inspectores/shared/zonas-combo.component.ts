import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { ZonasDto, ZonasFilter } from '../model/zonas.model';
import { ZonasService } from '../zonas/zonas.service';
import { CreateOrEditGruposInspectoresModalComponent } from '../gruposinspectores/create-or-edit-gruposinspectores-modal.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditZonasModalComponent } from '../zonas/create-or-edit-zonas-modal.component';


@Component({
    selector: 'zonas-combo',
    template: `
    <button *ngIf="showAddButton"
            type="button"
            class="smallesttext btn btn-primary blue"
            (click)="onAddButtonClick()">
            <i class="la la-plus"></i>
    </button>
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
            useExisting: forwardRef(() => ZonasComboComponent),
            multi: true
        }
    ]
})
export class ZonasComboComponent extends ComboBoxComponent<ZonasDto> implements OnInit {


    constructor(service: ZonasService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {

        //super.ngOnInit();
    }
    onSearch(): void {
        if (!this.BuscarZona) {
            super.onSearch();
        }
    }
    private _BuscarZona: boolean;
    get BuscarZona(): boolean {
        return this._BuscarZona;
    }

    @Input()
    set BuscarZona(buscarZona: boolean) {
        this._BuscarZona = buscarZona;
        if (!buscarZona) {
            this.onSearch();
        }
    }
    private _Anulado: number;
    get Anulado(): number {
        return this._Anulado;
    }

    @Input()
    set Anulado(anulado: number) {
        this._Anulado = anulado;
        if (anulado) {
            this.onSearch();
        }
    }

    protected GetFilter(): any {
        var f = { Anulado: this.Anulado };

        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<ZonasDto>> {
        return CreateOrEditZonasModalComponent;
    }

    getNewDto(): ZonasDto {
        return new ZonasDto();
    }
}
