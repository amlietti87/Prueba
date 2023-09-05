import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import 'rxjs/Rx';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { HKilometrosDto } from '../model/hkilometros.model';


@Injectable()
export class HKilometrosService extends CrudService<HKilometrosDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService
    ) {
        super(http);
        this.identityUrl = environment.identityUrl + '/hkilometros';
        this.endpoint = this.identityUrl;
    }
}

