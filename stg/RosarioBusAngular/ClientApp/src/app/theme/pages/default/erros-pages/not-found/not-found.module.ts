import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { LayoutModule } from '../../../../layouts/layout.module';
import { DefaultComponent } from '../../default.component';
import { NotFoundComponent, NotFoundComponentChild } from "./not-found.component";

const routes: Routes = [
    {
        "path": "",
        "component": DefaultComponent,
        "children": [
            {
                "path": "",
                "component": NotFoundComponent,
                "children": [
                    {
                        "path": "oops",
                        "component": NotFoundComponentChild
                    }]
            }
        ]
    }
];

@NgModule({
    imports: [
        CommonModule, RouterModule.forChild(routes), LayoutModule
    ], exports: [
        RouterModule
    ], declarations: [
        NotFoundComponent, NotFoundComponentChild
    ]
})
export class NotFoundModule {
}