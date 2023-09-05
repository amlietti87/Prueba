import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit, ChangeDetectorRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
declare let mApp: any;


import * as _ from 'lodash';

import { DetailEmbeddedComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { RutaService } from './ruta.service';
import { RutaDto } from '../model/ruta.model';
import { RbmapsComponent } from '../../../../components/rbmaps/rbmaps.component';
import { PuntoService } from '../punto/punto.service';
import { PuntoFilter, PuntoDto } from '../model/punto.model';
import { ESTADOS_RUTAS } from '../../../../shared/constants/constants';
import { ResponseModel, PaginListResultDto, ViewMode, StatusResponse } from '../../../../shared/model/base.model';
import { NgForm } from '@angular/forms';
import { SectorService } from '../sector/sector.service';
import { SectorDto, SectorFilter } from '../model/sector.model';
import { Observable } from 'rxjs/Observable';
import { retry } from 'rxjs/operators/retry';
import { debounce } from 'rxjs/operator/debounce';
import { elementAt } from 'rxjs/operators/elementAt';
import { EstadoRutaComboComponent } from '../shared/estadoruta-combo.component';
import { ActivatedRoute, Router, RoutesRecognized } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter, pairwise } from 'rxjs/operators';
import { forEach } from '@angular/router/src/utils/collection';




@Component({
    selector: 'createOrEditRutaDtoModal',
    templateUrl: './create-or-edit-ruta-modal.component.html',

})
export class CreateOrEditRutaModalComponent extends DetailEmbeddedComponent<RutaDto> implements IDetailComponent, OnInit {


    @ViewChild('mapaComponents') mapaComponents: RbmapsComponent;
    @ViewChild('EstadoRutaCombo') EstadoRutaCombo: EstadoRutaComboComponent;
    IdEstadoRutaOriginal: number;
    estadoEnabled: boolean;
    allowSaveAndContinue: boolean;
    allowSaveAndShowMap: boolean;
    EditEnabled: boolean;
    puntosList: PuntoDto[];
    sectoresList: SectorDto[];
    isLoadingMapa: boolean = false;
    // isfillData: boolean = false;
    isVisibleMapa: boolean;
    loadOnInit: boolean = false;
    instructions: any[];
    showMaptab: boolean;
    isTabMapActive: boolean;
    public EsOriginal: boolean;
    sub: Subscription;
    subqueryparam: Subscription;
    returnUrl: string;

    getDescription(item: RutaDto): string {
        return item.Nombre;
    }

    public Sucursalid: boolean = false;
    constructor(
        injector: Injector,
        protected service: RutaService,
        protected _Puntosservice: PuntoService,
        protected _sectoresService: SectorService,
        private _activatedRoute: ActivatedRoute,
        private cdr: ChangeDetectorRef,
        private _router: Router
    ) {
        super(service, injector);
        this.detail = new RutaDto();

        this.icon = "flaticon-route";
        this.title = "Ruta";

    }

    initFirtTab(): void {
        $('#m_Heder_portlet_tab_generalruta').click();
    }

    onSaveAndShowMap(): void {

        if (this.detail.Instructions == null && this.detailForm.controls['Instructions'].invalid) {
            this.GetInstructions();
            this.detailForm.controls['Instructions'].setValue(this.detail.Instructions);
            this.cdr.detectChanges();
        }
        this.closeOnSave = false;
        this.showMaptab = true;
        this.save(this.detailForm);
    }

    show(id: any) {

        //this.service.getById(id).subscribe(result => {
        //    this.viewMode = ViewMode.Modify;
        //    this.showDto(result.DataObject);
        //});

        let self = this;
        this.isLoadingMapa = true
        var rutaFilter = new PuntoFilter();
        rutaFilter.RutaId = id;
        rutaFilter.PageSize = 1000000;
        rutaFilter.Sort = "Orden ASC";
        var sectorFilter = new SectorFilter();
        sectorFilter.RutaId = id;
        Observable.forkJoin([
            this._Puntosservice.search(rutaFilter),
            this._sectoresService.search(sectorFilter),
            this.service.getById(id)
        ])
            .finally(() => {
                this.isLoadingMapa = false;
            }).toPromise().then(t => {
                this.viewMode = ViewMode.Modify;


                this.puntosList = t[0].DataObject.Items;
                this.sectoresList = t[1].DataObject.Items;
                this.showDto(t[2].DataObject);

            });
    }


    OriginalDetail: string;

    completedataBeforeShow(item: RutaDto): any {
        //if (this.EstadoRutaCombo) {
        //    this.EstadoRutaCombo.onSearch();
        //}
        this.isTabMapActive = false;
        this.EditEnabled = false;
        this.estadoEnabled = false;
        this.allowSaveAndContinue = false;
        this.allowSaveAndShowMap = false;
        this.initFirtTab();

        this.mapaComponents.InitializeList();
        this.isVisibleMapa = false;

        //this.isfillData = false;
        // this.fillDataMapa();
        if (this.viewMode == ViewMode.Add) {
            item.EstadoRutaId = ESTADOS_RUTAS.Borrador;
            this.puntosList = [];
            this.EsOriginal = false;
        } else {
            this.EsOriginal = this.detail.EsOriginal != EnumOriginal.No;;
        }

        this.IdEstadoRutaOriginal = item.EstadoRutaId;

        if (item.EstadoRutaId == ESTADOS_RUTAS.Borrador) {
            this.EditEnabled = true;
            this.estadoEnabled = true;
            this.allowSaveAndContinue = true;
            this.allowSaveAndShowMap = true;
        }
        else if (item.EstadoRutaId == ESTADOS_RUTAS.Aprobada) {
            this.EditEnabled = false;
            //this.estadoEnabled = new Date(this.detail.FechaVigenciaDesde) > new Date();
            this.estadoEnabled = false;
            this.allowSaveAndContinue = false;
            this.allowSaveAndShowMap = false;
        } else {
            this.EditEnabled = false;
            this.estadoEnabled = false;
            this.allowSaveAndContinue = false;
            this.allowSaveAndShowMap = false;
        }

        if (this.viewMode == ViewMode.Add) {
            this.estadoEnabled = false;

        }

        this.mapaComponents.estadoEnabled = this.estadoEnabled;
        let clone = Object.assign({}, item) as RutaDto; // { ...puntoDto }; 
        this.detail = null;

        this.OriginalDetail = JSON.stringify(clone);

        this.detail = clone;
        var self = this;




        if (this.showMaptab) {
            setTimeout(function() {
                self.showMaptab = false;
                $("#m_portlet_tab_Mapa_header").click();
                self.tabMapaClick()
            }, 10);
        }

    }

    ngAfterViewInit(): void {
        mApp.initPortlets()
    }

    closeSinGuardar(): void {
        if (this.detail && this.detail.EstadoRutaId && parseInt(this.detail.EstadoRutaId.toString()) == ESTADOS_RUTAS.Borrador && this.tineCambios()) {
            this.message.confirm('Se perderán los cambios no guardados. ¿Desea continuar?', "Abandonar ventana: Edición de rutas", (a) => {
                if (a.value) {
                    this.mapaComponents.clearLastUpdatedPunto();
                    this.detailForm.reset();
                    if (this.returnUrl !== "") {
                        this._router.navigate([this.returnUrl]);
                    } else {
                        super.close();
                    }
                }
            });
        }
        else {
            this.mapaComponents.clearLastUpdatedPunto();
            this.detailForm.reset();
            if (this.returnUrl !== "") {
                this._router.navigate([this.returnUrl]);
            } else {
                super.close();
            }

        }
    }

    private tineCambios(): boolean {
        return this.mapaComponents.tieneCambios() || this.OriginalDetail != JSON.stringify(this.detail);
    }



    save(form: NgForm): void {

        if (this.detailForm && this.detailForm.form.invalid) {
            if (this.isTabMapActive) {
                this.initFirtTab();
            }
            return;
        }

        this.saving = true;
        this.completedataBeforeSave(this.detail);

        if (!this.validateSave()) {
            this.saving = false;
            return;
        }

        this.SaveDetail();
    }


    ngOnInit(): void {

        super.ngOnInit();
        this.returnUrl = "";
        this.sub = this._activatedRoute.params.subscribe(params => {
            this.paramsSubscribe(params);
        }
        );
        this.subqueryparam = this._activatedRoute.queryParams.subscribe(params => {
            if (params.returnUrl) {
                this.returnUrl = params['returnUrl'];
            }
        }
        );

        this.mapaComponents.SaveRuta.subscribe(e => {
            this.detail.Puntos = e as PuntoDto[];
            this.save(this.detailForm);
        });
        this.mapaComponents.SavePunto.subscribe(e => {
            var punto = e as PuntoDto;
            this.saving = true;
            this.completedataBeforeSave(this.detail);
            this._Puntosservice.createOrUpdate(punto, this.viewMode)
                .finally(() => { this.saving = false; })
                .subscribe(() => {
                    this.notify.info('Guardado exitosamente');
                    this.close();
                    this.modalSave.emit(null);
                })
        });
        this.mapaComponents.OnImportarPuntos.subscribe(e => {
            this._Puntosservice.RecuperarDatosIniciales(this.detail.Id).subscribe(p => {
                setTimeout(() => {
                    this.mapaComponents.GenerarPuntos(p.DataObject);
                }, 10);
            });
        });


    }

    paramsSubscribe(params: any) {
        //this.breadcrumbsService.defaultBreadcrumbs(this.title);

        if (params.id) {
            this.viewMode = ViewMode.Modify;
            this.id = +params['id'];
            this.show(this.id);
        }
    }

    ngOnDestroy() {
        super.ngOnDestroy();
        if (this.sub) {
            this.sub.unsubscribe();
        }        
    }

    completedataBeforeSave(item: RutaDto): any {

        if (this.viewMode == ViewMode.Modify) {
               
            if (this.isVisibleMapa) {
                item.Puntos = this.mapaComponents.getPuntos();
                item.Sectores = this.mapaComponents.getSectores();
            }
            else {
                item.Puntos = this.puntosList;
                item.Sectores = this.sectoresList;
            }

            //lo paso a original
            if (this.detail.EsOriginal == 0 && this.EsOriginal) {
                this.detail.EsOriginal = EnumOriginal.Original;
            }
            //saco el original
            else if (this.detail.EsOriginal == EnumOriginal.Original && !this.EsOriginal) {
                this.detail.EsOriginal = EnumOriginal.No;
            }
            //saco el original anterior
            else if (this.detail.EsOriginal == EnumOriginal.AnteriorOriginal && !this.EsOriginal) {
                this.detail.EsOriginal = EnumOriginal.No;
            }

            //TODO: no hace falta guardarlo, ojo performance mas adelante;
            item.Calles = JSON.stringify(this.instructions);

        }
        else if (this.viewMode == ViewMode.Add) {
            if (this.EsOriginal) {
                this.detail.EsOriginal = EnumOriginal.Original;
            }
        }
    }

    affterSave(detail: RutaDto): void {

        if (!this.closeOnSave) {
            this.breadcrumbsService.RemoveItem(this.getSelector());
            this.active = false;
            //this.mapaComponents.InitializeList();
            //this.mapaComponents.InitializeList();
            this.show(this.detail.Id);
        }
    }


    validateSave(): boolean {
        var isValid = true;
        var p: PuntoDto = null;
        let idEstadoRuta = parseInt(this.detail.EstadoRutaId.toString());

        console.log(this.detail.Puntos);

        if (idEstadoRuta !== ESTADOS_RUTAS.Borrador) {
            if (this.detail.Puntos != null) {
                this.detail.Puntos.forEach((item, i) => {

                    if (!item.PlaCoordenadaId) {
                        let esCambioSector = item.EsCambioSector;
                        let esPuntoInicio = item.EsPuntoInicio;
                        let esPuntoTermino = item.EsPuntoTermino;

                        if (esPuntoInicio) {
                            isValid = false;
                            p = item;
                            this.notificationService.warn("Falta completar informacion en el punto inicio.");
                        }

                        if (esCambioSector) {
                            isValid = false;
                            p = item;
                            this.notificationService.warn("Falta completar informacion en un punto de tipo cambio sector.");
                        }

                        if (esPuntoTermino) {
                            isValid = false;
                            p = item;
                            this.notificationService.warn("Falta completar informacion en el punto de término.");
                        }

                    }
                    var steps = JSON.parse(item.Data);
                    if (steps.steps && steps.steps.length == 0 && steps.instructions && steps.instructions.length > 0 && !item.EsPuntoInicio || steps.steps == null && !item.EsPuntoInicio || steps.instructions == null && !item.EsPuntoInicio) {
                        isValid = false;
                        p = item;
                        this.notificationService.warn("Algun punto del mapa no contiene datos, por favor redibujar el mismo.");     
                    }
                    if (item.Data == null || item.Data == "" || item.Data == "{}" || item.Data == "{\"steps\":[],\"instructions\":[]}" ) {
                        isValid = false;
                        p = item;
                        this.notificationService.warn("Algun punto del mapa no contiene datos, por favor redibujar el mismo.");
                    }
                });
            }
        };

        if (p) {
            this.mapaComponents.showPuntoDto(p);
        }

        return isValid;
    }

    saveAndClose(f: NgForm) {

        if (this.viewMode == ViewMode.Modify && this.detail.Instructions == null) {
            this.message.error("Las instrucciones son requeridas", "Instrucciones Requeridas");
            return;
        }
        if (this.viewMode == ViewMode.Modify &&
            this.detail.EstadoRutaId == ESTADOS_RUTAS.Aprobada &&
            this.IdEstadoRutaOriginal != ESTADOS_RUTAS.Aprobada
        ) {

            this.service.validateAprobarRutaDto(this.detail).subscribe((e) => {
                if (e.Status == StatusResponse.Ok) {
                    return super.save(f);
                }
                else {

                    this.message.confirm(e.DataObject, "", (a) => {
                        if (a.value) {
                            return super.save(f);
                        }
                    });
                }
            });
        }
        else if (this.IdEstadoRutaOriginal == ESTADOS_RUTAS.Aprobada && (this.detail.EstadoRutaId == ESTADOS_RUTAS.Borrador || this.detail.EstadoRutaId == ESTADOS_RUTAS.Descartada)) {
            //this.message.confirm("Esta por cambiar el estado de la ruta de APROBADO a BORRADOR/DESCARTADO. Esto implica modificaciones en otras rutas. Está seguro de continuar?", "", (a) => {
            //    if (a.value) {
            //        return super.save(f);
            //    }
            //});

            this.message.error("No se permite editar una ruta aprobada", "Error");
        }
        else {
            return super.save(f);
        }
    }


    private drawMap(): void {
        if (this.isVisibleMapa) {
            var selft = this;
            setTimeout(() => {
                //We have access to the context values


                this.mapaComponents.createMap()

                if (this.detail.TipoBanderaId == 2) {
                    this.mapaComponents.maxMarker = 2;
                }
                else {
                    this.mapaComponents.maxMarker = null;
                }
                this.isLoadingMapa = false;
                setTimeout(() => {
                    this.mapaComponents.setRuta(selft.puntosList, selft.sectoresList, selft.detail.Id);
                }, 10);
            }, 10);
        }

    }






    tabGeneraClick(): void {
        this.isTabMapActive = false;
    }


    tabMapaClick(): void {
        this.isTabMapActive = true;
        if (!this.isVisibleMapa) {
            this.isVisibleMapa = true;
            this.drawMap();
        }
    }

    //fillDataMapa(): Promise<Boolean> {
    //    let self = this;
    //    this.isLoadingMapa = true
    //    var rutaFilter = new PuntoFilter();
    //    rutaFilter.RutaId = this.detail.Id;
    //    rutaFilter.PageSize = 1000000;
    //    rutaFilter.Sort = "Orden ASC";
    //    var sectorFilter = new SectorFilter();
    //    sectorFilter.RutaId = this.detail.Id;
    //    return Observable.forkJoin([
    //        this._Puntosservice.search(rutaFilter),
    //        this._sectoresService.search(sectorFilter)
    //    ])
    //        .finally(() => {
    //            this.isLoadingMapa = false;
    //        }).toPromise().then(t => {
    //            this.isfillData = true;
    //            this.puntosList = t[0].DataObject.Items;
    //            this.sectoresList = t[1].DataObject.Items;
    //            this.GetInstructions();

    //            this.drawMap3();
    //            return true;
    //        });
    //}

    private CopyInstructions(puntos: PuntoDto[]) {

        this.instructions = [];
        let lista = [...puntos];
        this.mapaComponents.parsePuntosDto(lista);
        this.instructions = this.mapaComponents.GetInstructions(lista);
        let puntoswithcoordenada = [...puntos.filter(e => e.PlaCoordenadaId && e.PlaCoordenadaId != null).sort(e => e.Orden)];

        if (this.instructions && this.instructions.length >= 1 && puntoswithcoordenada && puntoswithcoordenada.length >= 1) {
            var instr = '';


            instr = "Desde: " + puntoswithcoordenada[0].PlaCoordenadaCalle1 + " y " + puntoswithcoordenada[0].PlaCoordenadaCalle2;

            this.instructions.forEach((e) => {
                instr = instr + ", " + e.short_name;
            });

            var last = puntoswithcoordenada[puntoswithcoordenada.length - 1];;

            this.detail.Instructions = instr + ", " + last.PlaCoordenadaCalle1 + " hasta " + last.PlaCoordenadaCalle2;

        }
    }

    GetInstructions(): void {
        this.CopyInstructions(this.puntosList);
    }
}

export enum EnumOriginal {
    No = 0,
    AnteriorOriginal = 1,
    Original = 2,
}
