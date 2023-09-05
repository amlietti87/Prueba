declare let mApp: any;
import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { DocumentosProcesadosDto, DocumentosProcesadosFilter, ArchivosTotalesPorEstado, VisorArchivos, AplicarAccioneResponseDto } from '../model/documentosprocesados.model';
import { DocumentosProcesadosService } from '../services/documentosprocesados.service';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';
import { environment } from '../../../../../environments/environment';
import { AplicarAccionDto } from '../model/aplicaraccion.model';
import { FdAccionesService } from '../services/fdacciones.service';
import { FDAccionesGrillaComboComponent } from '../shared/fdaccionesgrilla-combo.component';
import { FileService } from '../../../../shared/common/file.service';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { AbrirArchivosModalComponent } from '../abrirarchivo-modal/abrirarchivo-modal.component';
import { ErrorResponseAplicarAccionModalComponent } from '../errorResponse/errorResponse.component';
import { SeleccionarCorreoModalComponent } from '../seleccionarcorreo-modal/seleccionarcorreo-modal.component';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { FDEstadosComboComponent } from '../shared/fdestados-combo.component';
import { ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';
import { FileDTO } from '../../../../shared/common/models/fileDTO.model';
import { EsperaFirmadorComponent } from '../documento-progreso/espera-firmador.component';

@Component({

    templateUrl: "./bandejaempleado.component.html",
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./bandejaempleado.component.css']
})
export class BandejaEmpleadoComponent extends BaseCrudComponent<DocumentosProcesadosDto, DocumentosProcesadosFilter> implements AfterViewInit, OnInit, OnDestroy {




    //material dialog
    protected popupHeight: string = '';
    protected popupWidth: string = '80%';
    protected dialog: MatDialog;
    protected detailElement: IDetailComponent;
    private checkedAll: boolean = false;
    IsCheckingAll: boolean = false;

    //ViewChilds
    @ViewChild('CerradoCombo') CerradoCombo: YesNoAllComboComponent;
    @ViewChild('AccionCombo') AccionCombo: FDAccionesGrillaComboComponent;
    @ViewChild('EstadoCombo') EstadoCombo: FDEstadosComboComponent;
    @ViewChild('Buttons') Buttons: HTMLDivElement;

    //Adjuntos
    time: string;
    appDownloadUrl: string;

    //Data seleccionada en la grilla
    selectedData: DocumentosProcesadosDto[] = [];

    //Data Total de la bandeja de entrada
    allData: DocumentosProcesadosDto[] = [];

    //Grilla de estados
    historialEstados: ArchivosTotalesPorEstado[] = [];
    mostrarGrilla: boolean = true;
    numberOfDocs: number;
    predeterminedState: number;
    maxValueInt: number = 999999;

    constructor(injector: Injector,
        private serviceDocumentos: DocumentosProcesadosService,
        protected serviceAcciones: FdAccionesService,
        protected fileService: FileService,
        protected cdr: ChangeDetectorRef,
    ) {
        super(serviceDocumentos, null, injector);
        this.dialog = injector.get(MatDialog);
        this.isFirstTime = false;
        this.title = "Bandeja de documentos - Empleado"
        this.moduleName = "Firma Digital";
        this.icon = "flaticon-settings";
        // this.loadInMaterialPopup = false;
        this.advancedFiltersAreShown = true;
        this.appDownloadUrl = environment.siniestrosUrl + '/Adjuntos/DownloadFiles';
        this.getTablaEstados();

    }

    getTablaEstados() {
        this.serviceDocumentos.HistorialArchivosPorEstado()
            .subscribe((t) => {
                this.historialEstados = t.DataObject;
            })
    }

    getDescription(item: DocumentosProcesadosDto): string {
        return "";
    }
    getNewItem(item: DocumentosProcesadosDto): DocumentosProcesadosDto {

        var item = new DocumentosProcesadosDto(item);
        return item;
    }

    GetEditComponent(): IDetailComponent {
        return null;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
        const buttons = document.getElementsByName('Buttons')[0] as HTMLDivElement;
        buttons.setAttribute("style", "pointer-events: none;");
        this.EstadoCombo.IsDisabled = true;
        this.CerradoCombo.writeValue(this.filter.Cerrado);
        this.CerradoCombo.refresh();
        this.loadEstadoCombo();
        this.cdr.detectChanges();
    }

    loadEstadoCombo() {
        var self = this;
        setTimeout(async () => {
            await this.setPredeterminedState()
                .then(() => {
                    this.filter.EstadoId = this.predeterminedState;
                    this.EstadoCombo.writeValue(this.filter.EstadoId);
                    setTimeout(() => {
                        self.EstadoCombo.isLoading = false;
                        self.EstadoCombo.IsDisabled = false;
                        //$(self.EstadoCombo.comboboxElement.nativeElement).selectpicker('refresh');
                        self.EstadoCombo.refresh();
                        const buttons = document.getElementsByName('Buttons')[0] as HTMLDivElement;
                        buttons.setAttribute("style", "pointer-events: auto;");
                    }, 3000);
                });
        }, 3500);
    }

    async setPredeterminedState() {
        await this.EstadoCombo.setPredeterminedState();
        this.predeterminedState = this.EstadoCombo.predeterminedState;
    }

    ngOnInit() {
        super.ngOnInit();

    }

    getNewfilter(): DocumentosProcesadosFilter {
        var filter = new DocumentosProcesadosFilter();
        setTimeout(async () => {
            await this.setPredeterminedState()
                .then(() => {
                    filter.Rechazado = 2;
                    filter.Cerrado = 2;
                    filter.EstadoId = this.predeterminedState;
                    filter.EsEmpleador = false;
                });
        }, 50);
        return filter;
    }

    renewFilter(): DocumentosProcesadosFilter {
        var filter = this.filter;
        filter.FechaDesde = null;
        filter.FechaHasta = null;
        filter.Rechazado = 2;
        filter.Cerrado = 2;
        filter.EstadoId = this.predeterminedState;
        filter.EsEmpleador = false;
        filter.TipoDocumentoId = null;
        return filter;
    }

    actualizar(event?: LazyLoadEventData) {
        this.selectedData = [];
        this.AccionCombo.IsDisabled = false;
        this.getTotalOfDocs(this.filter);
        this.onSearch(event);
        const checkAllCheckbox = document.getElementById('checkedAllRows') as HTMLInputElement;
        if (checkAllCheckbox != null) {
            checkAllCheckbox.checked = false;
        }
        this.filter.AccionId = null;
    }

    onSearch(event?: LazyLoadEventData) {

        super.onSearch(event);
        this.allSelectedChecker();
    }

    // ExportarExcel() {
    //     this.fileService.dowloadAuthenticatedByPost(environment.firmaDigitalUrl + '/FdDocumentosProcesados/ExportarExcel', this.filter);
    // }

    EjecutarAccion() {
        var apply = new AplicarAccionDto();
        apply.AccionId = this.filter.AccionId;
        apply.Empleador = false;

        this.selectedData.forEach(
            (e) => {
                apply.Documentos.push(e.Id);
            });

        if (apply.AccionId == -7) {//Enviar Correo
            this.AbrirPopupMail(apply);
        }
        else {
            this.ejecutarAccionInternal(apply);
        }
    }

    AbrirPopupMail(apply: AplicarAccionDto): void {

        const dialogConfig = new MatDialogConfig<string>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;
        this.serviceDocumentos.GetEmailDefault().subscribe(e => {

            dialogConfig.data = e.DataObject;

            let dialogRef = this.dialog.open(SeleccionarCorreoModalComponent, dialogConfig);

            dialogRef.afterClosed().subscribe(

                data => {
                    if (data && data != null) {
                        apply.Correo = data;
                        this.ejecutarAccionInternal(apply);
                    }

                }
            );
        });


    }
    DownloadFile(file: FileDTO) {
        var url = this.fileService.getBlobURL(file);
        if (file.ForceDownload) {
            var a = window.document.createElement('a');
            a.href = url;
            a.download = file.FileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        } else {
            window.open(url, "_blank");
        }
        this.selectedData = [];
        this.checkIfSelected();
    }
    private ejecutarAccionInternal(apply: AplicarAccionDto): void {
        mApp.blockPage();
        this.serviceAcciones.AplicarAccion(apply)
            .finally(() => { mApp.unblockPage(); })
            .subscribe((t) => {
                if (t.DataObject && t.DataObject != null) {


                    if (apply.AccionId == -1) {
                        this.AbrirPopupVisor(t.DataObject as VisorArchivos[]);
                    }
                    else if (apply.AccionId == -2) {

                        (t.DataObject as any[]).forEach((e) => {
                            this.fileService.downloadAnonymousFileByUrl(this.appDownloadUrl + '\?id=' + e + '&c=' + Date.now());
                        })
                        this.selectedData = [];
                        this.checkIfSelected();
                    }
                    else if (apply.AccionId == -4) {

                    }
                    else if (apply.AccionId == -5) {

                    }
                    else if (apply.AccionId == 3) {
                        let data = t.DataObject as AplicarAccioneResponseDto;

                        this.DownloadFile(data.FileDto);
                        this.LoadProgressBar(apply, data);
                    }
                    else if (apply.AccionId == -7) { //ENVIAR CORREO
                        var resp = t.DataObject as AplicarAccioneResponseDto;
                        if (resp != null) {
                            if (!resp.IsValid) {
                                this.MostrarErrores(resp);
                            }
                            else {
                                this.notificationService.info("Correo enviado exitosamente");
                            }
                        }
                        this.selectedData = [];
                        this.checkIfSelected();
                    }
                    else {
                        var resp = t.DataObject as AplicarAccioneResponseDto;
                        if (resp != null) {
                            if (!resp.IsValid) {
                                this.MostrarErrores(resp);
                            }
                            else {
                                this.notificationService.info("Se aplicó correctamente la acción para los documentos seleccionados");
                            }
                            this.selectedData = [];
                            this.allData = [];
                            this.checkedAll = false;
                            this.Search();
                        }
                    }

                }
            })
        // this.selectedData = [];
        // this.checkedAll = false;
        // //this.AccionCombo.IsDisabled = false;
        // this.refreshTable();
        // this.allSelectedChecker();
    }

    LoadProgressBar(apply: AplicarAccionDto, resp: AplicarAccioneResponseDto) {
        const dialogConfig = new MatDialogConfig<AplicarAccionDto>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;
        apply.DescripcionAccion = this.AccionCombo.items.find(e => e.Id == apply.AccionId).Description;
        dialogConfig.data = apply;

        let dialogRef = this.dialog.open(EsperaFirmadorComponent, dialogConfig);

        dialogRef.componentInstance.response = resp;

        dialogRef.afterClosed().subscribe(
            data => {
                if (data) {
                    this.selectedData = [];
                    this.checkIfSelected();
                    this.Search();
                }
            }
        );

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
                this.selectedData = [];
                this.checkIfSelected();
            }
        );
    }

    AbrirPopupVisor(lista: VisorArchivos[]): void {
        const dialogConfig = new MatDialogConfig<VisorArchivos[]>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        dialogConfig.data = lista;

        let dialogRef = this.dialog.open(AbrirArchivosModalComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(

            data => {
                this.selectedData = [];
                this.checkIfSelected();
            }
        );
    }

    onTableHeaderCheckboxToggle(event: any) {
        this.RemoveSeleccionadosSinAccionPermitida();
    }

    RemoverPermitirMarcarLote() {

        if (this.filter.AccionId > 0) {
            const clone = [...this.selectedData];
            var c = 0;
            clone.forEach((e) => {

                var findaccion = e.AccionesDisponibles.find(f => f.AccionPermitidaId == this.filter.AccionId);

                if (findaccion.PermiteMarcarLote == false) {
                    c = c + 1;
                }

                if (c > 1) {
                    var index = this.selectedData.findIndex(f => f.Id == e.Id);
                    this.selectedData.splice(index, 1);
                }
            });
        }
    }

    RemoveSeleccionadosSinAccionPermitida() {

        if (this.filter.AccionId > 0) {

            this.primengDatatableHelper.records.forEach((e) => {

                var findAccionPermitida = (<DocumentosProcesadosDto>e).AccionesDisponibles.find(f => f.AccionPermitidaId == this.filter.AccionId);
                if (findAccionPermitida == null) {
                    var index = this.selectedData.findIndex(f => f.Id == (<DocumentosProcesadosDto>e).Id);
                    this.selectedData.splice(index, 1);
                }
            });
        }
        else if (this.filter.AccionId == -3) {

            this.primengDatatableHelper.records.forEach((e) => {

                if (!(<DocumentosProcesadosDto>e).Rechazado == false && (<DocumentosProcesadosDto>e).PermiteRechazo == true) {
                    var index = this.selectedData.findIndex(f => f.Id == (<DocumentosProcesadosDto>e).Id);
                    this.selectedData.splice(index, 1);
                }

            });
        }

        this.RemoverPermitirMarcarLote();

    }

    ChangeAction() {
        if (!(this.filter.AccionId && this.filter.AccionId != null)) {
            this.selectedData = [];
        }

        this.RemoveSeleccionadosSinAccionPermitida();
        this.getTotalOfDocs(this.filter);
    }

    onRowSelect(event) {
        if (this.filter.AccionId > 0) {
            var findAccionPermitida = (<DocumentosProcesadosDto>event.data).AccionesDisponibles.find(f => f.AccionPermitidaId == this.filter.AccionId);
            if (findAccionPermitida == null) {
                var index = this.selectedData.findIndex(f => f.Id == (<DocumentosProcesadosDto>event.data).Id);
                this.selectedData.splice(index, 1);
            }
        }
        else if (this.filter.AccionId == -3 && (!(<DocumentosProcesadosDto>event.data).Rechazado == false && (<DocumentosProcesadosDto>event.data).PermiteRechazo == true)) {
            var index = this.selectedData.findIndex(f => f.Id == (<DocumentosProcesadosDto>event.data).Id);
            this.selectedData.splice(index, 1);
        }

        this.RemoverPermitirMarcarLote();
    }


    GetEditComponentType(): Type<IDetailComponent> {
        return null;
    }

    BorrarFiltros() {
        this.selectedData = [];
        this.checkedAll = false;
        this.filter = this.renewFilter();
        //this.AccionCombo.IsDisabled = false;
    }

    selectedItemChangeEstados(e: any) {
        //this.onSearch();
    }

    checkIfShowBox(row?: any) {
        var value = true;

        if (this.filter.AccionId == -3) {
            if (!row.PermiteRechazo || row.Rechazado) {
                value = false
            }
        }
        if (this.filter.AccionId == 1) {
            if (row.AccionesDisponibles.find(e => e.AccionPermitidaId == 1)) {
                value = true;
            } else {
                value = false;
            }
        }
        if (this.filter.AccionId == 3) {
            if (row.AccionesDisponibles.find(e => e.AccionPermitidaId == 3)) {
                value = true;
            } else {
                value = false;
            }
        }
        return value;
    }

    checkRow(row) {

        var selectedRow: DocumentosProcesadosDto;
        selectedRow = row;
        //this.AccionCombo.IsDisabled = true;
        if (selectedRow != null) {
            let isChecked = !selectedRow.IsSelected;
            if (isChecked) {
                var findaccion = selectedRow.AccionesDisponibles.find(f => f.AccionPermitidaId == this.filter.AccionId);
                if (this.filter.AccionId > 0 && selectedRow.AccionesDisponibles.some(e => e.AccionPermitidaId == this.filter.AccionId)) {
                    if (findaccion.PermiteMarcarLote == true || this.selectedData.length == 0 && findaccion.PermiteMarcarLote == false) {
                        this.selectedData.push(selectedRow);
                        selectedRow.IsSelected = isChecked;
                    }
                } else if (this.filter.AccionId < 0 && this.filter.AccionId != -3) {
                    if (findaccion.PermiteMarcarLote == true || this.selectedData.length == 0 && findaccion.PermiteMarcarLote == false) {
                        this.selectedData.push(selectedRow);
                        selectedRow.IsSelected = isChecked;
                    }

                }
            } else {
                selectedRow.IsSelected = isChecked;
                var index = this.selectedData.findIndex(f => f.Id == selectedRow.Id);
                this.selectedData.splice(index, 1);
                this.checkedAll = false;
                //this.AccionCombo.IsDisabled = false;
            }


        }

    }

    // uncheckOne(unselectedRow) {
    //     if (this.selectedData != null) {
    //         var index = this.selectedData.findIndex(f => f.Id == unselectedRow.Id);
    //         this.selectedData.splice(index, 1);
    //     }
    //     if (this.selectedData.length == 0) {
    //         this.AccionCombo.IsDisabled = false;
    //         this.refreshTable();
    //     }
    //     this.checkedAll = false;
    // }

    allSelectedChecker() {
        if (this.primengDatatableHelper.records) {
            var selectedNumber = this.selectedData.length;
            if (selectedNumber == this.numberOfDocs) {
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

    getTotalOfDocs(filter) {
        var counter = 0;
        let filterPages = this.filter.PageSize;
        let filterPage = this.filter.Page;
        this.filter.PageSize = this.maxValueInt;
        this.filter.Page = null;
        this.serviceDocumentos.search(this.filter).subscribe(result => {
            var Data = result.DataObject.Items;
            this.allData = result.DataObject.Items;
            var self = this;
            result.DataObject.Items.forEach(element => {
                if (self.filter.AccionId > 0) {
                    if (element != undefined) {
                        if (element.AccionesDisponibles.some(e => e.AccionPermitidaId == self.filter.AccionId)) {
                            counter = counter + 1;
                        }
                    }

                }
            });
            debugger;
            if (self.filter.AccionId > 0) {
                this.numberOfDocs = counter;
            } else {
                this.numberOfDocs = Data.length;
            }
        });
        this.filter.PageSize = filterPages;
        this.filter.Page = filterPage;
    }

    selectAllRows(event?: any) {
        var self = this;
        //this.AccionCombo.IsDisabled = true;
        let isChecked = !self.checkedAll;
        this.IsCheckingAll = true;
        if (this.primengDatatableHelper.records) {
            if (isChecked) {
                var toCheckIfIncluded: DocumentosProcesadosDto[] = [];
                toCheckIfIncluded = self.primengDatatableHelper.records;
                this.allData.map(e => {
                    var selected = this.selectedData.find(el => el.Id == e.Id)
                    if (e != undefined && e.Rechazado == false && !selected) {
                        var findaccion = e.AccionesDisponibles.find(f => f.AccionPermitidaId == this.filter.AccionId);
                        if (findaccion != undefined) {
                            if (findaccion.PermiteMarcarLote == true || this.selectedData.length == 0 && findaccion.PermiteMarcarLote == false) {
                                this.selectedData.push(<DocumentosProcesadosDto>e);
                            }
                        }

                    }
                })
                this.numberOfDocs = this.selectedData.length;
                this.selectedData.forEach(element => {
                    element.IsSelected = true;
                    var row = toCheckIfIncluded.find(e => e.Id == element.Id);
                    if (row) {
                        row.IsSelected = true;
                    }
                });
                this.isTableLoading = false;
                self.checkedAll = isChecked;
                self.IsCheckingAll = false;
                this.cdr.detectChanges();

            } else {
                this.checkedAll = false;
                self.primengDatatableHelper.records.forEach(e => {
                    e.IsSelected = false;
                })
                this.selectedData = [];
                this.IsCheckingAll = false;
                //this.AccionCombo.IsDisabled = false;
            }
        } else {
            self.checkedAll = false;
        }

        // this.RemoveSeleccionadosSinAccionPermitida();
    }

    checkIfSelected() {

        if (this.selectedData.length != 0) {
            this.selectedData.forEach(element => {
                var boxesToCheck = this.primengDatatableHelper.records.find(e => e.Id == element.Id);
                if (boxesToCheck != null && element.IsSelected) {
                    boxesToCheck.IsSelected = true;
                }
            });
        }
        else {
            this.primengDatatableHelper.records.map(e => {
                e.IsSelected = false;
            });
            this.checkedAll = false;
        }
    }

    // onUncheckingAll(event?: LazyLoadEventData) {
    //     this.primengDatatableHelper.isLoading = true;
    //     this.selectedData = [];
    //     this.allSelectedChecker();
    //     this.AccionCombo.IsDisabled = false;
    //     this.refreshTable();
    // }

    // refreshTable() {
    //     this.service.search(this.filter)
    //         .finally(() => {
    //             this.primengDatatableHelper.isLoading = false;
    //         })
    //         .subscribe((result: ResponseModel<PaginListResultDto<DocumentosProcesadosDto>>) => {
    //             this.list = result.DataObject.Items;
    //             this.primengDatatableHelper.records = result.DataObject.Items
    //             this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
    //             this.primengDatatableHelper.hideLoadingIndicator();
    //         })
    // }

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
            .subscribe((result: ResponseModel<PaginListResultDto<DocumentosProcesadosDto>>) => {
                this.list = result.DataObject.Items;
                this.primengDatatableHelper.records = result.DataObject.Items
                this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
                this.primengDatatableHelper.hideLoadingIndicator();
                if (this.filter.AccionId) {
                    if (this.filter.AccionId != null) {
                        setTimeout(function() { self.checkIfSelected() }, 200);
                    }
                }
            })

        if (this.allData.length == 0) {
            this.filter.PageSize = this.maxValueInt;
            this.filter.Page = null;
            this.service.search(this.filter)
                .subscribe((result: ResponseModel<PaginListResultDto<DocumentosProcesadosDto>>) => {
                    this.allData = new Array<DocumentosProcesadosDto>();
                    this.allData = result.DataObject.Items;
                })
        }

    }

    ngOnDestroy(): void {
        super.ngOnDestroy();

    }
}
