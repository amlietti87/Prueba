import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit, Input, ComponentFactoryResolver, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import * as _ from 'lodash';
declare let mApp: any;
import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { RouterModule, Routes, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { DetailEmbeddedComponent, IDetailComponent } from '../../../../../shared/manager/detail.component';
import { HFechasConfiDto, PlaDistribucionDeCochesPorTipoDeDiaDto } from '../../model/HFechasConfi.model';
import { HServiciosFilter, HServiciosDto, ExportarExcelDto } from '../../model/hServicios.model';
import { MediasVueltasDto, MediasVueltasFilter } from '../../model/mediasvueltas.model';
import { HHorariosConfiDto, HHorariosConfiFilter } from '../../model/hhorariosconfi.model';
import { SubGalponDto } from '../../model/subgalpon.model';
import { TipoDiaDto } from '../../model/tipoDia.model';
import { SubgalponComboComponent } from '../../shared/subgalpon-combo.component';
import { TipoDiaComboComponent } from '../../shared/tipoDia-combo.component';
import { HFechasConfiService } from '../HFechasConfi.service';
import { HServiciosService } from '../../hservicio/servicio.service';
import { MediasVueltasService } from '../../mediasvueltas/mediasvueltas.service';
import { HHorariosConfiService } from '../../HHorariosConfi/hhorariosconfi.service';
import { BanderaDto, BanderaFilter } from '../../model/bandera.model';
import { HTposHorasDto } from '../../model/htposhoras.model';
import { HMinxtipoService } from '../../hminxtipo/hminxtipo.service';
import { HSectoresDto, HMinxtipoFilter, HMinxtipoDto, HDetaminxtipoDto, CopiarHMinxtipoInput } from '../../model/hminxtipo.model';
import { BanderaComboComponent } from '../../shared/bandera-combo.component';
import { HTposHorasService } from '../../htposhoras/htposhoras.service';
import { HTposHorasComboComponent } from '../../shared/htphoras-combo.component';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { ImportarMinutosPorSectorComponent } from './importador/importar-minutosporsector.component';
import { SelectItem } from 'primeng/api';
import { Dropdown } from 'primeng/primeng';
import { CopiarMinutosPorSectorComponent } from '../copiarMinutos/copiar-minutosporsector.component';
import { ExportarMinutosPorSectorComponent } from './exportador/exportar-minutosporsector.component';
import { event } from 'd3';
import { BanderaService } from '../../bandera/bandera.service';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { ViewMode } from '../../../../../shared/model/base.model';
//import { moment } from 'ngx-bootstrap/chronos/test/chain';


declare let moment: any;






@Component({
    selector: 'minutos-por-sector',
    templateUrl: './minutos-por-sector.component.html',
    styleUrls: ['./minutos-por-sector.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class MinutosPorSectorComponent extends AppComponentBase implements OnInit {


    public mask = "09.99";

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();



    @ViewChild('banderaComboMinutos') banderaCombo: BanderaComboComponent;
    @ViewChild('codTipoDiaCombo') codTipoDiaCombo: TipoDiaComboComponent;
    @ViewChild('tipoHoraCombo') FiltroTipoHoraCombo: Dropdown;
    @ViewChild('filterMinutosSecorForm') filterForm: NgForm;

    detail: HFechasConfiDto;

    EditEnabled: boolean = true;

    isLoadingMediaVueltas: boolean;
    isLoading: boolean;
    filterValue: HServiciosFilter;
    viewMode: ViewMode = ViewMode.Undefined;
    active: boolean;
    saving: boolean;

    @Input()
    get filter(): HServiciosFilter {
        return this.filterValue;
    }

    @Output() filterChange = new EventEmitter<HServiciosFilter>();
    set filter(val: HServiciosFilter) {
        this.filterValue = val;
        if (this.filterChange)
            this.filterChange.emit(this.filterValue);
    }


    minutos: HMinxtipoDto[];
    sectores: HSectoresDto[];

    htposhoras: HTposHorasDto[];

    hTposHorasDto: HTposHorasDto[];
    tipoDiaDto: TipoDiaDto[];
    banderas: BanderaDto[]
    htposhorasSelectItem: SelectItem[];

    allowGuardarMinutosPorSector: boolean = false;
    allowCopiarMinutosPorSector: boolean = false;
    allowImportarMinutosPorSector: boolean = false;
    allowExportarMinutosPorSector: boolean = false;

    public dialog: MatDialog;
    injector: Injector;

    getDescription(item: HFechasConfiDto): string {
        return item.Description;
    }


    constructor(injector: Injector,
        protected service: HFechasConfiService,
        protected _hMinxtipoService: HMinxtipoService,
        protected router: Router,
        private cfr: ComponentFactoryResolver,
        protected _HTposHorasService: HTposHorasService) {

        super(injector);

        this.detail = new HFechasConfiDto();
        //this.icon = "flaticon-clock-2";
        //this.title = "Minutos por Sector";
        this.dialog = injector.get(MatDialog);
        this.injector = injector;
        this.filter = new HServiciosFilter();
        this.allowGuardarMinutosPorSector = this.permission.isGranted("Horarios.FechaHorario.GuardarMinutosPorSector");
        this.allowCopiarMinutosPorSector = this.permission.isGranted("Horarios.FechaHorario.CopiarMinutosPorSector");
        this.allowImportarMinutosPorSector = this.permission.isGranted("Horarios.FechaHorario.ImportarMinutosPorSector");
        this.allowExportarMinutosPorSector = this.permission.isGranted("Horarios.FechaHorario.ExportarMinutosxSector")

    }

    @ViewChild('myTr') inputEl: ElementRef;

    UpDownFocus(row: HMinxtipoDto, rowData: HDetaminxtipoDto, $event) {
        var x = $event.which || $event.keyCode;
        if (x == 38 || x == 40) {
            $event.preventDefault();
        }
        if (x == 40) {
            var index = this.minutos.findIndex(e => e.Id == row.Id);
            if (index != this.minutos.length - 1) {
                var newIndex = index + 1;
                var newId = this.minutos[newIndex].Id;
                var orden = rowData.Orden;
                var clase = "." + newId + "-" + orden;

                var input = $($event.target);
                input[0].blur();
                setTimeout(e => {
                    var td = $(clase);
                    var input = td.children();
                    input.click();
                }, 0);
            }
        }

        if (x == 38) {
            var index = this.minutos.findIndex(e => e.Id == row.Id);
            var newIndex = index - 1;
            if (newIndex >= 0) {
                var newId = this.minutos[newIndex].Id;
                var orden = rowData.Orden;
                var clase = "." + newId + "-" + orden;

                var input = $($event.target);
                input[0].blur();
                setTimeout(e => {
                    var td = $(clase);
                    var input = td.children();
                    input.click();
                }, 0);
            }
        }

    }


    frozenCols: any[];

    scrollableCols: any[];


    focusFunction($event) {

        try {
            var input = $($event.target);

            if (input.attr("tabindex") == "-1") {
                input[0].blur();
                setTimeout(e => {
                    var tr = $(input).parents("tr");
                    tr.find(".firstCE").click()
                }
                    , 0);
            }
            else {
                $($event.target).select();
            }


        } catch (e) {
            console.log(e);
        }
    }



    ngOnInit(): void {

        //super.ngOnInit();

        this.frozenCols = [
            { field: 'TotalMin', header: 'Total' },
            { field: 'Suma', header: 'Suma' },
            //{ field: 'Dif.', header: 'Dif.' },
        ];


        // this.FiltroTipoHoraCombo.isLoading = true;

        this._HTposHorasService.requestAllByFilter({})
            .finally(() => {
                //this.FiltroTipoHoraCombo.isLoading = false;
            })
            .subscribe(result => {

                var ordenado = result.DataObject.Items.sort((a, b) => {
                    if (a.Orden < b.Orden) return -1;
                    else if (a.Orden > b.Orden) return 1;
                    else return 0;
                });
                // ordenado.unshift( {CodTpoHorabsas: null, Description: "Todos", DscTpoHora: "Todos", Id: null, Orden: null } );
                this.htposhoras = ordenado;
                this.htposhorasSelectItem = this.htposhoras.map(e => { return { label: e.DscTpoHora, value: e.Id } })
                this.htposhorasSelectItem.unshift({ label: "Todos", value: "" })
                if (this.htposhoras && this.htposhoras.length > 0) {
                    this.filter.TipoHora = "";
                }


            });


    }



    CodBanitemstChange(items: BanderaDto[]): void {
        if (items && items.length > 0) {
            this.banderas = items;
            this.filter.CodBan = items[0].Id;
            this.LlenarGrilla();
        }
    }

    tipoDiaitemstChange(items: TipoDiaDto[]): void {
        if (items && items.length > 0) {
            this.tipoDiaDto = items;
            this.filter.CodTdia = this.filter.CodTdia;
            this.LlenarGrilla();
        }
    }


    ngAfterViewInit(): void {
        mApp.initPortlets();
    }

    completedataBeforeShow(item: HFechasConfiDto): any {

        if (this.filter == null) {
            this.filter = new HServiciosFilter()
        }
        //this.filter = new HMinxtipoFilter();
        this.filter.CodHfecha = item.Id;
        this.filter.CodTdia = item.CodTDia;


        if (this.htposhoras && this.htposhoras.length > 0) {
            this.filter.TipoHora = this.htposhoras[0].Id;
        }

        this.banderaCombo.CodHfecha = item.Id;


        this.codTipoDiaCombo.CodHfecha = item.Id;
    }

    Buscar(): void {
        //this.filterForm.onSubmit(null);
        if (this.filterForm.valid) {
            this.LlenarGrilla();
        }
    }

    LlenarGrilla(): void {

        if (this.filter.CodBan && this.filter.CodTdia && this.filter.CodHfecha /*&& this.filter.TipoHora*/) {


            this.isLoading = true;
            this.isLoadingMediaVueltas = true;
            //Busco los datos


            this.minutos = [];
            this.sectores = [];
            var indexInput = 0;
            this._hMinxtipoService.GetMinutosPorSector(this.filter)
                .finally(() => {
                    this.isLoading = false;
                    this.isLoadingMediaVueltas = false;
                })
                .subscribe(e => {
                    this.minutos = e.DataObject.Minutos;
                    this.minutos.forEach(m => {
                        m.HDetaminxtipo.forEach(f => {
                            f.Orden = f.Orden;
                            if (f.Orden > 1) {
                                indexInput = indexInput + 1;
                                f.indexInput = indexInput;

                            }
                            f.MinutoOriginal = f.Minuto;
                            this.OnInputChanged(m, f);
                        }
                        );
                    }
                    );

                    this.minutos.forEach(m => {
                        m.HDetaminxtipo.forEach(f => {
                            f.Orden = f.Orden;
                            if (f.Orden == 1) {

                                indexInput = indexInput + 1;
                                f.indexInput = indexInput;

                            }
                            f.MinutoOriginal = f.Minuto;
                            this.OnInputChanged(m, f);
                        }
                        );
                    }
                    );
                    this.sectores = e.DataObject.Sectores;

                    this.scrollableCols = [];
                    this.sectores.forEach(e => {
                        this.scrollableCols.push({ field: e.Calle1 + '_' + e.Calle2, header: e.Calle1 + '_' + e.Calle2 });
                    })
                });

            //Busco los servicios para el tipo de dia y sub galpon
        }
    }

    private decimalValidValues: string[] = ["00", "15", "30", "45"];


    OnInputChanged(parent: HMinxtipoDto, rowData: HDetaminxtipoDto): void {
        parent.Suma = 0;
        var s = 0;
        var date = moment("2000-01-01");


        parent.HDetaminxtipo.forEach(f => {
            f.HasError = false;

            if (f.Minuto) {
                var minutostring = f.Minuto.toFixed(2).replace(",", ".").split(".");
                date.add(minutostring[0], "minutes");
                if (minutostring.length == 2) {
                    if (minutostring[1] != "00" && minutostring[1] != "0") {
                        date.add(minutostring[1], "seconds");
                        if (!this.decimalValidValues.includes(minutostring[1])) {
                            f.HasError = true;
                        }
                    }
                }
            }
        }
        );
        parent.Suma = parseFloat(date.format("mm.ss")) + (date.hours() * 60);

        if (date.dates() > 1) {
            parent.Suma = parseFloat(date.format("mm.ss")) + (date.hours() * 60) + ((date.date() - 1) * 24 * 60);
        }

        this.calcularDiferencia(parent);

        if (parent.HasError) {
            if (parent.Suma == parent.TotalMin) {
                parent.HasError = false;
            }
        }
    }

    private calcularDiferencia(item: HMinxtipoDto): void {
        try {
            var t = this.GetDecimalToSexagecimal(item.TotalMin);
            var s = this.GetDecimalToSexagecimal(item.Suma);
            var diff = t - s;
            item.Dif = this.GetSexagecimalToDecimal(diff);

        } catch (e) {

        }

    }

    private GetDecimalToSexagecimal(d: number): number {
        var minutostring = d.toFixed(2).replace(",", ".").split(".");
        let result = parseInt(minutostring[0]);
        if (minutostring.length == 2) {
            if (minutostring[1] != "00" && minutostring[1] != "0") {
                result = Math.abs(result) + ((parseInt(minutostring[1]) * 100 / 60) / 100);
            }
        }
        return d.toString().indexOf('-') != -1 ? result * -1 : result;
    }

    private GetSexagecimalToDecimal(d: number): number {
        var minutostring = d.toFixed(2).replace(",", ".").split(".");
        let result = parseInt(minutostring[0]);
        if (minutostring.length == 2) {
            if (minutostring[1] != "00" && minutostring[1] != "0") {
                result = Math.abs(result) + ((parseInt(minutostring[1]) * 60) / 100 / 100);
            }
        }
        return d.toString().indexOf('-') != -1 ? result * -1 : result;
    }



    private GetMomentToDecimal(d: number): any {

        var date = moment("2000-01-01");
        var minutostring = d.toFixed(2).replace(",", ".").split(".");
        date.add(minutostring[0], "minutes");
        if (minutostring.length == 2) {
            if (minutostring[1] != "00" && minutostring[1] != "0") {
                date.add(minutostring[1], "seconds");
            }
        }

        return date;
    }



    validateSave(): boolean {

        if (this.detail.PlaEstadoHorarioFechaId != 1) {
            var isvalid = this.ValidateMinutos(this.minutos);
            if (!isvalid) {

                var f = this.minutos.filter(e => e.HasError).map(function(a) { return '</br>• ' + a.TotalMin; }).join(' ');

                this.notify.warn("Total de minutos: " + f, 'Existen minutos con errores');
            }

            if (this.minutos.length <= 0) {
                isvalid = false;
                this.notify.warn('No hay minutos por guardar', "Minutos");
            }
        } else {
            isvalid = true;
        }

        return isvalid;
    }

    SaveDetail(): any {
        this.saving = true;
        this._hMinxtipoService.UpdateHMinxtipo(this.minutos)
            .finally(() => {
                this.saving = false;
            })
            .subscribe((t) => {


                this.notify.info('Guardado exitosamente');
                this.LlenarGrilla();

            });
    }


    ValidateMinutos(minutos: HMinxtipoDto[]): boolean {
        var isValid = true;



        minutos.forEach(e => {
            e.HasError = false;
            e.ErrorMessages = [];
        });

        minutos.forEach(f => {
            if (f.Suma != f.TotalMin) {
                f.HasError = true;
                f.ErrorMessages.push("Verifique el total");
                isValid = false;
            }

            if (f.HasError) {
                f.HasError = true;
                f.ErrorMessages.push("Existen minutos mal cargados");
                isValid = false;
            }

        });


        return isValid;
    }





    ImportarExcel(): void {

        if (!this.filter.CodBan) {
            this.message.error("Debe elegir una bandera", "Error");
            return;
        }

        if (!this.filter.CodHfecha) {
            this.message.error("Debe elegir una CodHfecha", "Error");
            return;
        }

        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.data = new HMinxtipoDto({ CodBan: this.filter.CodBan, CodHfecha: this.filter.CodHfecha });
        dialogConfig.width = '80%';

        let dialogRef = this.dialog.open(ImportarMinutosPorSectorComponent, dialogConfig);
        dialogRef.componentInstance.IsInMaterialPopupMode = true;

        var bandera = this.banderaCombo.items.find(e => e.Id == this.filter.CodBan);
        if (bandera) {
            dialogRef.componentInstance.BanderaName = bandera.AbrBan;
        }


        dialogRef.afterClosed().subscribe(
            data => {
                if (data) {

                    this.Buscar();

                }

            }
        );
    }


    CopiarMinutos(): void {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        var data = new CopiarHMinxtipoInput();
        data.FechaDestino = moment(this.detail.FechaDesde).format('DD/MM/YYYY');
        data.CodHfechaOrigen = this.filter.CodHfecha;
        data.CodHfechaDestino = this.filter.CodHfecha;
        data.LineaId = this.detail.CodLinea;
        dialogConfig.width = '90%';
        dialogConfig.data = data;



        let dialogRef = this.dialog.open(CopiarMinutosPorSectorComponent, dialogConfig);

        var bannull = { label: 'Seleccione...', value: null };
        dialogRef.componentInstance.htposhorasSelectItem = [];
        dialogRef.componentInstance.htposhorasSelectItem.push(bannull);

        this.htposhorasSelectItem.forEach(f => {
            dialogRef.componentInstance.htposhorasSelectItem.push(Object.assign({}, f));
        })

        dialogRef.componentInstance.BanderasSource = [];

        this.banderaCombo.items.forEach(f => {
            dialogRef.componentInstance.BanderasSource.push(Object.assign({}, f));
        })


        dialogRef.afterClosed().subscribe(
            data => {
                if (data) {
                    this.LlenarGrilla();
                }
            }
        );
    }

    ExportarExcel(): void {

        if (!this.filter.CodBan) {
            this.message.error("Debe elegir una bandera", "Error");
            return;
        }

        if (!this.filter.CodHfecha) {
            this.message.error("Debe elegir una CodHfecha", "Error");
            return;
        }

        var dialog: MatDialog;
        dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.data = new ExportarExcelDto();
        dialogConfig.data.CodHfecha = this.filter.CodHfecha;
        dialogConfig.width = '60%';

        let dialogRef = this.dialog.open(ExportarMinutosPorSectorComponent, dialogConfig);



        dialogRef.afterClosed().subscribe(
            data => {
                if (data) {

                    this.Buscar();

                }

            }
        );
    }

    cleanData(): void {
        //this.filter = new HMinxtipoFilter();
        this.filter = new HServiciosFilter();
        this.minutos = [];
        this.sectores = [];
        this.tipoDiaDto = [];
        this.banderas = [];
        this.minutos = [];
        this.sectores = [];
    }

    save(form: NgForm): void {

        this.saving = true;

        if (!this.validateSave()) {
            this.saving = false;
            return;
        }

        this.SaveDetail();
    }

    close(): void {
        this.active = false;
        this.viewMode = ViewMode.Undefined;
        this.modalClose.emit(true);
    }
}
