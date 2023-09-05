import { Dto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class SentidoBanderaDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }
    Descripcion: string;
    Color: string;

}

