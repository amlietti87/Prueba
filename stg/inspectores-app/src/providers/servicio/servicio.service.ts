import { HServiciosDto } from './../../models/hServicios.model';
import { Injectable } from "../../../node_modules/@angular/core";
import { CrudService } from "../service/crud.service";
import { HttpClient } from "../../../node_modules/@angular/common/http";
import { environment as ENV} from "@app/env";
import { Observable } from "rxjs";
import { ResponseModel, ItemDto } from "../../models/Base/base.model";


@Injectable()
export class HServiciosService extends CrudService<HServiciosDto> {
    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,) {
        super(http);
        this.identityUrl = ENV.identityUrl + '/HServicios';
        this.endpoint = this.identityUrl;
    }

    RecuperarServiciosPorLinea(reqParams?: any): Observable<ResponseModel<ItemDto[]>> {

        var url= this.endpoint + '/RecuperarServiciosPorLinea';

        return this.http.get<ResponseModel<ItemDto[]>>(url, { params: reqParams });
    }
}