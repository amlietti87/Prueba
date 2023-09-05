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
import { RutaDto } from '../model/ruta.model';
import { PuntoDto, PuntoFilter } from '../model/punto.model';
import { FileDTO } from '../../../../shared/common/models/fileDTO.model';
import { FileService } from '../../../../shared/common/file.service';

@Injectable()
export class PuntoService extends CrudService<PuntoDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService,
        private fileService: FileService
    ) {
        super(http);
        this.identityUrl = environment.identityUrl + '/Puntos';
        this.endpoint = this.identityUrl;
    }



    RecuperarDatosIniciales(CodRec: number): Observable<ResponseModel<PuntoDto[]>> {
        var params = new HttpParams();
        params = params.set('CodRec', CodRec.toString());
        return this.http.get<ResponseModel<PuntoDto[]>>(this.endpoint + '/RecuperarDatosIniciales', { params: params });
    }

    RecuperarGeoJson(CodRec: number): Observable<ResponseModel<GeoJSON.FeatureCollection>> {
        var params = new HttpParams();
        params = params.set('CodRec', CodRec.toString());
        return this.http.get<ResponseModel<GeoJSON.FeatureCollection>>(this.endpoint + '/GetGeoJson', { params: params });
    }







    GetPuntosReporte(filter: PuntoFilter): void {
        var _url = this.identityUrl + "/GetReporte";
        this.fileService.dowloadAuthenticatedByPost(_url, filter);
    }


}

