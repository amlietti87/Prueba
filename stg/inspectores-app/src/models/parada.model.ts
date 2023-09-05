import { Dto, FilterDTO, ItemGenericDto } from "./Base/base.model";

export class ParadaDto extends Dto<number> {
  getDescription(): string {
      return this.Description;
  }
  
  Id: number;

  Codigo: string 
  Calle: string 
  Cruce: string 
  Localidad: string

}

export class PlaParadaFilter extends FilterDTO {    
  
  Fecha: string;
  LineaId: number;
  SoloParadasAsociadasALineas: boolean;
  FilterText: string

}
