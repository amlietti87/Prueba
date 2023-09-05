import { Dto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class TipoBanderaDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }
    Nombre: string

}
