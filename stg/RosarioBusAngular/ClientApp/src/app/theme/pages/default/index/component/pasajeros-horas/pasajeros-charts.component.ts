import { Component, OnInit, ViewEncapsulation, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { IDashboardBaseComponent } from '../dashboardComponent';
import { ScriptLoaderService } from '../../../../../../services/script-loader.service';
import { Helpers } from '../../../../../../helpers';
import { ItemDto } from '../../../../../../shared/model/base.model';
import { LineaService } from '../../../../planificacion/linea/linea.service';

declare let AmCharts: any;
declare let mApp: any;

@Component({
    selector: "pasajeros-charts",
    templateUrl: "./pasajeros-charts.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class PasajerosChartsComponent implements OnInit, IDashboardBaseComponent, AfterViewInit {
    data: any;

    @ViewChild('Todas') TodasTab: ElementRef;
    @ViewChild('Linea') LineaTab: ElementRef;
    lineas: ItemDto[];
    currentLlinea: ItemDto;


    constructor(private _script: ScriptLoaderService, private lineaService: LineaService) {

    }
    ngOnInit() {
        this.lineaService.GetItemsAsync({}).subscribe(e => {
            this.lineas = e.DataObject;
        });
    }
    ngAfterViewInit() {

        mApp.init();

        this._script.loadScripts('app-amcharts-stock-charts',
            ['//www.amcharts.com/lib/3/plugins/export/export.min.js']).then(e => {

                this.IniciarChart();
            })
        Helpers.loadStyles('app-amcharts-stock-charts', [
            '//www.amcharts.com/lib/3/plugins/export/export.css']);

    }


    IniciarChart(): void {

        var chartData = generateChartData();

        function generateChartData() {
            var chartData = [];
            var firstDate = new Date(2017, 1, 1);
            firstDate.setDate(firstDate.getDate() - 1000);
            firstDate.setHours(0, 0, 0, 0);

            for (var i = 0; i < 365; i++) {
                var newDate = new Date(firstDate);
                newDate.setDate(firstDate.getDate() + i);

                var a = Math.round(Math.random() * (40 + i)) + 100 + i;
                var b = Math.round(Math.random() * 100000000);

                chartData.push({
                    "date": newDate,
                    "value": a,
                    "volume": b
                });
            }
            return chartData;
        }

        var chart = AmCharts.makeChart("m_amcharts_4", {
            "hideCredits": true,
            "type": "stock",
            "theme": "light",
            "language": "es",
            "categoryAxesSettings": {
                "minPeriod": "mm"
            },

            "dataSets": [{
                "color": "#b0de09",
                "fieldMappings": [{
                    "fromField": "value",
                    "toField": "value"
                }, {
                    "fromField": "volume",
                    "toField": "volume"
                }],

                "dataProvider": chartData,
                "categoryField": "date"
            }],

            "panels": [{
                "showCategoryAxis": false,
                "title": "Recaudacion",
                "percentHeight": 70,

                "stockGraphs": [{
                    "id": "g1",
                    "valueField": "value",
                    "type": "smoothedLine",
                    "lineThickness": 2,
                    "bullet": "round"
                }],


                "stockLegend": {
                    "valueTextRegular": " ",
                    "markerType": "none"
                }
            }, {
                "title": "Horas Pagas",
                "percentHeight": 30,
                "stockGraphs": [{
                    "valueField": "volume",
                    "type": "column",
                    "cornerRadiusTop": 2,
                    "fillAlphas": 1
                }],

                "stockLegend": {
                    "valueTextRegular": " ",
                    "markerType": "none"
                }
            }],

            "chartScrollbarSettings": {
                "graph": "g1",
                "usePeriod": "10mm",
                "position": "top",
                "enabled": false
            },

            "chartCursorSettings": {
                "valueBalloonsEnabled": true,
            },

            "periodSelector": {
                "position": "top",
                "inputFieldsEnabled": false,
                "inputFieldWidth": 150,
                "periods": [{
                    "period": "MM",
                    "count": 1,
                    "label": "Mes",
                    "selected": true
                }, {
                    "period": "MAX",
                    "label": "Año"
                }]
            },

            "panelsSettings": {
                "usePrefixes": true
            },

            "export": {
                "enabled": true,
                "position": "bottom-right"
            }
        });

        jQuery("a[title='JavaScript charts']").remove()

    }




}