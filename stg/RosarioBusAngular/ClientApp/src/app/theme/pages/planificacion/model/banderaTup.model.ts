import { Dto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class BanderaTupDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }

    descripcion: string;
    Descripcion: string;

}
