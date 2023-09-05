import { RecaptchaModule, RECAPTCHA_SETTINGS, RecaptchaSettings } from 'ng-recaptcha';
import { RecaptchaFormsModule } from 'ng-recaptcha/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BaseRequestOptions, HttpModule } from '@angular/http';
import { MockBackend } from '@angular/http/testing';

import { AuthRoutingModule } from './auth-routing.routing';
import { AuthComponent } from './auth.component';
import { AlertComponent } from './directives/alert.component';
import { LogoutComponent } from './logout/logout.component';
import { AuthGuard } from './guards/auth.guard';
import { AlertService } from './services/alert.service';
import { AuthenticationService } from './services/authentication.service';
import { UserService } from './services/user.service';
import { fakeBackendProvider } from './helpers/index';
import { ResetPasswordComponent } from './reset-password.component';
import { PasswordComplexityValidator } from '../shared/utils/password-complexity-validator.directive';
import { CustomFormsModule } from 'ng2-validation';
import { ParametersService } from '../theme/pages/admin/parameters/parameters.service';
import { LoginIntrabusComponent } from './loginIntrabus/login-intrabus.component';


@NgModule({
    declarations: [
        PasswordComplexityValidator,

        AuthComponent,
        ResetPasswordComponent,
        AlertComponent,
        LogoutComponent,
        LoginIntrabusComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        CustomFormsModule,
        HttpModule,
        AuthRoutingModule,
        RecaptchaModule.forRoot(),
        RecaptchaFormsModule,
    ],
    providers: [
        AuthGuard,
        AlertService,
        AuthenticationService,
        UserService,
        // api backend simulation
        fakeBackendProvider,
        MockBackend,
        BaseRequestOptions,
        {
            provide: RECAPTCHA_SETTINGS,
            useValue: {
                siteKey: '6LcL3pUUAAAAAJFhON3BZ_Lm6ZHw6X86tH8qjmy5',
            } as RecaptchaSettings,
        },
        ParametersService
    ],
    exports: [
        PasswordComplexityValidator,
        CustomFormsModule
    ],
    entryComponents: [AlertComponent],
})

export class AuthModule {
}