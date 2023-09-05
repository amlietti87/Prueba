import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { EmpresaDto } from "./empresa.model";
import { SucursalDto } from "./sucursal.model";
import { LineaDto } from "./linea.model";


export class SubGalponDto extends Dto<number> {


    DesSubg: string;
    FecBaja: Date;
    Balanceo: string;
    FltComodines: string;
    Configu: ConfiguDto[] = [];

    BalanceoCheck: boolean;
    ComodinesCheck: boolean;
    getDescription(): string {
        return this.DesSubg;
    }



    constructor(data?: any) {
        super(data);
    }
}

export class ConfiguDto extends Dto<number> {

    CodGru: number;
    CodEmpr: number;
    CodSuc: number;
    CodLin: number;
    CodGal: number;
    CodSubg: number;
    PlanCam: number;
    FecBaja: Date;
    CodSucCast: number;

    Grupo: GruposDto;
    Empresa: EmpresaDto;
    Sucursal: SucursalDto;
    Linea: LineaDto;
    Galpon: GalponDto;
    PlanCamNav: PlanCamDto;


    selectLinea: ItemDto;
    GrupoGrilla: string;
    EmpresaGrilla: string;
    SucursalGrilla: string;
    GalponGrilla: string;
    PlanCamNavGrilla: string;
    getDescription(): string {
        return "";
    }

    constructor(data?: any) {
        super(data);
    }
}


export class GruposDto extends Dto<number> {

    DesGru: string;
    FecBaja: Date;


    getDescription(): string {
        return "";
    }

    constructor(data?: any) {
        super(data);
    }
}

export class GalponDto extends Dto<number> {
    Nombre: string;
    DesGal: string;
    DomGal: string;
    PosGal: string;
    TelGal: string;
    EncGal: string;
    HoraCorte: Date;
    Latitud: number;
    Longitud: number;
    Radio: number;
    PtoPedido: number;
    IdSap: string;
    IdSapRepuestos: string;
    IdentificadorMapa: string;


    getDescription(): string {
        return this.Nombre;
    }

    constructor(data?: any) {
        super(data);
    }
}

export class PlanCamDto extends Dto<number> {

    DesPlan: string;
    Depot: string;
    Total: number;
    Fecha: Date;


    getDescription(): string {
        return "";
    }

    constructor(data?: any) {
        super(data);
    }
}


export class SubGalponFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
} 
