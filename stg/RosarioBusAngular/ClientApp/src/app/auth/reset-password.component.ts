import { Component, Injector, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';


import { AppComponentBase } from '../shared/common/app-component-base';
import { ResetPasswordModel, ResetPasswordOutput } from './reset-password.model';
import { AuthenticationService } from './services/authentication.service';

@Component({
    selector: '.m-grid.m-grid--hor.m-grid--root.m-page',
    templateUrl: './reset-password.component.html'

})
export class ResetPasswordComponent extends AppComponentBase implements OnInit {

    model: ResetPasswordModel = new ResetPasswordModel();

    loading: boolean = false;
    saving: boolean = false;
    constructor(
        injector: Injector,
        private _authService: AuthenticationService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        //private _loginService: LoginService,
        //private _appUrlService: AppUrlService,
        //private _appSessionService: AppSessionService,
        //private _profileService: ProfileServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.model.userId = this._activatedRoute.snapshot.queryParams['userId'];
        this.model.resetCode = this._activatedRoute.snapshot.queryParams['resetCode'];




    }

    save(): void {
        this.loading = true;
        this.saving = true;
        this._authService.resetPassword(this.model)
            .finally(() => {
                this.loading = false;
                this.saving = false;
            })

            .subscribe((result: ResetPasswordOutput) => {
                if (result.errorMesagge) {
                    this.message.error(result.errorMesagge, 'ResetPassword');
                }
                else {
                    // Autheticate
                    //this.loading = true;

                    //this._authService.login(result.userName, this.model.password)
                    //    .finally(() => { this.saving = false; });
                    this._router.navigate(['login']);
                }

            });
    }


}
