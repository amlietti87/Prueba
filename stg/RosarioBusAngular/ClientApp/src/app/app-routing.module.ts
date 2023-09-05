import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LogoutComponent } from "./auth/logout/logout.component";
import { NgxPermissionsGuard } from 'ngx-permissions';
import { AuthGuard } from './auth/guards/auth.guard';
import { ResetPasswordComponent } from './auth/reset-password.component';
import { CroquiComponent } from './shared/croqui/croqui.component';
import { LoginIntrabusComponent } from './auth/loginIntrabus/login-intrabus.component';


const routes: Routes = [
    { path: 'login', loadChildren: './auth/auth.module#AuthModule' },
    { path: 'account', loadChildren: './auth/auth.module#AuthModule' },
    { path: 'logout', component: LogoutComponent },
    {
        path: '', redirectTo: 'index', pathMatch: 'full',
        canActivate: [AuthGuard]

    },
    { path: 'intrabuslogin', component: LoginIntrabusComponent},
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }