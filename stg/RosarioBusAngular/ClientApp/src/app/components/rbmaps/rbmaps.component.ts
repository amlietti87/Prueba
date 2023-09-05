import { Component, Input, OnInit, group, ViewChild, ChangeDetectorRef, Injector, EventEmitter, Output, DoCheck, SimpleChanges, SimpleChange, OnChanges, IterableDiffers, KeyValueDiffer, KeyValueDiffers, ElementRef } from '@angular/core';

// import { RbMap } from '../rbmaps/RbMap';
// import { HEROES } from '../rbmaps/RbMaps';

import { RbMapMarker, CustomMarker, PuntoInfo, DataRecorrido } from '../rbmaps/RbMapMarker';
import { RbMapGrupo } from '../rbmaps/RbMapGrupo';
import { RbMapLinea } from '../rbmaps/RbMapLinea';

import { RbMapServicesGrupo } from './RbMapServicesGrupo';
import { RbMapServicesLinea } from './RbMapServicesLinea';
import { RbMapServices } from './RbMapServices';
import { AppComponentBase } from '../../shared/common/app-component-base';
import { PuntoDto } from '../../theme/pages/planificacion/model/punto.model';
import { CreateOrEditPuntoModalComponent } from '../punto/create-or-edit-punto-modal.component';
import { ViewMode } from '../../shared/model/base.model';
import { retry } from 'rxjs/operators/retry';
import { elementAt } from 'rxjs/operator/elementAt';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { SectorDto } from '../../theme/pages/planificacion/model/sector.model';
import { Observable } from 'rxjs';
import { ConfigurationService } from '../../shared/common/services/configuration.service';
import { moment } from 'ngx-bootstrap/chronos/test/chain';
import { TipoViajeDto } from '../../theme/pages/planificacion/model/tipoviaje.model';
import { TipoViajeService } from './tipoviaje.service';
import { MessageService } from '../../shared/common/message.service';


declare var GMaps: any;
declare var google: any;
declare var bootbox: any;
declare var $: any;
declare let mApp: any;

@Component({
    selector: 'app-rbmaps',
    templateUrl: './rbmaps.component.html',
    styleUrls: ['./rbmaps.component.css']
})

export class RbmapsComponent extends AppComponentBase implements OnInit {

    @Input() loadOnInit: boolean = true;
    estadoEnabled: boolean;

    @Output() SaveRuta: EventEmitter<any[]> = new EventEmitter<any[]>();
    @Output() SavePunto: EventEmitter<any> = new EventEmitter<any>();
    @Output() OnClickMarker: EventEmitter<RbMapMarker> = new EventEmitter<RbMapMarker>();

    @Output() OnImportarPuntos: EventEmitter<any> = new EventEmitter<any>();


    @ViewChild('createOrEditPuntoDtoModal') createOrEditPuntoDtoModal: CreateOrEditPuntoModalComponent;

    LastPointIndex: number = 0;
    paretId: number;
    map: any;
    isCreatedMap: boolean = false;
    busqueda: String;

    service: RbMapServices;

    kmlFile: any;

    modal: NgbModalRef;

    savedPuntosdto: PuntoDto[];
    @Input() puntosdto: Array<PuntoDto>;
    sectoresdto: SectorDto[];
    Icons: any;
    dataRecorrido: DataRecorrido;
    OriginalData: any[];
    RedoData: any[];
    TiposViajes: TipoViajeDto[];
    travelModenuevo: string;
    TipoViajeDefault = 1;
    private DefaultColor: string = "#000000";
    message: MessageService;
    private lastUpdatedPunto : PuntoDto;

    @Input() maxMarker: number;
    @Input() TipoBanderaId: number;



    constructor(injector: Injector,
        private modalService: NgbModal,
        private cdRef: ChangeDetectorRef,
        private _rbMapServices: RbMapServices,
        private _element: ElementRef,
        private _tvservice: TipoViajeService
    ) {
        super(injector);
        //this.service = injector.get();
        this.OriginalData = [];
        this.RedoData = [];
        this.service = _rbMapServices;
        //this.configurationService=
        this.busqueda = "pellegrini y sarmiento, rosario";
        //this.puntos = [];
        this.dataRecorrido = new DataRecorrido();

        //https://hpneo.github.io/gmaps/documentation.html
        //this.objDiffer = {};
        mApp.CONST_DELAY_BUILD_ROUTE = 1100;

    }








    ngOnInit() {

        this._tvservice.requestAllByFilter().subscribe(result => {

            this.TiposViajes = result.DataObject.Items;
        });


        if (this.loadOnInit) {
            this.createMap();
        }
        this.createOrEditPuntoDtoModal.modalSaveRuta.subscribe(e => {
            this.saveRuta();
        });


        this.createOrEditPuntoDtoModal.ApplyPunto.subscribe(e => {

            this.ApplyPunto(e);
        });

        this.createOrEditPuntoDtoModal.ApplySectores.subscribe(e => {

            this.ApplySectores(e);
        });

    }

    InitializeList(): void {


        this.test_removeMarkersAndPolylines(this);
        this.puntosdto = [];


    }


    removePolylines(): void {

        if (this.map) {
            this.map.removePolylines();
            //this.map.removeMarkers();
        }

    }


    createMap(): void {
        //this.puntos = [];       
        if (!this.map) {

            this.InitializeList();
            var rbmaps = this;
            var myStyles = [
                {
                    featureType: "poi",
                    elementType: "labels",
                    stylers: [
                        { visibility: "off" }
                    ]
                }
            ];
            this.map = new GMaps({
                div: '#map',
                lat: -32.954517,
                lng: -60.655931,
                click: function(e) {


                    if (rbmaps.maxMarker) {
                        if (rbmaps.puntosdto.length >= rbmaps.maxMarker) {
                            return;
                        }
                    }

                    rbmaps.saveLocalData("Agregar marcador");

                    rbmaps.test_AgregarMarcador(e.latLng, false);



                },
                styles: myStyles,
                disableDefaultUI: true,
                zoomControl: true,
                fullscreenControl: true
            });



            this.map.setContextMenu({
                control: 'marker',
                options: [{
                    title: 'Eliminar',
                    name: 'delete_marker',
                    action: function(e) {
                        rbmaps.test_EliminarMarcadorByMarcador(e.marker, rbmaps);
                    }
                }, {
                    title: 'Center here',
                    name: 'center_here',
                    action: function(e) {
                        this.setCenter(e.latLng.lat(), e.latLng.lng());
                    }
                }]
            });



        }
        else {
            this.InitializeList();
            this.removePolylines();
        }


    }




    crearViewMapa(): void {

        this.InitializeList();
        var rbmaps = this;
        var myStyles = [
            {
                featureType: "poi",
                elementType: "labels",
                stylers: [
                    { visibility: "off" }
                ]
            }
        ];
        this.map = new GMaps({
            div: '#map',
            lat: -32.954517,
            lng: -60.655931,
            styles: myStyles,
            disableDefaultUI: true
        });
    }

    buscar(busqueda: String): void {
        this.buscar_Gmaps(busqueda);
    }






    setRuta(puntosdto: PuntoDto[], sectoresdto: SectorDto[], paretId: number) {


        var This = this;

        //console.log("sectoresdto", sectoresdto);

        this.puntosdto = puntosdto;


        this.sectoresdto = sectoresdto;
        this.paretId = paretId;

        this.findOrdenSectores(this.sectoresdto);

        try {
            this.OriginalData = [];
            this.RedoData = [];
            var p = JSON.stringify(puntosdto);
            var s = JSON.stringify(sectoresdto);
            this.OriginalData.push({ puntosdto: p, sectoresdto: s, action: "original data" });
        } catch (e) {
            //console.log(puntosdto);
        }


        puntosdto.forEach(item => {
            This.test_AgregarMarcadorDto(item);
        });


        this.calcularDistanciaTotal();

        if (this.puntosdto.length > 0) {
            var up = this.puntosdto[this.puntosdto.length - 1];
            this.goTo(up.Lat, up.Long);
        }
        if (this.puntosdto.length > 0) {
            if (this.puntosdto[this.puntosdto.length - 1].Data == "") {
                this.recrearSectores();
                this.RedibujarRutaDto(this.puntosdto, 0, this);
            }
        }

        this.setMapPositionForLastUpdatedCoordinate();
    }


    polylinesAnteriores: any[];

    getPuntos(): PuntoDto[] {
        var i = 1;

        this.polylinesAnteriores = [];

        if (this.puntosdto) {
            this.puntosdto.forEach(item => {
                item.Orden = i++;
                item.Data = JSON.stringify({ steps: item.Steps, instructions: item.Instructions });

                this.polylinesAnteriores.push(item.Polylines);

                item.Steps = null;
                item.Polylines = null;
            });

            return this.puntosdto;
        }
        return new Array<PuntoDto>();
    }

    getSectores(): SectorDto[] {

        this.crearsectorPordefecto();

        this.regenerarSectores();

        return this.sectoresdto;
    }


    GetInstructions(puntosdto: PuntoDto[]): any[] {

        var _instructions = [];
        if (puntosdto) {
            puntosdto.forEach((item, i) => {

                try {
                    if (item.Instructions && item.Instructions.length > 0) {

                        var pa = puntosdto[i - 1];
                        if (pa == null || pa.Instructions == null || pa.Instructions.length == 0 || pa.Instructions[0].short_name != item.Instructions[0].short_name) {

                            item.Instructions[0].color = "#000";
                            if (item.Color) {
                                item.Instructions[0].color = item.Color;
                            }

                            _instructions.push(item.Instructions[0]);

                        }
                    }
                } catch (e) {
                    //console.log(pa);
                }



            });
        }
        return _instructions;
    }


    private crearsectorPordefecto(): void {
        if (this.sectoresdto == null || this.sectoresdto.length == 0) {
            this.sectoresdto = new Array<SectorDto>();
            if (this.puntosdto && this.puntosdto.length >= 2) {
                var snew = new SectorDto();
                snew.Id = 0;
                snew.Data = "Sector 1";
                snew.Descripcion = "Este es el sector 1";
                snew.DistanciaKm = this.dataRecorrido._distancia;
                snew.PuntoInicioId = this.puntosdto[0].Id;
                snew.PuntoFinId = this.puntosdto[this.puntosdto.length - 1].Id;
                snew.RutaId = this.paretId;
                snew.Color = this.DefaultColor;
                this.sectoresdto.push(snew);
            }
        }
    }




    private saveRuta() {
        this.SaveRuta.emit(this.getPuntos());
    }

    private savePuntos() {
        this.SavePunto.emit({});
    }

    private onClickMarker(marcador: RbMapMarker) {

        var puntoDto = this.puntosdto.filter(p => p.Id === marcador.id)[0];
        if (puntoDto.PlaCoordenadaAnulada == true) {
            this.message.warn("La coordenada de este punto esta anulada. Por favor cambiarla.", 'Coordenada Anulada');
        }
        //var item = this.puntos.filter(p => p.marcador.id === marcador.id)[0];

        this.createOrEditPuntoDtoModal.viewMode = ViewMode.Modify;
        this.createOrEditPuntoDtoModal.estadoEnabled = this.estadoEnabled;

        if (!puntoDto) {
            this.createOrEditPuntoDtoModal.viewMode = ViewMode.Add;

            puntoDto = new PuntoDto();
            puntoDto.setGmapMarker(marcador);
            puntoDto.RutaId = this.paretId;
            puntoDto.PlaTipoViajeId = this.TipoViajeDefault;

            this.puntosdto.push(puntoDto);
        }

        this.getPuntoByRbMapMarker(marcador, puntoDto);

        puntoDto.Lat = marcador.position.lat();
        puntoDto.Long = marcador.position.lng();
        if (this.TipoBanderaId && this.TipoBanderaId != null) {
            puntoDto.TipoBanderaId = this.TipoBanderaId;
        }

        if (puntoDto.PlaTipoViajeId == 0 || puntoDto.PlaCoordenadaId == null) {
            puntoDto.PlaTipoViajeId = this.TipoViajeDefault;
        }
        this.showPuntoDto(puntoDto);
    }

    showPuntoDto(puntoDto: PuntoDto, ) {

        var data = { steps: puntoDto.Steps, instructions: puntoDto.Instructions };
        puntoDto.Data = JSON.stringify(data);

        let puntoDtoClone = Object.assign({}, puntoDto); // { ...puntoDto }; 

        this.createOrEditPuntoDtoModal.puntoOriginal = puntoDto;


        this.createOrEditPuntoDtoModal.showDto(puntoDtoClone);
    }


    private ApplyPunto(updateditem: PuntoDto) {
        this.saveLocalData();

        var puntoDto = this.puntosdto.filter(p => p.Id === updateditem.Id)[0];

        puntoDto.CodigoNombre = updateditem.CodigoNombre;


        puntoDto.RutaId = updateditem.RutaId;
        puntoDto.EsPuntoInicio = updateditem.EsPuntoInicio;
        puntoDto.EsPuntoTermino = updateditem.EsPuntoTermino;

        puntoDto.EsParada = updateditem.EsParada;
        puntoDto.EsCambioSectorTarifario = updateditem.EsCambioSectorTarifario;
        puntoDto.EsPuntoRelevo = updateditem.EsPuntoRelevo;
        puntoDto.TipoParadaId = updateditem.TipoParadaId;
        puntoDto.NumeroExterno = updateditem.NumeroExterno;
        puntoDto.Abreviacion = updateditem.Abreviacion;
        puntoDto.MostrarEnReporte = updateditem.MostrarEnReporte;

        if (puntoDto.PlaTipoViajeId != updateditem.PlaTipoViajeId) {
            var puntoDtoAnt = this.puntosdto.filter(p => p.Orden == (updateditem.Orden) - 1)[0];
            puntoDto.PlaTipoViajeId = updateditem.PlaTipoViajeId;
            this.hacerRuta(puntoDtoAnt, puntoDto, true, this)
        }

        if (puntoDto.SectoresTarifariosItem != updateditem.SectoresTarifariosItem) {
            puntoDto.SectoresTarifariosItem = updateditem.SectoresTarifariosItem;
            puntoDto.CodSectorTarifario = updateditem.SectoresTarifariosItem.Id;
        }

        if (puntoDto.PlaCoordenadaItem != updateditem.PlaCoordenadaItem) {
            puntoDto.PlaCoordenadaItem = updateditem.PlaCoordenadaItem;
            puntoDto.PlaCoordenadaId = updateditem.PlaCoordenadaItem.Id;
            puntoDto.PlaCoordenadaAnulada = updateditem.PlaCoordenadaAnulada;
        }

        if (updateditem.EsParada) {
            if (puntoDto.PlaParadaItem != updateditem.PlaParadaItem) {
                puntoDto.PlaParadaItem = updateditem.PlaParadaItem;
                puntoDto.PlaParadaId = updateditem.PlaParadaItem.Id;
            }
        } else {
            puntoDto.PlaParadaItem = null;
            puntoDto.PlaParadaId = null;
        }



        if (puntoDto.EsCambioSector != updateditem.EsCambioSector) {

            if (puntoDto.EsCambioSector == true && updateditem.EsCambioSector == false) {
                puntoDto.PlaCoordenadaId = null;
                puntoDto.PlaCoordenadaItem = null;
            }
            puntoDto.EsCambioSector = updateditem.EsCambioSector;
            this.aplicarCambioSector(puntoDto);

        }

        if (puntoDto.Color != updateditem.Color) {
            puntoDto.Color = updateditem.Color;
            this.pintarPolylines(puntoDto);
        }

        var marker = this.findMapMarker(puntoDto.Id);

        if (puntoDto.Lat != updateditem.Lat) {
            puntoDto.Long != updateditem.Long

            puntoDto.Lat = updateditem.Lat;
            puntoDto.Long = updateditem.Long;

            var myLatlng = new google.maps.LatLng(puntoDto.Lat, puntoDto.Long);
            marker.setPosition(myLatlng);

            var i = this.map.markers.findIndex(fruit => fruit.details.id === puntoDto.Id);
            this.rehacerLineas(puntoDto, i, this);
        }
        
        this.setDraggable(marker, puntoDto);
        this.refreshIcon(updateditem);
        this.lastUpdatedPunto  = puntoDto;
    }

    setMapPositionForLastUpdatedCoordinate() : void {
        if(this.lastUpdatedPunto){
            this.map.setCenter(this.lastUpdatedPunto.Lat, this.lastUpdatedPunto.Long);   
        }
    }

    clearLastUpdatedPunto() : void {
        this.lastUpdatedPunto = null;
    }

    setDraggable(marker: any, puntoDto: PuntoDto) {

        //if (puntoDto.EsCambioSector || puntoDto.EsPuntoInicio || puntoDto.EsPuntoTermino) {
        //    //console.log(marker);
        //    if (puntoDto.CodigoNombre && puntoDto.Abreviacion) {
        //        marker.setDraggable(false);
        //    }
        //}
        //else {
        marker.setDraggable(true);
        //}


    }





    private ApplySectores(sectores: SectorDto[]) {

        for (var i = 0; i < this.sectoresdto.length; i++) {
            var sectoractualizado = sectores[i];
            var sectoractual = this.sectoresdto[i];
            sectoractual.Descripcion = sectoractualizado.Descripcion;

            if (sectoractual.Color != sectoractualizado.Color) {

                sectoractual.Color = sectoractualizado.Color;

                this.pintarSector(sectoractual);


            }
        }
    }

    private pintarSector(sectoractual: SectorDto): void {

        var puntos = this.BuscarPuntosSectorDelSector(sectoractual);
        puntos.forEach(e => {
            e.LineColor = sectoractual.Color;
            this.pintarPolylines(e);



        });
    }

    private pintarPolylines(punto: PuntoDto): void {

        if (punto.Polylines) {
            punto.Polylines.forEach(po => {
                if (po) {
                    po.setOptions({ strokeColor: punto.Color || punto.LineColor });
                }
            })
        }

    }

    regenerarSectores(): void {
        this.calcularDistanciaTotal();

        this.sectoresdto = this.sectoresdto.sort(function(a, b) {
            var keyA = a.OrdenInicio,
                keyB = b.OrdenInicio;
            // Compare the 2 dates
            if (keyA < keyB) return -1;
            if (keyA > keyB) return 1;
            return 0;
        });

        this.refreshOrden()

        this.calcularDistanciasSectores();
    }


    verSectores(): void {

        this.regenerarSectores();


        var s = [];

        this.sectoresdto.forEach(item => {

            s.push(Object.assign({}, item))
        }
        );





        this.createOrEditPuntoDtoModal.showSectores(s);


    }

    private calcularDistanciaTotal(): void {

        var rbm = this;
        rbm.dataRecorrido._distancia = 0.0;
        rbm.dataRecorrido._tiempo = 0.0;
        if (rbm.puntosdto) {
            rbm.puntosdto.forEach(e => {

                if (e.Steps) {
                    e.Steps.forEach(step => {
                        if (step) {
                            rbm.dataRecorrido.SumarDistancia(step.distance.value);
                            rbm.dataRecorrido.SumarTiempo(step.duration.value);

                        }
                    })
                }


            });
        }

    }

    private BuscarPuntosSectorDelSector(sector: SectorDto): PuntoDto[] {
        return this.puntosdto.filter(s => s.Orden > sector.OrdenInicio && s.Orden <= sector.OrdenFin);
    }


    private findOrdenSectores(sectores: SectorDto[]) {
        var This = this;
        sectores.forEach(sector => {
            var puntoInicio = This.puntosdto.filter(p => p.Id == sector.PuntoInicioId)[0];
            var puntoFin = This.puntosdto.filter(p => p.Id == sector.PuntoFinId)[0];

            sector.OrdenInicio = puntoInicio.Orden;
            sector.OrdenFin = puntoFin.Orden;


            sector.Descripcion = this.getSectorDesciption(puntoInicio, puntoFin);
        });
    }


    getSectorDesciption(puntoInicio: PuntoDto, puntoFin: PuntoDto) {
        try {
            var ini = '' + (puntoInicio.PlaCoordenadaItem.Description || puntoInicio.Abreviacion) + '';
            var fin = '' + (puntoFin.PlaCoordenadaItem.Description || puntoFin.Abreviacion) + '';

            return ini + ' => ' + fin;
        } catch (e) {

        }

    }



    private aplicarCambioSector(puntoDto: PuntoDto) {
        try {

            var puntotAnterior = this.BuscarCambioSectorAnterior(puntoDto);
            var puntotPosterior = this.BuscarCambioSectorPosterior(puntoDto);


            if (puntoDto.EsCambioSector) {


                var sectoranterior = this.sectoresdto.find(e => e.PuntoInicioId == puntotAnterior.Id && e.PuntoFinId == puntotPosterior.Id);
                sectoranterior.PuntoFinId = puntoDto.Id;

                var snew = new SectorDto();
                snew.Id = 0;
                snew.Data = "Sector " + this.sectoresdto.length.toString();
                snew.Descripcion = puntoDto.Abreviacion + '-' + puntotPosterior.Abreviacion;

                snew.PuntoInicioId = puntoDto.Id;;
                snew.PuntoFinId = puntotPosterior.Id;
                snew.RutaId = this.paretId;
                snew.Color = this.DefaultColor;
                this.sectoresdto.push(snew);

                this.CalcularDiscanciaSector(snew);
                this.refreshOrden();
                this.pintarSector(snew);
            }
            else {
                var sectoranterior = this.sectoresdto.find(e => e.PuntoFinId == puntoDto.Id);

                var sectorposterior = this.sectoresdto.find(e => e.PuntoInicioId == puntoDto.Id);
                var sectorposteriorIndex = this.sectoresdto.findIndex(e => e.PuntoInicioId == puntoDto.Id);

                sectoranterior.PuntoFinId = sectorposterior.PuntoFinId;

                this.sectoresdto.splice(sectorposteriorIndex, 1);
                sectoranterior.Descripcion = puntotAnterior.Abreviacion + '-' + puntoDto.Abreviacion;

                this.CalcularDiscanciaSector(sectoranterior);

                this.refreshOrden();
                this.pintarSector(sectoranterior);
            }




        } catch (e) {

        }

    }

    private calcularDistanciasSectores(): void {
        this.sectoresdto.forEach(sector => {
            this.CalcularDiscanciaSector(sector);
        }
        );
    }

    private CalcularDiscanciaSector(sector: SectorDto): void {
        var puntos = this.BuscarPuntosSectorDelSector(sector);

        var distancia = 0.0;

        var duration = 0.0;

        puntos.forEach(e => {

            if (e.Steps) {
                e.Steps.forEach(step => {
                    if (step) {
                        distancia += step.distance.value;
                        duration += step.duration.value;
                    }
                })
            }


        });

        if (distancia) {
            sector.DistanciaKm = distancia / 1000;

        }

        if (duration) {
            sector.Data = JSON.stringify({ Duration: duration / 60 });
            sector.Duration = duration / 60;
        }


    }

    private BuscarCambioSectorAnterior(puntoDto: PuntoDto): PuntoDto {
        var p = this.puntosdto.findIndex(e => e.Id == puntoDto.Id);
        for (var i = p - 1; i != 0; i--) {
            if (this.puntosdto[i].Id != puntoDto.Id && this.puntosdto[i].EsCambioSector) {
                return this.puntosdto[i];
            }
        }
        return this.puntosdto[0];
    }

    private BuscarCambioSectorPosterior(puntoDto: PuntoDto): PuntoDto {
        var p = this.puntosdto.findIndex(e => e.Id == puntoDto.Id);
        for (var i = p + 1; i < this.puntosdto.length - 1; i++) {
            if (this.puntosdto[i].Id != puntoDto.Id && this.puntosdto[i].EsCambioSector) {
                return this.puntosdto[i];
            }
        }
        return this.puntosdto[this.puntosdto.length - 1];
    }



    //Busco el marker en el mapa
    private findMapMarker(id: any): any {
        const index = this.map.markers.findIndex(fruit => fruit.details.id === id);
        var marker = this.map.markers[index];
        return marker;
    }

    private getPuntoByRbMapMarker(marcador: RbMapMarker, p: PuntoDto): PuntoDto {

        //p.Lat = marcador.lat;
        //p.Long = marcador.lng;
        p.Id = marcador.id;
        // p.CodigoNombre = p.CodigoNombre;
        //p.Descripcion = marcador.details.info == null ? p.Descripcion : marcador.details.info;
        //p.RutaId = number;
        //p.EsPuntoInicio = boolean;
        //p.EsPuntoTermino = boolean;
        //p.EsParada = boolean;
        //p.EsCambioSector = boolean;

        //p.TipoParadaId = p.TipoParadaId || 0;
        //p.NumeroExterno = p.NumeroExterno || "Sin NumeroExterno." ;
        //p.Abreviacion = p.Abreviacion || "Sin Abreviacion.";

        return p
    }

    buscar_Gmaps(text): void {
        var rbmaps = this;
        GMaps.geocode({
            address: text,
            callback: function(results, status) {
                if (status == 'OK') {
                    var latlng = results[0].geometry.location;
                    rbmaps.goTo(latlng.lat(), latlng.lng());
                }
                else {
                    console.log(status);
                }
            }
        });
    }

    goTo(lat, lng): void {
        this.map.setCenter(lat, lng);
    }

    // Eventos


    deshacerCambios() {

        if (this.OriginalData.length == 1) {
            this.descartarCambios();

        }
        else {




            if (this.RedoData.length == 0) {
                this.RedoData.push(this.getlocaldata("deshacerCambios"));

                this.applyChange(this.OriginalData[this.OriginalData.length - 1], this);
            }
            else {
                var last = this.OriginalData.pop();
                this.RedoData.push(last);
                this.applyChange(this.OriginalData[this.OriginalData.length - 1], this);
            }




        }
    }

    rehacerCambios() {

        if (this.RedoData.length > 0) {
            var last = this.RedoData.pop();
            if (last.action != "deshacerCambios") {
                this.OriginalData.push(last);
            }
            this.applyChange(last, this);
        }

    }


    getlocaldata(_action?: string): any {

        try {

            var puntos = new Array<PuntoDto>();
            var i = 1;
            if (this.puntosdto) {
                this.puntosdto.forEach(item => {

                    let puntoDtoClone = Object.assign({}, item);
                    puntoDtoClone.Orden = i++;
                    puntoDtoClone.Data = JSON.stringify({ steps: puntoDtoClone.Steps, instructions: puntoDtoClone.Instructions });
                    puntoDtoClone.Steps = null;
                    puntoDtoClone.Polylines = null;
                    puntos.push(puntoDtoClone);
                });
            }

            var p = JSON.stringify(puntos);
            var s = JSON.stringify(this.getSectores());

            return { puntosdto: p, sectoresdto: s, action: _action }

        } catch (e) {

        }

    }

    saveLocalData(_action?: string) {
        try {

            this.OriginalData.push(this.getlocaldata(_action));
            this.RedoData = [];

        } catch (e) {
            console.log("Error en saveLocalData: " + e);
        }
    }
    private currentData: any;


    applyChange(data, rbm) {

        this.currentData = data;
        if (rbm.puntosdto)
            rbm.puntosdto.forEach(punto => {
                rbm.removePolylineArray(punto.Polylines);
            });

        this.puntosdto = JSON.parse(data.puntosdto);
        this.sectoresdto = JSON.parse(data.sectoresdto);
        rbm.test_removeMarkersAndPolylines(rbm);
        rbm.test_addMarkersAndPolylines(rbm);
    }


    descartarCambios() {
        // if (!this.linea) return;
        // this.lineas = this.serviceLinea.getAll();
        // this.mostrarLinea(this.linea.id, false);
        var rbm = this;

        rbm.message.confirm("", "¿Confirmas que deseas descartar los cambios?", (confirm) => {
            if (!confirm.value) return;

            if (rbm.puntosdto)
                rbm.puntosdto.forEach(punto => {
                    rbm.removePolylineArray(punto.Polylines);
                });

            this.OriginalData.splice(1, this.OriginalData.length - 1);
            this.RedoData = [];

            this.puntosdto = JSON.parse(this.OriginalData[0].puntosdto);
            this.sectoresdto = JSON.parse(this.OriginalData[0].sectoresdto);


            rbm.test_removeMarkersAndPolylines(rbm);
            rbm.test_addMarkersAndPolylines(rbm);
        });

    }


    tieneCambios(): boolean {

        if (this.OriginalData && this.OriginalData.length > 1) {
            return true;
        }
        return false;
    }

    // Polylines

    removePolylineArray(plArray) {
        if (plArray && plArray.length > 0) {
            plArray.forEach(element => {
                if (element) element.setMap(null);
            });

            plArray = [];
        }
    }

    // KML

    loadKml(event) {
        let files = event.target.files;
        if (files && files.length > 0) {
            this.kmlFile = files[0];
        }
    }

    uploadKml() {
        var rbm = this;

        if (this.kmlFile) {
            var rbm = this;
            var reader = new FileReader();
            reader.readAsText(this.kmlFile, "UTF-8");
            reader.onload = function(evt: any) {
                var $viaje = rbm.service.parseKML(evt.target.result);
                var group = new RbMapGrupo();
                group.nombre = $viaje.name;
                var linea1 = rbm.crearLineaKML(group.id, $viaje.ida);
                var linea2 = rbm.crearLineaKML(group.id, $viaje.vuelta);

                //console.log("linea 1", linea1);

                rbm.dibujarDesdeKML(linea1.points, 0, rbm);

                // for (let index = 0; index < linea1.points.length; index++) {
                //   const customMarker = linea1.points[index];
                //   rbm.test_AgregarMarcador_lat_lng(customMarker.lat, customMarker.lng, false);
                // }

                // rbm.lineas.push(linea1);
                // rbm.lineas.push(linea2);
                // rbm.grupos.push(group);

                // rbm.serviceLinea.save(linea1);
                // rbm.serviceLinea.save(linea2);
                // rbm.serviceGrupo.save(rbm.grupos);

                // bootbox.alert("Recorridos creados.");
            }
            reader.onerror = function(evt) {
                //console.log("Error reading")
            }
        }
    }

    dibujarDesdeKML(points: CustomMarker[], i: number, rbm: RbmapsComponent) {
        if (i == points.length) return;

        var customMarker = points[i];

        //console.log("punto " + i, customMarker)
        var center = i == 0;
        var next_i = i + 1;

        if (i == 0) {
            rbm.test_AgregarMarcador_lat_lng(customMarker.lat, customMarker.lng, center, customMarker.id);
            rbm.dibujarDesdeKML(points, next_i, rbm);
        }
        else {

            rbm.test_AgregarMarcador_lat_lng(customMarker.lat, customMarker.lng, center, customMarker.id, function() {
                setTimeout(() => {
                    rbm.dibujarDesdeKML(points, next_i, rbm);
                }, 800 * 1)
            });
        }

    }


    ImportarPuntos(): void {

        this.OnImportarPuntos.emit({});

    }

    GenerarPuntos(puntosdto: PuntoDto[]): void {

        this.test_removeMarkersAndPolylines(this);

        this.puntosdto = puntosdto;

        for (var i = 0; i < this.puntosdto.length; i = i + 1) {
            var item = this.puntosdto[i];

            //Fix para autocompletar
            item.CodigoNombre = this.paretId + '_' + item.Orden;
            item.Abreviacion = item.CodigoNombre;

            this.test_AgregarMarcadorDto(item);
        }

        this.recrearSectores();

        this.RedibujarRutaDto(this.puntosdto, 0, this);
    }




    private recrearSectores(): void {

        this.sectoresdto = [];

        var puntossector = this.puntosdto.filter(p => p.EsCambioSector || p.EsPuntoInicio || p.EsPuntoTermino);

        this.sectoresdto = new Array<SectorDto>();


        for (var i = 1; i < puntossector.length; i = i + 1) {


            var snew = new SectorDto();
            snew.Id = 0;
            snew.Data = "Sector " + i;

            snew.Descripcion = this.getSectorDesciption(puntossector[i - 1], puntossector[i]);


            snew.DistanciaKm = this.dataRecorrido._distancia;
            snew.PuntoInicioId = puntossector[i - 1].Id;
            snew.PuntoFinId = puntossector[i].Id;
            snew.RutaId = this.paretId;
            snew.Color = this.DefaultColor;
            this.sectoresdto.push(snew);

        }

        this.calcularDistanciasSectores();
        this.calcularDistanciaTotal();


    }



    GraficarRutaNueva() {
        this.test_removeMarkersAndPolylines(this);
        var p = JSON.stringify(this.puntosdto);
        this.puntosdto = [];
        this.sectoresdto = [];
        this.RedibujarRuta(JSON.parse(p), 0, this);
    }

    async RedibujarRutaDto(points: PuntoDto[], i: number, rbm: RbmapsComponent): Promise<void> {
        mApp.block($("#m_portlet_tab_Mapa"), {
            overlayColor: '#000000',
            opacity: 0.2,
            type: 'loader',
            state: 'primary',
            message: 'Generando Rutas...'
        });


        for (var i = 0; i < points.length; i++) {
            var customMarker = points[i];

            //console.log("punto " + i, customMarker)
            var center = true;
            var next_i = i + 1;
            this.LastPointIndex = next_i;

            if (i == 0) {
                //rbm.hacerRuta(customMarker[i-1], customMarker, true, this, null);
                this.generateInstructions(customMarker);
            }
            else {
                rbm.hacerRuta(points[i - 1], customMarker, true, this, null);

                await this.delay(mApp.CONST_DELAY_BUILD_ROUTE, next_i);
            }


        }
        //
        mApp.unblock($("#m_portlet_tab_Mapa"));

        return null;
    }






    async RedibujarRuta(points: PuntoDto[], i: number, rbm: RbmapsComponent): Promise<void> {
        for (var i = 0; i < points.length; i++) {
            var customMarker = points[i];

            //console.log("punto " + i, customMarker)
            var center = true;
            var next_i = i + 1;
            this.LastPointIndex = next_i;

            if (i == 0) {
                rbm.test_AgregarMarcador_lat_lng(customMarker.Lat, customMarker.Long, center, customMarker.Id);
            }
            else {

                rbm.test_AgregarMarcador_lat_lng(customMarker.Lat, customMarker.Long, center, customMarker.Id);
                await this.delay(mApp.CONST_DELAY_BUILD_ROUTE, next_i);
            }


        }
        return null;
    }


    delay(milliseconds: number, count: number): Promise<number> {
        return new Promise<number>(resolve => {
            setTimeout(() => {
                resolve(count);
            }, milliseconds);
        });
    }



    crearLineaKML(groupId, viajeDireccion) {

        var points = new Array() as CustomMarker[];
        viajeDireccion.points.forEach(element => {
            var punto = new CustomMarker();
            punto.lat = element.lat;
            punto.lng = element.lng;
            points.push(punto);
        });

        var linea = new RbMapLinea();
        linea.nombre = viajeDireccion.name;
        linea.color = viajeDireccion.color;
        linea.diametro = viajeDireccion.width;
        linea.points = points;
        linea.grupoId = groupId;

        return linea;
    }

    // TEST 

    test_AgregarMarcador(latlng, center): void {
        this.test_AgregarMarcador_lat_lng(latlng.lat(), latlng.lng(), center);
    }

    test_AgregarMarcador_lat_lng(lat: any, lng: any, center, id?: string, callback?: any): void {



        var marker = new RbMapMarker(lat, lng, id);
        marker.icon.url = this.service.markerIcon(9);

        var rbm = this;

        marker.SetDragend(function(e) {
            rbm.test_MoverMarcador(this, e, rbm);
        });




        marker.SetClick(function(e) {
            rbm.test_ClickMarcador(this, e, rbm);
        })

        this.map.addMarker(marker);

        if (center) {
            this.map.setCenter(lat, lng);
        }

        var totalPuntos = rbm.puntosdto.length;
        var nuevoPunto = new PuntoDto();

        nuevoPunto.setGmapMarker(marker);
        nuevoPunto.RutaId = this.paretId;

        nuevoPunto.EsPuntoTermino = totalPuntos != 0;
        nuevoPunto.EsPuntoInicio = totalPuntos == 0;
        nuevoPunto.PlaTipoViajeId = 1;


        this.agregarPunto(nuevoPunto, rbm);

        if (totalPuntos > 0) { // Hay al menos 2
            var puntoAnterior = rbm.puntosdto[totalPuntos - 1];
            puntoAnterior.EsPuntoTermino = false;
            this.refreshIcon(puntoAnterior);

            if (totalPuntos == 1) {
                puntoAnterior.EsPuntoInicio = true;
            }


            if (nuevoPunto.EsPuntoTermino) {
                //TODO : estirar ultimo sector hasta el punto final 
                var sectoractual = this.sectoresdto.find(e => e.PuntoFinId == puntoAnterior.Id);
                if (sectoractual) {
                    sectoractual.PuntoFinId = nuevoPunto.Id;
                    nuevoPunto.LineColor = sectoractual.Color;
                }
            }



            this.hacerRuta(puntoAnterior, nuevoPunto, false, rbm, callback);


        }
        else {
            this.refreshIcon(nuevoPunto);
        }



        if (this.puntosdto.length == 2) {

            this.crearsectorPordefecto();
        }


        this.refreshOrden();

    }


    refreshOrden(): void {
        var i = 1;
        this.puntosdto.forEach(item => {
            item.Orden = i++;
        });

        this.findOrdenSectores(this.sectoresdto);
    }

    refreshIcon(punto: PuntoDto) {
        this.findMapMarker(punto.Id).setIcon(this.service.markerIconFromDto(punto));
    }

    getMarkerFromDTO(puntodto: PuntoDto): RbMapMarker {
        var marker = new RbMapMarker(puntodto.Lat, puntodto.Long, puntodto.Id);
        var rbm = this;

        marker.SetDragend(function(e) {
            rbm.test_MoverMarcador(this, e, rbm);
        });

        marker.SetClick(function(e) {
            rbm.test_ClickMarcador(this, e, rbm);
        });

        marker.details.id = puntodto.Id.toString();
        marker.details.tipo = puntodto.TipoParadaId;

        marker.icon.url = this.service.markerIconFromDto(puntodto);

        return marker;

    }

    findPuntoDtoColor(punto: PuntoDto): void {
        var color = this.DefaultColor;
        var sector = this.sectoresdto.find(s => s.OrdenInicio < punto.Orden && punto.Orden <= s.OrdenFin);

        //if (sectores) {
        //    var sector = sectores[0];
        if (sector) color = sector.Color;
        //}

        punto.LineColor = color;
    }


    parsePuntoDto(punto: PuntoDto): void {
        if (!punto.Steps) {
            if (punto.Data) {
                var data = JSON.parse(punto.Data);
                punto.Steps = data.steps;
                punto.Instructions = data.instructions;
            }

        }
    }

    parsePuntosDto(puntosdto: PuntoDto[]): void {
        puntosdto.forEach(item => {
            this.parsePuntoDto(item);
        });
    }


    test_AgregarMarcadorDto(punto: PuntoDto, callback?: any): void {

        this.parsePuntoDto(punto);

        this.findPuntoDtoColor(punto);

        var marker = this.getMarkerFromDTO(punto)

        this.map.addMarker(marker);

        this.hacerRutaDto(punto, this, callback);
    }

    test_AgregarMarcador_Click_Linea(latlng, marcadorSiguiente: PuntoDto, center: boolean): void {
        var marker = new RbMapMarker(latlng.lat(), latlng.lng());
        var rbm = this;
        marker.SetDragend(function(e) {
            rbm.test_MoverMarcador(this, e, rbm);
        });
        marker.SetClick(function(e) {
            rbm.test_ClickMarcador(this, e, rbm);
        })

        this.map.addMarker(marker);

        if (center) {
            this.map.setCenter(latlng.lat(), latlng.lng());
        }

        var totalPuntos = rbm.puntosdto.length;
        var encontrado = false;
        for (let i = 0; i < rbm.puntosdto.length && !encontrado; i++) {
            const element = rbm.puntosdto[i];
            if (element.Id == marcadorSiguiente.Id) {
                encontrado = true;
                //var nuevoPunto = new PuntoInfo(marker);
                var nuevoPunto = new PuntoDto();
                nuevoPunto.setGmapMarker(marker);
                nuevoPunto.PlaTipoViajeId = this.TipoViajeDefault;

                // nuevoPunto.Id = marker.id;
                // nuevoPunto.Lat = marker.lat;
                // nuevoPunto.Long = marker.lng;
                rbm.puntosdto.splice(i, 0, nuevoPunto);



                var sectoractual = this.BuscarSectoractual(nuevoPunto);
                if (sectoractual) {
                    nuevoPunto.LineColor = sectoractual.Color;
                }
                this.lastUpdatedPunto = nuevoPunto;
                rbm.rehacerLineas(nuevoPunto, i, rbm);
            }
        }
        this.refreshOrden();
    }


    private BuscarSectoractual(punto: PuntoDto): SectorDto {

        var pa = this.BuscarCambioSectorAnterior(punto);
        var pp = this.BuscarCambioSectorPosterior(punto);
        var sectoractual = this.sectoresdto.find(e => e.PuntoInicioId == pa.Id && e.PuntoFinId == pp.Id);
        return sectoractual;
    }

    tiempoc: string = '08/08/2018 00:00';


    calcularruta() {

        var service = new google.maps.DirectionsService();

        var selft = this;

        //var deapr = new google.maps.DrivingOptions();

        //https://developers.google.com/maps/documentation/javascript/reference/3.exp/directions#TrafficModel
        //deapr.trafficModel = 
        // BEST_GUESS 
        //OPTIMISTIC 
        //   PESSIMISTIC 
        //https://stackoverflow.com/questions/45889485/using-google-maps-directions-api-to-generate-commute-info-at-specific-times-of-d

        service.route({
            origin: new google.maps.LatLng(-32.99963703997856, -60.77289614531014),
            destination: new google.maps.LatLng(-32.95469834193799, -60.65625668550808),
            travelMode: 'DRIVING',
            drivingOptions: {
                departureTime: moment(this.tiempoc, 'DD/MM/YYYY HH:mm').toDate(),
                trafficModel: google.maps.TrafficModel.BEST_GUESS
            }
        }, function(response, status) {

            if (status === 'OK') {

                //console.log(response.routes[0].legs[0].duration);
            } else {
                console.log("Error en calcular ruta: " + status);
                window.alert('Directions request failed due to ' + status);
            }
        });

    }
    errorTravelRoute(result, status) {
        console.log(result);
        console.log(status);
    }

    hacerRuta(puntoUltimo: PuntoDto, puntoNuevo: PuntoDto, reemplazar: boolean, rbm: RbmapsComponent, callback?: any) {
        //console.log("hacerRuta", puntoUltimo);
        //mApp.blockPage();

        if (reemplazar) { // quitamos polyline
            var encontrado = false;
            for (let i = 0; i < rbm.puntosdto.length && !encontrado; i++) {
                const element = rbm.puntosdto[i];
                if (element.Id == puntoNuevo.Id) {
                    encontrado = true;
                    if (element.Polylines) {
                        for (let ip = 0; ip < element.Polylines.length; ip++) {
                            element.Polylines[ip].setMap(null);
                        }
                    }

                    element.Polylines = [];
                    element.Steps = [];
                }
            }
        }

        var origen = [puntoUltimo.Lat, puntoUltimo.Long];
        var destino = [puntoNuevo.Lat, puntoNuevo.Long];
        var tipoviajenuevoid = puntoNuevo.PlaTipoViajeId;

        this.TiposViajes.forEach(tviaje => {
            if (tviaje.Id == tipoviajenuevoid) {
                this.travelModenuevo = tviaje.TravelMode;
            }
        });

        // var salida1 = new Date();
        // var salida2 = new Date(2018, 4, 25, 23, 9, 0);
        // rbm.getRuta(origen, destino, rbm, salida1, function() {
        //     rbm.getRuta(origen, destino, rbm, salida2);
        // })


        rbm.map.travelRoute({
            origin: origen,
            destination: destino,
            travelMode: this.travelModenuevo,
            error: this.errorTravelRoute,
            end: function(e) {


                //console.log("end", e);
                mApp.unblock();

                //puntoNuevo.Instructions = [];

                //var legs = e.legs;
                //if (legs == null) {
                //    legs = e[0].legs;
                //}

                //puntoNuevo.Instructions.push({ address: legs[0].start_address, type: 0  })

                //puntoNuevo.Steps.forEach(step => {
                //    if (step) {
                //        puntoNuevo.Instructions.push({ address: step.instructions, type: 2 });

                //    }
                //})

                //puntoNuevo.Instructions.push({ address: legs[0].end_address, type: 1 });

                rbm.generateInstructions(puntoNuevo);


            },
            step: function(e, total) {

                //console.log("step", e);
                rbm.agregarPolyline(puntoNuevo, e.path, rbm);

                puntoNuevo.Steps.push(e);
                rbm.calcularDistanciaTotal();

                if (total == e.step_number) {
                    //if (reemplazar) rbm.reemplazarPunto(puntoNuevo);
                    //else rbm.agregarPunto(puntoNuevo, rbm);


                    if (callback) {
                        callback();
                    }
                    else {
                        console.log(callback);
                    }


                }
            }
        });
    }


    generateInstructions(punto: PuntoDto, reitento?: any) {

        if (reitento) {
            //console.log("es reintento:" + reitento);
        }

        var selft = this;
        punto.Instructions = [];
        var geocoder = new google.maps.Geocoder;
        var latlng = { lat: punto.Lat, lng: punto.Long };




        GMaps.geocode({
            lat: punto.Lat,
            lng: punto.Long,
            callback: function(results, status) {

                if (status == 'OK') {
                    if (results != null && results.length > 0) {

                        var item = results.find(e => e.geometry.location_type == "RANGE_INTERPOLATED");

                        if (item) {
                            var route = item.address_components.find(e => e.types == "route");
                            if (route) {
                                punto.Instructions.push({ long_name: route.long_name, short_name: route.short_name, type: 1 });
                                return;
                            }
                        }

                        for (var i = 0; i < length; i++) {

                            if (results[i].address_components) {
                                var route = results[i].address_components.find(e => e.types == "route");
                                if (route) {
                                    punto.Instructions.push({ long_name: route.long_name, short_name: route.short_name, type: 1 });
                                    break;
                                }
                            }
                        }
                    }


                }
                else {

                    if (status == google.maps.GeocoderStatus.OVER_QUERY_LIMIT) {
                        reitento = (reitento | 0) + 1;
                        setTimeout(function() { selft.generateInstructions(punto, reitento) }, reitento * 800);

                    } else {
                        console.log("Error en GenerateInstructions: " + status);
                        toastr.error("Geocode was not successful for the following reason:", status);
                    }


                }
            }
        });


    }


    hacerRutaDto(punto: PuntoDto, rbm: RbmapsComponent, callback?: any) {

        //mApp.blockPage();

        // var data = JSON.parse(punto.Data);
        // var steps = data.steps;

        // steps.forEach(step => {
        //     rbm.agregarPolyline(punto, step.path, rbm);
        // });

        // punto.Steps = steps;
        // rbm.agregarPunto(punto, rbm);

        if (punto.Steps) {
            punto.Steps.forEach(step => {
                rbm.agregarPolyline(punto, step.path, rbm);
            });
        }

        //mApp.unblock();

        if (callback) callback();
    }

    agregarPunto(punto: PuntoDto, rbm: RbmapsComponent) {
        this.puntosdto.push(punto);

        //this.distancia_tiempo_agregar(punto);
        this.lastUpdatedPunto = punto;
        this.calcularDistanciaTotal();
        this.cdRef.detectChanges();
    }

    agregarPolyline(punto: PuntoDto, path: any, rbm: RbmapsComponent) {
        var p = rbm.map.drawPolyline({
            path: path,
            strokeColor: punto.Color || punto.LineColor, //'#131540',
            strokeOpacity: 0.6,
            strokeWeight: 6,
            click: function(e) {
                // Insertar nuevo_marcador antes de marcador
                // Mover nuevo_marcador

                if (rbm.maxMarker) {
                    if (rbm.puntosdto.length >= rbm.maxMarker) {
                        return;
                    }
                }
                rbm.saveLocalData("agregarPolyline ");

                rbm.test_AgregarMarcador_Click_Linea(e.latLng, punto, false);
            }
        });

        if (!punto.Polylines) punto.Polylines = [];
        console.log("Polyline Agregada");
        console.log(p);
        punto.Polylines.push(p);

    }

    eliminarPunto(punto: PuntoDto, data: any, i: number, rbm: RbmapsComponent) {
        //rbm.distancia_tiempo_restar(punto);

        var puntoDto = this.puntosdto.filter(p => p.Id === punto.Id)[0];

        if (!puntoDto.EsPuntoTermino && !puntoDto.EsPuntoInicio) {
            if (puntoDto.EsCambioSector) {
                puntoDto.EsCambioSector = false;
                this.aplicarCambioSector(puntoDto);
            }
        }




        // Quitamos las lineas pertenecientes al nodo a eliminar
        if (punto.Polylines)
            punto.Polylines.forEach(pl => {
                pl.setMap(null);
            });

        rbm.puntosdto.splice(i, 1);

        this.calcularDistanciaTotal();

        var index = rbm.map.markers.indexOf(data);

        if (index > -1) {
            rbm.map.markers.splice(index, 1);
            data.setMap(null);
        }

        var totalPuntos = rbm.puntosdto.length;



        if (puntoDto.EsPuntoTermino) {
            var puntoFinal = rbm.puntosdto[rbm.puntosdto.length - 1];
            const sectoractual = this.sectoresdto.findIndex(e => e.PuntoFinId == puntoDto.Id);
            this.sectoresdto.splice(sectoractual, 1);
        }

        if (puntoDto.EsPuntoInicio) {

            var puntoInicial = rbm.puntosdto[0];
            var sectoractual = this.sectoresdto.find(e => e.PuntoInicioId == puntoDto.Id);
            if (sectoractual && puntoInicial) {
                sectoractual.PuntoInicioId = puntoInicial.Id;
            }
            if (puntoInicial) {
                puntoInicial.EsPuntoInicio = true;
                puntoInicial.EsCambioSector = false;
                this.refreshIcon(puntoInicial);
            }

        }


        if (totalPuntos == 1) {
            this.sectoresdto = [];
        }


        // Siguiente punto
        var i_sig = i;
        if (i_sig < totalPuntos) {
            var punto_sig = rbm.puntosdto[i_sig];
            // Quitamos las lineas pertenecientes al nodo siguiente

            if (punto_sig.Polylines) {
                punto_sig.Polylines.forEach(pl => {
                    pl.setMap(null);
                });

            }

            if (i == 0) { // Ahora es el punto inicial
                //rbm.distancia_tiempo_restar(punto_sig);

                punto_sig.Polylines = [];
                punto_sig.Steps = [];
                this.calcularDistanciaTotal();
            }

            rbm.rehacerLineas(punto_sig, i, rbm);
        }
        else { // No hay siguiente
            var puntoFinal = rbm.puntosdto[rbm.puntosdto.length - 1];
            if (puntoFinal && totalPuntos != 1) {
                puntoFinal.EsPuntoTermino = true;
                puntoFinal.EsCambioSector = false;
                this.refreshIcon(puntoFinal);
            }
        }


        this.refreshOrden();

        this.cdRef.detectChanges();
    }

    reemplazarPunto(punto: PuntoDto) {
        var encontrado = false;
        for (let i = 0; i < this.puntosdto.length && !encontrado; i++) {
            const puntoEnArreglo = this.puntosdto[i];
            if (puntoEnArreglo.Id == punto.Id) {


                this.cdRef.detectChanges();

                puntoEnArreglo.Lat = punto.Lat;
                puntoEnArreglo.Long = punto.Long;
                puntoEnArreglo.Steps = punto.Steps;
                this.calcularDistanciaTotal();
                puntoEnArreglo.Polylines = punto.Polylines;
                encontrado = true;
            }
        }
    }



    test_MoverMarcador(marcador: RbMapMarker, data: any, rbm: RbmapsComponent) {
        // console.log("marcador", marcador);
        // console.log("data", data);

        var encontrado = false;
        for (let i = 0; i < rbm.puntosdto.length && !encontrado; i++) {
            const element = rbm.puntosdto[i];
            if (element.Id == marcador.id) {



                //Draggable
                //if (element.EsCambioSector || element.EsPuntoInicio || element.EsPuntoTermino) {

                //    if (element.Abreviacion && element.CodigoNombre) {
                //        var myLatlng = new google.maps.LatLng(element.Lat, element.Long);
                //        marcador.lat = element.Lat;
                //        marcador.lng = element.Long;

                //        var marker = this.findMapMarker(element.Id);

                //        var myLatlng = new google.maps.LatLng(marcador.lat, marcador.lng);
                //        marker.setPosition(myLatlng);

                //        return;
                //    }
                //}

                rbm.saveLocalData("mover item orden: " + element.Orden);

                element.Lat = data.latLng.lat();
                element.Long = data.latLng.lng();
                this.lastUpdatedPunto = element;


                rbm.rehacerLineas(element, i, rbm);

            }
        }

    }

    rehacerLineas(element: PuntoDto, i: number, rbm: RbmapsComponent) {

        var siguiente = rbm.puntosdto[i + 1];
        if (siguiente && (!siguiente.Steps || siguiente.Steps.length == 0)) {
            return;
        }



        if (i > 0) { // NO ES EL PRIMERO
            var punto_anterior = rbm.puntosdto[i - 1];
            // var lat = punto_anterior.Lat;
            // var lng = punto_anterior.Long;
            rbm.hacerRuta(punto_anterior, element, true, rbm, function() {
                var totalPuntos = rbm.puntosdto.length;
                if (i < totalPuntos - 1) { // NO ES EL ULTIMO
                    var punto_sig = rbm.puntosdto[i + 1];
                    // var lat = element.Lat;
                    // var lng = element.Long;
                    rbm.hacerRuta(element, punto_sig, true, rbm);
                }
            });
        }
        else {
            var totalPuntos = rbm.puntosdto.length;
            if (i < totalPuntos - 1) { // NO ES EL ULTIMO
                var punto_sig = rbm.puntosdto[i + 1];
                // var lat = element.Lat;
                // var lng = element.Long;
                rbm.hacerRuta(element, punto_sig, true, rbm);
            }
        }

    }

    test_ClickMarcador(marcador: RbMapMarker, e: any, rbm: RbmapsComponent) {

        rbm.onClickMarker(marcador);

        // rbm.message.confirm("Eliminar marcador", "Deseas eliminar este marcador?", (a) => {

        //     if (!a.value) return;

        //     rbm.test_EliminarMarcador(marcador, e, rbm);
        // });

    }

    test_EliminarMarcador(marcador: RbMapMarker, data: any, rbm: RbmapsComponent) {
        // console.log("marcador", marcador);
        // console.log("data", data);

        var encontrado = false;
        var totalPuntos = rbm.puntosdto.length;
        for (let i = 0; i < totalPuntos && !encontrado; i++) {
            const element = rbm.puntosdto[i];
            if (element.Id == marcador.id) {
                // Encontrado
                encontrado = true;

                // Eliminamos marcador
                rbm.eliminarPunto(element, data, i, rbm);
            }
        }
    }

    test_EliminarMarcadorByMarcador(marcador: RbMapMarker, rbm: RbmapsComponent) {
        // console.log("marcador", marcador);
        // console.log("data", data);

        var encontrado = false;
        var totalPuntos = rbm.puntosdto.length;
        for (let i = 0; i < totalPuntos && !encontrado; i++) {
            const element = rbm.puntosdto[i];
            if (element.Id == marcador.details.id) {
                // Encontrado
                encontrado = true;

                // Eliminamos marcador
                rbm.saveLocalData("eliminar marcador orden:" + element.Orden);
                rbm.eliminarPunto(element, marcador, i, rbm);
            }
        }
    }

    getRuta(origen: any, destino: any, rbm: RbmapsComponent, salida: Date, callback1?: any) {
        rbm.map.getRoutes({
            origin: origen,
            destination: destino,
            travelMode: 'driving',
            drivingOptions: {
                departureTime: salida,
                //trafficModel: google.maps.TrafficModel.PESSIMISTIC
            },
            callback: function(data, status) {
                //console.log("getRoutes " + salida, data[0].legs);
                //console.log("status ------------- ", status);
                //https://developers.google.com/maps/documentation/javascript/reference/3/#DirectionsLeg ---> duration_in_traffic
                //Only available to Premium Plan customers when drivingOptions is defined when making the request.
                if (status !== 'OK') {
                    console.log(status);
                }

                if (callback1) callback1();

            }
        });
    }

    test_removeMarkersAndPolylines(rbm: RbmapsComponent) {

        mApp.blockPage();

        if (rbm.map) {
            rbm.map.removeMarkers();
        }



        if (rbm.puntosdto)
            rbm.puntosdto.forEach(punto => {
                rbm.removePolylineArray(punto.Polylines);
            });


        if (this.polylinesAnteriores) {
            this.polylinesAnteriores.forEach(f => {
                this.removePolylineArray(f);
            });
        }


        mApp.unblock();
    }

    test_addMarkersAndPolylines(rbm: RbmapsComponent) {

        if (rbm.puntosdto)
            rbm.puntosdto.forEach(puntoDto => {
                rbm.test_AgregarMarcadorDto(puntoDto);
            });


        rbm.calcularDistanciaTotal();

    }

    redibujarMapa(): void {
        this.recrearSectores();
        this.RedibujarRutaDto(this.puntosdto, 0, this);
    }

}
