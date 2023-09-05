import { Dto, FilterDTO, ItemDto, GroupItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { LocalidadesDto } from "./localidad.model";
import { TipoDniDto } from "./tipodni.model";


export class PracticantesDto extends Dto<number> {
    getDescription(): string {
        return this.ApellidoNombre;
    }

    ApellidoNombre: string;
    TipoDocId: number;
    NroDoc: string;
    Celular: string;
    FechaNacimiento: Date;
    LocalidadId: number;
    selectLocalidades: ItemDto;
    Domicilio: string;
    Telefono: string;
    Anulado: boolean;
    TipoDoc: TipoDniDto;
}


export class PracticanteFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    selectLocalidades: ItemDto;
    Localidades: ItemDto[];
    LocalidadesSelect: LocalidadesDto[];
}
