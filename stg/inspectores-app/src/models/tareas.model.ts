import { Dto, FilterDTO } from "./Base/base.model";


export class TareasDto extends Dto<number> {
  getDescription(): string {
      return this.Description;
  }
  Description: string
  TareasCampos: TareasCamposDto[] = []
  isValid: boolean;

}

export class TareasFilter extends FilterDTO {    
  Anulado: boolean;
}

export class TareasCamposDto extends Dto<number>  {
  getDescription(): string {
    return this.NombreTareaCampo;
}
  TareaId: number
  TareaCampoConfigId: number
  Etiqueta: string
  Requerido: boolean
  Orden: number
  NombreTareaCampo: string
  Campo: string

}

//Entidad a guardar en bd
export class TareasRealizadasDto extends Dto<number>{
  getDescription(): string {
    return '';
  }

  FechaHora: Date
  Fecha: Date
  FechaString: string
  EmpleadoId: number
  SucursalId: number
  TareaId: number
  TareasRealizadasDetalle: TareasRealizadasDetalle []

} 

export class TareasRealizadasDetalle {

  TareaRealizadaId: number
  TareaCampoId: number

  Valor: any
   
  
  PosiblesCampos: TareasCamposDto[] = []
  

}

