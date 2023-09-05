import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
    selector: '[dashboard-host]',
})
export class DashboardDirective {
    constructor(public viewContainerRef: ViewContainerRef) { }
}
