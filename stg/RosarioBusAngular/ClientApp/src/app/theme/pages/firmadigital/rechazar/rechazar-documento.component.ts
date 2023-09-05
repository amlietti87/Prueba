declare let mApp: any;
import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ChangeDetectorRef, ComponentFactoryResolver, Inject } from '@angular/core';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent, DetailModalComponent } from '../../../../shared/manager/detail.component';
import { DocumentosProcesadosDto, DocumentosProcesadosFilter, ArchivosTotalesPorEstado, VisorArchivos, AplicarAccioneResponseDto } from '../model/documentosprocesados.model';
import { DocumentosProcesadosService } from '../services/documentosprocesados.service';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';
import { environment } from '../../../../../environments/environment';
import { DocumentosErrorDto, DocumentosErrorFilter } from '../model/documentoserror.model';
import { DocumentosErrorService } from '../services/documentosconerror.service';
import { FdAccionesService } from '../services/fdacciones.service';
import { AplicarAccionDto, RechazarDto } from '../model/aplicaraccion.model';
import { NotificationService } from '../../../../shared/notification/notification.service';
import { ErrorResponseAplicarAccionModalComponent } from '../errorResponse/errorResponse.component';
import { MatDialogConfig, MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { NgForm } from '@angular/forms';

@Component({

    templateUrl: "./rechazar-documento.component.html",
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./rechazar-documento.component.css']
})
export class RechazarDocumentoComponent implements OnInit {

    ngOnInit(): void {

    }
    protected cfr: ComponentFactoryResolver;

    detail: RechazarDto;

    constructor(
        injector: Injector,

        public dialogRef: MatDialogRef<RechazarDocumentoComponent>,
        @Inject(MAT_DIALOG_DATA) public data: RechazarDto
    ) {

        this.cfr = injector.get(ComponentFactoryResolver);
        this.detail = data;
    }


    save(form: NgForm): void {
        if (form && form.form.invalid) {
            return;
        }


        this.dialogRef.close(this.detail);
    }
    close(): void {

        this.dialogRef.close(false);
    }


}

