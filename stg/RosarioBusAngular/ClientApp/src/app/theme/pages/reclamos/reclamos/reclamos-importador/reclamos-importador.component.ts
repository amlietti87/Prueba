import { Component, ViewEncapsulation, ViewChild, Injector, ComponentFactoryResolver, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
import { NotificationService } from '../../../../../shared/notification/notification.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ReclamosService } from '../../../siniestros/reclamos/reclamos.service';

@Component({
    templateUrl: "./reclamos-importador.component.html",
    styleUrls: ['./reclamos-importador.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class ReclamosImportadorComponent {

    frozenCols: any[];
    anyErrores: boolean = false;
    protected cfr: ComponentFactoryResolver;

    planillaId: string;
    isFileSelected: boolean = false;
    isLoading: boolean = false;
    items: ReclamoImportadorDTO[];

    fileUpload: any;
    @ViewChild('fileInput') myInput;

    myControl = new FormControl();

    constructor(injector: Injector,
        private service: ReclamosService,
        public dialogRef: MatDialogRef<ReclamoImportadorDTO>,
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

        let filter = new ReclamoImportadorFileFilter();

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


        var input = new ReclamoImportadorFileFilter();
        input.PlanillaId = this.planillaId;

        this.isLoading = true;

        this.service.ImportarReclamos(input).finally(() => {
            this.isLoading = false;
        }).subscribe(e => {
            this.notificationService.success("Se importo la planilla con exito!");
            this.dialogRef.close(true);

        });

    }

}

export class ReclamoImportadorDTO {
    TipoReclamo: string;
    Estado: string;
    SubEstado: string;
    UnidadNegocio: string;
    Empresa: string;
    FechaReclamoExcel: string;
    NroDenuncia: string;
    EmpleadoDNI: string;
    EmpleadoCUIL: string;
    MontoDemandadoExcel: string;
    FechaOfrecimientoExcel: string;
    MontoOfrecidoExcel: string;
    MontoReconsideracionExcel: string;
    CausaReclamo: string;
    Hechos: string;
    FechaPagoExcel: string;
    MontoPagadoExcel: string;
    MontoFranquiciaExcel: string;
    Abogado: string;
    HonorariosAbogadoActorExcel: string;
    HonorariosMediadorExcel: string;
    HonorariosPeritoExcel: string;
    JuntaMedicaExcel: string;
    PorcIncapacidad: string;
    TipoAcuerdo: string;
    RubroSalarial: string;
    MontoTasasJudicialesExcel: string;
    Observaciones: string;
    ObsConvExtrajudicial: string;
    Autos: string;
    NroExpediente: string;
    Juzgado: string;
    AbogadoEmpresa: string;

    //Mapping
    FechaReclamo: Date;
    MontoDemandado: number;
    FechaOfrecimiento: Date;
    MontoOfrecido: number;
    MontoReconsideracion: number;
    FechaPago: Date;
    MontoPagado: Date;
    MontoFranquicia: number;
    HonorariosAbogadoActor: number;
    HonorariosMediador: number;
    HonorariosPerito: number;
    MontoTasasJudiciales: number;

    IsValid: boolean;
    Errors: string[];
}

export class ReclamoImportadorFileFilter {
    PlanillaId: string;
}