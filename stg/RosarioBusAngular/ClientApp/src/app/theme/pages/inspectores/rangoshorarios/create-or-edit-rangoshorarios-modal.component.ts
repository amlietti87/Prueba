import { NovedadesComboComponent } from './../shared/novedades-combo.component';
import { HoraDto } from './../model/rangoshorarios.model';
import { RangosHorariosService } from './rangoshorarios.service';
import { RangosHorariosDto } from '../model/rangoshorarios.model';
import { Component, ViewChild, Injector, ViewContainerRef, ComponentFactoryResolver, OnInit, Inject, ChangeDetectorRef } from '@angular/core';
import * as _ from 'lodash';
import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ViewMode } from '../../../../shared/model/base.model';

@Component({
    selector: 'createOrEditRangosHorariosDtoModal',
    templateUrl: './create-or-edit-rangoshorarios-modal.component.html',
    styleUrls: ['./create-or-edit-rangoshorarios-modal.component.css']
})

export class CreateOrEditRangosHorariosModalComponent extends DetailAgregationComponent<RangosHorariosDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;

    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    @ViewChild('NovedadId') NovedadId: NovedadesComboComponent;

    allowmodificarturno: boolean = false;
    allowNovedad: boolean = false;
    allowHoraDesde: boolean = false;
    allowHoraHasta: boolean = false;
    isDisabled: boolean = false;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditRangosHorariosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: RangosHorariosDto,
        injector: Injector, protected service: RangosHorariosService,
        private cdr: ChangeDetectorRef) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;
        this.allowmodificarturno = this.permission.isGranted("Inspectores.RangosHorarios.Modificar");
    }

    horaOnChange(detail: RangosHorariosDto) {
        if (!(detail.HoraDesde && detail.HoraHasta)) return;

        if (detail.HoraDesde.hour && detail.HoraDesde.minute) {
            this.allowHoraDesde = false;
        }
        if (detail.HoraHasta.hour && detail.HoraHasta.minute) {
            this.allowHoraHasta = false;
        }
        detail.HasError = false;
        if (detail.HoraDesde.hour > detail.HoraHasta.hour) {
            detail.HasError = true;
            detail.ErrorMessages = "El turno termina al día siguiente";
        } else if (detail.HoraDesde.hour == detail.HoraHasta.hour) {
            if (detail.HoraDesde.minute > detail.HoraHasta.minute) {
                detail.HasError = true;
                detail.ErrorMessages = "El turno termina al día siguiente";
            }
        }
    }

    completedataBeforeShow(item: RangosHorariosDto): any {

        if (this.viewMode == ViewMode.Modify) {
            this.isDisabled = !this.detail.EsFranco;
            this.NovedadId.refreshWithTimeout();
            this.cdr.detectChanges();
        }

        if (this.detail.Id == null) {
            this.detail.EsFranco = false;
            this.isDisabled = !this.detail.EsFranco;
            this.NovedadId.refreshWithTimeout();
            this.cdr.detectChanges();
        }

        if (this.detail.HoraDesde == null) {
            this.detail.HoraDesde = new HoraDto();
        }

        if (this.detail.HoraHasta == null) {
            this.detail.HoraHasta = new HoraDto();
        }

        if (this.detail.HoraDesde.fecha == null) {
            this.detail.HoraDesde.fecha = new Date(2000, 1, 1);
        }

        if (this.detail.HoraHasta.fecha == null) {
            this.detail.HoraHasta.fecha = new Date(2000, 1, 1);
        }

    }

    OnChangeEsFranco($event) {
        if ($event) {
            this.clearHoras();
            this.isDisabled = !this.detail.EsFranco;
        } else {
            this.detail.EsFrancoTrabajado = $event;
            this.detail.NovedadId = null;
            this.isDisabled = !this.detail.EsFranco;
        }
        this.NovedadId.refreshWithTimeout();
        this.cdr.detectChanges();
    }

    clearHoras() {
        this.detail.HoraDesde = null;
        this.detail.HoraHasta = null;
        this.detail.ErrorMessages = null;
        this.detail.HoraDesde = new HoraDto();
        this.detail.HoraHasta = new HoraDto();
    }

    save(form: NgForm): void {

        this.allowNovedad = this.detail.EsFranco && (this.detail.NovedadId == null || this.detail.NovedadId.toString() == "" || this.detail.NovedadId == undefined);
        this.allowHoraDesde = !this.detail.EsFranco && (this.detail.HoraDesde.hour == null || this.detail.HoraDesde.hour == undefined);
        this.allowHoraHasta = !this.detail.EsFranco && (this.detail.HoraHasta.hour == null || this.detail.HoraHasta.hour == undefined);

        if (this.allowNovedad || this.allowHoraDesde || this.allowHoraHasta) {
            return;
        }

        if (this.detailForm && this.detailForm.form.invalid) {
            return;
        }

        this.saving = true;
        this.completedataBeforeSave(this.detail);

        if (!this.validateSave()) {
            this.saving = false;
            return;
        }

        this.SaveDetail();
    }

    completedataBeforeSave(item: RangosHorariosDto): any {

        if (!item.HoraDesde.fecha) {
            item.HoraDesde.fecha = new Date(2000, 0, 1);
            item.HoraHasta.fecha = new Date(2000, 0, 1);
        }

        if (!item.HoraHasta.fecha) {
            item.HoraDesde.fecha = new Date(2000, 0, 1);
            item.HoraHasta.fecha = new Date(2000, 0, 1);
        }

        if (item.HoraDesde.hour > item.HoraHasta.hour) {

            item.HoraHasta.fecha = new Date(2000, 0, 2);
        }
    }

}



