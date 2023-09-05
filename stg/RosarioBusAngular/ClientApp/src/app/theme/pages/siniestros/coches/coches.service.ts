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

import { CochesDto } from '../model/coche.model';
import { LineaDto } from '../../planificacion/model/linea.model';
import { Time } from '@angular/common';

@Injectable()
export class CochesService extends CrudService<CochesDto> {

    private siniestrosUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.siniestrosUrl = environment.siniestrosUrl + '/CCoches';
        this.endpoint = this.siniestrosUrl;
    }

    GetLineaPorDefecto(CodEmpleado: number, FechaSiniestro: Date, HoraSiniestro: string): Observable<ResponseModel<number>> {
        var reqParams = {
            CodEmpleado: CodEmpleado,
            FechaSiniestro: FechaSiniestro.toISOString(),
            HoraSiniestro: HoraSiniestro
        };
        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }

        //let url = this.endpoint + '/GetLineaPorDefecto?CodEmpleado=' + CodEmpleado + '&FechaSiniestro=' + FechaSiniestro.toISOString() + '&HoraSiniestro=' + HoraSiniestro;
        let url = this.endpoint + '/GetLineaPorDefecto';
        return this.http.get<ResponseModel<number>>(url, { params: params });
    }
    GetCocheById(Id: string, FechaSiniestro: Date): Observable<ResponseModel<CochesDto>> {

        let url = this.endpoint + '/GetCocheById?Id=' + Id + '&FechaSiniestro=' + FechaSiniestro;
        return this.http.get<ResponseModel<CochesDto>>(url);
    }

    ExisteCoche(Id: string): Observable<ResponseModel<boolean>> {

        let url = this.endpoint + '/ExisteCoche?id=' + Id;
        return this.http.get<ResponseModel<boolean>>(url);
    }

    GetAllCoches(FechaSiniestro: string, Filter: string): Observable<ResponseModel<CochesDto[]>> {

        let url = this.endpoint + '/GetAllCoches?FechaSiniestro=' + FechaSiniestro + '&Filter=' + Filter;
        return this.http.get<ResponseModel<CochesDto[]>>(url);
    }
}

