import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import { ConductorDto } from "./conductor.model";
import { VehiculoDto } from "./vehiculo.model";
import { LesionadoDto } from "./lesionado.model";
import { TipoInvolucradoDto } from "./tipoinvolucrado.model";


export class InvolucradosDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }

    SiniestroId: number;
    TipoInvolucradoId: number;
    NroInvolucrado: string;
    ApellidoNombre: string;
    TipoDocId: number;
    NroDoc: string;
    FechaNacimiento: Date;
    LocalidadId: number;
    Domicilio: string;
    Telefono: string;
    Celular: string;
    Detalle: string;

    ConductorId: number;
    VehiculoId: number;
    LesionadoId: number;
    MuebleInmuebleId: number;

    TipoInvolucrado: TipoInvolucradoDto = new TipoInvolucradoDto(null);

    Conductor: ConductorDto = new ConductorDto(null);
    Vehiculo: VehiculoDto = new VehiculoDto(null);
    Lesionado: LesionadoDto = new LesionadoDto(null);
    MuebleInmueble: MuebleInmuebleDto = new MuebleInmuebleDto(null);

    DetalleLesion: DetalleLesionDto[] = [];

    DescripcionInv: string;
    NroInvolucradoPuro: number;
    CantidadDeSiniestros: number;
    TipoInvolucradoNombre: string;
    TipoDocNombre: string;
    VehiculoNombre: string;
    MuebleInmuebleNombre: string;
    LesionadoNombre: string;
    ConductorNombre: string;
    InvolucradoColumn: string;

    selectLocalidades: ItemDto;

    TipoLesionadoId: number;

    EstadoInsercion: string;

}

export class InvolucradosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    TipoInvolucradoId: number;
    Dominio: string;
    TipoDocumentoId: number;
    Documento: string;
    Apellido: string;
    Domicilio: string;
    SiniestroID: number;
}

export class DetalleLesionDto extends Dto<number> {
    getDescription(): string {
        return this.LugarAtencion;
    }

    InvolucradoId: number;
    LugarAtencion: string;
    FechaFactura: Date;
    Observaciones: string;
    NroFactura: string;
    ImporteFactura: number;
}

export class MuebleInmuebleDto extends Dto<number> {
    getDescription(): string {
        return this.Lugar + ' ' + this.selectLocalidades != null ? this.selectLocalidades.Description : "";
    }

    TipoInmuebleId: number;
    Lugar: string;
    LocalidadId: number;

    selectLocalidades: ItemDto;
}

export class HistorialInvolucrados extends Dto<number> {
    getDescription(): string {
        return '';
    }

    Tercero: number;
    Conductor: number;
    Lesionado: number;
    Testigo: number;
    Titular: number;

}