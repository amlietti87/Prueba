import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, Input, SimpleChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Helpers } from '../../../../helpers';
import { ScriptLoaderService } from '../../../../services/script-loader.service';
import { AppComponentBase } from '../../../../shared/common/app-component-base';

import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';

import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { SentidoBanderaDto } from '../model/sentidoBandera.model';

import { HorariosPorSectorDto } from '../model/horariosPorSector.model';
import { BanderaService } from '../bandera/bandera.service';
import { BanderaDto, BanderaFilter } from '../model/bandera.model';
import { ItemDto, ResponseModel, SabanaViewMode } from '../../../../shared/model/base.model';
import { SentidoBanderaService } from '../sentidoBandera/sentidoBandera.service';
import { retry } from 'rxjs/operators';
import { CreateOrEditHorariosPorSectorComponent } from './create-or-edit-horariosPorSector.component';
import { LineaDto } from '../model/linea.model';
import { moment } from 'ngx-bootstrap/chronos/test/chain';
import { SelectItem } from 'primeng/api';
import { LineaAutoCompleteComponent } from '../shared/linea-autocomplete.component';
import { SentidoBanderaComboComponent } from '../shared/sentidoBandera-combo.component';
import { BanderaComboComponent } from '../shared/bandera-combo.component';
import { NgForm } from '@angular/forms';
import { TipoDiaComboComponent } from '../shared/tipoDia-combo.component';
import { HFechasConfiComboComponent } from '../shared/hfechas_confi-combo.component';
import { saveAs } from 'file-saver'
import { MatDialog, MatDialogConfig } from '@angular/material';
import { ExportarsabanaComponent } from './exportarsabana.component';

@Component({

    templateUrl: "./horariosPorSector.component.html",
    styleUrls: ["./horariosPorSector.component.css"],
    encapsulation: ViewEncapsulation.None,
})
export class HorariosPorSectorComponent extends BaseCrudComponent<BanderaDto, BanderaFilter> implements AfterViewInit {

    viewModeList: SabanaViewModeClass[];
    viewMode: SabanaViewModeClass;

    fecha: string;
    CodHfecha: number;
    linea: ItemDto;
    sentidoBandera: number;

    banderasRelacionadas: ItemDto[] = [];
    banderasRelacionadasList: ItemDto[] = [];
    servicio: number;
    horarios: HorariosPorSectorDto;
    CodTdia: number;
    LineaId: number;
    codBan: number;
    hfechasconfiautoLoad: boolean = false;

    ReporteSinMinutos: boolean = false;

    public dialog: MatDialog;

    @ViewChild('Linea') Linea: LineaAutoCompleteComponent;
    @ViewChild('SentidoBandera') SentidoBandera: SentidoBanderaComboComponent;
    @ViewChild('Bandera') Bandera: BanderaComboComponent;
    @ViewChild('SabanaFilterForm') filterForm: NgForm;
    @ViewChild('codTipoDiaComboDetino') codTipoDiaComboDetino: TipoDiaComboComponent;
    @ViewChild('cod_hfechaFiltroCombo') cod_hfechaFiltroCombo: HFechasConfiComboComponent;
    @ViewChild('banderaComboMinutos') banderaComboMinutos: BanderaComboComponent;

    replaceLineBreak(s: string) {

        return s && s.replace('-', '<br />');
    }


    constructor(injector: Injector,
        protected banderaService: BanderaService,
        protected sentidoBanderaService: SentidoBanderaService) {

        super(banderaService, CreateOrEditHorariosPorSectorComponent, injector);

        this.isFirstTime = true;
        this.title = " Sabana Bandera";
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
        this.showbreadcum = false;

        // Setup viewModeList (Sabana / Servicio)
        this.viewModeList = new Array<SabanaViewModeClass>();
        this.viewModeList.push({
            Id: 0,
            Descripcion: "Sábana"
        });
        this.viewModeList.push({
            Id: 1,
            Descripcion: "Servicio"
        });
        this.viewModeList.push({
            Id: 2,
            Descripcion: "Bandera"
        });


        // Define default viewMode
        this.viewMode = {
            Id: 0,
            Descripcion: "Sábana"
        };

        this.horarios = new HorariosPorSectorDto();
        this.fecha = moment(new Date()).toISOString();

        this.ReporteSinMinutos = this.permission.isGranted('Horarios.Sabana.ExportarSabana');

    }

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditHorariosPorSectorComponent;
    };

    // ViewMode Events
    OnSelectButtonOptionClick(event: any) {
        if (event) {

            this.servicio = null;
            this.codBan = null;
            this.sentidoBandera = null;
            this.banderasRelacionadas = [];
            this.horarios.Items = [];
        }
    }


    onSearch(event?: LazyLoadEventData) {
        super.onSearch(event);
    }

    // Events
    OnFechaChange(event: SimpleChanges) {
        //this.linea = null;
        this.sentidoBandera = null;
        this.codBan = null;
        this.banderasRelacionadas = [];
        this.banderasRelacionadasList = [];
        $(this.SentidoBandera.comboboxElement.nativeElement).selectpicker('refresh');
        $(this.Bandera.comboboxElement.nativeElement).selectpicker('refresh');
    };
    OnLineaChange(event: SimpleChanges) {
        this.horarios.Items = [];
        this.sentidoBandera = null;
        this.codBan = null;
        this.banderasRelacionadas = [];
        this.banderasRelacionadasList = [];
        if (this.linea) {
            this.LineaId = this.linea.Id;
        }


        if (this.SentidoBandera) {
            $(this.SentidoBandera.comboboxElement.nativeElement).selectpicker('refresh');
        }
        if (this.Bandera) {
            $(this.Bandera.comboboxElement.nativeElement).selectpicker('refresh');
        }
    };
    OnSentidoBanderaChange(event: SimpleChanges) {

        this.codBan = null;
        this.banderasRelacionadas = [];
        this.banderasRelacionadasList = [];
        if (this.Bandera) {
            $(this.Bandera.comboboxElement.nativeElement).selectpicker('refresh');
        }
    };
    OnBanderaChange(event: SimpleChanges) {
        this.horarios.Items = [];
        this.banderasRelacionadas = [];
        this.banderasRelacionadasList = [];
        if (this.Bandera) {
            $(this.Bandera.comboboxElement.nativeElement).selectpicker('refresh');
        }

        let banderasRelacionadasFilter = new BanderaFilter();
        banderasRelacionadasFilter.LineaId = this.linea.Id;
        banderasRelacionadasFilter.BanderaRelacionadaID = this.codBan;
        //banderasRelacionadasFilter.Fecha = this.fecha;
        banderasRelacionadasFilter.CodHfecha = this.CodHfecha;
        banderasRelacionadasFilter.CodTdia = this.CodTdia;

        this.banderaService.recuperarBanderasRelacionadasPorSector(banderasRelacionadasFilter).subscribe((response) => {
            if (response.Status === "Ok") {
                this.banderasRelacionadasList = [...response.DataObject];
            } else {
                this.notify.error('Ocurrió un error al obtener las Banderas Relacionadas');
            }
        })
    };

    // GetReporteExcel() {
    //     this.horarios.LabelBandera = this.banderaComboMinutos.items.find(e => e.Id == this.codBan).Description;

    //     if (this.banderasRelacionadas && this.banderasRelacionadas != null && this.banderasRelacionadas.length >= 1) {
    //         this.horarios.LabelBandera = this.horarios.LabelBandera + " (con Banderas Asociadas)";
    //     }

    //     this.horarios.Linea = this.linea.Description;
    //     this.horarios.TipoDia = this.codTipoDiaComboDetino.items.find(e => e.Id == this.CodTdia).Description;
    //     this.horarios.FechaHorario = this.cod_hfechaFiltroCombo.items.find(e => e.Id == this.CodHfecha).Description;
    //     this.horarios.FechaDesde = this.cod_hfechaFiltroCombo.items.find(e => e.Id == this.CodHfecha).FechaDesde;
    //     this.banderaService.GetReporteSabanaSinMinutos(this.horarios);
    // }

    ExportarExcel(): void {
        var dialog: MatDialog;
        this.dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.data = new HorariosPorSectorDto();
        dialogConfig.data.LabelBandera = this.banderaComboMinutos.items.find(e => e.Id == this.codBan).Description;
        if (this.banderasRelacionadas && this.banderasRelacionadas != null && this.banderasRelacionadas.length >= 1) {
            dialogConfig.data.LabelBandera = dialogConfig.data.LabelBandera + " (Con Banderas Asociadas)";
        }
        dialogConfig.data.Linea = this.linea.Description;
        dialogConfig.data.TipoDia = this.codTipoDiaComboDetino.items.find(e => e.Id == this.CodTdia).Description;
        dialogConfig.data.FechaHorario = this.cod_hfechaFiltroCombo.items.find(e => e.Id == this.CodHfecha).Description;
        dialogConfig.data.FechaDesde = this.cod_hfechaFiltroCombo.items.find(e => e.Id == this.CodHfecha).FechaDesde;
        dialogConfig.data.Items = this.horarios.Items;
        dialogConfig.data.Colulmnas = this.horarios.Colulmnas;
        dialogConfig.data.TipoInforme = 1;

        let dialogRef = this.dialog.open(ExportarsabanaComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(
            data => {
                if (data) {

                    this.Search();

                }

            }
        );
    }




    OnTipoDiaChange(event: SimpleChanges) {
        this.horarios.Items = [];
        //this.BuscarDetalleFiltro();

    };

    OnfechasconfiChange(event: SimpleChanges) {
        this.horarios.Items = [];
        //this.BuscarDetalleFiltro();
    };


    BuscarDetalleFiltro() {
        //if (this.viewMode.Id == SabanaViewMode.Bandera) {
        //    this.BuscarBanderaPorHfechasConfig();
        //}
    }



    BuscarBanderaPorHfechasConfig() {

        //var banderafiltro = new BanderaFilter();

        //this.banderaService.requestAllByFilter(banderafiltro).subscribe(s => {
        //    this.banderasList = s.DataObject.Items;
        //});

    }






    getNewfilter(): BanderaFilter {
        var f = new BanderaFilter();
        return f;
    }

    Search(event?: LazyLoadEventData) {

        //event.first = Index of the first record
        //event.rows = Number of rows to display in new page
        //event.page = Index of the new page
        //event.pageCount = Total number of pages




        if (this.isFirstTime == false) {
            this.isFirstTime = true;
            return;
        }

        if (!this.filter) {
            this.filter = this.getNewfilter();
        }


        this.filterForm.onSubmit(null);

        if (!this.filterForm.valid) {
            return;
        }



        var filtro = new BanderaFilter();


        if (this.viewMode.Id == SabanaViewMode.Sabana) {
            if (!this.CodHfecha || !this.CodTdia || !this.linea || !this.codBan) {
                this.notify.error('Falta Información!');
                return;
            }
            filtro.NoDescartarPrimeryUltimoMV = true;
            filtro.CodHfecha = this.CodHfecha;
            filtro.CodTdia = this.CodTdia;
            filtro.LineaId = this.linea.Id;
            filtro.SentidoBanderaId = this.sentidoBandera;
            filtro.BanderasSeleccionadas = [];
            filtro.BanderasSeleccionadas.push(this.codBan);
            if (this.banderasRelacionadas) {
                for (var i = 0; i < this.banderasRelacionadas.length; i++) {
                    filtro.BanderasSeleccionadas.push(this.banderasRelacionadas[i].Id);
                };
            }

        } else if (this.viewMode.Id == SabanaViewMode.Servicio) {
            if (!this.fecha || !this.linea || !this.servicio) {
                this.notify.error('Falta Información!');
                return;
            }

            filtro.CodHfecha = this.CodHfecha;
            filtro.LineaId = this.linea.Id;
            filtro.CodTdia = this.CodTdia;
            filtro.cod_servicio = this.servicio;
            filtro.BanderasSeleccionadas = [];
            if (this.codBan)
                filtro.BanderasSeleccionadas.push(this.codBan);
        }
        else {

            filtro.CodHfecha = this.CodHfecha;
            filtro.LineaId = this.linea.Id;
            filtro.CodTdia = this.CodTdia;
            filtro.BanderasSeleccionadas = [];
            filtro.BanderasSeleccionadas.push(this.codBan);

        }

        filtro.ShowDecimalValues = true;

        this.isTableLoading = true;

        this.primengDatatableHelper.showLoadingIndicator();

        filtro.ValidarMediasVueltasIncompletas = true;
        this.banderaService.recuperarHorariosSectorPorSector(filtro)
            .finally(() => {
                this.primengDatatableHelper.hideLoadingIndicator()
            })
            .subscribe(e => {
            this.horarios = e.DataObject;
            this.primengDatatableHelper.records = e.DataObject.Items
            this.primengDatatableHelper.totalRecordsCount = e.DataObject.Items.length;

        });
    }
}

class SabanaViewModeClass {
    Id: number;
    Descripcion: string;
}