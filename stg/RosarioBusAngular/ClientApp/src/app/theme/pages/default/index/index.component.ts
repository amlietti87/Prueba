declare let mOffcanvas: any;
declare let mApp: any;

import { Component, OnInit, ViewEncapsulation, AfterViewInit, ViewChild, group } from '@angular/core';
import { Helpers } from '../../../../helpers';
import { ScriptLoaderService } from '../../../../services/script-loader.service';

import { SortablejsOptions } from 'angular-sortablejs/dist';

import { CalendarComponent } from "ap-angular2-fullcalendar";
import { Dashboard1Component } from './component/dashboardComponent';
import { DashboardItem } from './model/dashboardItem';
import { DashboardCalendarComponent } from './component/dashboardCalendarComponent';
import { IndicadoresDashboardComponent } from './component/Indicadores/indicadores.component';
import { RecentNotificationsDashboardComponent } from './component/recent-notifications/recent-notifications';
import { DshQuickSidebarComponent } from './component/sidebar/dsh-quick-sidebar.component';
import { ItemDto, StatusResponse } from '../../../../shared/model/base.model';
import { DashboardDto, UsuarioDashboardItemDto, UsuarioDashboardInput } from './model/dashboard.model';
import { DashboarService } from './services/dashboard.service';
import { retry } from 'rxjs/operator/retry';
import { PasajerosChartsComponent } from './component/pasajeros-horas/pasajeros-charts.component';
import { CumplimientoHorariosChartsComponent } from './component/cumplimiento-horarios/cumplimiento-horarios-charts.component';



@Component({
    selector: "app-index",
    templateUrl: "./index.component.html",
    styleUrls: ["./StyleIndex.css"],
    encapsulation: ViewEncapsulation.None,

})
export class IndexComponent implements OnInit, AfterViewInit {



    items1: DashboardItem[] = [];
    items2: DashboardItem[] = [];
    items3: DashboardItem[] = [];
    tipo: number = 5;
    isEdit: boolean = false;

    dashboardComponentConfiguration = {

        1: DashboardCalendarComponent,
        2: IndicadoresDashboardComponent,
        3: RecentNotificationsDashboardComponent,
        4: CumplimientoHorariosChartsComponent,
        5: PasajerosChartsComponent,

    }
    topbarAsideObj: any;

    normalOptions: SortablejsOptions = {
    };

    @ViewChild('dshquicksidebar') dshquicksidebar: DshQuickSidebarComponent;


    constructor(private _dashboarService: DashboarService) {

    }

    private getSortablejsOptions(_disabled: boolean) {
        let x: SortablejsOptions = {
            group: 'normal-group',
            disabled: _disabled
        };

        return x;
    }

    changeTipo(nro: number) {

        this.tipo = nro;
        if (this.tipo == 1) {
            for (var i = 0; i < this.items2.length; i++) {
                this.items1.push(this.items2[i]);
            }

            for (var i = 0; i < this.items3.length; i++) {
                this.items1.push(this.items3[i]);
            }

            this.items2 = [];
            this.items3 = [];

        }

        if (this.tipo == 2 || this.tipo == 3 || this.tipo == 4) {

            for (var i = 0; i < this.items3.length; i++) {
                this.items1.push(this.items3[i]);
            }

            this.items3 = [];
        }

    }

    ngOnInit() {
        let self = this;

    }
    ngAfterViewInit() {

        this.initSidebar();
        this.normalOptions = this.getSortablejsOptions(true);

        this.dshquicksidebar.SaveDashboard.subscribe(e => this.OnSaveDashboard(e));
        this.dshquicksidebar.CancelDashboard.subscribe(e => this.OnCancelDashboard(e));
        this.dshquicksidebar.AddDashboard.subscribe(e => this.OnAddDashboard(e));
        this.dshquicksidebar.RemoveDashboard.subscribe(e => this.OnRemoveDashboard(e));

        this._dashboarService.RecuperarDshUsuarioDashboardItems().subscribe(e => {

            if (e.Status == StatusResponse.Ok) {
                this.RenderItems(e.DataObject.Items);
                this.tipo = e.DataObject.DashboardLayoutId;
            }
        });

    }

    OnRemoveDashboard(item: DashboardDto) {


        var index = this.items1.findIndex(e => e.data.DashboardId == item.Id);
        if (index >= 0)
            this.items1.splice(index, 1);

        index = this.items2.findIndex(e => e.data.DashboardId == item.Id);
        if (index >= 0)
            this.items2.splice(index, 1);

        index = this.items3.findIndex(e => e.data.DashboardId == item.Id);
        if (index >= 0)
            this.items3.splice(index, 1);

    }


    OnAddDashboard(item: DashboardDto) {
        var all = this.items1.concat(this.items2).concat(this.items3);

        if (!all.find(e => e.data.DashboardId == item.Id)) {
            let data: UsuarioDashboardItemDto = new UsuarioDashboardItemDto();
            data.Columna = 1;
            data.DashboardId = item.Id;
            data.Orden = 0;
            var comp = this.dashboardComponentConfiguration[item.TipoDashboardId];
            if (comp) {
                this.items1.unshift(new DashboardItem(this.dashboardComponentConfiguration[item.TipoDashboardId], data))
            }
        }
    }

    OnSaveDashboard(obj: any): void {
        this.guardarClick();
    }

    OnCancelDashboard(obj: any): void {
        this.topbarAsideObj.hide();
        this._dashboarService.RecuperarDshUsuarioDashboardItems().subscribe(e => {

            if (e.Status == StatusResponse.Ok) {
                this.RenderItems(e.DataObject.Items);
                this.tipo = e.DataObject.DashboardLayoutId;
            }
        });
        this.isEdit = false;
        this.normalOptions = this.getSortablejsOptions(!this.isEdit);

    }

    initSidebar(): void {
        var topbarAside = $('#DSHDetailsidebar');
        var topbarAsideContent = topbarAside.find('.DSHsidebar_content');

        this.topbarAsideObj = new mOffcanvas('DSHDetailsidebar', {
            overlay: false,
            baseClass: 'm-quick-sidebar',

            //  closeBy: 'm_quick_sidebar_close',
            // toggleBy: 'm_quick_sidebar_toggle'
        });

        // run once on first time dropdown shown
        this.topbarAsideObj.one('afterShow', function() {
            mApp.block(topbarAside);

            setTimeout(function() {
                mApp.unblock(topbarAside);

                topbarAsideContent.removeClass('m--hide');


            }, 1000);
        });
    }


    editClick() {
        this.topbarAsideObj.show();
        this.isEdit = true;
        this.normalOptions = this.getSortablejsOptions(!this.isEdit);

    }

    guardarClick() {
        var data = new UsuarioDashboardInput();


        this.items1.forEach((e, index) => { e.data.Orden = index; e.data.Columna = 1; data.Items.push(e.data); });
        this.items2.forEach((e, index) => { e.data.Orden = index; e.data.Columna = 2; data.Items.push(e.data); });
        this.items3.forEach((e, index) => { e.data.Orden = index; e.data.Columna = 3; data.Items.push(e.data); });
        data.DashboardLayoutId = this.tipo;

        this._dashboarService.guardarDashboard(data).subscribe(e => {

            if (e.Status == StatusResponse.Ok) {
                this.RenderItems(e.DataObject);

                this.topbarAsideObj.hide();
                this.isEdit = false;
                this.normalOptions = this.getSortablejsOptions(!this.isEdit);


            }
        });



    }

    RenderItems(items: UsuarioDashboardItemDto[]): any {
        this.items1 = [];
        this.items2 = [];
        this.items3 = [];
        items.sort((a, b) => a.Orden - b.Orden).forEach(e => {
            if (e.Columna == 1) {
                this.items1.push(new DashboardItem(this.dashboardComponentConfiguration[e.TipoDashboardId], e));
            }
            if (e.Columna == 2) {
                this.items2.push(new DashboardItem(this.dashboardComponentConfiguration[e.TipoDashboardId], e));
            }
            if (e.Columna == 3) {
                this.items3.push(new DashboardItem(this.dashboardComponentConfiguration[e.TipoDashboardId], e));
            }

        });

        this.dshquicksidebar.SetItemsUsuario(items);
    }

}

