import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { Numeric } from "d3";
import { GruposInspectoresDto } from "./gruposinspectores.model";
import { EstadosDiagramaInspectoresDto } from "./estadosdiagramainspectores.model";
import { ZonasDto } from "./zonas.model";
import { PersTurnosDto } from "./persturnos.model";
import { RangosHorariosDto } from "./rangoshorarios.model";


export class DiagramasInspectoresDto extends Dto<number> {
    getDescription(): string {
        return this.Mes.toString() + '-' + this.Anio.toString();
    }
    Mes: number;
    Anio: number;
    GrupoInspectoresId: number;
    EstadoDiagramaId: number;
    GrupoNombre: string;
    EstadoNombre: string;
    InspDiagramaInspectoresTurnos: InspDiagramaInspectoresTurnosDto[] = [];
    GrupoInspectores: GruposInspectoresDto;
    EstadoDiagrama: EstadosDiagramaInspectoresDto;
}

export class InspDiagramaInspectoresTurnosDto {
    DiagramaInspectoresId: number;
    TurnoId: number;
    Turno: PersTurnosDto;
}




export class DiagramasInspectoresFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    estadoDI: boolean;
    Mes: number;
    GruposInspectores: number;
    Anio: number;
    EstadoDiagrama: number;
}



export class DiagramaMesAnioDto {
    Estado: string;
    Mes: number;
    Anio: number;
    DescTurno: string;
    GrupoInspectoresId: number;
    GrupoInspectores: string;
    DiasMes: DiasMesDto[] = [];
    DiasMesAP: DiasMesDto[] = [];
    BlockDate: Date;
}


export class DiasMesDto {
    Fecha: Date;
    CodDia: number;
    NombreDia: string;
    NumeroDia: number;
    Color: string;
    EsFeriado: boolean;

    Inspectores: InspectorDiaDto[] = []
    BlockDate: Date;
}

export class InspectorDiaDto {
    ColSpan: number;
    Nombre: string;
    Apellido: string;
    CodEmpleado: string;
    DescripcionInspector: string;
    InspColor: string;
    InspTurno: string;
    InspTurnoId: number;
    Legajo: string;
    Id: string;
    EsJornada: boolean;
    CodJornada: number;
    EsFrancoTrabajado: boolean;
    CantFrancos: number;
    Pago: number;
    allowPagaCambia: boolean;
    EsFranco: boolean;
    EsNovedad: boolean;
    FrancoNovedad: boolean;
    RangoHorarioId: number;
    ZonaId: number;

    private _HoraDesde: Date;
    get HoraDesde(): Date {
        return this._HoraDesde;
    }
    set HoraDesde(value: Date) {
        this._HoraDesde = value;
    }

    private _HoraHasta: Date;
    get HoraHasta(): Date {
        return this._HoraHasta;
    }
    set HoraHasta(value: Date) {
        this._HoraHasta = value;
    }

    private _HoraDesdeModificada: Date;
    get HoraDesdeModificada(): Date {
        return this._HoraDesdeModificada;
    }
    set HoraDesdeModificada(value: Date) {
        this._HoraDesdeModificada = value;
    }

    private _HoraHastaModificada: Date;
    get HoraHastaModificada(): Date {
        return this._HoraHastaModificada;
    }
    set HoraHastaModificada(value: Date) {
        this._HoraHastaModificada = value;
    }


    HoraDesdeValue: any;
    HoraHastaValue: any;
    HoraDesdeModificadaValue: any;
    HoraHastaModificadaValue: any;
    Color: string;
    NombreZona: string;
    DetalleZona: string;
    NombreRangoHorario: string;
    DescNovedad: string;
    DetalleNovedad: string;

    PasadaSueldos: string;
    zonasItems: ZonasDto[];
    rangosItems: RangosHorariosDto[];
    diaMes: DiasMesDto;
    listModel: DiasMesDto[];
    diasMesAP: DiasMesDto[];
    diaMesFecha: Date;
    GrupoInspectoresId: number;
    validations: ValidationResult[] = [];
    EsModificada: boolean;
}

export enum EstadosDiagrama {
    Borrador = 1,
    Publicado = 2
}

export class ValidationResult {

    isValid: boolean = false;
    Messages: string[] = [];

}



