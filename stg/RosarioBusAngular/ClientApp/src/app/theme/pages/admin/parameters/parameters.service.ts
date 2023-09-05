import { ParametersDto, ParametersFilter } from "../model/parameters.model";
import { CrudService } from "../../../../shared/common/services/crud.service";
import { HttpClient } from "@angular/common/http";
import { AuthService } from "../../../../auth/auth.service";
import { environment } from "../../../../../environments/environment";
import { Injectable } from "@angular/core";

@Injectable()
export class ParametersService extends CrudService<ParametersDto> {

    parametersValues = {};
    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/SysParameters';
        this.endpoint = this.identityUrl;
    }


    private getParameters(token) {

        if (this.parametersValues[token] != null) {
            return new Promise<string>(resolve => resolve(this.parametersValues[token] as string));
        }

        var f = new ParametersFilter();

        f.Token = token;
        return this.requestAllByFilter(f.Token).toPromise().then(e => {
            if (e.DataObject.Items.length > 0) {
                this.parametersValues[token] = e.DataObject.Items[0].Value;
                return e.DataObject.Items[0].Value;
            }
            return null;
        });
    }

    public getParametroString(token) {
        return this.getParameters(token);
    }

    public getParametroNumeric(token) {
        return this.getParameters(token).then(e => {
            if (e != null)
                return Number.parseInt(e);
            else
                return null;
        });
    }


}