import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class CausasDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Anulado: boolean;
    Responsable: boolean;
    SubCausas: SubCausasDto[] = [];
}


export class CausasFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}


export class SubCausasDto extends Dto<number> {

    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    CausaId: number;
    Anulado: boolean;
}
