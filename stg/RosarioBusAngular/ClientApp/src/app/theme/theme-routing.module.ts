import { NgModule } from '@angular/core';
import { ThemeComponent } from './theme.component';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from "../auth/guards/auth.guard";

const routes: Routes = [
    {
        "path": "",
        "component": ThemeComponent,
        "canActivate": [AuthGuard],
        "children": [
            {
                "path": "admin",
                "loadChildren": ".\/pages\/admin\/admin-default.module#AdminDefaultModule"
            },
            {
                "path": "planificacion",
                "loadChildren": ".\/pages\/planificacion\/planificacion.module#PlanificacionModule"
            },
            {
                "path": "siniestros",
                "loadChildren": ".\/pages\/siniestros\/siniestros.module#SiniestrosModule"
            },
            {
                "path": "art",
                "loadChildren": ".\/pages\/art\/art.module#ARTModule"
            },
            {
                "path": "index",
                "loadChildren": ".\/pages\/default\/index\/index.module#IndexModule"
            },
            {
                "path": "reclamos",
                "loadChildren": ".\/pages\/reclamos\/reclamos.module#ReclamosModule"
            },
            {
                "path": "inspectores",
                "loadChildren": ".\/pages\/inspectores\/inspectores.module#InspectoresModule"
            },
            {
                "path": "firmadigital",
                "loadChildren": ".\/pages\/firmadigital\/firmadigital.module#FirmaDigitalModule"
            },
            //{
            //    "path": "header\/actions",
            //    "loadChildren": ".\/pages\/default\/header\/header-actions\/header-actions.module#HeaderActionsModule"
            //},
            //{
            //    "path": "header\/profile",
            //    "loadChildren": ".\/pages\/default\/header\/header-profile\/header-profile.module#HeaderProfileModule"
            //},
            {
                "path": "404",
                "loadChildren": ".\/pages\/default\/erros-pages\/not-found\/not-found.module#NotFoundModule"
            },
            {
                "path": "401",
                "loadChildren": ".\/pages\/default\/erros-pages\/unauthorized\/unauthorized.module#UnauthorizedModule"
            },
            {
                "path": "",
                "redirectTo": "index",
                "pathMatch": "full"
            }
        ]
    },
    //{
    //    "path": "snippets\/pages\/errors\/error-6",
    //    "loadChildren": ".\/pages\/self-layout-blank\/snippets\/pages\/errors\/errors-error-6\/errors-error-6.module#ErrorsError6Module"
    //},
    {
        "path": "**",
        "redirectTo": "404",
        "pathMatch": "full"
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ThemeRoutingModule { }