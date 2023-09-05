import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import 'rxjs/Rx';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import { environment } from '../../../../../environments/environment';
import { HServiciosDto } from '../model/hServicios.model';
import { Observable } from 'rxjs/Rx';
import { ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';

@Injectable()
export class HServiciosService extends CrudService<HServiciosDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,

    ) {
        super(http);
        this.identityUrl = environment.identityUrl + '/HServicios';
        this.endpoint = this.identityUrl;
    }

    RecuperarServiciosPorLinea(reqParams?: any): Observable<ResponseModel<HServiciosDto[]>> {
        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }
        return this.http.get<ResponseModel<HServiciosDto[]>>(this.endpoint + '/RecuperarServiciosPorLinea', { params: params });
    }

}