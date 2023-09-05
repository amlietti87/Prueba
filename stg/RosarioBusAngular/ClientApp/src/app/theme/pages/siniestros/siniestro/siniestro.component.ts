import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, SimpleChanges } from '@angular/core';
import { SiniestrosDto, SiniestrosFilter } from '../model/siniestro.model';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { SiniestroTabsComponent } from './tabs/create-or-edit-siniestro.component';
import { SiniestroService } from './siniestro.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { moment } from 'ngx-bootstrap/chronos/test/chain';
import { MatDatepickerInputEvent, MatDatepicker } from '@angular/material';
import { getDate, isFirstDayOfWeek } from 'ngx-bootstrap/chronos/utils/date-getters';
import { ConsecuenciasDto } from '../model/consecuencias.model';
import { ConsecuenciasService } from '../consecuencias/consecuencias.service';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { saveAs } from 'file-saver'
import { debounce } from 'rxjs/operator/debounce';
import { ViewMode } from '../../../../shared/model/base.model';
import { Helpers } from '../../../../helpers';
import { SubEstadosService } from '../estados/subestados.service';
import { SubEstadosDto } from '../model/estados.model';
declare var Snap: any;
declare var mina: any;

@Component({

    templateUrl: "./siniestro.component.html",
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./siniestro.component.css']
})
export class SiniestroComponent extends BaseCrudComponent<SiniestrosDto, SiniestrosFilter> implements OnInit, AfterViewInit {



    sub: Subscription;
    subQ: Subscription;
    customdetail: SiniestroTabsComponent;
    listSubEstados: SubEstadosDto[] = [];
    //impresion siniestros
    allowImprimirSiniestro: boolean = false;
    showCreate: boolean = true;

    GetEditComponent(): IDetailComponent {

        if (!this.loadInMaterialPopup && !this.detailElement) {
            this.detailComponentType = this.GetEditComponentType();
            var factory = this.cfr.resolveComponentFactory(this.detailComponentType);
            const ref = this['createOrEdit'].createComponent(factory);
            ref.changeDetectorRef.detectChanges();
            this.detailElement = ref.instance;

            this.detailElement.modalClose.subscribe(e => {
                this.active = true;
                if (this.list && this.list.length > 0) {
                    this.Search();
                }

            });

            this.detailElement.modalSave.subscribe(e => {
                this.active = true;
            });
        }

        this.detailElement as SiniestroTabsComponent;
        return this.detailElement;
    }

    constructor(injector: Injector,
        protected _ConsecuenciasService: ConsecuenciasService,
        protected _Service: SiniestroService,
        protected SubEstadoService: SubEstadosService,
        private _activatedRoute: ActivatedRoute) {
        super(_Service, SiniestroTabsComponent, injector);
        this.isFirstTime = true;
        this.icon = "fa fa-car"
        this.title = "Siniestros";

        var datemesatras = new Date();
        datemesatras.setMonth(datemesatras.getMonth() - 24);
        this.advancedFiltersAreShown = true;
        //this.filter.FechaSiniestroDesde = datemesatras;
        //this.filter.FechaSiniestroHasta = new Date();
        this.allowImprimirSiniestro = this.permission.isGranted('Siniestro.Siniestro.Imprimir');
        this.CargarSubEstados();
    }

    CargarSubEstados(): any {

        this.SubEstadoService.requestAllByFilter({ Anulado: false }).subscribe(result => {
            this.listSubEstados = result.DataObject.Items;
            this.listSubEstados.forEach((e) => { e.Descripcion = e.EstadoNombre + " " + e.Descripcion; });

        });
    }

    events: string[] = [];

    addEvent(type: string, event: MatDatepickerInputEvent<Date>) {
        this.events.push(`${type}: ${event.value}`);
    }

    public onDate(event): void { };

    ngAfterViewInit() { super.ngAfterViewInit(); }

    getNewfilter(): SiniestrosFilter {
        var f = new SiniestrosFilter();
        //f.Activo = true;
        f.CheckAllConsecuencias = true;
        return f;
    }

    listConsecuencias: Array<ConsecuenciasDto> = [];
    selectedConsecuencias: Array<ConsecuenciasDto> = [];
    selectedSubEstados: Array<SubEstadosDto> = [];

    ngOnInit() {
        this.isFirstTime = false;
        super.ngOnInit();
        $('body').addClass("m-brand--minimize m-aside-left--minimize");
        this.sub = this._activatedRoute.params.subscribe(params => {
            this.paramsSubscribe(params);
        }
        );

        this.subQ = this._activatedRoute.queryParams.subscribe(params => {
            this.paramsSubscribe(params);
        });

        // Temporal hasta que pase a ser un componente reutilizable y se cargue en su propio OnInit ↓ ab2018
        this._ConsecuenciasService.GetConsecuenciasSinAnular().subscribe(result => {
            this.listConsecuencias = result.DataObject;
        });
    }

    BorrarFiltros() {
        this.filter = new SiniestrosFilter();
        this.filter.CheckAllConsecuencias = true;
    }

    format(date) {
        return moment(date, "hh:mm:ss").format("HH:mm");
    }

    paramsSubscribe(params: any) {

        //this.breadcrumbsService.AddItem(this.title, this.icon, '', null, close);
        this.breadcrumbsService.defaultBreadcrumbs(this.title);
        this.active = true;

        if (params.id) {
            this.onEditID(params.id);
        }
    }

    onPrint(row: SiniestrosDto): void {

        var name = "SiniestroNro-" + row.NroSiniestro + ".pdf";
        Helpers.setLoading(true);
        this._Service.GenerateReport(row)
            .subscribe(blob => {
                saveAs(blob, name, {
                    type: 'text/plain;charset=windows-1252' // --> or whatever you need here
                })
                Helpers.setLoading(false)
            });
    }

    CloseChild(): void {

        var e = super.GetEditComponent();
        (e as SiniestroTabsComponent).CloseChild();
        (e as SiniestroTabsComponent).close();
    }

    getNewItem(item: SiniestrosDto): SiniestrosDto {

        var item = new SiniestrosDto(item);
        item.Anulado = false;
        item.InformaTaller = false;
        return item;
    }


    onCreate() {
        if (!this.allowAdd) {
            return;
        }
        this.active = false;
        this.GetEditComponent().showNew(this.getNewItem(null));
        $('#fullscreentools')[0].click();
        $('body').addClass("smallsize");
    }

    ngOnDestroy() {
        super.ngOnDestroy();

        if (this.sub) {
            this.sub.unsubscribe();
            this.subQ.unsubscribe();
        }
    }

    onEditID(id: any) {
        if (!this.allowModify) {
            //return;
        }
        this.active = false;

        if (this.loadInMaterialPopup) {
            this.service.getById(id).subscribe(e => {
                this.Opendialog(e.DataObject, ViewMode.Modify);
            });
        } else {
            this.GetEditComponent().show(id);
        }
        $('#fullscreentools')[0].click();
    }

    onEdit(row: SiniestrosDto) {
        super.onEdit(row);
        $('body').addClass("smallsize");
    }

    getDescription(item: SiniestrosDto): string {
        return 'Nro. Siniestro: ' + item.NroSiniestro;
    }

    completeFilter(filter: SiniestrosFilter) {
        filter.Consecuencias = [];

        if (this.selectedConsecuencias && this.selectedConsecuencias.length >= 1) {
            this.selectedConsecuencias.forEach(e => {
                filter.Consecuencias.push(e.Id);
            });
        }
        filter.SubEstadoReclamo = [];

        if (this.selectedSubEstados && this.selectedSubEstados.length >= 1) {
            this.selectedSubEstados.forEach(e => {
                filter.SubEstadoReclamo.push(e.Id);
            });
        }
    }


    onEstadoReclamoSelected(event: any): void {
        this.filter.SubEstadoReclamo = null;
        this.selectedSubEstados = null;
    }

}