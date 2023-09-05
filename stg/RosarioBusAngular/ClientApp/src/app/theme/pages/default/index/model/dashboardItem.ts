import { Type } from '@angular/core';
import { UsuarioDashboardItemDto } from './dashboard.model';

export class DashboardItem {
    constructor(public component: Type<any>, public data: UsuarioDashboardItemDto) { }
}