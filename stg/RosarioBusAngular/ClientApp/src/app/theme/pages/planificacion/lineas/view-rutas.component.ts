import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
declare let mApp: any;


import * as _ from 'lodash';

import { DetailEmbeddedComponent, IDetailComponent } from '../../../../shared/manager/detail.component';

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
import { RutaService } from '../ruta/ruta.service';
import { RutasViewFilter } from '../model/linea.model';
import { RutasMapComponent } from '../../../../components/rbmaps/rutas.map.component';




@Component({
    selector: 'viewRutasComponent',
    templateUrl: './view-rutas.component.html',
    styleUrls: ['./view-rutas.component.css']

})
export class ViewRutasComponent extends DetailEmbeddedComponent<RutaDto> implements IDetailComponent, OnInit {

    @ViewChild('mapaComponents') mapaComponents: RutasMapComponent;

    //puntosList: PuntoDto[];
    //sectoresList: SectorDto[];
    isLoadingMapa: boolean = false;
    loadOnInit: boolean = false;
    Rutas: RutaDto[];

    getDescription(item: RutaDto): string {
        return item.Nombre;
    }

    public Sucursalid: boolean = false;
    constructor(
        injector: Injector,
        protected service: RutaService,
        protected _Puntosservice: PuntoService,
        protected _sectoresService: SectorService,
    ) {
        super(service, injector);
        this.detail = new RutaDto();

        this.icon = "flaticon-route";
        this.title = "Ruta";


    }


    ngOnInit(): void {
        super.ngOnInit();
    }

    close(): void {
        if ($('#m_portlet_detail').hasClass('m-portlet--fullscreen')) {
            $('#fullscreentools')[0].click();
        }
        super.close();
    }


    drawMap(): void {
        setTimeout(() => {
            //We have access to the context values
            this.mapaComponents.crearViewMapa();

            this.isLoadingMapa = false;

            this.Rutas.forEach(r => {
                r.Selected = true;
                this.mapaComponents.setRuta(r.Puntos, r.Sectores, this.detail.Id);
            }
            );

        }, 10);
    }

    drawGeoJson(Sucursalid: number): void {
        setTimeout(() => {
            //We have access to the context values

            var primerpunto: PuntoDto;

            if (this.Rutas && this.Rutas.length > 0) {
                var primerRuta = this.Rutas[0];
                if (primerRuta.Puntos && primerRuta.Puntos.length > 0) {
                    primerpunto = primerRuta.Puntos.sort(e => e.Orden)[0];
                    //this.mapaComponents.setCenter(primerpunto.Lat, primerpunto.Long)
                }
            }

            this.mapaComponents.crearViewMapa(Sucursalid, primerpunto);

            this.isLoadingMapa = false;

            this.Rutas.forEach(r => {
                //r.Selected = true;
                //this.mapaComponents.setRuta(r.Puntos, r.Sectores, this.detail.Id);
                this.verGeoJson(r);
            });


        }, 10);
    }


    fillMapa(fvm: RutasViewFilter) {
        this.active = true;

        var selft = this;
        this.feacture = {};
        var closeChild = function() {
            selft.CloseChild();
        }

        this.breadcrumbsService.AddItem("Ver " + this.title, this.icon, "", this.getSelector(), closeChild);

        this.service.GetRutas(fvm).subscribe(result => {
            this.Rutas = result.DataObject;
            this.drawGeoJson(fvm.Sucursalid);
        });
    }


    removeMarkersAndPolylinesByPuntoDto(ruta: RutaDto) {
        this.mapaComponents.removeMarkersAndPolylinesByPuntoDto(this.mapaComponents, ruta.Puntos);

    }

    addMarkersAndPolylinesByPuntoDto(ruta: RutaDto) {
        this.mapaComponents.setRuta(ruta.Puntos, ruta.Sectores, this.detail.Id);

    }

    setMarkersAndPolylinesBy(ruta: RutaDto) {
        if (ruta.Selected) {
            this.mapaComponents.setRuta(ruta.Puntos, ruta.Sectores, this.detail.Id);
        }
        else {
            this.mapaComponents.removeMarkersAndPolylinesByPuntoDto(this.mapaComponents, ruta.Puntos);
        }

    }

    toogleRuta(ruta: RutaDto) {

        ruta.Selected = !ruta.Selected;

        this.setMarkersAndPolylinesBy(ruta);

    }

    feacture: any = {};

    verGeoJson(ruta: RutaDto) {
        ruta.Selected = !ruta.Selected;

        if (ruta.Selected) {

            if (this.feacture[ruta.Id]) {
                this.feacture[ruta.Id] = this.mapaComponents.GenerarPuntosGeoJson(this.feacture[ruta.Id + "_data"]);
            }
            else {
                this._Puntosservice.RecuperarGeoJson(ruta.Id).subscribe(p => {
                    setTimeout(() => {
                        this.feacture[ruta.Id] = this.mapaComponents.GenerarPuntosGeoJson(p.DataObject);
                        this.feacture[ruta.Id + "_data"] = p.DataObject;

                    }, 10);

                });

            }

        }
        else {
            this.mapaComponents.RemoveFeacture(ruta.Id.toString());
            //this.mapaComponents.removeMarkersAndPolylinesByPuntoDto(this.mapaComponents, ruta.Puntos);
        }
    }

}
