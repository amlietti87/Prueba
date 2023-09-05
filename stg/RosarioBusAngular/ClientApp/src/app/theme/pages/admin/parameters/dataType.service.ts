import { DataTypeDto } from "../model/dataType.model";
import { CrudService } from "../../../../shared/common/services/crud.service";
import { HttpClient } from "@angular/common/http";
import { AuthService } from "../../../../auth/auth.service";
import { environment } from "../../../../../environments/environment";
import { Injectable } from "@angular/core";

@Injectable()
export class DataTypeService extends CrudService<DataTypeDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/SysDataTypes';
        this.endpoint = this.identityUrl;
    }
}