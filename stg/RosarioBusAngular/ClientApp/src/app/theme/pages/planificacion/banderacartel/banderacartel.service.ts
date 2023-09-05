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
import { BanderaDto } from '../model/bandera.model';
import { RutaFilter } from '../model/ruta.model';
import { FileService } from '../../../../shared/common/file.service';
import { BanderaCartelDto, BanderaCartelFilter } from '../model/banderacartel.model';

@Injectable()
export class BanderaCartelService extends CrudService<BanderaCartelDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService,
        private fileService: FileService
    ) {
        super(http);
        this.identityUrl = environment.identityUrl + '/BolBanderasCartel';
        this.endpoint = this.identityUrl;
    }

    RecuperarCartelPorImportador(filtro: BanderaCartelFilter): Observable<ResponseModel<BanderaCartelDto>> {
        var params = new HttpParams();
        if (filtro) {
            Object.keys(filtro).forEach(function(item) {
                params = params.set(item, filtro[item]);
            });
        }

        return this.http.get<ResponseModel<BanderaCartelDto>>(this.endpoint + '/RecuperarCartelPorImportador', { params: params });
    }



}

