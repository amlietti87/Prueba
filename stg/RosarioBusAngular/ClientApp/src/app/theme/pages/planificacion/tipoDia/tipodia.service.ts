import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';


import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { } from '../model/user.model';

import { TipoDiaDto } from '../model/tipoDia.model';

@Injectable()
export class TipoDiaService extends CrudService<TipoDiaDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/TiposDeDias';
        this.endpoint = this.identityUrl;
    }

    DescripcionPredictivo(reqParams?: any): Observable<ResponseModel<any[]>> {

        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }

        return this.http.get<ResponseModel<any[]>>(this.endpoint + '/DescripcionPredictivo', { params: params });
    }
}

