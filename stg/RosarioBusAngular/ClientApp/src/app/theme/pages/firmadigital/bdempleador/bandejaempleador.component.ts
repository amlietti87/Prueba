import { SucursalComboComponent } from './../../planificacion/shared/sucursal-combo.component';
declare let mApp: any;
import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ChangeDetectorRef } from '@angular/core';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { DocumentosProcesadosDto, DocumentosProcesadosFilter, ArchivosTotalesPorEstado, VisorArchivos, AplicarAccioneResponseDto, VerDetalle } from '../model/documentosprocesados.model';
import { DocumentosProcesadosService } from '../services/documentosprocesados.service';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';
import { environment } from '../../../../../environments/environment';
import { AplicarAccionDto, RechazarDto } from '../model/aplicaraccion.model';
import { FdAccionesService } from '../services/fdacciones.service';
import { FDAccionesGrillaComboComponent } from '../shared/fdaccionesgrilla-combo.component';
import { FileService } from '../../../../shared/common/file.service';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { AbrirArchivosModalComponent } from '../abrirarchivo-modal/abrirarchivo-modal.component';
import { ErrorResponseAplicarAccionModalComponent } from '../errorResponse/errorResponse.component';
import { VerArchivosModalComponent } from '../verarchivo-modal/verarchivo-modal.component';
import { RechazarDocumentoComponent } from '../rechazar/rechazar-documento.component';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { UserService } from "../../../../auth/services/user.service";
import { AuthService } from '../../../../auth/auth.service';
import * as signalR from '@aspnet/signalr'

import { ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';
import { FileDTO } from '../../../../shared/common/models/fileDTO.model';
import { SignalRService } from '../../../../services/signalr.service';
import { EsperaFirmadorComponent } from '../documento-progreso/espera-firmador.component';


@Component({

    templateUrl: "./bandejaempleador.component.html",
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./bandejaempleador.component.css']
})
export class BandejaEmpleadorComponent extends BaseCrudComponent<DocumentosProcesadosDto, DocumentosProcesadosFilter> implements AfterViewInit, OnInit {



    //material dialog
    protected popupHeight: string = '';
    protected popupWidth: string = '80%';
    protected dialog: MatDialog;
    protected detailElement: IDetailComponent;
    private checkedAll: boolean = false;
    IsCheckingAll: boolean = false;

    //ViewChilds
    @ViewChild('RechazadoCombo') RechazadoCombo: YesNoAllComboComponent;
    @ViewChild('CerradoCombo') CerradoCombo: YesNoAllComboComponent;
    @ViewChild('AccionCombo') AccionCombo: FDAccionesGrillaComboComponent;
    @ViewChild('SucursalCombo') SucursalCombo: SucursalComboComponent;

    //Adjuntos
    time: string;
    appDownloadUrl: string;

    //Data seleccionada en la grilla
    selectedData: DocumentosProcesadosDto[] = [];

    //Data Total de la bandeja de entrada
    allData: DocumentosProcesadosDto[] = [];

    //Grilla de estados
    historialEstados: ArchivosTotalesPorEstado[] = [];
    mostrarGrilla: boolean = false;
    allowExportarExcel: boolean = false;
    numberOfDocs: number;
    maxValueInt: number = 999999;

    constructor(injector: Injector,
        private serviceDocumentos: DocumentosProcesadosService,
        protected serviceAcciones: FdAccionesService,
        protected fileService: FileService,
        protected cdr: ChangeDetectorRef,
        private _authService: AuthService,
        private _SignalRService: SignalRService
    ) {
        super(serviceDocumentos, null, injector);
        this.dialog = injector.get(MatDialog);
        this.isFirstTime = false;
        this.title = "Bandeja de documentos - Empleador"
        this.moduleName = "Firma Digital";
        this.icon = "flaticon-settings";
        // this.loadInMaterialPopup = false;
        this.advancedFiltersAreShown = true;
        this.appDownloadUrl = environment.siniestrosUrl + '/Adjuntos/DownloadFiles';
        this.getTablaEstados(this.filter);

        this.allowExportarExcel = this.permission.isGranted('FirmaDigital.BD-Empleador.Exportar')
    }

    getTablaEstados(Filter: DocumentosProcesadosFilter) {
        this.serviceDocumentos.HistorialArchivosPorEstado(Filter)
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

        this.RechazadoCombo.writeValue(this.filter.Rechazado);
        this.RechazadoCombo.refresh();
        this.CerradoCombo.writeValue(this.filter.Cerrado);
        this.CerradoCombo.refresh();

        var usr = this._authService.GetUserData();
        if (usr.sucursalId != null) {
            this.filter.SucursalId = usr.sucursalId;
            this.SucursalCombo.writeValue(this.filter.SucursalId);
            this.SucursalCombo.refresh();
            this.SucursalCombo.IsDisabled = true;
        }

        this.cdr.detectChanges();
    }

    ngOnInit() {
        super.ngOnInit();

    }

    onSearch(event?: LazyLoadEventData) {

        super.onSearch(event);
        this.getTablaEstados(this.filter);
        this.allSelectedChecker();
    }

    getNewfilter(): DocumentosProcesadosFilter {
        var filter = new DocumentosProcesadosFilter();
        filter.Rechazado = 2;
        filter.Cerrado = 2;
        filter.EstadoId;
        filter.EsEmpleador = true;
        return filter;
    }

    ExportarExcel() {
        this.fileService.dowloadAuthenticatedByPost(environment.firmaDigitalUrl + '/FdDocumentosProcesados/ExportarExcel', this.filter);
    }

    EjecutarAccion() {
        var apply = new AplicarAccionDto();
        apply.AccionId = this.filter.AccionId;
        apply.Empleador = true;

        this.selectedData.forEach(
            (e) => {
                apply.Documentos.push(e.Id);
            });

        if (apply.AccionId == -3) {//Rechazar
            this.AbrirPopupRechazar(apply);
        }
        else {
            this.ejecutarAccionInternal(apply);
        }
    }

    AbrirPopupRechazar(apply: AplicarAccionDto): void {

        var dto = new RechazarDto();

        const dialogConfig = new MatDialogConfig<RechazarDto>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        dialogConfig.data = dto;

        let dialogRef = this.dialog.open(RechazarDocumentoComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(

            data => {

                if (data && data.Motivo) {
                    apply.Motivo = data.Motivo;

                    this.ejecutarAccionInternal(apply);
                }

            }
        );
    }

    actualizar(event?: LazyLoadEventData) {
        this.allData = [];
        this.selectedData = [];
        //this.AccionCombo.IsDisabled = false;
        this.getTotalOfDocs(this.filter);
        this.onSearch(event);
        const checkAllCheckbox = document.getElementById('checkedAllRows') as HTMLInputElement;
        if (checkAllCheckbox != null) {
            checkAllCheckbox.checked = false;
        }
        this.filter.AccionId = null;
    }

    aplicandoAccion: boolean = false;

    private ejecutarAccionInternal(apply: AplicarAccionDto): void {
        mApp.blockPage();
        this.aplicandoAccion = true;
        this.serviceAcciones.AplicarAccion(apply)
            .finally(() => {
                mApp.unblockPage();
                this.aplicandoAccion = false;
            })
            .subscribe((t) => {
                if (t.DataObject && t.DataObject != null) {


                    if (apply.AccionId == -1) {
                        this.AbrirPopupVisor(t.DataObject as VisorArchivos[]);
                        this.selectedData = [];
                        this.checkIfSelected();
                    }
                    else if (apply.AccionId == -2) {

                        (t.DataObject as any[]).forEach((e) => {
                            this.fileService.downloadAnonymousFileByUrl(this.appDownloadUrl + '\?id=' + e + '&c=' + Date.now());
                        })
                        this.selectedData = [];
                        this.checkIfSelected();
                    }
                    else if (apply.AccionId == -4) {
                        this.VerPopupVisor(t.DataObject as VerDetalle[])
                    }
                    else if (apply.AccionId == 2) {
                        let data = t.DataObject as AplicarAccioneResponseDto;
                        this.DownloadFile(data.FileDto);
                        this.LoadProgressBar(apply, data);
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

        //this.AccionCombo.IsDisabled = false;
        //this.refreshTable();
        //this.allSelectedChecker();
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

    VerPopupVisor(lista: VerDetalle[]): void {

        const dialogConfig = new MatDialogConfig<VerDetalle[]>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;


        dialogConfig.data = lista;

        let dialogRef = this.dialog.open(VerArchivosModalComponent, dialogConfig);


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

                if (!((<DocumentosProcesadosDto>e).Rechazado == false && (<DocumentosProcesadosDto>e).PermiteRechazo == true)) {
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

    onRowSelect(event) {
        if (this.filter.AccionId > 0) {
            var findAccionPermitida = (<DocumentosProcesadosDto>event.data).AccionesDisponibles.find(f => f.AccionPermitidaId == this.filter.AccionId);
            if (findAccionPermitida == null) {
                var index = this.selectedData.findIndex(f => f.Id == (<DocumentosProcesadosDto>event.data).Id);
                this.selectedData.splice(index, 1);
            }
        }
        else if (this.filter.AccionId == -3 && (!((<DocumentosProcesadosDto>event.data).Rechazado == false && (<DocumentosProcesadosDto>event.data).PermiteRechazo == true))) {
            var index = this.selectedData.findIndex(f => f.Id == (<DocumentosProcesadosDto>event.data).Id);
            this.selectedData.splice(index, 1);
        }

        this.RemoverPermitirMarcarLote();
    }


    GetEditComponentType(): Type<IDetailComponent> {
        return null;
    }

    BorrarFiltros() {
        this.allData = [];
        this.selectedData = [];
        this.checkedAll = false;
        this.filter = this.getNewfilter();
        //this.AccionCombo.IsDisabled = false;
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
        if (this.filter.AccionId == 2) {
            if (row.AccionesDisponibles.find(e => e.AccionPermitidaId == 2)) {
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
                } else if (this.filter.AccionId == -3 && (selectedRow.PermiteRechazo && !selectedRow.Rechazado)) {
                    this.selectedData.push(selectedRow);
                    selectedRow.IsSelected = isChecked;
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

    // checkOne(selectedRow) {
    //     if (this.selectedData == null) {
    //         this.selectedData = [];
    //         if (this.filter.AccionId > 0) {
    //             if (selectedRow.AccionesDisponibles.some(e => e.AccionPermitidaId == this.filter.AccionId)) {
    //                 this.selectedData.push(selectedRow);
    //             }
    //         } else {
    //             this.selectedData.push(selectedRow);
    //         }
    //     } else {
    //         var findaccion = selectedRow.AccionesDisponibles.find(f => f.AccionPermitidaId == this.filter.AccionId);


    //         if (this.filter.AccionId > 0) {
    //             if (selectedRow.AccionesDisponibles.some(e => e.AccionPermitidaId == this.filter.AccionId)) {
    //                 if (findaccion.PermiteMarcarLote == true || this.selectedData.length == 0 && findaccion.PermiteMarcarLote == false) {
    //                     this.selectedData.push(selectedRow);
    //                 }
    //                 else {
    //                     const Checkbox = document.getElementById('checkbox-' + selectedRow.Id) as HTMLInputElement;
    //                     Checkbox.checked = false;
    //                 }
    //             }
    //         } else {
    //             this.selectedData.push(selectedRow);
    //         }
    //     }

    // }

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
        var filterPages = this.filter.PageSize;
        var filterPage = this.filter.Page;
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
    //         .subscribe((result: ResponseModel<PaginListResultDto<DocumentosProcesadosDto>>) => {
    //             this.list = result.DataObject.Items;
    //             this.primengDatatableHelper.records = result.DataObject.Items
    //             this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
    //             this.primengDatatableHelper.hideLoadingIndicator();
    //         })
    //     let filter2 =  _.cloneDeep(this.filter);
    //     this.allData = [];    
    //     filter2.PageSize = null;
    //     filter2.Page = null;   
    //     this.service.search(filter2)
    //         .finally(() => {
    //             this.primengDatatableHelper.isLoading = false;
    //         })
    //         .subscribe((result: ResponseModel<PaginListResultDto<DocumentosProcesadosDto>>) => {
    //             debugger;
    //             this.allData = new Array<DocumentosProcesadosDto>();
    //             this.allData = result.DataObject.Items;
    //     })

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


}