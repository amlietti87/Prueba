import { Error } from './../../../../shared/constants/error-constants';
import { Data, StatusResponse, Result } from './../../../../shared/model/base.model';
import { DocumentosErrorService } from './../services/documentosconerror.service';
declare let mApp: any;
import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ChangeDetectorRef, ElementRef, ErrorHandler } from '@angular/core';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { DocumentosProcesadosDto, DocumentosProcesadosFilter, ArchivosTotalesPorEstado, VisorArchivos, AplicarAccioneResponseDto } from '../model/documentosprocesados.model';
import { DocumentosProcesadosService } from '../services/documentosprocesados.service';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';
import { environment } from '../../../../../environments/environment';
import { DocumentosErrorDto, DocumentosErrorFilter } from '../model/documentoserror.model';
import { FdAccionesService } from '../services/fdacciones.service';
import { AplicarAccionDto } from '../model/aplicaraccion.model';
import { NotificationService } from '../../../../shared/notification/notification.service';
import { ErrorResponseAplicarAccionModalComponent } from '../errorResponse/errorResponse.component';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { PaginListResultDto, ResponseModel } from '../../../../shared/model/base.model';
import { ParametersService } from '../../admin/parameters/parameters.service';
import { Observable } from 'rxjs';

@Component({

    templateUrl: "./revisarerrores.component.html",
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./revisarerrores.component.css']
})
export class RevisarErroresComponent extends BaseCrudComponent<DocumentosErrorDto, DocumentosErrorFilter> implements AfterViewInit, OnInit {

    //ViewChilds
    @ViewChild('RevisadoCombo') RevisadoCombo: YesNoAllComboComponent;
    @ViewChild('checkedAllRows') checkedAllRows: ElementRef;

    //material dialog
    protected popupHeight: string = '';
    protected popupWidth: string = '80%';
    protected dialog: MatDialog;
    protected detailElement: IDetailComponent;
    private checkedAll: boolean = false;

    //Data seleccionada en la grilla
    selectedData: DocumentosErrorDto[] = [];
    numberOfRegsNoRevisado: number;

    constructor(injector: Injector,
        private serviceDocumentos: DocumentosErrorService,
        private serviceAcciones: FdAccionesService,
        private serviceParameters: ParametersService,
        protected cdr: ChangeDetectorRef
    ) {
        super(serviceDocumentos, null, injector);
        this.dialog = injector.get(MatDialog);
        this.isFirstTime = false;
        this.title = "Archivos - Revisar errores"
        this.moduleName = "Firma Digital";
        this.icon = "flaticon-settings";
        // this.loadInMaterialPopup = false;
        this.advancedFiltersAreShown = true;
    }

    getDescription(item: DocumentosErrorDto): string {
        return "";
    }

    getNewItem(item: DocumentosErrorDto): DocumentosErrorDto {

        var item = new DocumentosErrorDto(item);
        return item;
    }

    GetEditComponent(): IDetailComponent {
        return null;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
        this.RevisadoCombo.writeValue(this.filter.Revisado);
        this.RevisadoCombo.refresh();
        this.cdr.detectChanges();
    }

    ngOnInit() {
        super.ngOnInit();
        this.getTotalOfNoRevisado();
    }

    getTotalOfNoRevisado() {
        let filter = this.filter;
        filter.Revisado = 2;
        this.serviceDocumentos.requestAllByFilter(filter).subscribe(result => {
            var Data = result.DataObject.Items;
            this.numberOfRegsNoRevisado = Data.length;
        });
    }

    getNewfilter(): DocumentosErrorFilter {
        var filter = new DocumentosErrorFilter();
        filter.Revisado = 2;
        return filter;
    }

    showSelectAll() {
        const onChangePage = document.getElementById('checkedAllRows') as HTMLInputElement;
        onChangePage.removeAttribute('hidden')
    }

    onRowSelect(event) {
        if ((<DocumentosErrorDto>event.data).Revisado == true) {
            var index = this.selectedData.findIndex(f => f.Id == (<DocumentosErrorDto>event.data).Id);
            this.selectedData.splice(index, 1);
        }
    }

    actualizar(event?: LazyLoadEventData) {
        this.selectedData = [];
        this.getTotalOfDocs(this.filter);
        this.onSearch(event);
        this.uncheckSelectAllCheckbox();
        this.checkedAll = false;
    }

    onSearch(event?: LazyLoadEventData) {

        super.onSearch(event);
        this.allSelectedChecker();
    }

    Search(event?: LazyLoadEventData) {
        var self = this;
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
        this.completeFilter(this.filter);

        this.isTableLoading = true;
        this.primengDatatableHelper.showLoadingIndicator();

        this.filter.Sort = this.primengDatatableHelper.getSorting(this.dataTable);
        this.filter.Page = this.primengDatatableHelper.getPageIndex(this.paginator, event);
        this.filter.PageSize = this.primengDatatableHelper.getPageSize(this.paginator, event);

        this.service.search(this.filter)
            .finally(() => {
                this.primengDatatableHelper.isLoading = false;
            })
            .subscribe((result: ResponseModel<PaginListResultDto<DocumentosErrorDto>>) => {
                this.list = result.DataObject.Items;
                this.primengDatatableHelper.records = result.DataObject.Items
                this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
                this.primengDatatableHelper.hideLoadingIndicator();
                if (this.primengDatatableHelper.totalRecordsCount != 0) {
                    setTimeout(function() { self.checkIfSelected() }, 50);
                }
            })
    }

    getTotalOfDocs(filter) {
        var counter = 0;
        var filterPages = this.filter.PageSize;
        var filterPage = this.filter.Page;
        this.filter.PageSize = null;
        this.filter.Page = null;
        this.serviceDocumentos.search(this.filter).subscribe(result => {
            var Data = result.DataObject.Items;

            var self = this;
            result.DataObject.Items.forEach(element => {
                if (self.filter.AccionId > 0) {
                    if (element != undefined) {
                        if (element.Revisado == false) {
                            counter = counter + 1;
                        }
                    }

                }
            });

            if (self.filter.AccionId > 0) {
                this.numberOfRegsNoRevisado = counter;
            } else {
                this.numberOfRegsNoRevisado = Data.length;
            }
        });
        this.filter.PageSize = filterPages;
        this.filter.Page = filterPage;
    }

    allSelectedChecker() {
        if (this.primengDatatableHelper.records) {
            var selectedNumber = this.selectedData.length;
            if (selectedNumber == this.numberOfRegsNoRevisado) {
                this.checkedAll = true;
            }
            const checkAllCheckbox = document.getElementById('checkedAllRows') as HTMLInputElement;
            if (this.checkedAll == true) {
                if (checkAllCheckbox != null) {
                    checkAllCheckbox.checked = true;
                }
            } else {
                if (checkAllCheckbox != null) {
                    checkAllCheckbox.checked = false;
                }
            }
        }
    }

    onUncheckingAll(event?: LazyLoadEventData) {
        this.primengDatatableHelper.isLoading = true;
        this.selectedData = [];
        this.allSelectedChecker();
        this.refreshTable();
    }

    refreshTable() {
        this.service.search(this.filter)
            .finally(() => {
                this.primengDatatableHelper.isLoading = false;
            })
            .subscribe((result: ResponseModel<PaginListResultDto<DocumentosErrorDto>>) => {
                this.list = result.DataObject.Items;
                this.primengDatatableHelper.records = result.DataObject.Items
                this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
                this.primengDatatableHelper.hideLoadingIndicator();
            })
    }

    onTableHeaderCheckboxToggle(event: any) {

        const clone = [...this.selectedData];

        clone.forEach((e) => {

            if (e.Revisado == true) {
                var index = this.selectedData.findIndex(f => f.Id == e.Id);
                this.selectedData.splice(index, 1);
            }
        });
    }

    MostrarErrores(resp: AplicarAccioneResponseDto): void {
        const dialogConfig = new MatDialogConfig<AplicarAccioneResponseDto>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        dialogConfig.data = resp;

        let dialogRef = this.dialog.open(ErrorResponseAplicarAccionModalComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(

            data => {

            }
        );
    }

    Revisar() {
        var accion = new AplicarAccionDto();
        accion.AccionId = -6;
        accion.Documentos = [];
        this.selectedData.forEach((e) => { accion.Documentos.push(e.Id) });
        this.serviceAcciones.AplicarAccion(accion).subscribe((t) => {

            var resp = t.DataObject as AplicarAccioneResponseDto;
            if (resp != null) {
                if (!resp.IsValid) {
                    this.MostrarErrores(resp);
                }
                else {
                    this.notificationService.info("Los documentos seleccionados se pasaron correctamente a revisados");
                }
                this.onSearch();
                this.selectedData = [];
            }

        });
    }

    GetEditComponentType(): Type<IDetailComponent> {
        return null;
    }

    BorrarFiltros() {
        this.selectedData = [];
        this.checkedAll = false;
        this.filter = this.getNewfilter();
    }

    checkRow(row) {
        var selectedRow: DocumentosErrorDto;
        selectedRow = row;
        if (this.selectedData != null) {
            if (this.selectedData.find(e => e.Id == selectedRow.Id)) {
                this.uncheckOne(selectedRow);
            } else {
                this.checkOne(selectedRow);
            }
        } else {
            this.checkOne(selectedRow);
        }
        this.allSelectedChecker();
    }

    checkOne(selectedRow) {
        if (this.selectedData == null) {
            this.selectedData = [];
            this.selectedData.push(selectedRow);
        } else {
            this.selectedData.push(selectedRow);
        }
    }

    uncheckOne(unselectedRow) {
        if (this.selectedData != null) {
            var index = this.selectedData.findIndex(f => f.Id == unselectedRow.Id);
            this.selectedData.splice(index, 1);
        }
        if (this.selectedData.length == 0) {
            this.refreshTable();
        }
        this.checkedAll = false;
    }

    selectAllRows(event?: any) {
        var self = this;
        if (this.primengDatatableHelper.records) {
            if (self.checkedAll == false) {
                self.checkedAll = true;

                var toCheckIfIncluded: DocumentosErrorDto[] = [];
                toCheckIfIncluded = self.primengDatatableHelper.records;
                var filterPages = this.filter.PageSize;
                var filterPage = this.filter.Page;
                this.filter.PageSize = null;
                this.filter.Page = null;
                this.serviceDocumentos.search(this.filter).subscribe(result => {
                    this.selectedData = new Array<DocumentosErrorDto>();

                    var self = this;
                    result.DataObject.Items.forEach(element => {
                        if (element != undefined) {
                            if (element.Revisado == false) {
                                this.selectedData.push(<DocumentosErrorDto>element);
                            }
                        }
                    });
                    this.numberOfRegsNoRevisado = this.selectedData.length;

                    this.selectedData.forEach(element => {
                        if (toCheckIfIncluded.find(e => e.Id == element.Id && e.Revisado == false)) {
                            var identifier = 'checkbox-' + element.Id;
                            const ele = document.getElementById(identifier) as HTMLInputElement;
                            ele.checked = true;
                        }
                    });
                    this.isTableLoading = false;

                    if (this.selectedData.length == 0) {
                        self.uncheckSelectAllCheckbox();
                        self.checkedAll = false;
                    }
                });
                this.filter.PageSize = filterPages;
                this.filter.Page = filterPage;
            } else {
                this.checkedAll = false;
                this.selectedData = [];
                this.onUncheckingAll();
            }
        } else {
            this.uncheckSelectAllCheckbox();
        }

    }

    uncheckSelectAllCheckbox() {
        const checkAllCheckbox = document.getElementById('checkedAllRows') as HTMLInputElement;
        if (checkAllCheckbox.checked != undefined) {
            checkAllCheckbox.checked = false;
        }
    }

    checkIfSelected() {
        if (this.selectedData.length != 0) {
            this.selectedData.forEach(element => {
                var boxesToCheck = this.primengDatatableHelper.records.find(e => e.Id == element.Id && e.Revisado == false);
                if (boxesToCheck != null) {
                    var identifier = 'checkbox-' + element.Id;
                    const ele = document.getElementById(identifier) as HTMLInputElement;
                    if (ele != null) {
                        ele.checked = true;
                    }
                }
            });
        }
    }
}  
