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
import { BanderaDto, BanderaFilter } from '../model/bandera.model';
import { RutaFilter } from '../model/ruta.model';
import { FileService } from '../../../../shared/common/file.service';
import { HorariosPorSectorDto } from '../model/horariosPorSector.model';
import { FileDTO } from '../../../../shared/common/models/fileDTO.model';

@Injectable()
export class BanderaService extends CrudService<BanderaDto> {






    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService,
        private fileService: FileService
    ) {
        super(http);
        this.identityUrl = environment.identityUrl + '/Bandera';
        this.endpoint = this.identityUrl;
    }

    recuperarCartelBandera(idBandera: any): Observable<ResponseModel<string>> {
        return this.http.post<ResponseModel<string>>(this.endpoint + '/RecuperarCartel', idBandera);
    }




    GetReporteCambiosDeSector(idBandera: number): void {
        var _url = this.identityUrl + "/GetReporteCambiosDeSector";
        this.fileService.dowloadAuthenticatedByPost(_url, { cod_band: idBandera });
    }

    GetReporteSabana(horarios: HorariosPorSectorDto, callbackFn: Function): void {

        var _url = this.identityUrl + "/GetReporteSabanaSinMinutos";
        this.fileService.dowloadSabanAuthenticatedByPost(_url, { Linea: horarios.Linea, TipoDia: horarios.TipoDia, FechaDesde: horarios.FechaDesde, Items: horarios.Items, Colulmnas: horarios.Colulmnas, LabelBandera: horarios.LabelBandera, TipoInforme: horarios.TipoInforme })
            .subscribe(data => {
                var url = this.getBlobURL(data.DataObject);
                if (data.DataObject.ForceDownload) {
                    var a = window.document.createElement('a');
                    a.href = url;
                    a.download = data.DataObject.FileName;
                    document.body.appendChild(a);
                    a.click();
                    document.body.removeChild(a);
                } else {
                    window.open(url, "_blank");
                }
                if (callbackFn) {
                    callbackFn();
                }
            });
    }

    getBlobURL(file: FileDTO) {
        var raw = window.atob(file.ByteArray);
        var rawLength = raw.length;
        var array = new Uint8Array(new ArrayBuffer(rawLength));
        for (var i = 0; i < rawLength; i++) {
            array[i] = raw.charCodeAt(i);
        }
        var url = window.URL.createObjectURL(new Blob([array], { type: file.FileType }));
        return url;
    }

    recuperarHorariosSectorPorSector(reqParams?: BanderaFilter): Observable<ResponseModel<HorariosPorSectorDto>> {

        var url = this.endpoint + '/RecuperarHorariosSectorPorSector';

        return this.http.post<ResponseModel<HorariosPorSectorDto>>(url, reqParams);
    }

    RecuperarLineasActivasPorFecha(reqParams?: any): Observable<ResponseModel<ItemDto[]>> {

        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }

        var url = this.endpoint + '/RecuperarLineasActivasPorFecha';

        return this.http.get<ResponseModel<ItemDto[]>>(url, { params: params });
    }


    RecuperarBanderasPorServicio(reqParams?: any): Observable<ResponseModel<ItemDto[]>> {

        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }
        var url = this.endpoint + '/RecuperarBanderasPorServicio';

        return this.http.get<ResponseModel<ItemDto[]>>(url, { params: params });
    }

    //OrigenPredictivoSSS(filter: any): Observable<ResponseModel<string[]>> {
    //    let url = this.endpoint + '/OrigenPredictivo';
    //    let data = filter;

    //    return this.http.post<ResponseModel<string[]>>(url, data);
    //}

    OrigenPredictivo(reqParams?: any): Observable<ResponseModel<string[]>> {
        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }

        return this.http.get<ResponseModel<string[]>>(this.endpoint + '/OrigenPredictivo', { params: params });
    }

    DestinoPredictivo(reqParams?: any): Observable<ResponseModel<string[]>> {
        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }

        return this.http.get<ResponseModel<string[]>>(this.endpoint + '/DestinoPredictivo', { params: params });
    }

    recuperarBanderasRelacionadasPorSector(reqParams?: any): Observable<ResponseModel<ItemDto[]>> {

        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }

        return this.http.get<ResponseModel<ItemDto[]>>(this.endpoint + '/RecuperarBanderasRelacionadasPorSector', { params: params });
    }
}

