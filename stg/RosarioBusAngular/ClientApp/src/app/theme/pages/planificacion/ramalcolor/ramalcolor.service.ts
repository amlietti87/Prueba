import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto, ItemDto } from '../../../../shared/model/base.model';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';


import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { RamalColorDto } from '../model/ramalcolor.model';

@Injectable()
export class RamalColorService extends CrudService<RamalColorDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/RamalColor';
        this.endpoint = this.identityUrl;
    }

    GetAllAsyncSinSentidos(reqParams?: any): Observable<ResponseModel<ItemDto[]>> {
        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }

        return this.http.get<ResponseModel<ItemDto[]>>(this.endpoint + '/GetAllAsyncSinSentidos', { params: params });
    }

    TieneMapasEnBorrador(id: any): Observable<ResponseModel<any>> {
        var params = new HttpParams();
        if (id) {
            params = params.set("Id", id);
        }

        return this.http.get<ResponseModel<any>>(this.endpoint + '/TieneMapasEnBorrador', { params: params });
    }

}

