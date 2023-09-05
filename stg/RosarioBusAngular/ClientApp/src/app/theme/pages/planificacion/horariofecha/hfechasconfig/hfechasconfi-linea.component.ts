import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ComponentFactoryResolver, HostListener } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { CrudComponent, BaseCrudComponent } from '../../../../../shared/manager/crud.component';
import { IDetailComponent, DetailEmbeddedComponent } from '../../../../../shared/manager/detail.component';
import { HFechasConfiService } from '../HFechasConfi.service';
import { HFechasConfiDto, HorarioFechaFilter, PlaHorarioFechaLineaListView } from '../../model/HFechasConfi.model';
import { LazyLoadEventData } from '../../../../../shared/helpers/PrimengDatatableHelper';
import { ResponseModel, PaginListResultDto } from '../../../../../shared/model/base.model';
import { Subscription } from 'rxjs';
import { LineaService } from '../../linea/linea.service';
import { LineaDto } from '../../model/linea.model';
import * as moment from 'moment';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { HfechasConfiNewComponent } from './hfechasconfi-new.component';
import { ComponentCanDeactivate, AppComponentBase } from '../../../../../shared/common/app-component-base';
import { TabsDetalleHorariofechaComponent } from '../servicios/tabsdetalle-horariofecha.component';
import { CreateOrEditHFechasConfiModalComponent } from './create-or-edit-hfechasconfi-modal.component';


@Component({
    selector: 'hfechasconfi-linea',
    templateUrl: "./hfechasconfi-linea.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class HFechasConfiLineaComponent extends BaseCrudComponent<HFechasConfiDto, HorarioFechaFilter> implements OnInit, AfterViewInit, ComponentCanDeactivate {



    //@ViewChild('createOrEditHFechasConfiServicios') createOrEditHFechasConfiServicios: CreateOrEditHFechasConfiServiciosComponent;
    @ViewChild('tabsdetail') tabsdetail: TabsDetalleHorariofechaComponent;


    sub: Subscription;
    EditEnabled: boolean = true;
    linea: LineaDto = new LineaDto();
    protected dialog: MatDialog;
    allowCopy: boolean = false;
    id_hfechaconfi: number;
    //allowImportarHorario: boolean = false;


    //prevent default 
    //@HostListener('window:beforeunload', ['$event'])
    //unloadNotification($event: any) {
    //    if (!this.canDeactivate()) {
    //        $event.returnValue = true;
    //    }
    //}

    canDeactivate(): boolean {
        return this.tabsdetail.canDeactivate();
    }
    confirmMessage(): string {
        return this.tabsdetail.createOrEditHFechasConfiServicios.confirm;
    }



    getNewfilter(): HorarioFechaFilter {
        var f = new HorarioFechaFilter();
        f.Sort = "FecDesde DESC";
        return f;
    }

    getDescription(item: HFechasConfiDto): string {
        return item.Description;
    }

    constructor(injector: Injector,
        protected _Service: HFechasConfiService,
        protected _lineaService: LineaService
    ) {
        super(_Service, CreateOrEditHFechasConfiModalComponent, injector);

        this.icon = "la la-clock-o";
        this.title = "Horario";
        this.dialog = injector.get(MatDialog);
        this.SetAllowPermission()
    }

    SetAllowPermission() {
        this.allowAdd = this.permission.isGranted('Horarios.FechaHorario.CrearLineasAsociadas');
        this.allowCopy = this.permission.isGranted('Horarios.FechaHorario.CopiarFechaHorario');
        this.allowModify = this.permission.isGranted('Horarios.FechaHorario.EditarFechaHorario');
        this.allowDelete = this.permission.isGranted('Horarios.FechaHorario.EliminarFechaHorario');
        //this.allowImportarHorario = this.permission.isGranted('Horarios.FechaHorario.ImportarHorario');
    }


    GetEditComponent(): IDetailComponent {
        if (!this.loadInMaterialPopup && !this.detailElement) {
            this.detailComponentType = this.GetEditComponentType();
            var factory = this.cfr.resolveComponentFactory(this.detailComponentType);
            const ref = this['createOrEdit'].createComponent(factory);
            ref.changeDetectorRef.detectChanges();
            this.detailElement = ref.instance;
            this.detailElement.modalClose.subscribe(e => {

                if (e && e.detail && e.event == "ChangeScreen") {
                    try {
                        this.onShowServicio(e.detail as HFechasConfiDto);
                    } catch (e) {
                        this.active = true;
                    }
                }
                else {
                    this.active = true;
                }
            });

            this.detailElement.modalSave.subscribe(e => {
                this.active = true;
                this.Search(null);
            });
        }
        return this.detailElement;
    }


    ngOnInit() {
        super.ngOnInit();




        this.tabsdetail.modalClose.subscribe(e => {

            if (e && e.detail && e.event == "ChangeScreen") {
                try {
                    var detail = <HFechasConfiDto>e.detail;
                    this.onEdit(detail);
                } catch (e) {
                    this.active = true;
                }
            }
            else {
                this.active = true;
            }
        });

        this.tabsdetail.modalSave.subscribe(e => {
            this.active = true;
            this.onSearch();
        });


        this.sub = this.route.params.subscribe(params => {
            if (params.id) {
                this.id_hfechaconfi = params.id;
            }
            if (params.lineaid) {
                this.filter.LineaId = params.lineaid;
                this.onSearch();

                this._lineaService.getById(this.filter.LineaId).subscribe(l => {
                    this.linea = l.DataObject;

                    var selft = this;
                    var close = function() {
                        if (selft.canDeactivate()) {
                            if (selft.tabsdetail.createOrEditHFechasConfiServicios)
                                selft.tabsdetail.createOrEditHFechasConfiServicios.clearUnsavedData();
                            selft.CloseChild();
                        }
                        else {
                            selft.message.confirm(selft.confirmMessage(), 'Confirmación', (a) => {
                                if (a.value) {
                                    if (selft.tabsdetail.createOrEditHFechasConfiServicios)
                                        selft.tabsdetail.createOrEditHFechasConfiServicios.clearUnsavedData();
                                    selft.CloseChild();
                                }
                            });
                        }

                    }


                    setTimeout(e => {
                        if (this.showDefaultBreadcum)
                            this.breadcrumbsService.defaultBreadcrumbs(this.title);

                        this.breadcrumbsService.AddItem('Horario', null, 'planificacion/horariofecha', null, null);


                        var title = "Linea " + this.linea.DesLin;
                        this.breadcrumbsService.AddItem(title, this.icon, '', null, close);

                    }
                        , 10);


                }

                )

            }

        }
        );

    }

    ngAfterViewInit() {

    }

    CloseChild(): void {
        this.GetEditComponent().close();
        this.tabsdetail.close();
    }


    onDelete(item: HFechasConfiDto) {

        if (!this.allowDelete) {
            return;
        }

        var strindto = this.getDescription(item);
        //var aa = this.getNewItem(item);
        //var stringentity = aa.getDescription();

        this.message.confirm('Esta por borrar todos los datos relacionados a la Linea Horaria. ¿Está seguro de que desea eliminar la Linea?', strindto || 'Confirmación', (a) => {

            //this.isshowalgo = !this.isshowalgo;
            if (a.value) {
                this.service.delete(item.Id)
                    .subscribe(() => {
                        this.Search();
                        this.notify.success(this.l('Registro eliminado correctamente'));
                    });
            }

        });
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

        this.isTableLoading = true;
        this.primengDatatableHelper.showLoadingIndicator();



        this.filter.Sort = this.primengDatatableHelper.getSorting(this.dataTable) || "FecDesde DESC";
        this.filter.Page = this.primengDatatableHelper.getPageIndex(this.paginator, event);
        this.filter.PageSize = this.primengDatatableHelper.getPageSize(this.paginator, event);
        this.service.search(this.filter)
            .finally(() => {
                this.isTableLoading = false;
                if (this.id_hfechaconfi > 0) {
                    var row = this.list.find(e => e.Id == this.id_hfechaconfi);
                    this.id_hfechaconfi = null;
                    if (row !== null) {
                        this.onEdit(row);
                    }
                }
            })
            .subscribe((result: ResponseModel<PaginListResultDto<HFechasConfiDto>>) => {
                this.list = result.DataObject.Items;
                this.primengDatatableHelper.records = result.DataObject.Items
                this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
                this.primengDatatableHelper.hideLoadingIndicator()

            });

    }




    ngOnDestroy() {
        super.ngOnDestroy();
        if (this.sub) {
            this.sub.unsubscribe();
        }

    }

    //Search(event?: LazyLoadEventData) {
    //    if (this.filter.LineaId) {
    //        super.Search();
    //    }
    //}


    completedataBeforeSave(item: HFechasConfiDto): any {
        item.CodLinea = item.Linea.Id;
    }


    onShowServicio(row: HFechasConfiDto): void {
        this.active = false;
        this.tabsdetail.active = true;
        this.tabsdetail.isLoading = true;
        this.tabsdetail.title = "Servicios " + moment(row.FechaDesde).format('DD/MM/YYYY');
        this.tabsdetail.show(row.Id);
    }


    onCreate() {
        this.OpendialogHFechasConfiConfirm(false);
    }

    onCopy() {
        this.OpendialogHFechasConfiConfirm(true);
    }

    OpendialogHFechasConfiConfirm(IsInCopyMode: boolean) {
        this.OpendialogHFechasConfi(IsInCopyMode);
    }



    OpendialogHFechasConfi(IsInCopyMode: boolean) {
        const dialogConfig = new MatDialogConfig<HFechasConfiDto>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        //dialogConfig.id = "1";
        dialogConfig.width = '80%';
        dialogConfig.height = '';

        dialogConfig.data = new HFechasConfiDto();

        let dialogRef = this.dialog.open(HfechasConfiNewComponent, dialogConfig);

        dialogRef.componentInstance.LineaId = this.linea.Id;
        dialogRef.componentInstance.IsInCopyMode = IsInCopyMode;

        //this.detailElement = dialogRef.componentInstance;

        dialogRef.afterClosed().subscribe(


            data => {
                //console.log("Dialog output:", data);
                //this.active = true;

                if (data) {
                    this.message.success("La Fecha Fue generada correctamente");
                    this.Search();
                    if (IsInCopyMode) {
                        this.onEditID(data.Id);
                    }
                }

            }
        );




    }

}