import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { SucursalDto } from "../../planificacion/model/sucursal.model";
import { EmpresaDto } from "../../planificacion/model/empresa.model";
import { ContingenciasDto } from "./contingencias.model";
import { PatologiasDto } from "./patologias.model";
import { PrestadoresMedicosDto } from "./prestadoresmedicos.model";
import { TratamientosDto } from "./tratamientos.model";
import { MotivosAudienciasDto } from "./motivosaudencias.model";
import { DenunciaNotificacionesDto } from "./denunciasnotificaciones.model";
import { ReingresosDto } from "./reingresos.model";
import { SiniestrosDto } from "../../siniestros/model/siniestro.model";


export class DenunciasDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }

    NroDenuncia: string;
    SucursalId: number;
    EmpresaId: number;
    EmpleadoId: number;
    EmpleadoFechaIngreso: Date;
    EmpleadoEmpresaId: number;
    EmpleadoLegajo: string;
    EmpleadoAntiguedad: Date;
    EmpleadoArea: string;
    FechaOcurrencia: Date;
    FechaRecepcionDenuncia: Date;
    ContingenciaId: number;
    Diagnostico: string;
    PatologiaId: number;
    PrestadorMedicoId: number;
    BajaServicio: boolean;
    FechaBajaServicio: Date;
    TratamientoId: number;
    FechaUltimoControl: Date;
    FechaProximaConsulta: Date;
    FechaAudienciaMedica: Date;
    MotivoAudienciaId: number;
    PorcentajeIncapacidad: number;
    AltaMedica: boolean;
    FechaAltaMedica: Date;
    AltaLaboral: boolean;
    FechaAltaLaboral: Date;
    TieneReingresos: boolean;
    DenunciaIdOrigen: number;
    SiniestroId: number;
    Juicio: boolean;
    Observaciones: string;
    Anulado: boolean;
    EstadoId: number;
    NombreEmpleado: string;
    CantidadDiasBaja: number;
    MotivoAnulado: string;
    EmpleadoGrilla: string;
    DniEmpleado: string;
    FechaProbableAlta: Date;
    Color: string;
    //custom properties
    selectedEmpleado: ItemDto;
    selectedSiniestro: ItemDto;
    CreatedUserName: string;
    SucursalGrilla: string;
    EmpresaGrilla: string;
    Contingencia: ContingenciasDto;
    Patologia: PatologiasDto;
    PrestadorMedico: PrestadoresMedicosDto;
    Tratamiento: TratamientosDto;
    Siniestro: SiniestrosDto;
    MotivoAudiencia: MotivosAudienciasDto;
    DenunciaNotificaciones: DenunciaNotificacionesDto[] = [];
    ActualizarConductor: boolean = false;
}


export class DenunciasFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    NroDenuncia: string;
    selectEmpleados: ItemDto;
    Empleados: ItemDto[];
    SucursalId: number;
    EmpresaId: number;
    FechaOcurrenciaDesde: Date;
    FechaOcurrenciaHasta: Date;
    FechaDenunciaDesde: Date;
    FechaDenunciaHasta: Date;
    FechaUltimoControlDesde: Date;
    FechaUltimoControlHasta: Date;
    FechaProxConsultaDesde: Date;
    FechaProxConsultaHasta: Date;
    EstadoDenuncia: number;
    ContingenciaId: number;
    PatologiaId: number;
    PrestadorMedicoId: number;
    BajaServicio: number;
    TratamientoId: number;
    FechaAudienciaDesde: Date;
    FechaAudienciaHasta: Date;
    AltaLaboral: number;
    AltaMedica: number;
    TieneReingresos: number;
    DenunciaIdOrigen: number;
    MotivoAudienciaId: number;
    Siniestro: string;
    Juicio: number;
    Anulado: number;
    NotId: number;


}
export class ExcelDenunciasFilter {
    Ids: string;
}

