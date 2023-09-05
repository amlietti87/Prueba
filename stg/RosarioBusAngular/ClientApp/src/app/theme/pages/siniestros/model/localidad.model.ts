import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class LocalidadesDto extends Dto<number> {
    getDescription(): string {
        return this.DscLocalidad;
    }
    DscLocalidad: string;
    CodPostal: string;
    CodProvincia: number;
    ProvinciaName: string;
}


export class LocalidadesFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}

export class ProvinciasDto extends Dto<number> {
    getDescription(): string {
        return this.DscProvincia;
    }

    DscProvincia: string;
}


export class ProvinciasFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
