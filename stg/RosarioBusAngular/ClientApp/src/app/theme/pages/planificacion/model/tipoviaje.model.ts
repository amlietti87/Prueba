import { Dto } from '../../../../shared/model/base.model';
import * as moment from 'moment';

export class TipoViajeDto extends Dto<number> {
    getDescription(): string {
        throw new Error("Method not implemented.");
    }

    Id: number;
    TravelMode: string;
    Descripcion: string;

}