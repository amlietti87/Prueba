import { Dto, FilterDTO, ItemDto, ItemDtoStr } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { Time } from "@angular/common";
import { EmpleadosDto } from "./empleado.model";
import { PracticantesDto } from "./practicantes.model";
import { CochesDto } from "./coche.model";
import { LineaDto } from "../../planificacion/model/linea.model";
import { CategoriasDto, ConsecuenciasDto } from "./consecuencias.model";
import { CausasDto } from "./causas.model";
import { SucursalDto } from "../../planificacion/model/sucursal.model";
import { EmpresaDto } from "../../planificacion/model/empresa.model";
import { InvolucradosDto } from "./involucrados.model";
import { SancionSugeridaDto } from "./sancionsugerida.model";
import { ReclamosDto } from "./reclamos.model";
import { SubEstadosDto } from "./estados.model";


export class SiniestrosDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    SucursalId: number;
    Fecha: Date;
    Hora: string;
    Dia: string;
    CocheId: string;
    EmpPract: string;
    CroquiId: number;
    ConductorId: number;
    PracticanteId: number;
    Lugar: string;
    Comentario: string;
    CostoReparacion: string;
    Responsable: boolean;
    Descargo: boolean;
    FechaDenuncia: Date;
    ConductorEmpresaId: number;
    EmpresaId: number;
    CocheLineaId: number;
    NroSiniestro: string;
    CausaId: number;
    SubCausaId: number;
    GenerarInforme: boolean;
    CodInforme: string;
    ObsDanios: string;
    ObsInterna: string;
    Latitud: number;
    Longitud: number;
    FactoresId: number;
    ConductaId: number;
    SeguroId: number;
    NroSiniestroSeguro: string;
    CocheInterno: string;
    CocheDominio: string;
    CocheFicha: number;
    ConductorLegajo: string;
    TipoDanioId: number;
    TipoColisionId: number;
    Anulado: boolean;
    SiniestrosConsecuencias: SiniestrosConsecuenciasDto[];
    Localidad: string;
    Direccion: string;
    EmpPractConId: number;
    InformeConId: number;
    DescargoConId: number;
    ResponsableConId: number;
    NombreConductor: string;
    DniConductor: string;
    CuilEmpleado: string;
    InformaTaller: boolean;
    SancionSugeridaId: number;
    SancionSugerida: SancionSugeridaDto;
    selectEmpleados: ItemDto;
    selectPracticantes: ItemDto;
    selectCoches: ItemDtoStr;
    selectLineas: ItemDto;
    Causa: CausasDto;
    Sucursal: SucursalDto;
    Empresa: EmpresaDto;
    ConductorEmpresa: EmpresaDto;
    Empleado: EmpleadosDto;
    CocheLinea: LineaDto;
    Practicante: PracticantesDto;
    nro_informe: string;
    Reclamos: ReclamosDto[] = [];
    GrillaConductor: GrillaConductor;
    CreatedUserName: string;
    ApellidoInvolucrado: string;
    PrimerConsecuencia: string;
    EstadoDeReclamos: string;

    DescripcionLinea: string;
    DescripcionSucursal: string;

    ActualizarConductor: boolean = false;
}

export class SiniestroHistorialEmpleado extends Dto<number> {
    getDescription(): string {
        return '';
    }

    UltimoAnio: number;
    Total: number;
    Practicante: number;
}

export class GrillaConductor extends Dto<string> {
    getDescription(): string {
        return '';
    }

    NombreConductor: string;
    Legajo: string;
    NroDoc: string;
}

export class SiniestrosConsecuenciasDto extends Dto<number> {
    Id: number;

    SiniestroId: number;

    ConsecuenciaId: number;
    ConsecuenciaNombre: string;
    Consecuencia: ConsecuenciasDto;

    Observaciones: string;

    CategoriaId: number;
    CategoriaNombre: string;
    Categoria: CategoriasDto;

    Categorias: CategoriasDto[];

    Cantidad: number;

    getDescription(): string {
        return 'err';
    }
}



export class SiniestrosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    SucursalId: number;
    NroSiniestro: string;
    FechaSiniestroDesde: Date;
    FechaSiniestroHasta: Date;
    FechaDenunciaDesde: Date;
    FechaDenunciaHasta: Date;
    Ficha: number;
    EmpresaId: number;
    Linea: number;
    Interno: string;
    Dominio: string;
    PracticanteId: number;
    ConductorId: number;
    ResponsableConId: number;
    DescargoConId: number;
    Ubicacion: string;
    DominioInvolucrado: string;
    NroDocInvolucrado: string;
    ApellidoInvolucrado: string;
    EstadoReclamo: number;
    selectEmpleados: ItemDto;
    selectPracticantes: ItemDto;
    selectLineas: ItemDto;
    selectCoches: ItemDtoStr;
    Empleados: ItemDto[];
    Practicantes: ItemDto[];
    Coches: ItemDtoStr[];
    Lineas: ItemDto[];
    EmpleadosSelect: EmpleadosDto[];
    PracticantesSelect: PracticantesDto[];
    CochesSelect: CochesDto[];
    LineasSelect: LineaDto[];
    Consecuencias: number[];
    SubEstadoReclamo: number[];
    CheckAllConsecuencias: boolean;
    CausaId: number;
}

export class SiniestroMapDto extends Dto<string> {
    getDescription(): string {
        return this.Nombre;
    }
    Nombre: string;
    Lat: number;
    Long: number;
    SiniestroId: number;
    constructor(data?: any) {
        super(data);
    }
}
