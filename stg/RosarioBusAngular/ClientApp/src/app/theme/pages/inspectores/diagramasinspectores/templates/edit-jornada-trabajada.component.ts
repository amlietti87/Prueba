import { ZonasComboComponent } from './../../shared/zonas-combo.component';
// Angular
import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild, EventEmitter, Output, OnDestroy, Inject, Injector, ChangeDetectorRef } from '@angular/core';
import { InspectorDiaDto, ValidationResult } from '../../model/diagramasinspectores.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { NgForm } from '@angular/forms';
import * as moment from 'moment';
import { DiagramasInspectoresValidatorService } from '../diagramas-inspectores-validator.service';
import { PagaCambiaComboComponent } from '../../shared/pagacambia-combo.component';

@Component({
    selector: 'exportarminutosporsector',
    templateUrl: './edit-jornada-trabajada.component.html',
})
export class EditJornadaTrabajadaComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {

    detail: InspectorDiaDto;
    newZonaId: any;
    allowZona: boolean = false;
    allowPago: boolean = false;
    titulo: string;

    @ViewChild('editJornadaForm') editJornadaForm: NgForm;
    @ViewChild('ZonaComboComponent') ZonaComboComponent: ZonasComboComponent;
    @ViewChild('ComboPagaCambia') ComboPagaCambia: PagaCambiaComboComponent;

    constructor(
        protected dialogRef: MatDialogRef<EditJornadaTrabajadaComponent>,
        @Inject(MAT_DIALOG_DATA) public data: InspectorDiaDto,
        private _validator: DiagramasInspectoresValidatorService,
        injector: Injector,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
        this.detail = data;
    }

    ngOnInit() {

        if (this.detail.EsFrancoTrabajado) {
            this.titulo = 'Editar Franco Trabajado';


        } else if (this.detail.EsJornada) {
            this.titulo = 'Editar Jornada Trabajada';
        }
        //FORMATEAR HORA Fri Sep 10 2010 21:00:00 GMT-0300 (hora estándar de Argentina)
        let HoraDesdeModificadaValue = {
            hour: moment(this.detail.HoraDesdeModificada).hour(),
            minute: moment(this.detail.HoraDesdeModificada).minutes()
        }

        let HoraHastaModificadaValue = {
            hour: moment(this.detail.HoraHastaModificada).hour(),
            minute: moment(this.detail.HoraHastaModificada).minutes()
        }

        this.detail.HoraDesdeModificadaValue = HoraDesdeModificadaValue;
        this.detail.HoraHastaModificadaValue = HoraHastaModificadaValue;

        this.detail.Pago = this.data.Pago;


        //Setear zona
        this.ZonaComboComponent.itemstChange.subscribe(e => {

            if (this.ZonaComboComponent.items.find(z => z.Id == this.detail.ZonaId) == null)
                this.detail.ZonaId = null;
        })
    }

    ngAfterViewInit() {
    }

    ngOnDestroy(): void {
        //this.subs.forEach(e => e.unsubscribe());
    }

    close(): void {
        this.dialogRef.close(false);
    }

    saveChanges(form: NgForm) {

        this.allowZona = (this.detail.ZonaId == null || this.detail.ZonaId.toString() == "" || this.detail.ZonaId == undefined || this.detail.ZonaId.toString() == "null");

        this.allowPago = (this.detail.Pago == null || this.detail.Pago.toString() == "" || this.detail.Pago == undefined || this.detail.Pago.toString() == "null");

        if (this.allowZona || (this.allowPago && this.detail.EsFrancoTrabajado)) return;

        let fechaInicialMoment = moment(this.detail.diaMes.Fecha);
        let horaDesdeModificadaValue = this.editJornadaForm.controls['HoraDesdeModificadaValue'].value;
        let horaHastaModificadaValue = this.editJornadaForm.controls['HoraHastaModificadaValue'].value;
        this.detail.ZonaId = this.editJornadaForm.controls['ZonaId'].value;

        //Hora Desde Formateada
        let horaDesdeModificada = new Date(fechaInicialMoment.year(), fechaInicialMoment.month(), fechaInicialMoment.date(), horaDesdeModificadaValue.hour, horaDesdeModificadaValue.minute);
        this.detail.HoraDesdeModificada = horaDesdeModificada;

        //Hora Hasta Formateada 
        let horaHastaModificada = new Date(fechaInicialMoment.year(), fechaInicialMoment.month(), fechaInicialMoment.date(), horaHastaModificadaValue.hour, horaHastaModificadaValue.minute);
        this.detail.HoraHastaModificada = horaHastaModificada;

        this.detail.diaMesFecha = this.detail.diaMes.Fecha;

        let agregarUnDia: Boolean = false;

        // Validacion cambio de dia
        if (horaDesdeModificadaValue.hour > horaHastaModificadaValue.hour) {
            agregarUnDia = true;
            this.detail.HoraHastaModificada.setDate(this.detail.HoraHastaModificada.getDate() + 1);
        } else if (horaDesdeModificadaValue.hour == horaHastaModificadaValue.hour) {
            if (horaDesdeModificadaValue.minute > horaHastaModificadaValue.minute) {
                agregarUnDia = true;
                this.detail.HoraHastaModificada.setDate(this.detail.HoraHastaModificada.getDate() + 1);
            }
        }

        var fpe = new ValidationResult();
        fpe.isValid = true;
        var hfi = new ValidationResult();
        hfi.isValid = true;
        let hfg = new ValidationResult();
        hfg.isValid = true;

        fpe = this._validator.ValidateFeriadoPermiteHsExtras(this.detail, this.detail.diaMes, this.detail.listModel, this.detail.diasMesAP);
        hfi = this._validator.ValidateHorasFeriadoParaInspector(this.detail, this.detail.listModel, this.detail.diasMesAP);
        hfg = this._validator.ValidateHorasFeriadoPorGrupo(this.detail,this.detail.listModel, this.detail.diasMesAP);

        let hfti = new ValidationResult();
        hfti.isValid = true;
        let hftg = new ValidationResult();
        hftg.isValid = true;

        if (this.detail.EsFrancoTrabajado) {
            hfti = this._validator.ValidateHorasFrancoTrabajadoParaInspector(this.detail, this.detail.listModel, this.detail.diasMesAP);
            hftg = this._validator.ValidateHorasFrancoTrabajadoPorGrupo(this.detail,this.detail.listModel, this.detail.diasMesAP);
        }

        let hei = this._validator.ValidateHorasExtrasParaInspector(this.detail, this.detail.listModel, this.detail.diasMesAP);
        let heg = this._validator.ValidateHorasExtrasPorGrupo(this.detail,this.detail.listModel, this.detail.diasMesAP);

        if (!hfti.isValid || !hftg.isValid || !fpe.isValid || !hfi.isValid || !hfg.isValid || !hei.isValid || !heg.isValid) {



            if (!hfti.isValid) {
                this.notify.warn(hfti.Messages[0]);
                this.detail.validations.push(hfti);
            }
            if (!hftg.isValid) {
                this.notify.warn(hftg.Messages[0]);
                this.detail.validations.push(hftg);
            }
            if (!fpe.isValid) {
                this.notify.warn(fpe.Messages[0]);
                this.detail.validations.push(fpe);
            }
            if (!hfi.isValid) {
                this.notify.warn(hfi.Messages[0]);
                this.detail.validations.push(hfi);
            }
            if (!hfg.isValid) {
                this.notify.warn(hfg.Messages[0]);
                this.detail.validations.push(hfg);
            }
            if (!hei.isValid) {
                this.notify.warn(hei.Messages[0]);
                this.detail.validations.push(hei);
            }
            if (!heg.isValid) {
                this.notify.warn(heg.Messages[0]);
                this.detail.validations.push(heg);
            }

            return;
        }


        if (agregarUnDia) {
            //El turno termina al día siguiente
            this.message.confirm("", "El turno termina al dia siguiente", r => {
                if (r.value) {
                    this.dialogRef.close(this.detail);
                }
            })
        }
        else {
            this.dialogRef.close(this.detail);
        }

    }

}
