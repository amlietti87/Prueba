import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import { TareaCampoDto } from "./tarea-campo.model";


export class TareaDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
    Anulado : boolean;
    TareasCampos : TareaCampoDto[];

}

export class TareaFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    Descripcion : string;
    Anulado: boolean;
}