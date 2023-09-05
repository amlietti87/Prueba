import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';

import { HttpClient, HttpParams } from '@angular/common/http';


import 'rxjs/Rx';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { ProvinciasDto } from '../model/localidad.model';
import { Observable } from 'rxjs/Rx';
import { ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';

@Injectable()
export class ProvinciasService extends CrudService<ProvinciasDto> {

    private siniestrosUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.siniestrosUrl = environment.siniestrosUrl + '/Provincias';
        this.endpoint = this.siniestrosUrl;
    }

    requestAllByFilter(reqParams?: any): Observable<ResponseModel<PaginListResultDto<ProvinciasDto>>> {
        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }
        return this.http.get<ResponseModel<PaginListResultDto<ProvinciasDto>>>(this.endpoint + '/GetAllAsync', { params: params });
    }


}

