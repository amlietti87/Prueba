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
export class LineaService extends CrudService<LineaDto> {


    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/Linea';
        this.endpoint = this.identityUrl;
    }

    UpdateLineasAsociadas(data: LineaDto): Observable<ResponseModel<LineaDto>> {

        let url = this.endpoint + '/UpdateLineasAsociadas';

        return this.http.post<ResponseModel<LineaDto>>(url, data);
    }

    RecuperarLineasConLineasAsociadas(id: number): Observable<ResponseModel<LineaDto>> {
        var params = new HttpParams();
        params = params.set("lineaId", id.toString());


        let url = this.endpoint + '/RecuperarLineasConLineasAsociadas';

        return this.http.get<ResponseModel<LineaDto>>(url, { params: params });
    }
}

