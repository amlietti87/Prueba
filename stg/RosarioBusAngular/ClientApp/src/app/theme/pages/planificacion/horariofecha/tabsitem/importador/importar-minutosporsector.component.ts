import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, OnDestroy, ComponentFactoryResolver, ElementRef, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
import { SelectItem } from 'primeng/primeng';
import { DetailEmbeddedComponent, DetailAgregationComponent } from '../../../../../../shared/manager/detail.component';
import { PlaDistribucionDeCochesPorTipoDeDiaDto, HMediasVueltasImportadaDto, PlaDistribucionDeCochesPorTipoDeDiaFilter, ImportarServiciosInput } from '../../../model/HFechasConfi.model';
import { BanderaCartelDto, BanderaCartelFilter } from '../../../model/banderacartel.model';
import { PlaDistribucionDeCochesPorTipoDeDiaService } from '../../HFechasConfi.service';
import { BanderaCartelService } from '../../../banderacartel/banderacartel.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HMinxtipoDto, MinutosPorSectorDto, HMinxtipoFilter, HMinxtipoImportado, SectorImportador } from '../../../model/hminxtipo.model';
import { HMinxtipoService } from '../../../hminxtipo/hminxtipo.service';


@Component({
    selector: 'importarhorariofecha',
    templateUrl: "./importar-minutosporsector.component.html",
    styleUrls: ['./importar-minutosporsector.component.css'],
    encapsulation: ViewEncapsulation.None,
})
export class ImportarMinutosPorSectorComponent extends DetailEmbeddedComponent<HMinxtipoDto> implements OnInit {

    active = true;
    autoLoad: boolean = false;
    isLoading: boolean;
    BanderaName: string;

    constructor(
        protected dialogRef: MatDialogRef<ImportarMinutosPorSectorComponent>,
        @Inject(MAT_DIALOG_DATA) public data: HMinxtipoDto,
        protected service: HMinxtipoService,
        injector: Injector) {

        super(service, injector);
        this.CodHfecha = data.CodHfecha;
        this.CodBan = data.CodBan;
    }





    fileUpload: any;
    @ViewChild('fileInput') myInput;

    myControl = new FormControl();
    PlanillaId: string;
    CodHfecha: number;
    CodBan: number;
    resultadoImportacion: HMinxtipoImportado[];
    sectores: SectorImportador[] = [];



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

        let filter = new HMinxtipoFilter();

        filter.PlanillaId = this.PlanillaId;
        filter.CodHfecha = this.CodHfecha;
        filter.CodBan = this.CodBan;

        this.service.RecuperarPlanilla(filter)
            .finally(() => this.isLoading = false)
            .subscribe(response => {

                this.resultadoImportacion = response.DataObject.HMinxtipoImportados;

                this.sectores = response.DataObject.Sectores;
            });
    }




    Procesar() {
        if (this.resultadoImportacion.filter(e => e.Errors.length > 0).length > 0) {
            this.notificationService.error("Verifique errores y vuelva a importar el excel.");
            return;
        }


        var input = new HMinxtipoFilter();
        input.PlanillaId = this.PlanillaId;

        input.CodHfecha = this.CodHfecha
        input.CodBan = this.CodBan;

        this.isLoading = true;
        this.service.ImportarMinutos(input).finally(() => {
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