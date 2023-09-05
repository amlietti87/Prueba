import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/Rx';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import { environment } from '../../../../../environments/environment';
import { HTposHorasDto } from '../model/htposhoras.model';

@Injectable()
export class HTposHorasService extends CrudService<HTposHorasDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,

    ) {
        super(http);
        this.identityUrl = environment.identityUrl + '/htposhoras';
        this.endpoint = this.identityUrl;
    }

}