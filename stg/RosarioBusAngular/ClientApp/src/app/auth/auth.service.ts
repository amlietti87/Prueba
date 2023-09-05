import { Injectable } from '@angular/core';
@Injectable()
export class AuthService {
    public getToken(): string {
        var cu = JSON.parse(localStorage.getItem('currentUser'));
        if (cu) {
            return cu.token
        }
        return null;
    }

    public isAuthenticated(): boolean {
        //TODO ver  angular2-jwt
        //https://medium.com/@ryanchenkie_40935/angular-authentication-using-the-http-client-and-http-interceptors-2f9d1540eb8
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));

        if (currentUser != null && currentUser.token != null && new Date(currentUser.expires) > new Date()) {
            // logged in so return true
            return true;
        }
        return false;
    }

    public GetUserData(): any {
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        return currentUser;

    }
}