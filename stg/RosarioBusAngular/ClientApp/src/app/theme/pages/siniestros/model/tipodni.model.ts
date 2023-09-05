import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class TipoDniDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Anulado: boolean;
}


export class TipoDniFilter extends FilterDTO {
    Anulado: boolean;
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    TipoDocId: number;
}
