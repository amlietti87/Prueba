import { Dto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class TipoDiaDto extends Dto<number> {
    getDescription(): string {
        return this.DesTdia;
    }
    DesTdia: string;
    Activo: boolean;
    CopiaTipoDiaId: number;
    Color: string;
    Descripcion: string;
    Orden: number;
}
