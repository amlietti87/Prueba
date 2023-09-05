import { Dto, FilterDTO, ItemDto, GroupItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { Numeric } from "d3";
import { PlaLineaLineaHorariaDto } from "./linea_lineahoraria.model";


export class LineaDto extends Dto<number> {
    getDescription(): string {
        return this.DesLin;
    }
    DesLin: string
    SucursalId: number
    Sucursal: string
    PlaTipoLineaId: number
    TipoLinea: string
    Activo: boolean
    UrbInter: string
    PlaLineaLineaHoraria: PlaLineaLineaHorariaDto[] = [];
    CodRespInformes: string

}


export class LineaFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    SucursalId: number;
    Activo: boolean;
    EmpresaId: number;
    TipoLineaId: number;
    GrupoLineaId: number;
}



export class LineasFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    SucursalId: number;
    selectLinea: ItemDto;
    Lineas: ItemDto[];
    BanderasPosicionamiento: BanderaItemLongDto[];
    BanderasComerciales: BanderaItemLongDto[];
    Ramales: GroupItemDto[];
    RamalesSelect: GroupItemDto[];
    selectBanderaPosicionamiento: any;
    selectBanderaComercial: any;
    selectRamal: any;
}





export class RutasViewFilter extends FilterDTO {
    Original: boolean;
    Vigente: boolean;
    BanderasId: number[];
    Sucursalid: number;
    FechaDesde: Date;
    FechaHasta: Date;
    EstadoRecorrido: number;
}

export class RutasFilteredFilter extends FilterDTO {
    BanderaId: number;
    FechaDesde: Date;
    FechaHasta: Date;
    CodHFecha: number;
    CodRec: number;
    BanderaDetalle: string;

}



export class BanderaItemLongDto extends ItemDto {
    RamaColorId: number;
    RamaColor_Nombre: string;
    LineaNombre: string;
}


