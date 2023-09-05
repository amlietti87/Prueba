import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';


import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';

import { } from '../model/user.model';

import { TallerDto, TallerFilter } from '../model/taller.model';
import { PuntoDto } from '../model/punto.model';
import { RutaDto } from '../model/ruta.model';

@Injectable()
export class TallerService extends CrudService<TallerDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/Galpon';
        this.endpoint = this.identityUrl;
    }



    SaveGalponPorSucursal(data: TallerDto[]): Observable<ResponseModel<any>> {

        for (var i = 0; i < data.length; i++) {
            data[i].Rutas = [];
        }

        let url = this.endpoint + '/SaveGalponPorSucursal';
        return this.http.post<ResponseModel<any>>(url, data);
    }

    UpdateRutasPorGalpon(data: TallerDto): Observable<ResponseModel<any>> {
        let url = this.endpoint + '/UpdateRutasPorGalpon';
        return this.http.post<ResponseModel<any>>(url, data);
    }


    GetRutasByGalpon(filter: TallerFilter): Observable<ResponseModel<RutaDto[]>> {

        let url = this.endpoint + '/GetPuntosInicioFin';
        let data = filter;
        return this.http.post<ResponseModel<RutaDto[]>>(url, data);

    }


    CanDeleteGalpon(data: TallerDto): Observable<ResponseModel<boolean>> {

        let url = this.endpoint + '/CanDeleteGalpon';

        return this.http.post<ResponseModel<boolean>>(url, data);

    }



}

