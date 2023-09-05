import { Component, OnInit, Injector, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { RangosHorariosService } from '../rangoshorarios/rangoshorarios.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { RangosHorariosDto } from '../model/rangoshorarios.model';
import { CreateOrEditRangosHorariosModalComponent } from '../rangoshorarios/create-or-edit-rangoshorarios-modal.component';

@Component({
    selector: 'rangoshorarios-combo',
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
            useExisting: forwardRef(() => RangosHorariosComboComponent),
            multi: true
        }
    ]
})
export class RangosHorariosComboComponent extends ComboBoxComponent<RangosHorariosDto> implements OnInit {


    constructor(service: RangosHorariosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        //super.ngOnInit();
    }

    @Input()
    isInEditFranco: boolean = false;

    @Input()
    francoTrabajado: boolean = null;

    @Input()
    EsFranco: boolean = null;

    @Input()
    GrupoInspectoresId: number = null;

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

    onSearch(): void {
        if (!this.BuscarRangoHorario) {
            super.onSearch();
        }
    }

    onSearchFinish(): void {
        if (this.isInEditFranco && this.items.length == 1) {
            this.IsDisabled = true;
            this.value = this.items[0].Id;
        } else {
            this.IsDisabled = false;
        }
    }

    private _BuscarRangoHorario: boolean;
    get BuscarRangoHorario(): boolean {
        return this._BuscarRangoHorario;
    }

    @Input()
    set BuscarRangoHorario(buscarRangoHorario: boolean) {
        this._BuscarRangoHorario = buscarRangoHorario;
    }

    // RangoHorario
    private _RangoHorarioId: number;
    get RangoHorarioId(): number {
        return this._RangoHorarioId;
    }

    @Input()
    set RangoHorarioId(rangoHorarioId: number) {
        this._RangoHorarioId = rangoHorarioId;
        if (rangoHorarioId) {
            this.onSearch();
        }
    }


    protected GetFilter(): any {
        var f = {
            Anulado: this.Anulado,
            RangoHorarioId: this.RangoHorarioId,
            FrancoTrabajado: this.francoTrabajado,
            GrupoInspectoresId: this.GrupoInspectoresId,
            EsFranco: this.EsFranco,
        };

        if (this.GrupoInspectoresId != null) {
            f.GrupoInspectoresId = this.GrupoInspectoresId;
        }

        if (this.francoTrabajado != null) {
            f.FrancoTrabajado = this.francoTrabajado;
        }

        if (this.EsFranco != null) {
            f.EsFranco = this.EsFranco;
        }

        return f;
    }

    getIDetailComponent(): ComponentType<DetailComponent<RangosHorariosDto>> {
        return CreateOrEditRangosHorariosModalComponent;
    }

    getNewDto(): RangosHorariosDto {
        return new RangosHorariosDto();
    }
}
