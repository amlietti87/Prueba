import { ParadaDto } from './../../models/parada.model';
import { ResponseModel, PaginListResultDto } from './../../models/Base/base.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CrudService } from '../../providers/service/crud.service';
import { environment as ENV} from "@app/env";
import { Observable } from 'rxjs/Rx';

@Injectable()
export class ParadaService extends CrudService<ParadaDto> {

  private identityUrl: string = '';
  constructor(
      protected http: HttpClient,) {
      super(http);
      this.identityUrl = ENV.identityUrl + '/PlaParadas';
      this.endpoint = this.identityUrl;
  }

  GetPagedList(reqParams?: any): Observable<ResponseModel<PaginListResultDto<ParadaDto>>> {
    var url= this.endpoint + '/GetPagedList';     
    return this.http.post<ResponseModel<PaginListResultDto<ParadaDto>>>(url, reqParams );
  }

}