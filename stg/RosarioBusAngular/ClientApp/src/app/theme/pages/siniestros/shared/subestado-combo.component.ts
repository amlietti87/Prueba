import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { CausasService } from '../causas/causas.service';
import { CausasDto, SubCausasDto } from '../model/causas.model';
import { SubCausasService } from '../causas/subcausas.service';
import { SubEstadosDto, EstadosDto } from '../model/estados.model';
import { SubEstadosService } from '../estados/subestados.service';
import { DetailComponent } from '../../../../shared/manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';
import { CreateOrEditEstadosModalComponent } from '../estados/create-or-edit-estados-modal.component';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { EstadosService } from '../estados/estados.service';
import { Observable } from 'rxjs';
import { ResponseModel } from '../../../../shared/model/base.model';

@Component({
    selector: 'subestado-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SubEstadoComboComponent),
            multi: true
        }
    ]
})
export class SubEstadoComboComponent extends ComboBoxComponent<SubEstadosDto> implements OnInit {
    _estadoid: number;
    _subestadoid: number;
    _Cierre: boolean;
    estadosService: EstadosService;
    @Input()
    get EstadoId(): number {

        return this._estadoid;
    }

    set EstadoId(value: number) {
        this._estadoid = value;
        if (value && value != null) {
            this.onSearch();
        }
    }

    @Input()
    get SubEstadoId(): number {

        return this._subestadoid;
    }

    set SubEstadoId(value: number) {
        this._subestadoid = value;
        if (!(this.items.find(e => e.Id == value))) {
            this.onSearch();
        }
    }

    @Input()
    get Cierre(): boolean {
        return this._Cierre;
    }
    set Cierre(value: boolean) {
        this._Cierre = value;
        if (value) {
            this.onSearch();
        }
    }


    constructor(service: SubEstadosService,
        injector: Injector,
        estadosService: EstadosService) {

        super(service, injector);

        this.estadosService = estadosService;
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    protected GetFilter(): any {

        var f = {
            EstadoId: this.EstadoId,
            SubEstadoId: this.SubEstadoId,
            Cierre: this.Cierre,
            Anulado: false
        };

        return f;
    }

    onSearch(): void {

        var self = this;
        this.isLoading = true;
        this.service.requestAllByFilter(this.GetFilter()).subscribe(result => {
            this.items = result.DataObject.Items;
            self.isLoading = false;
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 200);
        });
    }

    onAddButtonClick() {
        var dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;

        this.getNewDto2().subscribe(e => {
            let dialogRef = dialog.open(this.getIDetailComponent2(), dialogConfig);
            dialogRef.componentInstance.showEdit(e.DataObject);

            dialogRef.afterClosed().subscribe(
                data => {
                    if (data) {
                        this.onSearch();
                        this.selectedItem = data.Id;
                    }

                }
            );

        });

    }

    getIDetailComponent2(): ComponentType<DetailComponent<EstadosDto>> {
        return CreateOrEditEstadosModalComponent;
    }

    getNewDto2(): Observable<ResponseModel<EstadosDto>> {
        var dto: EstadosDto = new EstadosDto();
        if (this.EstadoId) {
            return this.estadosService.getById(this.EstadoId);
        } else {
            return Observable.empty<ResponseModel<EstadosDto>>(); // aca esta el maneje
        }
    }
}
