import { Injectable } from "@angular/core";
import { Http, Response, Headers } from "@angular/http";
import "rxjs/add/operator/map";

import { environment } from "../../../environments/environment";
import { Observable } from "rxjs/Observable";
import { ResetPasswordInput, ResetPasswordOutput } from "../reset-password.model";

@Injectable()
export class AuthenticationService {

    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    constructor(private http: Http) {
    }

    login(email: string, password: string, captchaValue: string) {
        //TODO: sacar del entorno
        return this.http.post(environment.identityUrl + '/api/Auth/login', JSON.stringify({ username: email, password: password, captchaValue: captchaValue }), { headers: new Headers({ 'Content-Type': 'application/json' }) })
            .map((response: Response) => {
                // login successful if there's a jwt token in the response
                let user = response.json();
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(user));
                }
            })
    }

    intrabuslogin(accessToken: string) {
        //TODO: sacar del entorno
        return this.http.post(environment.identityUrl + '/api/Auth/intrabuslogin', JSON.stringify({ accesToken: accessToken}), { headers: new Headers({ 'Content-Type': 'application/json' }) })
            .map((response: Response) => {
                // login successful if there's a jwt token in the response
                let user = response.json();
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(user));
                }
            })
    }


    private handleError(error: any) {

        //console.error('server error:', error);
        if (error instanceof Response) {
            let errMessage = '';
            try {
                errMessage = error.json();
            } catch (err) {
                errMessage = error.statusText;
            }
            return Observable.throw(errMessage);
        }
        return Observable.throw(error || 'server error');
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        localStorage.clear();
        sessionStorage.clear();
    }



    resetPassword(input: ResetPasswordInput): Observable<ResetPasswordOutput> {
        let url_ = environment.identityUrl + '/api/Auth/ResetPassword';
        return this.http.post(url_, JSON.stringify(input), { headers: new Headers({ 'Content-Type': 'application/json' }) })

            .map(r => new ResetPasswordOutput(r.json()))
            .catch((response_: any) => {
                return Observable.of<ResetPasswordOutput>(<any>{ errorMesagge: response_.text() });
            })


            ;
    }




}