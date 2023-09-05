import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class RubrosSalarialesDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Anulado: boolean;
}


export class RubrosSalarialesFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
