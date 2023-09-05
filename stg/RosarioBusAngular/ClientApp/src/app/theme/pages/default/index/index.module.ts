import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { IndexComponent } from './index.component';
import { LayoutModule } from '../../../layouts/layout.module';
import { DefaultComponent } from '../default.component';
import { SortablejsModule } from 'angular-sortablejs/dist';
import { CalendarModule } from "ap-angular2-fullcalendar";
import { ManagmentComponent } from './component/managmentComponent';
import { Dashboard1Component } from './component/dashboardComponent';
import { DashboardDirective } from './component/dashboardDirective';
import { DashboardCalendarComponent } from './component/dashboardCalendarComponent';

import { IndicadoresDashboardComponent } from './component/Indicadores/indicadores.component';
import { RecentNotificationsDashboardComponent } from './component/recent-notifications/recent-notifications';
import { DshQuickSidebarComponent } from './component/sidebar/dsh-quick-sidebar.component';
import { DashboarService } from './services/dashboard.service';
import { PasajerosChartsComponent } from './component/pasajeros-horas/pasajeros-charts.component';
import { CumplimientoHorariosChartsComponent } from './component/cumplimiento-horarios/cumplimiento-horarios-charts.component';
import { SharedModule } from '../../../../shared/shared.module';
import { AuthGuard } from '../../../../auth/guards';
import { NgxPermissionsGuard } from 'ngx-permissions';


const routes: Routes = [
    {
        "path": "",
        "component": DefaultComponent,
        "children": [
            {
                "path": "",
                "component": IndexComponent,
                canActivate: [AuthGuard],
            }
        ]
    }
];
@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        LayoutModule,
        SharedModule,
        SortablejsModule,
        CalendarModule
    ], exports: [
        RouterModule
    ], declarations: [
        IndexComponent,
        ManagmentComponent,
        DshQuickSidebarComponent,
        Dashboard1Component,
        DashboardCalendarComponent,
        DashboardDirective,
        IndicadoresDashboardComponent,
        RecentNotificationsDashboardComponent,
        PasajerosChartsComponent,
        CumplimientoHorariosChartsComponent

    ],
    entryComponents: [
        Dashboard1Component,
        DashboardCalendarComponent,
        IndicadoresDashboardComponent,
        RecentNotificationsDashboardComponent,
        PasajerosChartsComponent,
        CumplimientoHorariosChartsComponent
    ],
    providers: [
        DashboarService
    ]
})
export class IndexModule {



}