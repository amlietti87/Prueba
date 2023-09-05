import { Component, ViewEncapsulation, AfterViewInit, Injector, Type, ChangeDetectorRef, ViewChild, ElementRef, EventEmitter, Output, Input, Renderer } from '@angular/core';
import { BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent, DetailEmbeddedComponent } from '../../../../shared/manager/detail.component';
import { DiagramasInspectoresDto, DiagramasInspectoresFilter, DiagramaMesAnioDto, DiasMesDto, InspectorDiaDto, ValidationResult, InspDiagramaInspectoresTurnosDto } from '../model/diagramasinspectores.model';
import { DiagramasInspectoresService } from './diagramasinspectores.service';
import { CreateOrEditDiagramasInspectoresModalComponent } from './create-or-edit-diagramasinspectores-modal.component';
import { ActivatedRoute, Router, NavigationStart } from '@angular/router';
import { Subscription, Observer } from 'rxjs';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { TabsetComponent } from 'ngx-bootstrap';
import { NgForm, FormGroup } from '@angular/forms';
import { ViewMode, ItemDto, ResponseModel } from '../../../../shared/model/base.model';
import { ZonasDto, ZonasFilter } from '../model/zonas.model';
import { ZonasService } from '../zonas/zonas.service';
import { GruposInspectoresService } from '../gruposinspectores/gruposinspectores.service';
import { GruposInspectoresFilter } from '../model/gruposinspectores.model';
import { forEach } from '@angular/router/src/utils/collection';
import { RangosHorariosService } from '../rangoshorarios/rangoshorarios.service';
import * as _ from "lodash"
import * as moment from 'moment';
import { RangosHorariosDto, RangosHorariosFilter } from '../model/rangoshorarios.model';
import { DiagramasInspectoresValidatorService } from './diagramas-inspectores-validator.service';
import { ObserveOnSubscriber } from 'rxjs/operators/observeOn';
import { Observable } from 'rxjs/Observable';
import { ComponentCanDeactivate } from '../../../../shared/common/app-component-base';
import { setTimeout } from 'timers';

@Component({

    templateUrl: "./diagramacion.component.html",
    encapsulation: ViewEncapsulation.None,
    exportAs: 'Diagramacion',
    styleUrls: ["./diagramacion.component.css"]
})
export class DiagramacionComponent extends DetailEmbeddedComponent<DiagramasInspectoresDto> implements AfterViewInit, ComponentCanDeactivate {


    diagramacionSave: any;
    listModel: DiasMesDto[] = [];
    diasMesAP: DiasMesDto[] = [];
    listModelOriginal: DiasMesDto[] = [];
    turnosDiagrama: InspDiagramaInspectoresTurnosDto[] = [];
    innerHeight: number;

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditDiagramasInspectoresModalComponent
    }
    @ViewChild('tabSet') tabSet: TabsetComponent;
    @ViewChild('detailForm') detailForm: NgForm;
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @Input() zonasItems: ZonasDto[] = [];
    @Input() rangoshorariosItems: RangosHorariosDto[] = [];
    @Input() rangohorariosItemsSinFT: RangosHorariosDto[] = [];
    columns: InspectorDiaDto[] = [];
    listaDiagramacion: DiagramaMesAnioDto = new DiagramaMesAnioDto();
    data: any[];
    subQ: Subscription;
    subQ1: Subscription;
    DiagramaInspectorId: number;
    isLoading: boolean;
    protected element: ElementRef;
    mainForm: FormGroup;
    active = true;
    loading = true;
    diagramacionBusyText = "Cargando...";
    TurnosId: number[] = [];
    DiagramaInspectoresId: number;
    diagramacion: DiagramaMesAnioDto = new DiagramaMesAnioDto();
    allowmodificarDiagramacion: boolean = false;


    selft: any;
    constructor(injector: Injector, protected _DInspectoresService: DiagramasInspectoresService, private route: ActivatedRoute, private _renderer: Renderer,
        protected cdr: ChangeDetectorRef, private _activatedRoute: ActivatedRoute, private router: Router, private _serviceZona: ZonasService, private _serviceRangosHorarios: RangosHorariosService, private _validator: DiagramasInspectoresValidatorService) {
        super(_DInspectoresService, injector);
        this.title = "Diagramación"
        this.icon = "flaticon-settings";
        this.advancedFiltersAreShown = true;
        this.element = injector.get(ElementRef);
    }

    getDescription(item: DiagramasInspectoresDto): string {
        return item.Description
    }
    getNewItem(item: DiagramasInspectoresDto): DiagramasInspectoresDto {

        var item = new DiagramasInspectoresDto(item);
        return item;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
        this.setInnerWidthHeightParameters();
        this.cdr.detectChanges();
    }


    setInnerWidthHeightParameters() {

        this.innerHeight = window.innerHeight * 0.70;
    }
    ngOnInit() {
        let showDiagramacion = localStorage.getItem('showDiagramacion');
        if (showDiagramacion != 'true') {
            
            this.router.navigate(['/inspectores/diagramasinspectores']);
            return; 
        }
        else {
            localStorage.setItem('showDiagramacion', null);
        }
        this.allowmodificarDiagramacion = this.permission.isGranted("Inspectores.Diagramacion.Modificar");
        super.ngOnInit();
        this.route.queryParams
            .subscribe(parametros => {
                this.TurnosId = parametros.TurnoId;
                this.DiagramaInspectorId = parametros.DiagramaInspectoresId;
            });

        this.active = true;
        this.loading = true;

        this.RecuperarDiagrama();

        $('#fullscreentools')[0].click();
        $('body').addClass("smallsize");
    }

    RecuperarDiagrama() {

        this.loading = true;
        this.diagramacionBusyText = "Cargando...";
        this._DInspectoresService.getDiagramaMesAnio(this.DiagramaInspectorId, this.TurnosId, true)
            .finally(() => {
                this.diagramacionBusyText = "Cargando...";
                this.loading = false;
            })

            .subscribe(e => {
                if (e.DataObject) {
                    this.listaDiagramacion = e.DataObject;
                    this.columns = _.cloneDeep(this.listaDiagramacion.DiasMes[0].Inspectores);

                    let lastInspTurno: string = "";

                    this.columns.forEach(e => {
                        lastInspTurno = e.InspTurno;
                        if (e.ColSpan == null) {
                            let other = this.columns.filter(c => c.InspTurno == lastInspTurno);
                            other.forEach(o => o.ColSpan = 0);
                            other[0].ColSpan = other.length;
                        }

                    });

                    this.listModel = this.listaDiagramacion.DiasMes;
                    this.diasMesAP = this.listaDiagramacion.DiasMesAP;
                    this.clearValidations();
                    this.listaDiagramacion.DiasMes.forEach(e => e.Inspectores.forEach(i => i.EsModificada = false));
                    this.listModelOriginal = _.cloneDeep(this.listaDiagramacion.DiasMes);
                    //Se creo servicio para volver atras el eliminar
                    this._DInspectoresService.setDiagramaOriginal(this.listModelOriginal);
                    var filtroZona = new ZonasFilter();
                    filtroZona.Anulado = 2;

                    //TODO e.DataObject.GrupoInspectoresId
                    filtroZona.GrupoInspectoresId = e.DataObject.GrupoInspectoresId;
                    this._serviceZona.requestAllByFilter(filtroZona)
                        .subscribe(e => {
                            this.zonasItems = e.DataObject.Items
                            //this.zonasItems.unshift(new ZonasDto({ Descripcion:"Todos"}));
                        });

                    var filtroRangosHorarios = new RangosHorariosFilter();
                    filtroRangosHorarios.Anulado = 2;
                    //TODO e.DataObject.GrupoInspectoresId
                    filtroRangosHorarios.GrupoInspectoresId = e.DataObject.GrupoInspectoresId;
                    this._serviceRangosHorarios.requestAllByFilter(filtroRangosHorarios)
                        .subscribe(e => {
                            this.rangoshorariosItems = e.DataObject.Items
                            this.rangohorariosItemsSinFT = e.DataObject.Items.filter(e => e.EsFrancoTrabajado == false);
                            //this.zonasItems.unshift({ Descripcion:"Todos", Id: null });
                        });

                    this._validator.recuperarDiagramacionTurnosNoSeleccionados(this.DiagramaInspectorId, this.columns);
                }
            }, error => {
                this.notify.warn(error.error.Messages[0]);
                this.router.navigate(['inspectores/diagramasinspectores'])
                this.active = false;
            });
    }

    ngOnDestroy() {
        super.ngOnDestroy();
        if (this.subQ) {
            this.subQ.unsubscribe();
        }
    }

    BorrarFiltros() {
        this.filter = this.getNewfilter();
    }

    getNewfilter(): DiagramasInspectoresFilter {
        var filter = new DiagramasInspectoresFilter();

        return filter;
    }

    scrollToElement(): void {
        $('#porletDiagramacion')[0].scrollTo(0, 0);
    }

    getSelector(): string {
        return this.element.nativeElement.tagName;
    }


    canDeactivate(): boolean {
        let isInValid: boolean = this.validarPublicado();
        let model = this.getModel();
        return isInValid || (!isInValid && model.length == 0);
    }
    confirmMessage(): string {
        return "Existen Datos sin guardar, ¿Desesa continuar?";
    }


    close(): void {
        let selft = this;
        let internalClose = async function() {

            selft.breadcrumbsService.RemoveItem(selft.getSelector());
            selft.scrollToElement();
            $('#fullscreentools')[0].click();
            selft.listModel = [];
            selft.router.navigate(['inspectores/diagramasinspectores'])
            selft.closeMax();
            selft.active = false;
            selft.viewMode = ViewMode.Undefined;
            selft.modalClose.emit(true);

            if (selft.detailForm) {
                selft.detailForm.resetForm();
            }

            if (selft.mainForm) {
                selft.mainForm.reset();
            }
            $('body').removeClass("smallsize");
            await selft.service.unBlockEntity(selft.DiagramaInspectorId).toPromise();
            
        }

        let model = this.getModel();
        let isInValid: boolean = this.validarPublicado();
        if (!isInValid) {
            if (model.length > 0) {
                this.message.confirm("¿Desea guardar los cambios realizados?", "Diagramación", e => {
                    if (e.value) {
                        this.guardarDiagramacion(internalClose);
                    }
                    else {
                        //internalClose();
                    }
                }, { confirmButtonText: 'Aceptar', cancelButtonText: 'Cancelar' });
            }
            else {
                internalClose();
            }
        }

    }

    getColorInsp(color: string) {
        return color;
    }

    getColor(num: number) {

        return this.listModel.find(c => c.NumeroDia == num).Color;
    }



    guardarDiagramacion(callback = null): void {
        let saveData: InspectorDiaDto[];

        var isInValid: boolean = this.validarPublicado();


        if (!isInValid) {
            saveData = this.getModel();
            saveData = _.cloneDeep(saveData);
            

            //if (!this.validateSave() ) {
            saveData.forEach(e => {
                //debugger;
                //e.diaMes = _.cloneDeep(e.diaMes);
                //e.diaMes.Inspectores.forEach(d => d.diaMes = null);
                if ((e.ZonaId as any) == "null")
                    e.ZonaId = null;
                e.diasMesAP = null;
                if (e.HoraDesde) {
                    e.HoraDesde = (moment(e.HoraDesde).format("YYYY-MM-DDTHH:mm:ss") as any)
                }
                if (e.HoraHasta) {
                    e.HoraHasta = (moment(e.HoraHasta).format("YYYY-MM-DDTHH:mm:ss") as any)
                }
                if (e.HoraDesdeModificada) {
                    e.HoraDesdeModificada = (moment(e.HoraDesdeModificada).format("YYYY-MM-DDTHH:mm:ss") as any)
                }
                if (e.HoraHastaModificada) {
                    e.HoraHastaModificada = (moment(e.HoraHastaModificada).format("YYYY-MM-DDTHH:mm:ss") as any)
                }
            })
            this.loading = true;
            this.diagramacionBusyText = "Guardando...";
            this._DInspectoresService.saveDiagramacion(saveData, this.DiagramaInspectorId, this.listaDiagramacion.BlockDate)
                //.finally(() => {
                //    this.loading = false;
                //})
                .subscribe(s => {
                    if (s.Status == "Ok") {
                        if (callback) {
                            callback();
                        }
                        else {
                            this.RecuperarDiagrama();
                        }
                        this.notify.info('Guardado exitosamente');
                    }

                }, error => {
                        if (callback) {
                            callback();
                        }

                        this.handleErros(error, null);
                        
                        //this.notify.error(error);
                        this.loading = false;
        });

        }

    }
    validarPublicado(): boolean {

        let isInValid: boolean = false;
        if (this.listaDiagramacion.Estado) {
            if (this.listaDiagramacion.Estado.toUpperCase() == 'PUBLICADO') {
                this.listModel.forEach(c => {
                    c.Inspectores.forEach(i => {
                        if (!i.EsFranco && !i.EsFrancoTrabajado && !i.EsJornada && !i.EsNovedad) {
                            isInValid = true;
                        }
                    });
                });
            }
        }

        if (isInValid) {
            this.message.warn('La diagramación no está completa y está Publicada.');
        }

        return isInValid;
    }

    validateSave(): boolean {
        this.clearValidations();

        let validacion: ValidationResult[] = [];

        var isValid = true;
        isValid = this.ValidateFeriadoPermiteHsExtras(this.listModel);
        let isValida: ValidationResult = new ValidationResult();
        isValida.isValid = isValid;
        validacion.push(isValida);

        var isValid1 = true;
        isValid1 = this.ValidateFeriadoPermiteFrancoTrabajadoGeneral(this.listModel);
        let isValida1: ValidationResult = new ValidationResult();
        isValida1.isValid = isValid1;
        validacion.push(isValida1);

        //let isValid2: ValidationResult;
        //isValid2 = this._validator.ValidateHorasExtrasPorGrupo(this.listModel, this.diasMesAP);
        //validacion.push(isValid2);

        //let isValid3: ValidationResult;
        //isValid3 = this._validator.ValidateHorasFeriadoPorGrupo(this.listModel, this.diasMesAP);
        //validacion.push(isValid3);

        //let isValid4: ValidationResult;
        //isValid4 = this._validator.ValidateHorasFrancoTrabajadoPorGrupo(col,this.listModel, this.diasMesAP);
        //validacion.push(isValid4);

        //Hacemos la Validacion por empleado
        this.columns.forEach(col => {

            let val = this._validator.ValidateHorasFeriadoParaInspector(col, this.listModel, this.diasMesAP);
            if (!val.isValid) {
                col.validations.push(val);
            }
            validacion.push(val);

            let val1 = this._validator.ValidateHorasExtrasParaInspector(col, this.listModel, this.diasMesAP);
            if (!val1.isValid) {
                col.validations.push(val1);
            }
            validacion.push(val1);

            let val2 = this._validator.ValidateHorasFrancoTrabajadoParaInspector(col, this.listModel, this.diasMesAP);
            if (!val2.isValid) {
                col.validations.push(val2);
            }
            validacion.push(val2);
        });
        return validacion.filter(e => !e.isValid).length > 0;
    }
    clearValidations() {
        this.listModel.forEach(row => {
            row.Inspectores.forEach(celda => celda.validations = []);
        });

        this.columns.forEach(e => e.validations = []);
    }

    ValidateFeriadoPermiteHsExtras(row: DiasMesDto[]): boolean {
        //Revisar esta validacion ya que el modelo cambio

        let isValid: ValidationResult[] = [];

        row.forEach(dia => {

            dia.Inspectores.forEach(celda => {
                let val = this._validator.ValidateFeriadoPermiteHsExtras(celda, dia, this.listModel, this.diasMesAP);
                isValid.push(val);
                if (!val.isValid) {
                    celda.validations.push(val);
                }
            });
        });

        return isValid.filter(e => !e.isValid).length > 0;

    }

    ValidateFeriadoPermiteFrancoTrabajadoGeneral(row: DiasMesDto[]): boolean {
        //Revisar esta validacion ya que el modelo cambio

        let isValid: ValidationResult[] = [];

        row.forEach(dia => {
            dia.Inspectores.forEach(celda => {
                let val = this._validator.ValidateFeriadoPermiteFrancoTrabajadoGeneral(celda, dia, this.listModel);
                isValid.push(val);
                if (!val.isValid) {
                    celda.validations.push(val);
                }
            });
        });

        return isValid.filter(e => !e.isValid).length > 0;

    }

    getModel(): InspectorDiaDto[] {

        let items: InspectorDiaDto[] = [];
        this.listModel.forEach(dia => {
            dia.Inspectores.forEach(insp => {
                if (this.ContieneInformacion(insp)) {
                    if (this.hasChange(insp)) {
                        items.push(insp)
                    }
                }
            });

        });

        return items;
    }

    ContieneInformacion(insp: InspectorDiaDto): boolean {
        return insp.EsFranco || insp.EsJornada || insp.EsFrancoTrabajado;
    }

    hasChange(item: InspectorDiaDto): boolean {
        let original = {};
        let selft = this;



        selft.listModelOriginal.forEach(dia => {
            dia.Inspectores.forEach(insp => {
                if (insp.Id == item.Id) {
                    original = insp;
                }
            })

        });
        let resp = _.isEqualWith(item, original, selft.compareCustom);

        if (!resp) {

            let diffProperties = _.reduce(item, function (result, value, key) {
                return _.isEqual(value, original[key], selft.compareCustom) ?
                    result : result.concat(key);
            }, []);

            console.log(item);
            console.log(original);
            console.log("", diffProperties);
            

        }

        return !resp
    }


    compareCustom(value1, value2, key) {

        if (key == "EsModificada") {
            return true;
        }

        if (key == "diaMesFecha") {
            return true;
        }

        if (key == "diaMes") {
            return true;
        }

        if (key == "HoraDesdeModificada") {

            return _.isEqual(moment(value1).toDate(), moment(value2).toDate());
        }

        if (key == "HoraHastaModificada") {

            return _.isEqual(moment(value1).toDate(), moment(value2).toDate())
        }

        if (value1 == "null") {
            return value2 == "null" || value2 == undefined || value2 == null;
        }

        if (value2 == "null") {
            return value1 == "null" || value1 == undefined || value1 == null;
        }

        //let compare = _.differenceWith(value1, value2);
        //console.log(compare);


        return undefined

    }

    getErrorMessage(insp: InspectorDiaDto): string {
        return insp.validations.map(e => e.Messages).join("-");
    }

    onsetloading($event) {
        this.loading = $event;
        this.diagramacionBusyText = "Cargando...";        
    }
}