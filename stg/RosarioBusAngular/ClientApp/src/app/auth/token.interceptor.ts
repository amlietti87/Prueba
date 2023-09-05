
import { Injectable, Injector } from '@angular/core';
import {
    HttpInterceptor,
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpResponse,
    HttpErrorResponse
} from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs/Observable';

import { Router } from '@angular/router';
import { PFXERROR, SESSION_EXPIRED } from '../shared/constants/error-constants';
import { RestError } from '../shared/model/error.model';
import { environment } from '../../environments/environment';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    private router: Router;
    private auth: AuthService;
    constructor(private injector: Injector) { }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        this.router = this.injector.get(Router);
        this.auth = this.injector.get(AuthService);

        if (this.auth.getToken()) {
            request = request.clone({
                setHeaders: {
                    Authorization: 'Bearer ' + this.auth.getToken(), Version: environment.version
                }
                //headers: request.headers.set("Authorization", 'Bearer ' + this.auth.getToken()),
            });

            // request.headers.set("Version", "1.1.2");
        }
        else {
            request = request.clone({
                setHeaders: {
                    Version: environment.version
                }
                //headers: request.headers.set("Authorization", 'Bearer ' + this.auth.getToken()),
            });
        }



        return next.handle(request).do((event: HttpEvent<any>) => {
            if (event instanceof HttpResponse) {
                // do stuff with response if you want
            }
        }, (err: any) => {
            if (err instanceof HttpErrorResponse) {

                if (err.status === 401 || err.status == 403) {

                    if (err.status == 403) {
                        this.router.navigate(['/401'], { queryParams: { error: SESSION_EXPIRED } });
                    } else {
                        try {
                            // var errorBody = JSON.parse(err.error) as RestError;
                            // if (errorBody.errorCode == 'REST012') {
                            //    this.router.navigate(['/login'], { queryParams: { error: SESSION_EXPIRED } });
                            // } else {
                            this.router.navigate(['/401']);
                            //}
                        } catch (error) {
                            //this.authenticationService.logout();
                            //this.router.navigate(['/login']);
                        }
                    }

                } else if (err.status == 505) {
                    window.location.reload(true)
                }
            }
        });
    }
}