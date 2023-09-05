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
import { LineaDto } from '../model/linea.model';

@Injectable()
export class PlaLineaService extends CrudService<LineaDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/PlaLinea';
        this.endpoint = this.identityUrl;
    }


    TieneMapasEnBorrador(id: any): Observable<ResponseModel<any>> {
        var params = new HttpParams();
        if (id) {
            params = params.set("Id", id);
        }

        return this.http.get<ResponseModel<any>>(this.endpoint + '/TieneMapasEnBorrador', { params: params });
    }

}

