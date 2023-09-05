import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class ParadaDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }

    Codigo: string;
    Calle: string;
    Cruce: string;
    LocalidadId: number;
    Sentido: string;
    Lat: number;
    Long: number;
    Anulada: boolean;
    Localidad: string;
    PickUpType: boolean;
    DropOffType: boolean;
    LocationType: number;
    ParentStationId: number;
    TipoParadaDesc: string;
    ParentStation: ItemDto;
    selectLocalidades: ItemDto;

}



export class ParadaFilter extends FilterDTO {
    Codigo: string;
    Calle: string;
    Cruce: string;
    LocalidadId: number;
    Sentido: string;
    Lat: number;
    Long: number;
    Anulada: boolean;
    AnuladoCombo: number;
    location_type: number;

}