import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { TipoElementoDto } from "./tipoelemento.model";


export class ElementosDto extends Dto<string> {
    getDescription(): string {
        return this.Nombre;
    }
    TipoId: number;
    TipoElementoId: number;
    Descripcion: string;
    Nombre: string;
    DescripcionTipoElemento: string;

}


export class ElementosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}


export class CroTipoDto extends Dto<number> {
    getDescription(): string {
        return this.Nombre;
    }
    Nombre: string;

}

export class CroTipoFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
