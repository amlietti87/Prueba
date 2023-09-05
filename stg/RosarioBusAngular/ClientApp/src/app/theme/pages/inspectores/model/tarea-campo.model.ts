import { Dto } from "../../../../shared/model/base.model";
import { TareaDto } from "./tarea.model";


export class TareaCampoDto extends Dto<number> {
    getDescription(): string {
        return this.Etiqueta;
    }
    TareaId : number;
    TareaCampoConfigId : number
    NombreTareaCampo : string
    Etiqueta: string
    Requerido : boolean;
    Orden : number;
    Tarea : TareaDto;
}