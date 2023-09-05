import {
    Component,
    ComponentFactoryResolver,
    OnInit,
    ViewChild,
    ViewContainerRef,
    ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ScriptLoaderService } from '../services/script-loader.service';
import { AuthenticationService } from './services/authentication.service';
import { AlertService } from './services/alert.service';
import { UserService } from './services/user.service';
import { AlertComponent } from './directives/alert.component';
import { LoginCustom } from './helpers/login-custom';
import { Helpers } from '../helpers';
import { PermissionCheckerService } from '../shared/common/permission-checker.service';
import { ParametersService } from '../theme/pages/admin/parameters/parameters.service';
import { ParametersDto, CantidadIntentosLoginKey } from '../theme/pages/admin/model/parameters.model';
import { RecaptchaComponent } from 'ng-recaptcha';


export const Code_UserPasswordInvalid = 1;
export const Code_RequiredCaptcha = 2;
export const Code_InvalidCaptcha = 3;


@Component({
    selector: '.m-grid.m-grid--hor.m-grid--root.m-page',
    templateUrl: './templates/login-4.component.html',
    encapsulation: ViewEncapsulation.None

})
export class AuthComponent implements OnInit {
    model: any = {};
    loading = false;
    returnUrl: string;
    inputType: string = "password";
    eyeIcon: string = "fa fa-eye-slash";
    captchaVisible: boolean = false;
    captchaValue: string;

    @ViewChild('alertSignin', { read: ViewContainerRef }) alertSignin: ViewContainerRef;
    @ViewChild('alertSignup',
        { read: ViewContainerRef }) alertSignup: ViewContainerRef;
    @ViewChild('alertForgotPass',
        { read: ViewContainerRef }) alertForgotPass: ViewContainerRef;

    @ViewChild('recaptcha') captcha: RecaptchaComponent;

    constructor(
        private _router: Router,
        private _script: ScriptLoaderService,
        private _userService: UserService,
        private _route: ActivatedRoute,
        private _authService: AuthenticationService,
        private _alertService: AlertService,
        private cfr: ComponentFactoryResolver,
        private permissionsService: PermissionCheckerService
    ) {
    }

    ngOnInit() {
        this.model.remember = true;
        // get return url from route parameters or default to '/'
        this.returnUrl = this._route.snapshot.queryParams['returnUrl'] || '/';
        this._router.navigate([this.returnUrl]);

        this._script.loadScripts('body', [
            'assets/vendors/base/vendors.bundle.js',
            'assets/demo/default/base/scripts.bundle.js'], true).then(() => {
                Helpers.setLoading(false);
                LoginCustom.init();
            });


    }

    resolved(captchaResponse: string) {
        this.captchaValue = captchaResponse;
    }


    signin() {
        this.loading = true;
        this._authService.logout();


        this._authService.login(this.model.email, this.model.password, this.captchaValue)
            .subscribe(
            data => {
                this.permissionsService.GetPermissions().subscribe(async per => {
                    await this.permissionsService.setPermissions(per.DataObject);
                    await this.permissionsService.loadPermissions();
                    console.log(this.returnUrl);

                    this._router.navigateByUrl(this.returnUrl);
                    //this._router.navigate([this.returnUrl]);
                });
            },
            error => {

                this.showAlert('alertSignin');

                try {
                    var text = error.text();
                    if (text.indexOf('"isTrusted": true') != -1) {
                        this._alertService.error('Ha ocurrido un error');

                    }


                    var data = error.json();
                    if (data.code == Code_InvalidCaptcha || data.code == Code_RequiredCaptcha) {
                        this.captchaVisible = true;
                        this.captcha.reset();
                        this._alertService.error(data.message);
                    }

                    if (data.code == Code_UserPasswordInvalid) {
                        this.captchaVisible = false;
                        this._alertService.error(data.message);
                    }

                } catch (e) {

                }



                this.loading = false;
            });
    }

    signup() {
        this.loading = true;
        this._userService.create(this.model).subscribe(
            data => {
                this.showAlert('alertSignin');
                this._alertService.success(
                    'Thank you. To complete your registration please check your email.',
                    true);
                this.loading = false;
                LoginCustom.displaySignInForm();
                this.model = {};
            },
            error => {
                this.showAlert('alertSignup');
                this._alertService.error(error);
                this.loading = false;
            });
    }

    forgotPass() {
        this.loading = true;
        this._userService.forgotPassword(this.model.email).subscribe(
            data => {
                this.showAlert('alertSignin');
                this._alertService.success(
                    'Cool! Password recovery instruction has been sent to your email.',
                    true);
                this.loading = false;
                LoginCustom.displaySignInForm();
                this.model = {};
            },
            error => {
                this.showAlert('alertForgotPass');
                this._alertService.error(error);
                this.loading = false;
            });
    }

    showAlert(target) {
        this[target].clear();
        let factory = this.cfr.resolveComponentFactory(AlertComponent);
        let ref = this[target].createComponent(factory);
        ref.changeDetectorRef.detectChanges();
    }

    showPassword() {
        this.inputType = (this.inputType == "password") ? "text" : "password";
        this.eyeIcon = (this.eyeIcon == "fa fa-eye") ? "fa fa-eye-slash" : "fa fa-eye";
    }
}