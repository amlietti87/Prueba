import { Component, Input, OnInit, ViewChild, ComponentFactoryResolver, OnDestroy } from '@angular/core';

import { DashboardDirective } from './dashboardDirective';
import { DashboardItem } from '../model/dashboardItem';
import { IDashboardBaseComponent, Dashboard1Component } from './dashboardComponent';


@Component({
    selector: 'app-ad-ditem',
    template: `
              <div class="app-porlet-item">
                <ng-template dashboard-host></ng-template>
              </div>
            `
})
export class ManagmentComponent implements OnInit, OnDestroy {
    @Input() dashboardItem: DashboardItem;

    @ViewChild(DashboardDirective) dashboardHost: DashboardDirective;


    constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

    ngOnInit() {
        this.loadComponent();

    }

    ngOnDestroy() {

    }

    loadComponent() {

        let adItem = this.dashboardItem;
        //let adItem = new DashboardItem(Dashboard1Component, { headline: "seba", body: "body seba" });

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(adItem.component);

        let viewContainerRef = this.dashboardHost.viewContainerRef;
        viewContainerRef.clear();

        let componentRef = viewContainerRef.createComponent(componentFactory);
        (<IDashboardBaseComponent>componentRef.instance).data = adItem.data;
    }


}
