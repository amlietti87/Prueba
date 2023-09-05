import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { moment } from 'ngx-bootstrap/chronos/test/chain';
import { MatDatepickerInputEvent, MatDatepicker, MatDialogConfig, MatDialog } from '@angular/material';
import { getDate, isFirstDayOfWeek } from 'ngx-bootstrap/chronos/utils/date-getters';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { DenunciasDto, DenunciasFilter, ExcelDenunciasFilter } from '../model/denuncias.model';
import { DenunciasService } from './denuncias.service';
import { DenunciasTabsComponent } from './tabs/create-or-edit-denuncias.component';
import { AnularDenunciaModalComponent } from './anular/anular-modal.component';
import { ViewMode } from '../../../../shared/model/base.model';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { forEach } from '@angular/router/src/utils/collection';
import { DenunciaImportadorDTO, DenunciasImportadorComponent } from './denuncias-importador/denuncias-importador.component';
import { AdjuntoComponent } from '../../siniestros/siniestro/adjunto/adjunto.component';
import { environment } from '../../../../../environments/environment';
import { saveAs } from 'file-saver'
import { Helpers } from '../../../../helpers';

@Component({

    templateUrl: "./denuncias.component.html",
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./denuncias.component.css']
})
export class DenunciasComponent extends BaseCrudComponent<DenunciasDto, DenunciasFilter> implements OnInit, AfterViewInit {
    sub: Subscription;
    subQ: Subscription;
    customdetail: DenunciasTabsComponent;

    //material dialog
    protected popupHeight: string = '';
    protected popupWidth: string = '80%';
    protected dialog: MatDialog;

    showCreate: boolean = true;

    //PERMISOS
    allowAdd: boolean = false;
    allowModify: boolean = false;
    allowDelete: boolean = false;
    allowAnular: boolean = false;
    allowVisualizar: boolean = false;
    allowImprimir: boolean = false;
    allowExportar: boolean = false;
    allowImportar: boolean = false;
    allowAdjunto: boolean = false;
    //ADJUNTOS
    GetAdjuntosDenuncias: string;
    appDeleteFileById: string;
    appUploadFiles: string;

    GetEditComponent(): IDetailComponent {

        var e = super.GetEditComponent();
        e as DenunciasTabsComponent;
        return e;
    }

    //ViewChilds
    @ViewChild('ComboAnulado') ComboAnulado: YesNoAllComboComponent;

    constructor(injector: Injector,
        private _Service: DenunciasService,
        private _activatedRoute: ActivatedRoute,
        protected cdr: ChangeDetectorRef) {
        super(_Service, DenunciasTabsComponent, injector);

        this.dialog = injector.get(MatDialog);
        this.isFirstTime = true;
        this.icon = "fa fa-car"
        this.title = "Denuncias";
        this.advancedFiltersAreShown = true;
        this.SetAllowPermission();

        this.GetAdjuntosDenuncias = environment.artUrl + '/Denuncias/GetAdjuntosDenuncias';
        this.appUploadFiles = environment.artUrl + '/Denuncias/UploadFiles/?DenunciaId=';
        this.appDeleteFileById = environment.artUrl + '/Denuncias/DeleteFileById';
    }

    setColorRow(rowData: DenunciasDto) {
        if (rowData.Color && rowData.Color != null) {
            return rowData.Color;
        }
        else {
            return '';
        }
    }

    SetAllowPermission() {

        this.allowAnular = this.permission.isGranted('ART.Denuncia.Anular');
        this.allowAdd = this.permission.isGranted('ART.Denuncia.Agregar');
        this.allowModify = this.permission.isGranted('ART.Denuncia.Modificar');
        this.allowDelete = this.permission.isGranted('ART.Denuncia.Eliminar');
        this.allowVisualizar = this.permission.isGranted('ART.Denuncia.Visualizar');
        this.allowImprimir = this.permission.isGranted('ART.Denuncia.Imprimir');
        this.allowExportar = this.permission.isGranted('ART.Denuncia.Exportar');
        this.allowImportar = this.permission.isGranted('ART.Denuncia.Importar');
        this.allowAdjunto = this.permission.isGranted('ART.Denuncia.Adjunto');
    }

    events: string[] = [];

    addEvent(type: string, event: MatDatepickerInputEvent<Date>) {
        this.events.push(`${type}: ${event.value}`);
    }

    public onDate(event): void { };


    ngAfterViewInit() {
        super.ngAfterViewInit();
        this.ComboAnulado.writeValue(this.filter.Anulado);
        this.ComboAnulado.refresh();
        this.cdr.detectChanges();
    }

    getNewfilter(): DenunciasFilter {
        var f = new DenunciasFilter();
        f.Anulado = 2;
        return f;
    }

    BorrarFiltros() {
        this.filter = this.getNewfilter();
    }

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
    }

    format(date) {
        return moment(date, "hh:mm:ss").format("HH:mm");
    }

    paramsSubscribe(params: any) {
        this.breadcrumbsService.defaultBreadcrumbs(this.title);
        this.active = true;

        if (params.id) {
            this.onEditID(params.id);
        }
    }

    CloseChild(): void {
        var e = super.GetEditComponent();
        (e as DenunciasTabsComponent).CloseChild();
        (e as DenunciasTabsComponent).close();
    }

    getNewItem(item: DenunciasDto): DenunciasDto {

        var item = new DenunciasDto(item);
        item.Anulado = false;
        item.BajaServicio = true;
        item.AltaMedica = false;
        item.AltaLaboral = false;
        item.TieneReingresos = false;
        return item;
    }

    onAnularShow(item: DenunciasDto) {
        const dialogConfig = new MatDialogConfig<DenunciasDto>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        //this.CompleteBeforeChangeState(JSON.parse(JSON.stringify(item)) as DenunciasDto)

        dialogConfig.data = item;

        let dialogRef = this.dialog.open(AnularDenunciaModalComponent, dialogConfig);


        dialogRef.afterClosed().subscribe(
            data => {
                this.Search();
                this.active = true;
            }
        );

    }


    onAdjuntarArchivosShow(item: DenunciasDto) {
        const dialogConfig = new MatDialogConfig<DenunciasDto>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        //dialogConfig.id = "1";
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        //dialogConfig.data = this.CompleteDataBeforeShow(JSON.parse(JSON.stringify(_detail)) as T);

        let dialogRef = this.dialog.open(AdjuntoComponent, dialogConfig);

        dialogRef.componentInstance.appGetAllFileByParent = this.GetAdjuntosDenuncias;
        dialogRef.componentInstance.appUploadUrl = this.appUploadFiles + item.Id;
        dialogRef.componentInstance.appRemoveFileUrl = this.appDeleteFileById;
        dialogRef.componentInstance.allowClose = true;
        dialogRef.componentInstance.Parent = item.Id;
        dialogRef.componentInstance.AllowUploadFiles = this.allowAdjunto;
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


    onEdit(row: DenunciasDto) {
        super.onEdit(row);
        $('body').addClass("smallsize");
    }


    onEditID(id: any) {
        if (!this.allowVisualizar && !this.allowModify) {
            return;
        }
        this.active = false;

        if (this.loadInMaterialPopup) {
            this.service.getById(id).subscribe(e => this.Opendialog(e.DataObject, ViewMode.Modify));

        }
        else {
            this.GetEditComponent().show(id);
            $('#fullscreentools')[0].click();
        }
    }


    getDescription(item: DenunciasDto): string {
        return 'Nro. Denuncia: ' + item.NroDenuncia;
    }

    Search(event?: LazyLoadEventData) {
        if (this.filter && this.filter.selectEmpleados && this.filter.selectEmpleados != null) {
            $('#datatable-denuncias').addClass("tableDenuncias");
        }
        else {
            $('#datatable-denuncias').removeClass("tableDenuncias");
        }
        super.Search(event);
    }

    GetReporteExcel(): void {
        //var filter: ExcelDenunciasFilter = new ExcelDenunciasFilter();
        //filter.Ids = "";
        //for (var i = 0; i < this.list.length; i++) {
        //    filter.Ids += this.list[i].Id.toString() + ",";
        //}
        //filter.Ids = filter.Ids.substring(0, filter.Ids.length - 1);
        this._Service.GetReporteExcel(this.filter);
    }

    onPrint(row: DenunciasDto): void {

        var name = "DenunciaNro-" + row.NroDenuncia + ".pdf";
        Helpers.setLoading(true);
        this._Service.GenerateReport(row)
            .subscribe(blob => {
                saveAs(blob, name, {
                    type: 'text/plain;charset=windows-1252' // --> or whatever you need here
                })
                Helpers.setLoading(false)
            });
    }

    ImportarDesdeExcel(): void {
        const dialogConfig = new MatDialogConfig<DenunciaImportadorDTO>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = true;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        let dialogRef = this.dialog.open(DenunciasImportadorComponent, dialogConfig);

        //this.detailElement = dialogRef.componentInstance;
        dialogRef.afterClosed().subscribe(
            data => {

                this.active = true;
            }
        );

    }

}