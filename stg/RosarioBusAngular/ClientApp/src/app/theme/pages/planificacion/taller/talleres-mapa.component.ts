import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';
import { Helpers } from '../../../../helpers';
import { ScriptLoaderService } from '../../../../services/script-loader.service';
import { AppComponentBase } from '../../../../shared/common/app-component-base';

import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';



import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { SucursalDto } from '../model/sucursal.model';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditTallerModalComponent } from './create-or-edit-taller-modal.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { TallerDto, TallerFilter } from '../model/taller.model';
import { TallerService } from './taller.service';
import { SucursalService } from '../sucursal/sucursal.service';
import { Subscription } from 'rxjs';
import { PaginListResultDto, ResponseModel, ViewMode } from '../../../../shared/model/base.model';
import { TallerMapsComponent } from '../../../../components/rbmaps/taller.maps.component';
import { debounce } from 'rxjs/operators';
import { NgForm } from '@angular/forms';
import { RbMapMarker } from '../../../../components/rbmaps/RbMapMarker';
import { PuntoDto } from '../model/punto.model';
import { SectorDto } from '../model/sector.model';
import { RbMapServices } from '../../../../components/rbmaps/RbMapServices';
import { RutaDto, RutaFilter } from '../model/ruta.model';
import { pie } from 'd3';

@Component({

    templateUrl: "./talleres-mapa.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TallerComponent extends BaseCrudComponent<TallerDto, TallerFilter> implements OnInit, AfterViewInit {
    puntosInicioFin: PuntoDto[] = [];
    rutas: RutaDto[] = [];
    sub: Subscription;
    Sucursal: string;
    Sucursalid: number;
    customdetail: CreateOrEditTallerModalComponent;
    Sucursales: SucursalDto[] = [];
    loadOnInit: boolean = false;
    saving: boolean = false;

    @ViewChild('mapaComponents') mapaComponents: TallerMapsComponent;



    constructor(injector: Injector,
        protected _SucursalService: SucursalService,
        protected _Service: TallerService,
        private _activatedRoute: ActivatedRoute,
        private tallerService: TallerService
    ) {
        super(_Service, CreateOrEditTallerModalComponent, injector);
        this.icon = "fa fa-wrench"
        this.title = "Talleres";

    }


    getNewfilter(): TallerFilter {
        return new TallerFilter();
    }


    ngAfterViewInit() {
        this.GetEditComponent();
    }


    GetEditComponent(): IDetailComponent {

        var e = super.GetEditComponent();
        (e as CreateOrEditTallerModalComponent).setSucursal(this.Sucursalid, this.Sucursal);
        return e;
    }

    ngOnInit() {
        super.ngOnInit();
        var selft = this;
        (this.GetEditComponent() as CreateOrEditTallerModalComponent).ApplyTaller.subscribe(t => {
            this.notify.info('Guardado exitosamente');
            this.onSearch(null);
        });

        (this.GetEditComponent() as CreateOrEditTallerModalComponent).CancelTaller.subscribe(t => {
            var item = selft.list.find(e => e.Id == t.Id);
            if (item) {

                if (item.isNew) {
                    this.mapaComponents.callbackEliminarMarcador(item);
                    //(this.GetEditComponent() as CreateOrEditTallerModalComponent) 
                }
                else {
                    this.removerTodasLasRutas();
                    this.recuperarRutasPorTaller(item);
                }



            }
        });



        (this.GetEditComponent() as CreateOrEditTallerModalComponent).OnMapSelected.subscribe(t => {
            selft.OnMapSelected(t);
        });

        (this.GetEditComponent() as CreateOrEditTallerModalComponent).OnTabMapSelected.subscribe(t => {
            selft.OnTabMapSelected(t);
        });



        //(this.GetEditComponent() as CreateOrEditTallerModalComponent).modalSaveTalleres.subscribe(t => {




        //});



        this.mapaComponents.OnClickMarcador.subscribe(e => {

            this.OnClickMArcador(e, true)

        });



        this.mapaComponents.OnDeleteMarcador.subscribe(e => {

            var data = e as TallerDto;

            var filter = new TallerDto();
            filter.Id = data.Id;
            filter.Nombre = data.Nombre;
            filter.PosGal = data.PosGal;
            filter.SucursalId = data.SucursalId;

            if (data.isNew) {
                this.mapaComponents.callbackEliminarMarcador(data);
            }
            else {
                this._Service.delete(Number.parseInt(filter.Id))
                    .finally(() => {

                    })
                    .subscribe(resul => {

                        if (resul.DataObject) {
                            this.mapaComponents.callbackEliminarMarcador(data);
                        }
                        else {

                            //this.notificationService.warn("No se puede eliminar el taller, porque tiene rutas asocia.");
                        }

                    });
            }
        });







        this.mapaComponents.AfterAddMaker.subscribe(e =>
            this.AfterAddMaker(e)
        );

        this._SucursalService.requestAllByFilterCached()
            .then(e => {
                this.Sucursales = e;
            });

        this.sub = this._activatedRoute.params.subscribe(params => {
            this.paramsSubscribe(params);
        }
        );
    }


    OnClickMArcador(e: TallerDto, show: boolean) {
        this.removerTodasLasRutas();

        if (e.isNew) {
            if (show)
                (this.GetEditComponent() as CreateOrEditTallerModalComponent).showDto(e);
        }
        else {

            if (show)
                (this.GetEditComponent() as CreateOrEditTallerModalComponent).showDto(e);
        }
        //this.recuperarRutasPorTaller(e);

    }

    recuperarRutasPorTaller(e: TallerDto) {

        var filter: TallerFilter = new TallerFilter();

        filter.Lat = e.Lat;
        filter.Long = e.Long;
        filter.Nombre = e.Nombre;
        filter.SucursalId = this.Sucursalid;


        //Se puede mejorar para performance
        this._Service.GetRutasByGalpon(filter)
            .finally(() => {
                this.isTableLoading = false;
            })
            .subscribe((result: ResponseModel<RutaDto[]>) => {
                result.DataObject.forEach(r => {
                    r.Selected = r.BanderaId != 0;
                    this.OnMapSelected(r);

                });
                e.Rutas = result.DataObject;
            });
    }


    removerTodasLasRutas() {
        var puntos = [];

        //this.list.forEach(t => {
        //    if (t.Rutas)
        //        t.Rutas.forEach(r => {
        //            if (r.Puntos)
        //                r.Puntos.forEach(p => puntos.push(p));
        //        });
        //});

        var sucursales = (this.GetEditComponent() as CreateOrEditTallerModalComponent).sucursales;

        sucursales.forEach(s => {
            if (s.Rutas) {
                s.Rutas.forEach(ruta => {
                    if (ruta.Selected) {
                        this.removerRutaDelMapa(ruta);
                    }
                });
                s.Rutas = [];
            }



        });

        //TODO: agreggue esto porque no eliminaba las polylines 
        this.mapaComponents.map.polylines.forEach(e => e.setMap(null))

    }


    removerRutaDelMapa(ruta: RutaDto) {
        var element = this.getPuntoFin(ruta);
        if (element.Polylines) {
            for (let ip = 0; ip < element.Polylines.length; ip++) {
                element.Polylines[ip].setMap(null);
            }
        }

        element.Polylines = [];
        element.Steps = [];
    }

    OnTabMapSelected(sucursalSelecionada: SucursalDto) {

        (this.GetEditComponent() as CreateOrEditTallerModalComponent).sucursales.forEach(sucursal => {
            if (sucursal && sucursal.Rutas) {
                sucursal.Rutas.forEach(ruta => {
                    if (ruta.Selected) {
                        this.removerRutaDelMapa(ruta);
                    }
                });
            }
        });

        this.mapaComponents.removeLayerPuntos();

        if ((this.GetEditComponent() as CreateOrEditTallerModalComponent).viewMode != ViewMode.Add) {
            this.searchPuntosInicioFin(sucursalSelecionada.Id);
        }
    }


    OnMapSelected(ruta: RutaDto) {

        try {


            if (!ruta.Selected) {
                this.removerRutaDelMapa(ruta);
            }
            else {

                var selft = this;
                var f = function() {

                    try {
                        if (ruta.Sectores == null || ruta.Sectores.length == 0) {

                            var pi = selft.getPuntoInicio(ruta)
                            var pf = selft.getPuntoFin(ruta);

                            var distancia = 0.0;

                            if (ruta.Puntos) {
                                ruta.Puntos.forEach(e => {

                                    if (e.Steps) {
                                        e.Steps.forEach(step => {
                                            if (step) {
                                                distancia += step.distance.value;
                                            }
                                        })
                                    }
                                });
                            }

                            var snew = new SectorDto();
                            snew.Id = 0;
                            snew.Data = "Tiempo";
                            snew.Descripcion = pi.Abreviacion + '-' + pf.Abreviacion;
                            snew.DistanciaKm = distancia / 1000;
                            snew.PuntoInicioId = pi.Id;
                            snew.PuntoFinId = pf.Id;
                            snew.RutaId = ruta.Id;
                            snew.Color = "#000";
                            ruta.Sectores = [];
                            ruta.Sectores.push(snew);
                        }
                    } catch (e) {

                    }
                }

                this.mapaComponents.hacerRuta(this.getPuntoInicio(ruta), this.getPuntoFin(ruta), false, this.mapaComponents, f);

                this.mapaComponents.fitBounds(ruta.Puntos);

            }



        } catch (e) {
            console.log(e);
        }


    }
    getPuntoInicio(ruta: RutaDto): PuntoDto {

        return ruta.Puntos.find(e => e.EsPuntoInicio);
    }
    getPuntoFin(ruta: RutaDto): PuntoDto {
        return ruta.Puntos.find(e => e.EsPuntoTermino);
    }


    AfterAddMaker(marker: RbMapMarker): any {


        var marcadorDto = this.mapaComponents.marcadores.filter(p => p.Id === marker.id)[0];
        if (marcadorDto == undefined) {
            marcadorDto = new TallerDto({
                Id: marker.id,
                Nombre: undefined,
                SucursalId: this.Sucursalid,
                Lat: marker.lat,
                Long: marker.lng
            });
            this.mapaComponents.marcadores.push(marcadorDto);
            // let marcadorclone = Object.assign({}, marcadorDto); // { ...puntoDto };
            marcadorDto.isNew = true;
            this.removerTodasLasRutas();
            (this.GetEditComponent() as CreateOrEditTallerModalComponent).showDto(marcadorDto);
        }
    }

    paramsSubscribe(params: any) {

        this.breadcrumbsService.defaultBreadcrumbs(this.title);
        if (params.sucursalid) {
            this.active = true;

            this.Sucursalid = +params['sucursalid'];

            this.filter.SucursalId = this.Sucursalid;

            this.onSearch();


            if (this.Sucursales.length > 0) {
                var e = this.Sucursales.find(e => e.Id == this.Sucursalid);
                this.SetUnidadNegocio(e, params.id);
            }
            else {
                this._SucursalService.getByIdCached(this.Sucursalid)
                    .then(e => {
                        this.SetUnidadNegocio(e, params.id);
                    });
            }

        }
    }

    private SetUnidadNegocio(e: SucursalDto, id: any) {
        this.Sucursal = e.Description
        this.GetEditComponent().active = false;


        var selft = this;
        var close = function() {
            selft.CloseChild()
        }

        this.breadcrumbsService.AddItem(this.title + ' ' + this.Sucursal, this.icon, '', null, close);
        if (id) {
            this.onEditID(id);
        }
        else {

            this.list = [];
            this.primengDatatableHelper.records = [];
        }
    }



    ngOnDestroy() {
        super.ngOnDestroy();


        if (this.sub) {
            this.sub.unsubscribe();
        }
    }

    getDescription(item: TallerDto): string {
        return item.Nombre;
    }
    getNewItem(item: TallerDto): TallerDto {
        var itemconunidad = new TallerDto(item)

        itemconunidad.SucursalId = this.Sucursalid;
        return itemconunidad;
    }



    Search(event?: LazyLoadEventData) {

        //if (this.isFirstTime == false) {
        //    this.isFirstTime = true;
        //    return;
        //}

        if (!this.filter) {
            this.filter = this.getNewfilter();
        }
        this.isTableLoading = true;

        this.filter.PageSize = 10000;
        this.filter.SucursalId = this.Sucursalid;
        this.mapaComponents.crearMapa();
        this.mapaComponents.setSucursal(this.Sucursalid, this.Sucursal);
        var selft = this;
        this.service.search(this.filter)
            .finally(() => {
                selft.isTableLoading = false;
            })
            .subscribe((result: ResponseModel<PaginListResultDto<TallerDto>>) => {

                selft.list = result.DataObject.Items;

                selft.mapaComponents.setTalleres(selft.list);

                if (this.list.length > 0) {
                    selft.searchPuntosInicioFin(selft.Sucursalid);
                }
            });


    }

    searchPuntosInicioFin(sucursalid: number) {


        if (this.list && this.list.length > 0) {
            var filter = new TallerFilter();
            filter.Lat = (this.GetEditComponent() as CreateOrEditTallerModalComponent).detail.Lat;
            filter.Long = (this.GetEditComponent() as CreateOrEditTallerModalComponent).detail.Long;
            filter.Nombre = (this.GetEditComponent() as CreateOrEditTallerModalComponent).detail.Nombre;
            filter.SucursalId = sucursalid;
            this._Service.GetRutasByGalpon(filter)
                .finally(() => {
                    this.isTableLoading = false;
                })
                .subscribe((result: ResponseModel<RutaDto[]>) => {
                    var rutas = result.DataObject;


                    var p: PuntoDto[] = [];
                    for (var i = 0; i < rutas.length; i++) {
                        for (var j = 0; j < rutas[i].Puntos.length; j++) {
                            if (!this.list.some(t => t.Nombre == rutas[i].Puntos[j].Abreviacion)) {
                                p.push(rutas[i].Puntos[j]);
                            }

                        }

                    }
                    this.puntosInicioFin = p;
                    setTimeout(() => {
                        this.mapaComponents.addLayerMaestros(this.puntosInicioFin);
                    }, 10);
                });
        }

    }

    toogleLayer() {
        this.mapaComponents.toogleLayer(this.puntosInicioFin);
    }


    save(form: NgForm): void {




        var sinDesc = this.list.filter(e => e.Nombre === undefined || e.Nombre === '');

        if (sinDesc.length > 0) {
            this.message.warn('Existen Talleres sin nombre, ingrese el nombre a todos los talleres', 'Taller');
            return;
        }
        var i = 0;

        this.list.forEach(e => {
            if (e.isNew) {
                e.Id = (i--).toString();
            }
        });


        this.saving = true;
        this._Service.SaveGalponPorSucursal(this.list)
            .finally(() => {
                this.saving = false;
            })
            .subscribe((t) => {
                this.notify.info('Guardado exitosamente');
                this.onSearch(null);
            })
    }




}