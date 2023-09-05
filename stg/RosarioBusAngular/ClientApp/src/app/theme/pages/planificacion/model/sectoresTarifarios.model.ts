import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class SectoresTarifariosDto extends Dto<number> {
    DscSectorTarifario: string;
    CodManualSectorTarifario: number;
    DscCompleta: string;
    Nacional?: boolean;

    getDescription(): string {
        return this.DscSectorTarifario;
    }
}

export class SectoresTarifariosFilter extends FilterDTO {
    DscSectorTarifario: string
}