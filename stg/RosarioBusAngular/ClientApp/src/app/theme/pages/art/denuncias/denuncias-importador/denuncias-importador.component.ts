import { Component, ViewEncapsulation, ViewChild, Injector, ComponentFactoryResolver, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
import { DenunciasService } from '../denuncias.service';
import { NotificationService } from '../../../../../shared/notification/notification.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DenunciasFilter } from '../../model/denuncias.model';

@Component({
    templateUrl: "./denuncias-importador.component.html",
    styleUrls: ['./denuncias-importador.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class DenunciasImportadorComponent {

    frozenCols: any[];
    anyErrores: boolean = false;
    protected cfr: ComponentFactoryResolver;

    planillaId: string;
    isFileSelected: boolean = false;
    isLoading: boolean = false;
    items: DenunciaImportadorDTO[];

    fileUpload: any;
    @ViewChild('fileInput') myInput;

    myControl = new FormControl();

    constructor(injector: Injector,
        private service: DenunciasService,
        public dialogRef: MatDialogRef<DenunciaImportadorDTO>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private notificationService: NotificationService) {

        this.cfr = injector.get(ComponentFactoryResolver);
        this.frozenCols = [
            { field: 'Errores', header: 'Errores' },
        ];
    }

    close(): void {
        this.dialogRef.close(false);
    }

    onSeleccionarArchivoExcelClick(): void {
        this.myInput.nativeElement.click();
    }

    onFileInputChange(event: any): void {
        try {
            this.fileUpload = event.target.files[0];
            this.onImportarClick();
        } catch (e) {
            this.fileUpload = null;
        }
    }

    onBorrarClick(event: any): void {
        this.myControl.setValue(null);
        this.fileUpload = null;
        this.planillaId = null;
        this.items = [];
    }

    onImportarClick(): void {
        this.myControl.setValue(null);
        if (this.fileUpload) {
            this.isLoading = true;
            this.service.UploadExcel(this.fileUpload)
                .finally(() => {
                    this.isLoading = false;
                    this.fileUpload = null;
                })
                .subscribe(response => {
                    this.isLoading = false;
                    this.planillaId = response.DataObject;
                    this.anyErrores = false;
                    this.ReloadPlanilla();
                });
        }
    }

    ReloadPlanilla(): any {

        this.items = [];

        this.isLoading = true;

        let filter = new DenunciaImportadorFileFilter();

        filter.PlanillaId = this.planillaId;
        this.service.RecuperarPlanilla(filter)
            .finally(() => this.isLoading = false)
            .subscribe(response => {

                this.items = response.DataObject;
                this.items.forEach(e => {
                    if (e.Errors && e.Errors.length >= 1) {
                        this.anyErrores = true;
                    }

                });
                this.isLoading = false;
            });
    }


    onProcesarClick(): void {
        if (this.items.filter(e => e.Errors.length > 0).length > 0) {
            this.notificationService.error("Verifique errores y vuelva a importar el excel.");
            return;
        }


        var input = new DenunciaImportadorFileFilter();
        input.PlanillaId = this.planillaId;

        this.isLoading = true;

        this.service.ImportarDenuncias(input).finally(() => {
            this.isLoading = false;
        }).subscribe(e => {
            this.notificationService.success("Se importo la planilla con exito!");
            this.dialogRef.close(true);

        });

    }

}

export class DenunciaImportadorDTO {
    PrestadorMedico: string;
    NroDenuncia: string;
    Estado: string;
    Empresa: string;
    EmpleadoDNI: string;
    EmpleadoCUIL: string;
    FechaOcurrencia: Date;
    FechaRecepcionDenuncia: Date;
    Contingencia: string;
    Diagnostico: string;
    Patologia: string;
    FechaBajaServicio: Date;
    Tratamiento: string;
    FechaUltimoControl: Date;
    FechaProximaConsulta: Date;
    FechaAudienciaMedica: Date;
    MotivoAudiencia: string;
    PorcentajeIncapacidad: number;
    FechaAltaMedica: Date;
    FechaAltaLaboral: Date;
    FechaProbableAlta: Date;
    NroDenunciaOrigen: string;
    NroSiniestro: string;
    Observaciones: string;
    MotivoNotificacion: string;
    FechaNotificacion: Date;
    ObservacionesNotificacion: string;
    IsValid: boolean;
    Errors: string[];

    EstadoId: number;
    EmpresaId: number;
    EmpleadoId: number;
    PrestadorMedicoId: number;
    SucursalId: number;
    ContingenciaId: number;
    PatologiaId: number;
    TratamientoId: number;
    MotivoAudienciaId: number;
    MotivoNotificacionId: number;
    DenunciaOrigenId: number;
    SiniestroId: number;
    EmpleadoAntiguedad: Date;
    EmpleadoArea: string;
    EmpleadoEmpresaId: number;
    EmpleadoFechaIngreso: Date;
    EmpleadoLegajo: string;
    CantidadDiasBaja: number;
}

export class DenunciaImportadorFileFilter {
    PlanillaId: string;
}