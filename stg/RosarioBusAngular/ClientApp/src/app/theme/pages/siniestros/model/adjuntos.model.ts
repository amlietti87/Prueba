import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class AdjuntosDto extends Dto<string> {
    getDescription(): string {
        return this.Nombre;
    }

    Nombre: string;
    //Archivo: ArrayBuffer;
    Parent: any;
}


export class AdjuntosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
