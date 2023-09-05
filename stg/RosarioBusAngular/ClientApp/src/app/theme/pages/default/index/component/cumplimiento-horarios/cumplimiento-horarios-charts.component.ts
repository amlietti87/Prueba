import { Component, OnInit, ViewEncapsulation, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { IDashboardBaseComponent } from '../dashboardComponent';
import { ScriptLoaderService } from '../../../../../../services/script-loader.service';
import { Helpers } from '../../../../../../helpers';
import { ItemDto } from '../../../../../../shared/model/base.model';
import { LineaService } from '../../../../planificacion/linea/linea.service';

declare let AmCharts: any;
declare let mApp: any;

@Component({
    selector: "cumplimiento-horarios-charts",
    templateUrl: "./cumplimiento-horarios-charts.component.html",
    styleUrls: ["./cumplimiento-horarios-charts.component.css"],
    encapsulation: ViewEncapsulation.None,
})
export class CumplimientoHorariosChartsComponent implements OnInit, IDashboardBaseComponent, AfterViewInit {
    data: any;

    @ViewChild('Todas') TodasTab: ElementRef;
    @ViewChild('Linea') LineaTab: ElementRef;
    lineas: ItemDto[];
    currentLlinea: ItemDto;
    selectedLineas

    constructor(private _script: ScriptLoaderService, protected lineaService: LineaService) {

        this.lineaService.GetItemsAsync({}).subscribe(e => {
            this.lineas = e.DataObject;
        });

    }
    ngOnInit() {



    }
    ngAfterViewInit() {

        mApp.init();

        this._script.loadScripts('cumplimiento-horarios-charts',
            ['https://www.amcharts.com/lib/3/plugins/tools/polarScatter/polarScatter.min.js',
                'https://www.amcharts.com/lib/3/plugins/export/export.min.js']).then(e => {

                    this.IniciarChart();
                }

            )

        Helpers.loadStyles('cumplimiento-horarios-charts', [
            'https://www.amcharts.com/lib/3/plugins/export/export.css']);


    }


    IniciarChart(): void {

        var chart = AmCharts.makeChart("m_amcharts_3", {
            "hideCredits": true,
            "theme": "light",
            "type": "serial",
            "dataProvider": [{
                "linea": "315",
                "salida": 3.5,
                "recorido": 4.2
            }, {
                "linea": "440",
                "salida": 1.7,
                "recorido": 3.1
            }, {
                "linea": "740",
                "salida": 2.8,
                "recorido": 2.9
            }, {
                "linea": "41",
                "salida": 2.6,
                "recorido": 2.3
            }],
            "valueAxes": [{
                "unit": "%",
                "position": "left",
                "title": "Cumplimiento porcentual",
            }],
            "startDuration": 1,
            "graphs": [{
                "balloonText": "Cumplimiento [[category]] Salidas: <b>[[value]]</b>",
                "fillAlphas": 0.9,
                "lineAlpha": 0.2,
                "title": "Salidas",
                "type": "column",
                "valueField": "salida"
            }, {
                "balloonText": "Cumplimiento [[category]] Recoridos: <b>[[value]]</b>",
                "fillAlphas": 0.9,
                "lineAlpha": 0.2,
                "title": "Recorridos",
                "type": "column",
                "clustered": false,
                "columnWidth": 0.5,
                "valueField": "recorido"
            }],
            "plotAreaFillAlphas": 0.1,
            "categoryField": "linea",
            "categoryAxis": {
                "gridPosition": "start"
            },
            "export": {
                "enabled": true
            }

        });


    }




}