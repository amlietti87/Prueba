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
import { BanderaDto } from '../model/bandera.model';
import { RutaFilter } from '../model/ruta.model';
import { FileService } from '../../../../shared/common/file.service';
import { HFechasConfiDto, PlaHorarioFechaLineaListView, PlaDistribucionDeCochesPorTipoDeDiaDto, PlaDistribucionDeCochesPorTipoDeDiaFilter, HMediasVueltasImportadaDto, ImportarServiciosInput, HBasecDto } from '../model/HFechasConfi.model';
import { ExportarExcelDto, HServiciosFilter } from '../model/hServicios.model';


@Injectable()
export class HFechasConfiService extends CrudService<HFechasConfiDto> {



    private identityUrl: string = '';

    constructor(
        protected http: HttpClient,
        private fileService: FileService
    ) {

        super(http);

        this.identityUrl = environment.identityUrl + '/HFechasConfi';
        this.endpoint = this.identityUrl;

    }

    public HorarioDiagramado(CodHfecha: number, idServicio: number): Observable<ResponseModel<boolean>> {
        return this.http.get<ResponseModel<boolean>>(this.endpoint + '/HorarioDiagramado?CodHfecha=' + CodHfecha + "&idServicio=" + idServicio);
    }

    public GetLineasHorarias(): Observable<ResponseModel<PlaHorarioFechaLineaListView[]>> {
        return this.http.get<ResponseModel<PlaHorarioFechaLineaListView[]>>(this.endpoint + '/GetLineasHorarias');
    }

    public CopiarHorario(cod_hfecha: number, fec_desde: Date, CopyConductores: boolean): Observable<ResponseModel<ItemDto>> {

        var params = {
            cod_hfecha: cod_hfecha,
            fec_desde: fec_desde,
            CopyConductores: CopyConductores
        };

        return this.http.post<ResponseModel<ItemDto>>(this.endpoint + '/CopiarHorario', params);
    }


    public RemapearRecoridoBandera(row: HBasecDto): Observable<ResponseModel<number>> {
        var params = {
            cod_hfecha: row.CodHfecha,
            cod_ban: row.CodBan
        };

        return this.http.post<ResponseModel<number>>(this.endpoint + '/RemapearRecoridoBandera', params);
    }

    public GetReporteExcel(model: ExportarExcelDto): void {
        var _url = this.endpoint + "/GenerarExcelHorarios";
        this.fileService.dowloadAuthenticatedByPost(_url, model);
    }

    GenerateReportRelevo(data: any): any {
        var _url = this.endpoint + "/GenerateReporteRelevo";
        this.fileService.dowloadAuthenticatedByPost(_url, data);

        //let url = this.endpoint + '/GenerateReportRelevo';
        //return this.http.post(url, data, { responseType: 'blob' });
    }

    public UpdateHBasec(row: HBasecDto): Observable<ResponseModel<HBasecDto>> {
        var params = {
            CodHFecha: row.CodHfecha,
            CodBan: row.CodBan,
            CodSec: row.CodSec,
            CodRec: row.CodRec

        }
        return this.http.post<ResponseModel<HBasecDto>>(this.endpoint + '/UpdateHBasec', params);
    }

    public GuardarBanderaPorSerctor(dto: HFechasConfiDto) : Observable<ResponseModel<HFechasConfiDto>> {
        return this.http.post<ResponseModel<HFechasConfiDto>>(this.endpoint + '/GuardarBanderaPorSerctor', dto);
    }
}

@Injectable()
export class PlaDistribucionDeCochesPorTipoDeDiaService extends CrudService<PlaDistribucionDeCochesPorTipoDeDiaDto> {


    private identityUrl: string = '';

    constructor(protected http: HttpClient,
        private fileService: FileService) {

        super(http);

        this.identityUrl = environment.identityUrl + '/PlaDistribucionDeCochesPorTipoDeDia';
        this.endpoint = this.identityUrl;
    }

    uploadPlanillaIvu(data: FileList): Observable<ResponseModel<string>> {

        let testData: FormData = new FormData();
        for (var i = 0; i < data.length; i++) {
            testData.append('file', data[i], data[i].name);
        }

        let url = this.endpoint + '/uploadPlanillaIvu';
        return this.http.post<ResponseModel<string>>(url, testData);
    }

    uploadPlanilla(data: any): Observable<ResponseModel<string>> {

        let testData: FormData = new FormData();
        testData.append('file', data, data.name);
        let url = this.endpoint + '/uploadPlanilla';
        return this.http.post<ResponseModel<string>>(url, testData);
    }

    TieneMinutosAsignados(Id: number): Observable<ResponseModel<boolean>> {
        var params = new HttpParams();
        var filter = new PlaDistribucionDeCochesPorTipoDeDiaFilter();
        filter.Id = Id;

        if (filter) {
            Object.keys(filter).forEach(function(item) {
                params = params.set(item, filter[item]);
            });
        }
        let url = this.endpoint + '/TieneMinutosAsignados';

        return this.http.get<ResponseModel<boolean>>(url, { params: params });
    }


    uploadPlanillaMinutosPorSector(data: any): Observable<ResponseModel<string>> {

        let testData: FormData = new FormData();
        testData.append('file', data, data.name);
        let url = this.endpoint + '/uploadPlanillaMinutosPorSector';
        return this.http.post<ResponseModel<string>>(url, testData);
    }
    RecuperarPlanilla(filter: PlaDistribucionDeCochesPorTipoDeDiaFilter): Observable<ResponseModel<HMediasVueltasImportadaDto[]>> {

        var params = new HttpParams();
        if (filter) {
            Object.keys(filter).forEach(function(item) {
                params = params.set(item, filter[item]);
            });
        }
        let url = this.endpoint + '/RecuperarPlanilla';
        return this.http.get<ResponseModel<HMediasVueltasImportadaDto[]>>(url, { params: params });
    }

    ImportarServicios(input: ImportarServiciosInput): Observable<ResponseModel<string>> {

        let url = this.endpoint + '/ImportarServicios';
        return this.http.post<ResponseModel<string>>(url, input);
    }

    ExistenMediasVueltasIncompletas(data: PlaDistribucionDeCochesPorTipoDeDiaDto): Observable<ResponseModel<number>> {


        var params = new HttpParams();
        if (data) {
            Object.keys(data).forEach(function(item) {
                params = params.set(item, data[item]);
            });
        }
        let url = this.endpoint + '/ExistenMediasVueltasIncompletas';

        return this.http.get<ResponseModel<number>>(url, { params: params });
    }

    RecrearSabanaSector(data: PlaDistribucionDeCochesPorTipoDeDiaDto): Observable<ResponseModel<boolean>> {
        let url = this.endpoint + '/RecrearSabanaSector';
        return this.http.post<ResponseModel<boolean>>(url, data);
    }








}

