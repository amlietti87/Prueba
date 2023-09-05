import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, OnDestroy, ComponentFactoryResolver, ElementRef, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
import { SelectItem } from 'primeng/primeng';
import { DetailEmbeddedComponent, DetailAgregationComponent } from '../../../../../../shared/manager/detail.component';




import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HMinxtipoDto, MinutosPorSectorDto, HMinxtipoFilter, HMinxtipoImportado, SectorImportador } from '../../../model/hminxtipo.model';

import { HChoxser, ChofXServImportado, HChoxserFilter } from '../../../model/hChoxser.model';
import { HChoxserService } from '../../../HChoxser/hChoxser.service';
import { HServiciosFilter } from '../../../model/hServicios.model';

@Component({
    selector: 'importar-duracion',
    templateUrl: "./importar-duracion.component.html",
    styleUrls: ['./importar-duracion.component.css'],
    encapsulation: ViewEncapsulation.None,
})
export class ImportarDuracionComponent extends DetailEmbeddedComponent<HChoxser> implements OnInit {

    active = true;
    autoLoad: boolean = false;
    isLoading: boolean;


    constructor(
        protected dialogRef: MatDialogRef<ImportarDuracionComponent>,
        @Inject(MAT_DIALOG_DATA) public data: HServiciosFilter,
        protected service: HChoxserService,
        injector: Injector) {

        super(service, injector);
        this.CodHfecha = data.CodHfecha;
        this.CodTdia = data.CodTdia;
        this.CodSubg = data.CodSubg;
        this.DescTdia = data.DescTdia;

    }

    fileUpload: any;
    @ViewChild('fileInput') myInput;

    myControl = new FormControl();
    PlanillaId: string;
    CodHfecha: number;
    CodSubg: number;
    CodTdia: number;
    DescTdia: string;
    resultadoImportacion: ChofXServImportado[];




    upload(event) {
        try {
            this.fileUpload = event.target.files[0];
            this.uploadPlanilla();
        } catch (e) {
            this.fileUpload = null;
        }
    }

    ElegirArchivo(): void {
        this.myInput.nativeElement.click();
    }


    uploadPlanilla() {

        this.myControl.setValue(null);
        if (this.fileUpload) {
            this.isLoading = true;
            this.service.uploadPlanilla(this.fileUpload)
                .subscribe(response => {
                    this.PlanillaId = response.DataObject;
                    this.ReloadPlanilla();
                    //this.notificationService.info(response.DataObject);
                });
        }
    }



    ClearFile(): void {
        this.myControl.setValue(null);
        this.fileUpload = null;
        this.PlanillaId = null;

    }


    ReloadPlanilla(): any {


        this.isLoading = true;

        let filter = new HChoxserFilter();

        filter.PlanillaId = this.PlanillaId;
        filter.CodHfecha = this.CodHfecha;
        filter.CodSubg = this.CodSubg;
        filter.CodTdia = this.CodTdia;


        this.service.RecuperarPlanilla(filter)
            .finally(() => this.isLoading = false)
            .subscribe(response => {
                this.resultadoImportacion = response.DataObject.List;
            });
    }








    Procesar() {
        if (this.resultadoImportacion.filter(e => e.Errors.length > 0).length > 0) {
            this.notificationService.error("Verifique errores y vuelva a importar el excel.");
            return;
        }


        let filter = new HChoxserFilter();

        filter.PlanillaId = this.PlanillaId;
        filter.CodHfecha = this.CodHfecha;
        filter.CodSubg = this.CodSubg;
        filter.CodTdia = this.CodTdia;


        this.isLoading = true;
        this.service.ImportarMinutos(filter).finally(() => {
            this.isLoading = false;
        }).subscribe(e => {
            this.notificationService.success("Se importo la planilla con exito!");
            this.dialogRef.close(true);

        });

    }

    close(): void {
        this.dialogRef.close(true);
    }


}