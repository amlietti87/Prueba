import { Dto, FilterDTO, ItemDto, GroupItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { LocalidadesDto } from "./localidad.model";


export class ConductorDto extends Dto<number> {
    getDescription(): string {
        return this.ApellidoNombre + ' ' + this.NroDoc
    }

    ApellidoNombre: string;
    TipoDocId: number;
    NroDoc: string;
    FechaNacimiento: string;
    NroLicencia: string;
    Domicilio: string;
    LocalidadId: number;
    selectLocalidades: ItemDto;
    Telefono: string;
    Celular: string;
}

