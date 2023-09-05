import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class DescargoDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
}

