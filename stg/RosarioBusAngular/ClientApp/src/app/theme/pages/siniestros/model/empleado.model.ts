import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class EmpleadosDto extends Dto<number> {
    getDescription(): string {
        return this.Apellido + ' ' + this.Nombre;
    }
    Cuil: string;
    Apellido: string;
    Nombre: string;
    Dni: string;
    Telefono: string;
    Area: string;
    Categoria: string;
    Convenio: string;
    CodLinea: number;
    FecVacaciones: Date;
    FecAntiguedad: Date;
    Jubilado: boolean;
    GestionTiempoReal: boolean;
    CalleDomicilio: string;
    NroDomicilio: string;
    BlockDomicilio: string;
    PisoDomicilio: string;
    DeptoDomicilio: string;
    CodLocalidad: number;
    FecProcesado: Date;
    Pin: string;
    CodObraSocial: string;
    ObraSocial: string;
    FecNacimiento: Date;
    FecProbJubilacion: Date;
    AportesAntPrivilegiados: number;
    AportesAntSimples: number;
    ConvColectivo: string;
    IntimadoJubilarse: boolean;
    ObsJubilacion: string;
    IdLector: string;
    Un: string;
    LatDomicilio: number;
    LonDomicilio: number;
    Sexo: string;
    cod_sucursal: number;
}


export class LegajosEmpleadoDto extends Dto<number> {
    getDescription(): string {
        return '';
    }

    FecIngreso: Date;
    LegajoSap: string;
    FecBaja: Date;
    CodEmpresa: number;
    FecProcesado: Date;
}

export class EmpleadosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}


