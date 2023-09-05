import { Component, ViewChild, Injector, ViewContainerRef, Inject, OnInit, Input } from '@angular/core';

import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { NgForm } from '@angular/forms';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { SiniestroMapsComponent } from '../../../../../components/rbmaps/siniestro.maps.component';
import { SiniestrosDto } from '../../model/siniestro.model';
import { ItemDto } from '../../../../../shared/model/base.model';



@Component({
    selector: 'mapa-modal',
    templateUrl: './mapa-modal.component.html'
})
export class MapaModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('detailForm') detailForm: NgForm;
    @ViewChild('mapaComponents') mapaComponents: SiniestroMapsComponent;


    constructor(
        injector: Injector,
        @Inject(MAT_DIALOG_DATA) private data: {
            Latitud: number,
            Longitud: number,
            Localidad: string,
            Direccion: string,
            SucursalId: number
        },
        public dialogRef: MatDialogRef<MapaModalComponent>
    ) {
        super(injector);
        this.detail = new SearchMapDto();
    }

    ngOnInit() {
        this.dialogRef.updateSize('70%');
        this.inicializarMapa();
    }

    inicializarMapa() {
        var long: number;
        var lat: number;
        if (this.data.SucursalId && this.data.SucursalId != null) {
            switch (this.data.SucursalId.toString()) {
                case "1":
                    long = -60.655931;
                    lat = -32.954517;
                    var localidad = new ItemDto();
                    localidad.Id = 12741;
                    localidad.Description = "Rosario - 2000";
                    localidad.IsSelected = true;
                    this.detail.selectLocalidades = localidad;
                    break;
                case "2":
                    long = -58.71917;
                    lat = -34.54325;
                    var localidad = new ItemDto();
                    localidad.Id = 21435;
                    localidad.Description = "Buenos Aires - LPGB";
                    localidad.IsSelected = true;
                    this.detail.selectLocalidades = localidad;
                    break;
                case "3":
                    long = -60.69809;
                    lat = -31.61863;
                    var localidad = new ItemDto();
                    localidad.Id = 13250;
                    localidad.Description = "Santa Fe - 3000";
                    localidad.IsSelected = true;
                    this.detail.selectLocalidades = localidad;
                    break;
                default:
                    long = -60.655931;
                    lat = -32.954517;
                    var localidad = new ItemDto();
                    localidad.Id = 12741;
                    localidad.Description = "Rosario - 2000";
                    localidad.IsSelected = true;
                    this.detail.selectLocalidades = localidad;
                    break;
            }
        }
        else {
            long = -60.655931;
            lat = -32.954517;
            var localidad = new ItemDto();
            localidad.Id = 12741;
            localidad.Description = "Rosario - 2000";
            localidad.IsSelected = true;
            this.detail.selectLocalidades = localidad;
        }
        if (this.data.Longitud && this.data.Latitud) {
            this.mapaComponents.crearMapa(this.data.Latitud, this.data.Longitud);
            this.mapaComponents.setCenter(this.data.Latitud, this.data.Longitud);
            this.mapaComponents.removeLayerPuntos();
            var marker = this.mapaComponents.AgregarMarcador_lat_lng(this.data.Latitud, this.data.Longitud, true);
            this.mapaComponents.AfterAddMaker.emit(marker);
        }
        else {
            this.mapaComponents.crearMapa(lat, long);
            this.mapaComponents.setCenter(lat, long);
        }
        this.mapaComponents.AfterAddMaker.subscribe(e => {
            this.data.Latitud = e.lat;
            this.data.Longitud = e.lng;
            this.mapaComponents.generateInstructions(this.data);
        }
        );
    }

    detail: SearchMapDto;





    private searchGmapCount: number = 0;
    searchGmap(): void {
        this.searchGmapCount = 1;
        this.searchGmapInternal(this.detail.Calle, this.detail.Cruce);
    }

    private searchGmapInternal(Calle: string, Cruce: string): void {


        var localidad = this.detail.selectLocalidades ? this.detail.selectLocalidades.Description : "";
        localidad = (localidad.split(' - ')[0]).trim();
        var buscarpo = `${Calle} y ${Cruce} ,  ${localidad}`;
        let self = this;
        var callback = function(results, status) {
            if (status == 'OK') {
                if (self.searchGmapCount == 1 && results.length > 1) {
                    self.searchGmapCount = 2;
                    self.searchGmapInternal(Cruce, Calle);
                }
                else {
                    self.mapaComponents.successfullyGeocodePorCruce(results);

                }
            }
        };

        this.mapaComponents.buscar_Gmaps(buscarpo, callback);

    }


    save(): void {
        if (this.detailForm && this.detailForm.form.invalid) {
            return;
        }
        this.dialogRef.close(this.data);
    }



    close(): void {
        if (this.detailForm) {
            this.detailForm.reset();
        }
        this.dialogRef.close(false);
    }
}

export class SearchMapDto {
    Calle: string;
    Cruce: string;
    selectLocalidades: ItemDto;
}