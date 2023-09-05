import { Dto, FilterDTO, ItemDto, ItemDtoStr } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { ConductorDto } from "./conductor.model";
import { VehiculoDto } from "./vehiculo.model";
import { LesionadoDto } from "./lesionado.model";
import { AbogadosDto } from "./abogados.model";
import { EstadosDto, SubEstadosDto } from "./estados.model";
import { InvolucradosDto } from "./involucrados.model";
import { JuzgadosDto } from "./juzgados.model";
import { TiposReclamoDto } from "../../reclamos/model/tiposreclamo.model";
import { EmpleadosDto } from "./empleado.model";
import { SucursalDto } from "../../planificacion/model/sucursal.model";
import { EmpresaDto } from "../../planificacion/model/empresa.model";


export class ReclamosDto extends Dto<number> {
    getDescription(): string {
        return this.NroExpediente;
    }

    constructor(data?: any) {
        super(data);

    }
    TipoReclamoId: number;
    InvolucradoId: number;
    SiniestroId: number;
    EmpresaId: number;
    EmpleadoFechaIngreso: Date;
    EmpleadoEmpresaId: number;
    EmpleadoLegajo: string;
    EmpleadoAntiguedad: Date;
    EmpleadoArea: string;
    SucursalId: number;
    Fecha: Date;
    EstadoId: number;
    SubEstadoId: number;
    DenunciaId: number;
    MontoDemandado: number;
    FechaOfrecimiento: Date;
    MontoOfrecido: number;
    MontoReconsideracion: number;
    Cuotas: boolean;
    FechaPago: Date;
    MontoPagado: number;
    MontoFranquicia: number;
    AbogadoId: number;
    MontoHonorariosAbogado: number;
    MontoHonorariosMediador: number;
    MontoHonorariosPerito: number;
    MontoTasasJudiciales: number;
    JuntaMedica: boolean;
    MotivoAnulado: string;
    PorcentajeIncapacidad: number;
    Observaciones: string;
    ObsConvenioExtrajudicial: string;
    Autos: string;
    NroExpediente: string;
    JuzgadoId: number;
    AbogadoEmpresaId: number;
    EstadoSelected: EstadosItemDto;
    Estado: EstadosDto;
    Involucrado: InvolucradosDto;
    SubEstado: SubEstadosDto;
    ReclamoCuotas: ReclamoCuotasDto[] = [];
    Anulado: boolean;
    EmpleadoId: number;
    CausaId: number;
    TipoAcuerdoId: number;
    RubroSalarialId: number;
    Hechos: string;
    Sucursal: SucursalDto;
    Empresa: EmpresaDto;
    TipoReclamo: TiposReclamoDto;
    EmpleadoGrilla: string;
    InvolucradoGrilla: string;
    itemsHistorial: ReclamosHistoricosDto[] = [];
    InvolucradoName: string;
    AbogadoName: string;
    EstadoName: string;
    SubEstadoName: string;
    selectedEmpleado: ItemDto;
    selectedSiniestro: ItemDto;
    JudicialSelected: boolean;


    ReclamoHistorico: string;
    EnableADet: boolean;
    AccessFromSiniestros: boolean;
    ActualizarConductor: boolean = false;
}

export class ReclamosHistoricosDto extends ReclamosDto {
    getDescription(): string {
        return this.NroExpediente;
    }


    ReclamoId: number;
}

export class ReclamosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    EstadoId: number;
    SubEstadoId: number;
    Dominio: string;
    TipoDocumentoId: number;
    Documento: string;
    Apellido: string;
    SiniestroId: number;
    TipoInvolucradoId: number;
    SucursalId: number;
    EmpresaId: number;
    AnuladoCombo: number;
    selectEmpleados: ItemDto;
    FechaReclamoDesde: Date;
    FechaReclamoHasta: Date;
    TipoReclamoId: number;
    NroDenuncia: string;
    NroSiniestro: string;
    FechaPagoDesde: Date;
    FechaPagoHasta: Date;
    AbogadoId: number;
    SubEstadoReclamo: number[];
    InvolucradoId: number;
}

export class ReclamosHistoricosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    ReclamoId: number;
}


export class ReclamoCuotasDto extends Dto<number>  {
    ReclamoId: number;
    FechaVencimiento: Date;
    Monto: number;
    Concepto: string;

    getDescription(): string {
        return this.Concepto;
    }
}

export class EstadosItemDto extends Dto<string> {


    getDescription(): string {
        return this.Description;
    }
    Id: string;
    Description: string;
    IsSelected: boolean;
    animate: boolean;
    Judicial: boolean;

    constructor(data?: any) {
        super(data);
        this.animate = false;
    }
}