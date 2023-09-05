import { LineaDto } from './../../planificacion/model/linea.model';

import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { ZonasDto } from "./zonas.model";
import { RangosHorariosDto } from "./rangoshorarios.model";
import { TiposTareaDto } from "./tipostarea.model";
import { PersTurnosDto } from "./persturnos.model";
import { TareaDto } from './tarea.model';


export class GruposInspectoresDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
    Anulado: boolean;
    NotificacionId: number;
    LineaId: number;
    Linea: LineaDto;
    InspGrupoInspectoresZona: InspGrupoInspectoresZonaDto[];
    InspGrupoInspectoresRangosHorarios: InspGrupoInspectoresRangosHorariosDto[];
    InspGrupoInspectoresTurnos: InspGrupoInspectoresTurnoDto[];
    InspGrupoInspectoresTareas: InspGrupoInspectoresTareasDto[];

}

export class GruposInspectoresFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    Anulado: number;
}

export class InspGrupoInspectoresZonaDto extends Dto<number>
{
    Id: number;

    GrupoInspectoresId: number;
    ZonaId: number;
    Zona: ZonasDto;
    ZonaNombre: string;

    getDescription(): string {
        return 'err';
    }
}

export class InspGrupoInspectoresRangosHorariosDto extends Dto<number>
{
    Id: number;

    GrupoInspectoresId: number;
    RangoHorarioId: number;
    RangoHorario: RangosHorariosDto;
    NombreRangoHorario: string;

    getDescription(): string {
        return 'err';
    }
}

export class InspGrupoInspectoresTareasDto extends Dto<number>
{
    Id: number;

    GrupoInspectoresId: number;
    TareaId: number;
    Tarea: TareaDto;
    TareaNombre: string;

    getDescription(): string {
        return 'err';
    }
}

export class InspGrupoInspectoresTurnoDto extends Dto<number>
{
    Id: number;

    GrupoInspectoresId: number;
    TurnoId: number;
    Turno: PersTurnosDto;
    TurnoNombre: string;

    getDescription(): string {
        return 'err';
    }
}