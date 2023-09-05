import { Dto } from "../../../../shared/model/base.model";
import { TareaDto } from "./tarea.model";


export class TareaCampoConfigDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    
    Campo: string;
    Descripcion: string;   
}