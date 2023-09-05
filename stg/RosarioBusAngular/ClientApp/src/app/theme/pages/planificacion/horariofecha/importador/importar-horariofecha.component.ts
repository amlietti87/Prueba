import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, OnDestroy, ComponentFactoryResolver, ElementRef } from '@angular/core';
import { PlaDistribucionDeCochesPorTipoDeDiaService } from '../HFechasConfi.service';
import { DetailEmbeddedComponent } from '../../../../../shared/manager/detail.component';
import { PlaDistribucionDeCochesPorTipoDeDiaDto, PlaDistribucionDeCochesPorTipoDeDiaFilter, HMediasVueltasImportadaDto, ImportarServiciosInput } from '../../model/HFechasConfi.model';
import { FormControl } from '@angular/forms';
import { BanderaCartelDto, BanderaCartelFilter } from '../../model/banderacartel.model';
import { BanderaCartelService } from '../../banderacartel/banderacartel.service';
import { SelectItem } from 'primeng/primeng';

@Component({
    selector: 'importarhorariofecha',
    templateUrl: "./importar-horariofecha.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class ImportarHorarioFechaComponent extends DetailEmbeddedComponent<PlaDistribucionDeCochesPorTipoDeDiaDto> implements OnInit {

    active = true;
    nroStep: number = 1;
    banderaCartel: BanderaCartelDto;
    banderasCartel: string;
    banderasPosicionamiento: string;
    mobile: SelectItem[];
    tipodia: string;
    autoLoad: boolean = false;

    _desde: number;
    set desde(desde: number) {
        this._desde = desde;
        this.FiltrarServicios();
    }
    get desde(): number {
        return this._desde;
    }


    _hasta: number;
    set hasta(hasta: number) {
        this._hasta = hasta;
        this.FiltrarServicios();
    }
    get hasta(): number {
        return this._hasta;
    }


    _ingresaRango: boolean;
    set ingresaRango(ingresaRango: boolean) {
        this._ingresaRango = ingresaRango;
        this.FiltrarServicios();
    }
    get ingresaRango(): boolean {
        return this._ingresaRango;
    }

    fileUpload: any;
    ivuUpload: any;
    @ViewChild('fileInput') myInput;
    @ViewChild('ivuInput') ivuInput;
    myControl = new FormControl();
    ivuControl = new FormControl();
    PlanillaId: string;
    items: HMediasVueltasImportadaDto[] = [];
    private allItems: HMediasVueltasImportadaDto[] = [];
    isLoadingMediaVueltas: boolean;
    isLoadingCarteles: boolean;



    constructor(
        protected service: PlaDistribucionDeCochesPorTipoDeDiaService,
        protected banderaCartelService: BanderaCartelService,
        injector: Injector
    ) {
        super(service, injector);
        this.banderaCartel = new BanderaCartelDto({ BolBanderasCartelDetalle: [] });

        this.mobile = [
            { label: 'SI', value: 'S' },
            { label: 'NO', value: 'N' },
        ];


    }

    uploadIvu(event) {
        try {

            this.ivuUpload = event.target.files;
            this.uploadPlanillaIvu();
        } catch (e) {
            this.ivuUpload = null;
        }
    }

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

    ElegirArchivoIvu(): void {
        this.ClearFile();
        this.ivuInput.nativeElement.click();
    }

    completedataBeforeShow(item: PlaDistribucionDeCochesPorTipoDeDiaDto): any {
        this.tipodia = item.TipoDediaDescripcion;
        super.completedataBeforeShow(item);
    }

    uploadPlanillaIvu() {

        //this.ivuControl.setValue(null);
        if (this.ivuUpload) {
            this.isLoadingMediaVueltas = true;
            this.service.uploadPlanillaIvu(this.ivuUpload)
                .finally(() => this.isLoadingMediaVueltas = false)
                .subscribe(response => {
                    this.PlanillaId = response.DataObject;
                    this.ReloadPlanilla();
                    //this.notificationService.info(response.DataObject);
                });
        }

    }

    uploadPlanilla() {
        this.myControl.setValue(null);
        if (this.fileUpload) {
            this.isLoadingMediaVueltas = true;
            this.service.uploadPlanilla(this.fileUpload)
                .finally(() => this.isLoadingMediaVueltas = false)
                .subscribe(response => {
                    this.PlanillaId = response.DataObject;
                    this.ReloadPlanilla();
                    //this.notificationService.info(response.DataObject);
                });
        }

    }


    FiltrarServicios() {
        //this.itemsFiltered = [];
        if (this.ingresaRango) {
            this.items = this.allItems.filter((v, i) => parseInt(v.NumServicio) >= (this.desde || 0) && parseInt(v.NumServicio) <= (this.hasta || Number.MAX_SAFE_INTEGER));
        }
        else {
            this.items = this.allItems;
        }

    }


    ClearFile(): void {
        this.myControl.setValue(null);
        this.ivuControl.setValue(null);
        this.fileUpload = null;
        this.PlanillaId = null;
        this.ivuUpload = null;
        this.items = [];
        this.allItems = [];
        this.nroStep = 1;
        this.banderaCartel = new BanderaCartelDto({ BolBanderasCartelDetalle: [] });
    }

    ReloadPlanilla(): any {
        this.items = [];
        this.allItems = [];
        this.isLoadingMediaVueltas = true;

        let filter = new PlaDistribucionDeCochesPorTipoDeDiaFilter();

        filter.PlanillaId = this.PlanillaId;
        filter.CodHfecha = this.detail.CodHfecha;

        this.service.RecuperarPlanilla(filter)
            .finally(() => this.isLoadingMediaVueltas = false)
            .subscribe(response => {
                this.ingresaRango = false;
                this.desde = null;
                this.hasta = null;
                this.allItems = response.DataObject;
                this.FiltrarServicios();
            });
    }

    PreviousStep() {
        this.nroStep = 1;
    }

    NextStep() {
        if (this.items.filter(e => e.Errors.length > 0).length > 0) {
            this.notificationService.error("Verifique errores y vuelva a importar el excel.");
            return;
        }
        this.nroStep = 2;
        this.ReloadCarteles();
    }
    ReloadCarteles(): any {

        if (this.detail.CodHfecha) {

            this.banderaCartel = new BanderaCartelDto({ BolBanderasCartelDetalle: [] });

            var filtro = new BanderaCartelFilter();
            filtro.CodHfecha = this.detail.CodHfecha;
            filtro.PlanillaId = this.PlanillaId;

            this.isLoadingCarteles = true;
            this.banderaCartelService.RecuperarCartelPorImportador(filtro)
                .finally(() => {
                    this.isLoadingCarteles = false;
                })
                .subscribe(e => {
                    this.banderaCartel = e.DataObject;

                    var array = this.banderaCartel.BolBanderasCartelDetalle.filter(e => !e.EsPosicionamiento).map(item => item.AbrBan);

                    //const unique = [...new Set(s)];
                    var flags = [], output = [], l = array.length, i;
                    for (i = 0; i < l; i++) {
                        if (flags[array[i]]) continue;
                        flags[array[i]] = true;
                        output.push(array[i]);
                    }
                    this.banderasCartel = output.join(" - ");

                    var array = this.banderaCartel.BolBanderasCartelDetalle.filter(e => e.EsPosicionamiento).map(item => item.AbrBan);

                    //const unique = [...new Set(s)];
                    var flags = [], output = [], l = array.length, i;
                    for (i = 0; i < l; i++) {
                        if (flags[array[i]]) continue;
                        flags[array[i]] = true;
                        output.push(array[i]);
                    }
                    this.banderasPosicionamiento = output.join(" - ");


                });
        }



    }

    Procesar() {
        if (this.items.filter(e => e.Errors.length > 0).length > 0) {
            this.notificationService.error("Verifique errores y vuelva a importar el excel.");
            return;
        }


        var input = new ImportarServiciosInput();
        input.PlanillaId = this.PlanillaId;
        input.BolBanderasCartel = this.banderaCartel;
        input.CodHfecha = this.detail.CodHfecha
        input.CodLinea = this.banderaCartel.CodLinea;
        if (this.ingresaRango) {
            input.desde = this.desde;
            input.hasta = this.hasta;
        }
        input.CodTdia = this.detail.CodTdia;


        this.isLoadingCarteles = true;
        this.service.ImportarServicios(input).finally(() => {
            this.isLoadingCarteles = false;
        }).subscribe(e => {
            this.notificationService.success(this.detail.TipoDediaDescripcion, e.DataObject || "Se importo la planilla con exito!");
            this.modalSave.emit(true);
            this.close();

        });

    }

}