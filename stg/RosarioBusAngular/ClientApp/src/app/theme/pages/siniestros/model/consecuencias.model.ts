import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class ConsecuenciasDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Adicional: boolean;
    Anulado: boolean;
    Responsable: boolean;
    Categorias: CategoriasDto[];
    CategoriaElegida: number;
    CategoriaNombre: string;
    Observaciones: string;
    SinConsecuenciaId: number;
}


export class ConsecuenciasFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}


export class CategoriasDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    ConsecuenciaId: number;
    InfoAdicional: string;
    Anulado: boolean;
    IsSelected: boolean;
}
