import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { ElementosDto } from "./elemento.model";


export class TipoElementoDto extends Dto<number> {
    getDescription(): string {
        return this.Nombre;
    }

    Nombre: string;
    CroElemeneto: ElementosDto[] = [];
}


export class TipoElementoFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
