import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { DiasMesDto, ValidationResult, InspectorDiaDto, DiagramasInspectoresDto, InspDiagramaInspectoresTurnosDto, DiagramaMesAnioDto } from '../model/diagramasinspectores.model';
import { PersTopesHorasExtrasService } from './pers-topes-horas-extras.service';
import { PersTopesHorasExtrasDto } from '../model/pers-topes-horas-extras.model';
import * as moment from 'moment';
import { PersTurnosDto } from '../model/persturnos.model';
import { DiagramasInspectoresService } from './diagramasinspectores.service';
import { Observer } from 'rxjs';
import { ResponseModel } from '../../../../shared/model/base.model';
import { Observable } from 'rxjs/Observable';
import { json } from 'd3';


@Injectable()
export class DiagramasInspectoresValidatorService {


    topes: PersTopesHorasExtrasDto[];

    turnosDiagrama: InspDiagramaInspectoresTurnosDto[] = [];

    diagramacion: DiagramaMesAnioDto = null;

    constructor(private topesService: PersTopesHorasExtrasService, protected _DInspectoresService: DiagramasInspectoresService) {
        topesService.requestAllByFilter({}).subscribe(e => this.topes = e.DataObject.Items);

    }


    ValidateHorasFrancoTrabajadoParaInspector(codEmpleado: InspectorDiaDto, listModel: DiasMesDto[], diasAP: DiasMesDto[]): ValidationResult {
        let result: ValidationResult = new ValidationResult();
        result.isValid = true;
        let cantidadMinutos = 0;
        let grupoId = listModel[0].Inspectores[0].GrupoInspectoresId;
        listModel.forEach(e => {
            let row = e;
            let celda = e.Inspectores.find(e => e.CodEmpleado == codEmpleado.CodEmpleado);
            if (codEmpleado.Id == celda.Id) {
                if ((codEmpleado.EsFranco || codEmpleado.EsFrancoTrabajado) && codEmpleado.Pago != 0) {
                    let horaDesdeMoment = moment(codEmpleado.HoraDesdeModificada);
                    let fecha = moment(codEmpleado.HoraDesdeModificada);
                    let horaHastaMoment = moment(codEmpleado.HoraHastaModificada);
                    let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                    cantidadMinutos = cantidadMinutos + cantMinutosTrabajadas;
                }
            }
            else {
                if (celda.EsFrancoTrabajado && celda.Pago != 0) {
                    let horaDesdeMoment = moment(celda.HoraDesdeModificada);
                    let fecha = moment(celda.HoraDesdeModificada);
                    let horaHastaMoment = moment(celda.HoraHastaModificada);
                    let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute")
                    cantidadMinutos = cantidadMinutos + cantMinutosTrabajadas;
                }
            }
        });

        let tope = this.topes.find(e => e.IdGrupoInspectores == grupoId);
        if (tope) {
            let topeEnMinutos = tope.FrancosTrabajadosPersona * 60;
            if (cantidadMinutos > topeEnMinutos) {
                result.isValid = false;
                result.Messages.push("Supera el tope de horas maxima definidas para Francos Trabajados para el inspector");
            }
        }
        return result;

    }

    ValidateHorasFrancoTrabajadoPorGrupo(codEmpleado: InspectorDiaDto, listModel: DiasMesDto[], diasAP: DiasMesDto[]): ValidationResult {
        let result: ValidationResult = new ValidationResult();
        result.isValid = true;
        let cantidadMinutos = 0;
        let grupoId = listModel[0].Inspectores[0].GrupoInspectoresId;

        // recupera la cantidad de minutos  de la diagramación para los francos trabajados para los turnos que fueron seleccionados

        listModel.forEach(e => {
            let row = e;
            e.Inspectores.forEach(celda => {
                if (codEmpleado.Id == celda.Id) {
                    if ((codEmpleado.EsFranco || codEmpleado.EsFrancoTrabajado) && codEmpleado.Pago != 0) {
                        let horaDesdeMoment = moment(codEmpleado.HoraDesdeModificada);
                        let fecha = moment(codEmpleado.HoraDesdeModificada);
                        let horaHastaMoment = moment(codEmpleado.HoraHastaModificada);
                        let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute")
                        cantidadMinutos = cantidadMinutos + cantMinutosTrabajadas;
                    }
                }
                else {
                    if (celda.EsFrancoTrabajado && celda.Pago != 0) {
                        let horaDesdeMoment = moment(celda.HoraDesdeModificada);
                        let fecha = moment(celda.HoraDesdeModificada);
                        let horaHastaMoment = moment(celda.HoraHastaModificada);
                        let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute")
                        cantidadMinutos = cantidadMinutos + cantMinutosTrabajadas;
                    }
                }
            });
        });

        // recupera la cantidad de minutos de la diagramación para los francos trabajados para los turnos no que fueron seleccionados
        if (this.diagramacion != null)
            cantidadMinutos = cantidadMinutos + this.recuperarCantidadDeMinutosFrancosTrabajados(this.diagramacion.DiasMes, diasAP);

        let tope = this.topes.find(e => e.IdGrupoInspectores == grupoId);
        if (tope) {
            let topeEnMinutos = tope.FrancosTrabajadosTaller * 60;
            if (cantidadMinutos > topeEnMinutos) {
                result.isValid = false;
                result.Messages.push("Supera el tope de horas maxima definidas para Francos Trabajados para el grupo de inspectores");
            }
        }
        //console.log("HsFrancoTrabajadoPorGrupo", cantidadMinutos, "FrancosTrabajadosTaller", topeEnMinutos);
        return result;

    }

    ValidateHorasExtrasParaInspector(codEmpleado: InspectorDiaDto, listModel: DiasMesDto[], diasAP: DiasMesDto[]): ValidationResult {
        let result: ValidationResult = new ValidationResult();
        result.isValid = true;
        let cantidadMinutos = 0;
        let grupoId = 0; 
        

        listModel.forEach(e => {
            let row = e;
            let celda = e.Inspectores.find(e => e.CodEmpleado == codEmpleado.CodEmpleado);
            grupoId = celda.GrupoInspectoresId;
            let tope = this.topes.find(e => e.IdGrupoInspectores == grupoId);
            if (tope) {
                var cantidadMinExtras = 0;
                //var cantidadMinExtras = this.recuperarCanrtidadDeMinutosTrabajados(celda, row, listModel, diasAP) - tope.MinutosComunes;
                if (celda.Id == codEmpleado.Id) {
                    let horaDesdeMoment = moment(codEmpleado.HoraDesdeModificada);
                    let fecha = moment(codEmpleado.HoraDesdeModificada);
                    let horaHastaMoment = moment(codEmpleado.HoraHastaModificada);
                    let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute")
                    cantidadMinExtras = cantMinutosTrabajadas - tope.MinutosComunes;
                }
                else {
                    if (celda.EsJornada) {
                        let horaDesdeMoment = moment(celda.HoraDesdeModificada);
                        let fecha = moment(celda.HoraDesdeModificada);
                        let horaHastaMoment = moment(celda.HoraHastaModificada);
                        let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute")
                        cantidadMinExtras = cantMinutosTrabajadas - tope.MinutosComunes;
                    }
                }
                if (cantidadMinExtras > 0) {
                    cantidadMinutos = cantidadMinutos + cantidadMinExtras;
                }

            }
        });
        let tope = this.topes.find(e => e.IdGrupoInspectores == grupoId);
        if (tope) {
            let topeEnMinutos = tope.Hs50Persona * 60;
            if (cantidadMinutos > topeEnMinutos) {
                result.isValid = false;
                result.Messages.push("Supera el tope de horas maxima definidas para horas extras para el inspector");

            }
        }
        //console.log("inspectorHsExtras", codEmpleado, "cantidadMinutos", cantidadMinutos, "Hs50Personas", topeEnMinutos);
        return result;
    }

    ValidateHorasExtrasPorGrupo(codEmpleado: InspectorDiaDto,listModel: DiasMesDto[], diasAP: DiasMesDto[]): ValidationResult {
        let result: ValidationResult = new ValidationResult();
        result.isValid = true;
        let cantidadMinutos = 0;
        let grupoId = listModel[0].Inspectores[0].GrupoInspectoresId;


        // recupera la cantidad de minutos extras de la diagramación para los turnos que fueron seleccionados
        listModel.forEach(e => {
            let row = e;
            e.Inspectores.forEach(celda => {
                grupoId = celda.GrupoInspectoresId;
                let tope = this.topes.find(e => e.IdGrupoInspectores == grupoId);
                if (tope) {
                    //var cantidadMinExtras = this.recuperarCanrtidadDeMinutosTrabajados(celda, row, listModel, diasAP) - tope.MinutosComunes;
                    if (celda.Id == codEmpleado.Id) {
                        let horaDesdeMoment = moment(codEmpleado.HoraDesdeModificada);
                        let fecha = moment(codEmpleado.HoraDesdeModificada);
                        let horaHastaMoment = moment(codEmpleado.HoraHastaModificada);
                        let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute")
                        var cantidadMinExtras = cantMinutosTrabajadas - tope.MinutosComunes;
                    }
                    else {
                        if (celda.EsJornada) {
                            let horaDesdeMoment = moment(celda.HoraDesdeModificada);
                            let fecha = moment(celda.HoraDesdeModificada);
                            let horaHastaMoment = moment(celda.HoraHastaModificada);
                            let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute")
                            var cantidadMinExtras = cantMinutosTrabajadas - tope.MinutosComunes;
                        }
                    }
                    if (cantidadMinExtras > 0) {
                        cantidadMinutos = cantidadMinutos + cantidadMinExtras;
                    }
                }
            });
        });

        // recupera la cantidad de minutos de la diagramación para los turnos no que fueron seleccionados
        if (this.diagramacion != null) {
            this.diagramacion.DiasMes.forEach(e => {
                let row = e;
                e.Inspectores.forEach(celda => {
                    grupoId = celda.GrupoInspectoresId;
                    let tope = this.topes.find(e => e.IdGrupoInspectores == grupoId);
                    if (tope) {
                        let horaDesdeMoment = moment(celda.HoraDesdeModificada);
                        let fecha = moment(celda.HoraDesdeModificada);
                        let horaHastaMoment = moment(celda.HoraHastaModificada);
                        let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute")
                        var cantidadMinExtras = cantMinutosTrabajadas - tope.MinutosComunes;
                        if (cantidadMinExtras > 0) {
                            cantidadMinutos = cantidadMinutos + cantidadMinExtras;
                        }
                    }
                });
            });
        }

        let tope = this.topes.find(e => e.IdGrupoInspectores == grupoId);

        if (tope) {
            let topeEnMinutos = tope.Hs50Taller * 60;
            if (cantidadMinutos > topeEnMinutos) {
                result.isValid = false;
                result.Messages.push("Supera el tope de horas maxima definidas para horas extras para el grupo de inspectores");

            }
        }
        //console.log("HsExtrasPorGrupo", cantidadMinutos, "Horas50Taller", topeEnMinutos);
        return result;
    }

    ValidateHorasFeriadoParaInspector(codEmpleado: InspectorDiaDto, listModel: DiasMesDto[], diasAP: DiasMesDto[]): ValidationResult {
        let result: ValidationResult = new ValidationResult();
        result.isValid = true;
        let cantidadMinutos = 0;
        let grupoId = listModel[0].Inspectores[0].GrupoInspectoresId;


        if (codEmpleado.diaMes.EsFeriado) {
            listModel.forEach(e => {
                if (e.EsFeriado) {
                    let row = e;
                    let celda = e.Inspectores.find(e => e.CodEmpleado == codEmpleado.CodEmpleado);
                    if (celda.EsJornada || (celda.EsFrancoTrabajado && celda.Pago != 0) || (codEmpleado.Id == celda.Id && codEmpleado.EsFranco && codEmpleado.Pago != 0)) {
                        
                        if (codEmpleado.Id == celda.Id) {
                            grupoId = celda.GrupoInspectoresId;
                            cantidadMinutos = cantidadMinutos + this.recuperarCanrtidadDeMinutosTrabajados(codEmpleado, row, listModel, diasAP);
                        }
                        else {
                            grupoId = celda.GrupoInspectoresId;
                            cantidadMinutos = cantidadMinutos + this.recuperarCanrtidadDeMinutosTrabajados(celda, row, listModel, diasAP);
                        }
                    }
                    else {
                        let celdaDiaAnterior: InspectorDiaDto = null;
                        let rowAnterior = listModel.find(e => e.NumeroDia == row.NumeroDia - 1);

                        if (rowAnterior) {
                            celdaDiaAnterior = rowAnterior.Inspectores.find(e => e.CodEmpleado == celda.CodEmpleado);
                            if (celdaDiaAnterior) {
                                if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada) {
                                    let horaDesdeMoment = moment(celdaDiaAnterior.HoraDesdeModificada);
                                    let fecha = moment(celdaDiaAnterior.diaMesFecha);
                                    let horaHastaMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                    if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                        let fechaprincipioDia = new Date(e.Fecha);
                                        let horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);

                                        let dif = horaHastaMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                        cantidadMinutos = cantidadMinutos + dif;
                                    }
                                }
                            }
                        }
                        else {
                            if (diasAP[0]) {
                                celdaDiaAnterior = diasAP[0].Inspectores.find(e => e.CodEmpleado == celda.CodEmpleado);
                                if (celdaDiaAnterior) {
                                    if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada) {
                                        let horaDesdeMoment = moment(celdaDiaAnterior.HoraDesdeModificada);
                                        let fecha = moment(celdaDiaAnterior.diaMesFecha);
                                        let horaHastaMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                        if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                            let fechaprincipioDia = new Date(celda.diaMesFecha);
                                            let horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);

                                            let dif = horaHastaMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                            cantidadMinutos = cantidadMinutos + dif;
                                        }
                                    }
                                }
                            }
                        }
                    }                    
                }

            });
        }
        else {

            if (codEmpleado.HoraHastaModificada.getDate() != codEmpleado.HoraDesdeModificada.getDate()) {

                let rowSiguiente = listModel.find(r => r.NumeroDia == codEmpleado.diaMes.NumeroDia + 1);

                if (rowSiguiente) {
                    if (rowSiguiente.EsFeriado) {
                        let celdaSiguiente = rowSiguiente.Inspectores.find(i => i.CodEmpleado == codEmpleado.CodEmpleado)
                        if ((celdaSiguiente.EsFrancoTrabajado && celdaSiguiente.Pago !=0) || celdaSiguiente.EsJornada) {
                            let horaDesdeMoment = moment(celdaSiguiente.HoraDesdeModificada);
                            let fecha = moment(celdaSiguiente.diaMesFecha);
                            let horaHastaMoment = moment(celdaSiguiente.HoraHastaModificada);
                            if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                let finalDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 24, 0);
                                horaHastaMoment = moment(finalDelDia);
                            }
                            cantidadMinutos = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        }

                        let celdaDiaAnterior = codEmpleado;

                        if (celdaDiaAnterior) {
                            if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada || (codEmpleado.EsFranco && codEmpleado.Pago != 0)) {
                                let horaHastaDiaAnteriorMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                let fechaprincipioDia = new Date(rowSiguiente.Fecha);
                                let horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);

                                let dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                cantidadMinutos = cantidadMinutos + dif;
                            }
                        }

                        listModel.forEach(e => {
                            if (e.EsFeriado) {
                                let row = e;
                                var fechaDia = new Date(e.Fecha);
                                let celda = e.Inspectores.find(e => e.CodEmpleado == codEmpleado.CodEmpleado);
                                if (celdaSiguiente.Id != celda.Id) {
                                    if (celda.EsJornada || (celda.EsFrancoTrabajado && celda.Pago != 0)) {
                                        cantidadMinutos = cantidadMinutos + this.recuperarCanrtidadDeMinutosTrabajados(celda, row, listModel, diasAP);
                                    }
                                    else {
                                        let rowAnteriorAFeriado = listModel.find(r => r.NumeroDia == fechaDia.getDate() - 1);
                                        if (rowAnteriorAFeriado) {
                                            let celdaAnterirorAFeriado = rowAnteriorAFeriado.Inspectores.find(i => i.CodEmpleado == celda.CodEmpleado)
                                            if (celdaAnterirorAFeriado) {
                                                if (celdaAnterirorAFeriado.EsJornada || (celdaAnterirorAFeriado.EsFrancoTrabajado && celdaAnterirorAFeriado.Pago != 0)) {
                                                    if ((moment(celdaAnterirorAFeriado.HoraHastaModificada).date()) > moment(celdaAnterirorAFeriado.HoraDesdeModificada).date()) {
                                                        let horaHastaDiaAnteriorMoment = moment(celdaAnterirorAFeriado.HoraHastaModificada);
                                                        let fechaprincipioDia = new Date(e.Fecha);
                                                        let horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);

                                                        let dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                                        cantidadMinutos = cantidadMinutos + dif;
                                                    }

                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        });
                    }
                }
            }                  
        }
        if (cantidadMinutos != 0) {
            let tope = this.topes.find(e => e.IdGrupoInspectores == grupoId);
            if (tope) {
                let topeEnMinutos = tope.FeriadosPersona * 60;
                if (cantidadMinutos > topeEnMinutos) {
                    result.isValid = false;
                    result.Messages.push("Supera el tope de horas maxima definidas para feriados para el inspector");

                }
            }
        }
        
        return result;
    }


    ValidateHorasFeriadoPorGrupo(codEmpleado: InspectorDiaDto, listModel: DiasMesDto[], diasAP: DiasMesDto[]): ValidationResult {
        let result: ValidationResult = new ValidationResult();
        result.isValid = true;
        let cantidadMinutos = 0;
        let grupoId = listModel[0].Inspectores[0].GrupoInspectoresId;

        // recupera la cantidad de minutos de la diagramación para los turnos que fueron seleccionados
        if (codEmpleado.diaMes.EsFeriado) {
            listModel.forEach(e => {
                if (e.EsFeriado) {
                    let row = e;
                    var fechaDia = new Date(e.Fecha);
                    e.Inspectores.forEach(celda => {
                        if (celda.EsJornada || (celda.EsFrancoTrabajado && celda.Pago != 0) || (codEmpleado.Id == celda.Id && codEmpleado.EsFranco && codEmpleado.Pago !=0)) {

                            if (codEmpleado.Id == celda.Id) {
                                grupoId = celda.GrupoInspectoresId;
                                cantidadMinutos = cantidadMinutos + this.recuperarCanrtidadDeMinutosTrabajados(codEmpleado, row, listModel, diasAP);
                            }
                            else {
                                grupoId = celda.GrupoInspectoresId;
                                cantidadMinutos = cantidadMinutos + this.recuperarCanrtidadDeMinutosTrabajados(celda, row, listModel, diasAP);
                            }
                        }
                        else {
                            let celdaDiaAnterior: InspectorDiaDto = null;
                            let rowAnterior = listModel.find(e => e.NumeroDia == row.NumeroDia - 1);

                            if (rowAnterior) {
                                celdaDiaAnterior = rowAnterior.Inspectores.find(e => e.CodEmpleado == celda.CodEmpleado);
                                if (celdaDiaAnterior) {
                                    if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada) {
                                        let horaDesdeMoment = moment(celdaDiaAnterior.HoraDesdeModificada);
                                        let fecha = moment(celdaDiaAnterior.diaMesFecha);
                                        let horaHastaMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                        if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                            let fechaprincipioDia = new Date(e.Fecha);
                                            let horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);

                                            let dif = horaHastaMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                            cantidadMinutos = cantidadMinutos + dif;
                                        }
                                    }
                                }
                            }
                            else {
                                if (diasAP[0]) {
                                    celdaDiaAnterior = diasAP[0].Inspectores.find(e => e.CodEmpleado == celda.CodEmpleado);
                                    if (celdaDiaAnterior) {
                                        if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada) {
                                            let horaDesdeMoment = moment(celdaDiaAnterior.HoraDesdeModificada);
                                            let fecha = moment(celdaDiaAnterior.diaMesFecha);
                                            let horaHastaMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                            if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                                let fechaprincipioDia = new Date(e.Fecha);
                                                let horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);

                                                let dif = horaHastaMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                                cantidadMinutos = cantidadMinutos + dif;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    });
                }

            });
        }
        else {

            if (codEmpleado.HoraHastaModificada.getDate() != codEmpleado.HoraDesdeModificada.getDate()) {

                let rowSiguiente = listModel.find(r => r.NumeroDia == codEmpleado.diaMes.NumeroDia + 1);

                if (rowSiguiente) {
                    if (rowSiguiente.EsFeriado) {
                        let celdaSiguiente = rowSiguiente.Inspectores.find(i => i.CodEmpleado == codEmpleado.CodEmpleado)
                        if ((celdaSiguiente.EsFrancoTrabajado && celdaSiguiente.Pago != 0) || celdaSiguiente.EsJornada) {
                            let horaDesdeMoment = moment(celdaSiguiente.HoraDesdeModificada);
                            let fecha = moment(celdaSiguiente.diaMesFecha);
                            let horaHastaMoment = moment(celdaSiguiente.HoraHastaModificada);
                            if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                let finalDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 24, 0);
                                horaHastaMoment = moment(finalDelDia);
                            }
                            cantidadMinutos = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        }

                        let celdaDiaAnterior = codEmpleado;

                        if (celdaDiaAnterior) {
                            if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada || (codEmpleado.EsFranco && celdaDiaAnterior.Pago != 0)) {
                                let horaHastaDiaAnteriorMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                let fechaprincipioDia = new Date(rowSiguiente.Fecha);
                                let horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);

                                let dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                cantidadMinutos = cantidadMinutos + dif;
                            }
                        }

                        listModel.forEach(e => {
                            if (e.EsFeriado) {
                                let row = e;
                                var fechaDia = new Date(e.Fecha);
                                e.Inspectores.forEach(celda => {
                                    if (celdaSiguiente.Id != celda.Id) {
                                        if (celda.EsJornada || (celda.EsFrancoTrabajado && celda.Pago != 0)) {
                                            cantidadMinutos = cantidadMinutos + this.recuperarCanrtidadDeMinutosTrabajados(celda, row, listModel, diasAP);
                                        }
                                        else {
                                            let rowAnteriorAFeriado = listModel.find(r => r.NumeroDia == fechaDia.getDate() - 1);
                                            if (rowAnteriorAFeriado) {
                                                let celdaAnterirorAFeriado = rowAnteriorAFeriado.Inspectores.find(i => i.CodEmpleado == celda.CodEmpleado)
                                                if (celdaAnterirorAFeriado) {
                                                    if (celdaAnterirorAFeriado.EsJornada || (celdaAnterirorAFeriado.EsFrancoTrabajado && celdaAnterirorAFeriado.Pago != 0)) {
                                                        if ((moment(celdaAnterirorAFeriado.HoraHastaModificada).date()) > moment(celdaAnterirorAFeriado.HoraDesdeModificada).date()) {
                                                            let horaHastaDiaAnteriorMoment = moment(celdaAnterirorAFeriado.HoraHastaModificada);
                                                            let fechaprincipioDia = new Date(e.Fecha);
                                                            let horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);

                                                            let dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                                            cantidadMinutos = cantidadMinutos + dif;
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                });
                            }
                        });
                    }
                }
            }
        }

        // recupera la cantidad de minutos de la diagramación para los turnos que no fueron seleccionados
        if (this.diagramacion != null) {
            cantidadMinutos = cantidadMinutos + this.recuperarCantidadDeMinutosFeriados(this.diagramacion.DiasMes, diasAP);
        }

        let tope = this.topes.find(e => e.IdGrupoInspectores == grupoId);
        if (tope) {
            let topeEnMinutos = tope.FeriadosTaller * 60;
            if (cantidadMinutos > topeEnMinutos) {
                result.isValid = false
                result.Messages.push("Supera el tope de horas maxima definidas para feriados para el grupo de inspectores");
            }
        }
        return result;
    }

    ValidateFeriadoPermiteFrancoTrabajadoGeneral(celda: InspectorDiaDto, row: DiasMesDto, listModel: DiasMesDto[]): ValidationResult {
        let result: ValidationResult = new ValidationResult();
        result.isValid = true;

        if (row.EsFeriado) {
            if (celda.EsFrancoTrabajado) {

                let tope = this.topes.find(e => e.IdGrupoInspectores == celda.GrupoInspectoresId);
                if (tope) {
                    if (!tope.PermiteFrancosTrabajadosFeriado) {

                        result.isValid = false;
                        result.Messages.push("No se permite un franco trabajado un feriado");
                    }
                }
            }
        }

        return result;
    }

    ValidateFeriadoPermiteFrancoTrabajadoCelda(celda: InspectorDiaDto, row: DiasMesDto): ValidationResult {
        let result: ValidationResult = new ValidationResult();
        result.isValid = true;

        if (row.EsFeriado) {
            if (celda.EsFranco) {

                let tope = this.topes.find(e => e.IdGrupoInspectores == celda.GrupoInspectoresId);
                if (tope) {
                    if (!tope.PermiteFrancosTrabajadosFeriado) {

                        result.isValid = false;
                        result.Messages.push("No se permite un franco trabajado un feriado");
                    }
                }
            }
        }

        return result;
    }

    ValidateFeriadoPermiteHsExtras(celda: InspectorDiaDto, row: DiasMesDto, listModel: DiasMesDto[], diasAP: DiasMesDto[]): ValidationResult {
        let result: ValidationResult = new ValidationResult();
        result.isValid = true;

        let horaDesdeMoment = moment(celda.HoraDesdeModificada);
        let fecha = moment(celda.diaMesFecha);
        let horaHastaMoment = moment(celda.HoraHastaModificada);
        let tope = this.topes.find(e => e.IdGrupoInspectores == celda.GrupoInspectoresId);

        if (tope) {

            if (!tope.PermiteHsExtrasFeriado) {

                if (row.EsFeriado) {

                    if (horaHastaMoment.date() == horaDesdeMoment.date()) {
                        //Feriado no pasa al dia siguiente
                        let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        if (cantMinutosTrabajadas > tope.MinutosComunes) {
                            result.isValid = false;
                            result.Messages.push("No se permite hs extras un feriado");
                        }
                    }
                    else {
                        //Feriado pasa al dia siguiente con horas extras en su dia
                        let finalDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 24, 0);
                        let horaHastafinalDiaMoment = moment(finalDelDia);
                        let cantMinutosTrabajadas = horaHastafinalDiaMoment.diff(horaDesdeMoment, "minute");

                        if (cantMinutosTrabajadas > tope.MinutosComunes) {
                            result.isValid = false;
                            result.Messages.push("No se permite hs extras un feriado");
                        }
                        else {
                            //Feriado pasa al dia siguiente con horas extras en el dia siguiente
                            cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                            if (cantMinutosTrabajadas > tope.MinutosComunes) {

                                let celdaDiaSiguiente: InspectorDiaDto = null;
                                let rowSiguiente = listModel.find(e => e.NumeroDia == row.NumeroDia + 1);
                                if (rowSiguiente) {
                                    if (rowSiguiente.EsFeriado) {
                                        result.isValid = false;
                                        result.Messages.push("No se permite hs extras un feriado");
                                    }
                                }
                                else {
                                    if (diasAP[diasAP.length - 1].EsFeriado) {
                                        result.isValid = false;
                                        result.Messages.push("No se permite hs extras un feriado");
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                    //Normal control dia siguiente feriado
                    if (horaHastaMoment.date() != horaDesdeMoment.date()) {
                        let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        if (cantMinutosTrabajadas > tope.MinutosComunes) {
                            let celdaDiaSiguiente: InspectorDiaDto = null;
                            let rowSiguiente = listModel.find(e => e.NumeroDia == row.NumeroDia + 1);
                            if (rowSiguiente) {
                                if (rowSiguiente.EsFeriado) {
                                    result.isValid = false;
                                    result.Messages.push("No se permite hs extras un feriado");
                                }
                            }
                            else {
                                if (diasAP[diasAP.length - 1].EsFeriado) {
                                    result.isValid = false;
                                    result.Messages.push("No se permite hs extras un feriado");
                                }
                            }
                        }                        
                    }
                }
            }
        }
        return result;
    }

    private recuperarCanrtidadDeMinutosTrabajados(celda: InspectorDiaDto, row: DiasMesDto, listModel: DiasMesDto[], diasAP: DiasMesDto[]): number {
        let horaDesdeMoment = moment(celda.HoraDesdeModificada);
        let fecha = moment(celda.HoraDesdeModificada);
        let horaHastaMoment = moment(celda.HoraHastaModificada);
        if (horaHastaMoment.date() > horaDesdeMoment.date()) {
            let finalDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 24, 0);
            horaHastaMoment = moment(finalDelDia);
        }

        let cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");


        //Ahora vamos a buscar la cantidad de horas que el inspector trabajo en el dia anterior y que porcion le pertenece al dia que se esta evaluando
        let celdaDiaAnterior: InspectorDiaDto = null;
        let rowAnterior = listModel.find(e => e.NumeroDia == row.NumeroDia - 1);

        if (rowAnterior) {
            celdaDiaAnterior = rowAnterior.Inspectores.find(e => e.CodEmpleado == celda.CodEmpleado);
            if (celdaDiaAnterior) {
                if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada) {
                    let horaHastaDiaAnteriorMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                    if (horaHastaDiaAnteriorMoment.calendar() == horaDesdeMoment.calendar() || horaHastaDiaAnteriorMoment.calendar() == fecha.calendar()) {
                        let fechaprincipioDia = new Date(fecha.year(), fecha.month(), fecha.date(), 0, 0);
                        let principoDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 0, 0);
                        let horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);

                        let dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                        cantMinutosTrabajadas = cantMinutosTrabajadas + dif;
                    }
                }
            }
        }
        else {
            celdaDiaAnterior = diasAP[0].Inspectores.find(e => e.CodEmpleado == celda.CodEmpleado);
            if (celdaDiaAnterior) {
                if (celdaDiaAnterior.EsFrancoTrabajado || celdaDiaAnterior.EsJornada) {
                    let horaHastaDiaAnteriorMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                    if (horaHastaDiaAnteriorMoment.calendar() == horaDesdeMoment.calendar() || horaHastaDiaAnteriorMoment.calendar() == fecha.calendar()) {
                        let fechaprincipioDia = new Date(fecha.year(), fecha.month(), fecha.date(), 0, 0);
                        let principoDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 0, 0);
                        let horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);

                        let dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                        cantMinutosTrabajadas = cantMinutosTrabajadas + dif;
                    }
                }
            }
        }

        return cantMinutosTrabajadas;

    }

    public recuperarDiagramacionTurnosNoSeleccionados(Id: number, columns: InspectorDiaDto[]): void {

        this._DInspectoresService.getTurnosDeLaDiagramacion(Id)
            .subscribe(async t => {
                var turnosNoSeleccionados: number[] = [];
                this.turnosDiagrama = t.DataObject;
                this.turnosDiagrama.forEach(tur => {
                    var turexiste = false;
                    columns.forEach(turs => {
                        if (tur.TurnoId == turs.InspTurnoId) {
                            turexiste = true;
                        }
                    });
                    if (!turexiste) {
                        turnosNoSeleccionados.push(tur.TurnoId);
                    }
                });

                if (turnosNoSeleccionados.length > 0) {
                    this._DInspectoresService.getDiagramaMesAnio(Id, turnosNoSeleccionados,false)

                        .subscribe(e => {
                            if (e.DataObject) {
                                this.diagramacion = e.DataObject;
                                console.log('newlistModel', this.diagramacion);
                            }
                        }, error => {
                        }
                        );
                }
            });


    }

    private recuperarCantidadDeMinutosFeriados(diasMes: DiasMesDto[], diasAP: DiasMesDto[]): number {

        let cantidadMinutos = 0;
        diasMes.forEach(d => {
            if (d.EsFeriado) {
                let row = d;
                d.Inspectores.forEach(i => {
                    if ((i.EsFrancoTrabajado && i.Pago != 0) || i.EsJornada) {
                        cantidadMinutos = cantidadMinutos + this.recuperarCanrtidadDeMinutosTrabajados(i, row, diasMes, diasAP);
                    }
                });
            }
        });

        return cantidadMinutos;
    }

    private recuperarCantidadDeMinutosExtras(diasMes: DiasMesDto[], diasAP: DiasMesDto[]): number {

        let cantidadMinutos = 0;
        let grupoId = diasMes[0].Inspectores[0].GrupoInspectoresId;
        let tope = this.topes.find(e => e.IdGrupoInspectores == grupoId);

        diasMes.forEach(e => {
            let row = e;
            if (tope) {
                e.Inspectores.forEach(i => {
                    var cantidadMinExtras = this.recuperarCanrtidadDeMinutosTrabajados(i, row, diasMes, diasAP) - tope.MinutosComunes;
                    if (cantidadMinExtras > 0) {
                        cantidadMinutos = cantidadMinutos + cantidadMinExtras;
                    }
                });
            }
        });
        return cantidadMinutos;
    }

    private recuperarCantidadDeMinutosFrancosTrabajados(diasMes: DiasMesDto[], diasAP: DiasMesDto[]): number {

        let cantidadMinutos = 0;

        diasMes.forEach(e => {
            let row = e;
            e.Inspectores.forEach(i => {
                if (i.EsFrancoTrabajado && i.Pago != 0) {
                    var cantidadMinExtras = this.recuperarCanrtidadDeMinutosTrabajados(i, row, diasMes, diasAP);
                    if (cantidadMinExtras > 0) {
                        cantidadMinutos = cantidadMinutos + cantidadMinExtras;
                    }
                }
            });
        });

        return cantidadMinutos;

    }
}

