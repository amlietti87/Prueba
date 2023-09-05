import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { RbMapMarker } from "../../../../components/rbmaps/RbMapMarker";
import { Numeric } from "d3";


export class SectorFilter extends FilterDTO {
    Id: string;
    Page: number;
    PageSize: number;
    Sort: String;
    RutaId: number;
}


export class SectorDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
    PuntoInicioId: string;
    PuntoFinId: string;
    Data: string;
    DistanciaKm: number;
    Duration: number;
    RutaId: number;
    Color: string;

    OrdenInicio: number;
    OrdenFin: number;
}

export class SectorViewDto extends Dto<number> {
    getDescription(): string {
        return this.desc;
    }
    value: number;
    name: string;
    EsCambioSector: boolean;
    desc: string;
    p: string;
    background: string;
    linecolor: string;
    lineWidth: number;
    radius: number;
    stroke: string;
    Items: ItemSectorViewDto[];



}


export class ItemSectorViewDto extends Dto<number> {
    getDescription(): string {
        return this.desc;
    }
    value: number;
    name: string;
    desc: string;

    NumeroExterno: string;
    Abreviacion: string;
    CodigoNombre: string;

}

export class RutaSectoresDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }
    Description: string;
    BanderaId: number;
    Sectores: SectorViewDto[];
}

