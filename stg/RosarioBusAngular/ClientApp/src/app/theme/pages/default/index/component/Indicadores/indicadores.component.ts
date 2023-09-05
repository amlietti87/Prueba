import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { IDashboardBaseComponent } from '../dashboardComponent';

@Component({
    selector: 'm-packages',
    templateUrl: './indicadores.component.html'
})
export class IndicadoresDashboardComponent implements IDashboardBaseComponent, OnInit {
    data: any;

    constructor() { }

    ngOnInit() {
    }

}
