import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './auth.component';
import { ResetPasswordComponent } from './reset-password.component';
import { LoginIntrabusComponent } from './loginIntrabus/login-intrabus.component';

const routes: Routes = [
    { path: '', component: AuthComponent },
    { path: 'reset', component: ResetPasswordComponent },
    { path: 'intrabuslogin', component: LoginIntrabusComponent}

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AuthRoutingModule {
}