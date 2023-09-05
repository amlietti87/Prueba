import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class CoordenadasDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }
    Abreviacion: string;
    CodigoNombre: string;
    Lat: number;
    Long: number;

    Calle1: string;
    Calle2: string;
    Descripcion: string;
    DescripcionCalle1: string;
    DescripcionCalle2: string;
    selectLocalidades: ItemDto;
    LocalidadId: number;
    Localidad: string;
    NumeroExternoIVU: string;
    Anulado: boolean;
}



export class CoordenadasFilter extends FilterDTO {
    Abreviacion: string;
    CodigoNombre: string;
    Lat: number;
    Long: number;
    Calle1: string;
    Calle2: string;
    DescripcionCalle1: string;
    DescripcionCalle2: string;
    AnuladoCombo: number;
}