import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit, AfterViewInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
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
import { SectorDto, SectorFilter, SectorViewDto, RutaSectoresDto } from '../model/sector.model';
import { Observable } from 'rxjs/Observable';
import { retry } from 'rxjs/operators/retry';
import * as d3 from "d3";
declare let TimeKnots: any;
import { ScriptLoaderService } from '../../../../services/script-loader.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { debounce } from 'rxjs/operator/debounce';

@Component({
    selector: 'sectorViewComponent',
    templateUrl: './sector-view.component.html',

})
export class SectorViewComponent extends DetailEmbeddedComponent<SectorDto> implements IDetailComponent, OnInit, AfterViewInit {
    BanderaId: number;
    sub: Subscription;
    getDescription(item: SectorDto): string {
        return item.Descripcion;
    }
    public Sucursalid: boolean = false;
    isLoadingData: boolean = false;
    sectoresList: SectorViewDto[];
    Descripcion: string = '';

    constructor(
        injector: Injector,
        protected service: SectorService,
        protected _Puntosservice: PuntoService,
        private _script: ScriptLoaderService,
        private _activatedRoute: ActivatedRoute,
        protected router: Router,
    ) {

        super(service, injector);
        this.active = true;

        this.detail = new SectorDto();
        this.icon = "flaticon-route";
        this.title = "Ruta";
    }


    RutaId: any;

    ngOnInit(): void {

        super.ngOnInit();

        this.sub = this._activatedRoute.params.subscribe(params => {
            if (params.RutaId) {
                this.RutaId = params.RutaId;


            }
        }
        );
    }



    GoToBandera() {
        this.router.navigate(['/planificacion/bandera/1', { id: this.BanderaId, RutaId: this.RutaId }]);
    }



    ngAfterViewInit(): void {

        super.ngAfterViewInit()


        this._script.loadScripts('sectorViewComponent',
            ['assets/app/js/timeknots.js']).then(() => {

                if (this.RutaId) {

                    var sectorFilter = new SectorFilter();
                    sectorFilter.RutaId = this.RutaId;

                    this.service.getSectorView(sectorFilter).subscribe((e) => {



                        var lastidex = e.DataObject.Sectores.length - 1;

                        e.DataObject.Sectores.forEach((item, index) => {

                            if (index == 0) {
                                item.radius = 12;
                                item.lineWidth = 0;
                                item.background = "#000";
                                item.stroke = "#275d67";
                                item.linecolor = "#3580b8";
                            }
                            else if (lastidex == index) {
                                item.radius = 12;
                                item.lineWidth = 0;
                                item.background = "#000";
                                item.stroke = "#275d67";
                                item.linecolor = "#3580b8";

                            }
                            else if (item.EsCambioSector) {

                                item.radius = 16;
                                item.lineWidth = 3;
                                item.background = "#3580b8";
                                item.stroke = "#275d67";
                                item.linecolor = "#3580b8";
                            }
                            else {
                                item.p = "t";
                                item.radius = 2;
                                item.lineWidth = 0;
                                item.background = "#3580b8";
                                item.linecolor = "#3580b8";
                            }
                        });


                        TimeKnots.draw("#timeline3", e.DataObject.Sectores, { dateDimension: false, color: "#3580b8", labelFormat: "%Y", radius: 20 });
                        this.sectoresList = e.DataObject.Sectores.filter(p => p.EsCambioSector == false);
                        this.Descripcion = e.DataObject.Description;
                        this.BanderaId = e.DataObject.BanderaId;
                    })

                    //var series = ["Serie1", "Serie 2", "Serie 3"];
                    //var data = [
                    //    { "value": 220, "name": "PO", "desc": "", "radius": 12, "lineWidth": 0, "background": "#000", stroke: "#275d67", linecolor: "#3580b8"  },
                    //    { "value": 222, "name": "Parada", "desc": "10500101", "radius": 16, "lineWidth": 3, "background": "#3580b8", stroke: "#275d67", linecolor: "#3580b8"  },
                    //    { "value": 224, "name": "Parada", "desc": "10500102", "radius": 16, "lineWidth": 3, "background": "#3580b8", stroke: "#275d67", linecolor: "#3580b8"  },

                    //    { "value": 225, "name": "CORR-PELLE", "desc": "Largo: 3000", "radius": 2, "lineWidth": 0, linecolor: "#3580b8", "background": "#3580b8" , "p": "t" },


                    //    { "value": 226, "name": "Parada", "desc": "10500103", "radius": 16, "lineWidth": 3, "background": "#3580b8", stroke: "#275d67", linecolor: "#3580b8"  },
                    //    { "value": 228, "name": "Parada", "desc": "10500104", "radius": 16, "lineWidth": 3, "background": "#3580b8", stroke: "#275d67", linecolor: "#3580b8"  },

                    //    { "value": 229, "name": "PELLE-LOPI", "desc": "Largo: 4000", "radius": 2, "lineWidth": 0, "linecolor": "#3580b8", "background": "#3580b8" , "p": "t" },

                    //    { "value": 230, "name": "Parada", "desc": "10500105", "radius": 16, "lineWidth": 3, "background": "#3580b8", stroke: "#275d67", linecolor: "#3580b8"  },
                    //    { "value": 232, "name": "Parada", "desc": "10500106", "radius": 16, "lineWidth": 3, "background": "#3580b8", stroke: "#275d67", linecolor: "#3580b8"  },
                    //    { "value": 234, "name": "Parada", "desc": "10500107", "radius": 16, "lineWidth": 3, "background": "#3580b8", stroke: "#275d67", linecolor: "#3580b8"  },
                    //    { "value": 236, "name": "Parada", "desc": "10500108", "radius": 16, "lineWidth": 3, "background": "#3580b8", stroke: "#275d67", linecolor: "#3580b8"  },

                    //    { "value": 237, "name": "LOPI-TATI", "desc": "Largo: 4000", "radius": 2, "lineWidth": 0, "linecolor": "#3580b8", "background": "#3580b8" , "p": "t" },

                    //    { "value": 238, "name": "Parada", "desc": "10500109", "radius": 16, "lineWidth": 3, "background": "#3580b8", stroke: "#275d67", linecolor: "#3580b8"  },
                    //    { "value": 240, "name": "Parada", "desc": "10500110", "radius": 16, "lineWidth": 3, "background": "#3580b8", stroke: "#275d67", linecolor: "#3580b8"  },
                    //    { "value": 242, "name": "Parada", "desc": "10500111", "radius": 16, "lineWidth": 3, "background": "#3580b8", stroke: "#275d67", linecolor: "#3580b8"  },
                    //    { "value": 244, "name": "PD", "desc": "", "radius": 12, "lineWidth": 0, "background": "#000", linecolor: "#3580b8"  },
                    //];


                }

            }





            )

    }




}
