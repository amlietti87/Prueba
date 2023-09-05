import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';

export class NovedadesDto extends Dto<number> {

    getDescription(): string {
        return this.DesNov;
    }

    DesNov: string
    HorSue: string;
    Variable: string;
    PorHora: string;
    FranNov: string;
    AbrNov: string;
    FecHastaEditable: string;
    ClaseAusensia: string;
}

export class NovedadesFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}