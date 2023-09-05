import { Component, ViewEncapsulation, OnInit, Injector, ViewChild, ComponentFactoryResolver, ViewContainerRef } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { AppComponentBase } from '../../shared/common/app-component-base';
import { AuthenticationService, AlertService } from "../services";
import { PermissionCheckerService } from "../../shared/common/permission-checker.service";
import { RecaptchaComponent } from "ng-recaptcha";
import { AlertComponent } from '../directives/alert.component';

@Component({
    selector: '.m-grid.m-grid--hor.m-grid--root.m-page',
    templateUrl: './login-intrabus.component.html',
    encapsulation: ViewEncapsulation.None

})
export class LoginIntrabusComponent extends AppComponentBase implements OnInit {
    model: any = {};
    loading = true;
    returnUrl: string;
    inputType: string = "password";
    eyeIcon: string = "fa fa-eye-slash";
    captchaVisible: boolean = false;
    captchaValue: string;
    accessToken: string;

    @ViewChild('alertSignin', { read: ViewContainerRef }) alertSignin: ViewContainerRef;

    constructor(
         private _router: Router,
        // private _script: ScriptLoaderService,
        // private _userService: UserService,
        private _route: ActivatedRoute,
        private _injector: Injector,
        private _authService: AuthenticationService,
        private _alertService: AlertService,
        private cfr: ComponentFactoryResolver,
        private permissionsService: PermissionCheckerService
    ) {
        super(_injector);
    }

    ngOnInit() {

        this.loading = true;
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        if (currentUser) {
            localStorage.setItem('currentUser', null);
        }

        this._route.queryParams.subscribe(params => 
        {   
            if (params.accessToken) {                
                this.accessToken = params.accessToken;
    
                this._authService.intrabuslogin(this.accessToken)
                .subscribe(
                    data => {
                        this.permissionsService.GetPermissions().subscribe(async per => {
                            await this.permissionsService.setPermissions(per.DataObject);
                            await this.permissionsService.loadPermissions();
                            
                            this._router.navigateByUrl("/index");
                            //this._router.navigate([this.returnUrl]);
                        });
                    },
                    error => {
                        this.showAlert('alertSignin');
        
                        try {
                            var text = error.text();
                            if (text.indexOf('accessToken inválido') != -1) {
                                this._alertService.error('accessToken inválido');
        
                            }
                            if (text.indexOf('No existe usuario') != -1 && text.indexOf('accessToken inválido') == -1) {
                                this._alertService.error('No existe usuario');
                            }
                            if (text.indexOf('Empleado no habilitado') != -1) {
                                this._alertService.error('Empleado no habilitado');
                            }
        
                        } catch (e) {
                        }
                        this.loading = false;
                    });
            }
        });

    
    }

    showAlert(target) {
        this[target].clear();
        let factory = this.cfr.resolveComponentFactory(AlertComponent);
        let ref = this[target].createComponent(factory);
        ref.changeDetectorRef.detectChanges();
    }
}