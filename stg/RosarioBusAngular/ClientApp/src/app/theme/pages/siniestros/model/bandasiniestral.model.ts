import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class BandaSiniestralDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Anulado: boolean;
}


export class BandaSiniestralFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
