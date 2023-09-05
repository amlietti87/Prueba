import { CreateOrEditRespuestasModalComponent } from './../respuestasincognitos/create-or-edit-respuestas-modal.component';
import { RespuestasIncognitosService } from './../respuestasincognitos/respuestasIncognitos.service';
import { RespuestasIncognitosDto } from './../model/respuestasIncognitos.model';
import { Component, OnInit, Injector, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';

@Component({
    selector: 'respuestas-combo',
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
            useExisting: forwardRef(() => RespuestasComboComponent),
            multi: true
        }
    ]
})
export class RespuestasComboComponent extends ComboBoxComponent<RespuestasIncognitosDto> implements OnInit {


    constructor(service: RespuestasIncognitosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {

        //super.ngOnInit();
    }
    onSearch(): void {
        if (!this.BuscarRespuesta) {
            super.onSearch();
        }
    }
    private _BuscarRespuesta: boolean;
    get BuscarRespuesta(): boolean {
        return this._BuscarRespuesta;
    }

    @Input()
    set BuscarRespuesta(buscarRespuesta: boolean) {
        this._BuscarRespuesta = buscarRespuesta;
        if (!buscarRespuesta) {
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

    getIDetailComponent(): ComponentType<DetailComponent<RespuestasIncognitosDto>> {
        return CreateOrEditRespuestasModalComponent;
    }

    getNewDto(): RespuestasIncognitosDto {
        return new RespuestasIncognitosDto();
    }
}
