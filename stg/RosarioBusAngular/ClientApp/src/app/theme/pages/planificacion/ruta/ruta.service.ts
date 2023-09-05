import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto, ItemDto } from '../../../../shared/model/base.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';


import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { RutaDto, RutaFilter } from '../model/ruta.model';
import { RutasViewFilter, RutasFilteredFilter } from '../model/linea.model';
import { FileService } from '../../../../shared/common/file.service';

@Injectable()
export class RutaService extends CrudService<RutaDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService,
        private fileService: FileService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/Rutas';
        this.endpoint = this.identityUrl;
    }

    aprobarRuta(id: number): Observable<any> {
        let url = this.endpoint + '/AprobarRutaAsync?id=' + id
        return this.http.post(url, null);
    }

    validateAprobarRuta(id: number): Observable<ResponseModel<string>> {
        let url = this.endpoint + '/ValidateAprobarRuta?id=' + id
        return this.http.post<ResponseModel<string>>(url, null);
    }

    validateAprobarRutaDto(detil: RutaDto): Observable<ResponseModel<string>> {
        let url = this.endpoint + '/ValidateAprobarRutaDto';
        return this.http.post<ResponseModel<string>>(url, detil);
    }


    GetRutas(detil: RutasViewFilter): Observable<ResponseModel<RutaDto[]>> {
        let url = this.endpoint + '/GetRutas';
        return this.http.post<ResponseModel<RutaDto[]>>(url, detil);
    }
    GetRutasFiltradas(detil: RutasFilteredFilter): Observable<ResponseModel<RutaDto[]>> {
        let url = this.endpoint + '/GetRutasFiltradas';
        return this.http.post<ResponseModel<RutaDto[]>>(url, detil);
    }

    RecuperarHbasecPorLinea(cod_lin: number): Observable<ResponseModel<ItemDto[]>> {
        let url = this.endpoint + '/RecuperarHbasecPorLinea?cod_lin=' + cod_lin;
        return this.http.get<ResponseModel<ItemDto[]>>(url);
    }

    MinutosPorSectorReporte(filter: RutaFilter): void {
        var _url = this.identityUrl + "/MinutosPorSectorReporte";
        this.fileService.dowloadAuthenticatedByPost(_url, filter);
    }


}

