import { Injectable } from '@angular/core';
import { Response } from '@angular/http';




import { HttpClient, HttpHeaders } from '@angular/common/http';


import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';


import { } from '../model/user.model';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../auth/auth.service';
import { CrudService } from '../common/services/crud.service';
import { ResponseModel, ViewMode } from '../model/base.model';
import { CroquisDto } from '../model/croqui.model';

@Injectable()
export class CroquiService extends CrudService<CroquisDto> {


    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/CroCroquis';
        this.endpoint = this.identityUrl;
    }

    createOrUpdate(data: CroquisDto, mode: ViewMode): Observable<ResponseModel<any>> {

        let url = this.endpoint + '/UpdateEntity';
        if (mode == ViewMode.Add) {
            url = this.endpoint + '/SaveNewEntity';
        }
        return this.http.post<ResponseModel<CroquisDto>>(url, data);
    }






}

