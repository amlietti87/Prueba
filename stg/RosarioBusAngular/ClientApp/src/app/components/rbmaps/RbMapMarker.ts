import { RbMapServices, MapIcons } from "../rbmaps/RbMapServices";
import { Injector, ReflectiveInjector } from "@angular/core";
import { LocatorService } from "../../shared/common/services/locator.service";

declare var google: any;

export class RbMapMarker {
    id: string;
    name: string;
    lat: number;
    lng: number;
    draggable: boolean;
    details: RbMapMarkerDetail;
    icon: RbMapMarkerIcon;
    click: any;
    dragend: any;
    infoWindow: any;
    position: any;

    service: RbMapServices;



    constructor(lat: number, lng: number, id?: string) {

        this.service = LocatorService.injector.get(RbMapServices);
        var tipo = 1;
        var tipoIcon = MapIcons.BluePin;

        this.draggable = true;
        this.id = id || RbMapServices.guid();
        this.lat = lat;
        this.lng = lng;

        var icon_url = this.service.markerIcon(tipoIcon);
        var icon_size = new google.maps.Size(24, 24);
        this.icon = new RbMapMarkerIcon(icon_url, icon_size);

        this.details = new RbMapMarkerDetail(this.id, tipo);

    }

    public SetDragend(callback: any) {
        this.dragend = callback;
    }

    public SetClick(callback: any) {
        this.click = callback;
    }

    public SetSaved(saved: CustomMarker): void {
        this.lat = saved.lat;
        this.lng = saved.lng;
        this.details.id = saved.id;
        this.details.tipo = saved.tipo;
        this.details.info = saved.info;
        this.icon.url = this.service.markerIcon(saved.tipo);
    }

    public SetDraggable(draggable: boolean): void {
        this.draggable = draggable;
    }

}

export class RbMapMarkerIcon {
    size: number;
    url: string;

    constructor(url: string, size: number) {
        this.url = url;
        this.size = size;
    }
}

export class RbMapMarkerDetail {
    id: string;
    tipo: number;
    info: string;


    constructor(id: string, tipo: number) {
        this.id = id;
        this.tipo = tipo;

    }

    public SetInfo(info: string) {
        this.info = info;
    }


}

export class CustomMarker {
    id: string;
    tipo: number;
    info: string;
    lat: number;
    lng: number;

    constructor() {
        this.id = RbMapServices.guid();
        this.info = "";
        this.tipo = 1;
    }
}

export class PuntoInfo {
    marcador: RbMapMarker;
    steps: any[];
    polylines: any[];

    constructor(marcador: RbMapMarker) {
        this.marcador = marcador;
        this.steps = [];
        this.polylines = [];
    }
}

export class DataRecorrido {
    _distancia: number;
    _tiempo: number;

    constructor() {
        this._distancia = 0;
        this._tiempo = 0;
    }

    public SumarTiempo(tiempo: number) {
        this._tiempo += tiempo;
    }

    public RestarTiempo(tiempo: number) {
        this._tiempo -= tiempo;
    }

    public SumarDistancia(distancia: number) {
        this._distancia += distancia;
    }

    public RestarDistancia(distancia: number) {
        this._distancia -= distancia;
    }
}