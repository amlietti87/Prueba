"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var diagramasinspectores_model_1 = require("../model/diagramasinspectores.model");
var pers_topes_horas_extras_service_1 = require("./pers-topes-horas-extras.service");
var moment = require("moment");
var diagramasinspectores_service_1 = require("./diagramasinspectores.service");
var DiagramasInspectoresValidatorService = /** @class */ (function () {
    function DiagramasInspectoresValidatorService(topesService, _DInspectoresService) {
        var _this = this;
        this.topesService = topesService;
        this._DInspectoresService = _DInspectoresService;
        this.turnosDiagrama = [];
        this.diagramacion = null;
        topesService.requestAllByFilter({}).subscribe(function (e) { return _this.topes = e.DataObject.Items; });
    }
    DiagramasInspectoresValidatorService.prototype.ValidateHorasFrancoTrabajadoParaInspector = function (codEmpleado, listModel, diasAP) {
        var result = new diagramasinspectores_model_1.ValidationResult();
        result.isValid = true;
        var cantidadMinutos = 0;
        var grupoId = listModel[0].Inspectores[0].GrupoInspectoresId;
        listModel.forEach(function (e) {
            var row = e;
            var celda = e.Inspectores.find(function (e) { return e.CodEmpleado == codEmpleado.CodEmpleado; });
            if (codEmpleado.Id == celda.Id) {
                if ((codEmpleado.EsFranco || codEmpleado.EsFrancoTrabajado) && codEmpleado.Pago != 0) {
                    var horaDesdeMoment = moment(codEmpleado.HoraDesdeModificada);
                    var fecha = moment(codEmpleado.HoraDesdeModificada);
                    var horaHastaMoment = moment(codEmpleado.HoraHastaModificada);
                    var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                    cantidadMinutos = cantidadMinutos + cantMinutosTrabajadas;
                }
            }
            else {
                if (celda.EsFrancoTrabajado && celda.Pago != 0) {
                    var horaDesdeMoment = moment(celda.HoraDesdeModificada);
                    var fecha = moment(celda.HoraDesdeModificada);
                    var horaHastaMoment = moment(celda.HoraHastaModificada);
                    var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                    cantidadMinutos = cantidadMinutos + cantMinutosTrabajadas;
                }
            }
        });
        var tope = this.topes.find(function (e) { return e.IdGrupoInspectores == grupoId; });
        if (tope) {
            var topeEnMinutos = tope.FrancosTrabajadosPersona * 60;
            if (cantidadMinutos > topeEnMinutos) {
                result.isValid = false;
                result.Messages.push("Supera el tope de horas maxima definidas para Francos Trabajados para el inspector");
            }
        }
        return result;
    };
    DiagramasInspectoresValidatorService.prototype.ValidateHorasFrancoTrabajadoPorGrupo = function (codEmpleado, listModel, diasAP) {
        var result = new diagramasinspectores_model_1.ValidationResult();
        result.isValid = true;
        var cantidadMinutos = 0;
        var grupoId = listModel[0].Inspectores[0].GrupoInspectoresId;
        // recupera la cantidad de minutos  de la diagramación para los francos trabajados para los turnos que fueron seleccionados
        listModel.forEach(function (e) {
            var row = e;
            e.Inspectores.forEach(function (celda) {
                if (codEmpleado.Id == celda.Id) {
                    if ((codEmpleado.EsFranco || codEmpleado.EsFrancoTrabajado) && codEmpleado.Pago != 0) {
                        var horaDesdeMoment = moment(codEmpleado.HoraDesdeModificada);
                        var fecha = moment(codEmpleado.HoraDesdeModificada);
                        var horaHastaMoment = moment(codEmpleado.HoraHastaModificada);
                        var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        cantidadMinutos = cantidadMinutos + cantMinutosTrabajadas;
                    }
                }
                else {
                    if (celda.EsFrancoTrabajado && celda.Pago != 0) {
                        var horaDesdeMoment = moment(celda.HoraDesdeModificada);
                        var fecha = moment(celda.HoraDesdeModificada);
                        var horaHastaMoment = moment(celda.HoraHastaModificada);
                        var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        cantidadMinutos = cantidadMinutos + cantMinutosTrabajadas;
                    }
                }
            });
        });
        // recupera la cantidad de minutos de la diagramación para los francos trabajados para los turnos no que fueron seleccionados
        if (this.diagramacion != null)
            cantidadMinutos = cantidadMinutos + this.recuperarCantidadDeMinutosFrancosTrabajados(this.diagramacion.DiasMes, diasAP);
        var tope = this.topes.find(function (e) { return e.IdGrupoInspectores == grupoId; });
        if (tope) {
            var topeEnMinutos = tope.FrancosTrabajadosTaller * 60;
            if (cantidadMinutos > topeEnMinutos) {
                result.isValid = false;
                result.Messages.push("Supera el tope de horas maxima definidas para Francos Trabajados para el grupo de inspectores");
            }
        }
        //console.log("HsFrancoTrabajadoPorGrupo", cantidadMinutos, "FrancosTrabajadosTaller", topeEnMinutos);
        return result;
    };
    DiagramasInspectoresValidatorService.prototype.ValidateHorasExtrasParaInspector = function (codEmpleado, listModel, diasAP) {
        var _this = this;
        var result = new diagramasinspectores_model_1.ValidationResult();
        result.isValid = true;
        var cantidadMinutos = 0;
        var grupoId = 0;
        listModel.forEach(function (e) {
            var row = e;
            var celda = e.Inspectores.find(function (e) { return e.CodEmpleado == codEmpleado.CodEmpleado; });
            grupoId = celda.GrupoInspectoresId;
            var tope = _this.topes.find(function (e) { return e.IdGrupoInspectores == grupoId; });
            if (tope) {
                var cantidadMinExtras = 0;
                //var cantidadMinExtras = this.recuperarCanrtidadDeMinutosTrabajados(celda, row, listModel, diasAP) - tope.MinutosComunes;
                if (celda.Id == codEmpleado.Id) {
                    var horaDesdeMoment = moment(codEmpleado.HoraDesdeModificada);
                    var fecha = moment(codEmpleado.HoraDesdeModificada);
                    var horaHastaMoment = moment(codEmpleado.HoraHastaModificada);
                    var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                    cantidadMinExtras = cantMinutosTrabajadas - tope.MinutosComunes;
                }
                else {
                    if (celda.EsJornada) {
                        var horaDesdeMoment = moment(celda.HoraDesdeModificada);
                        var fecha = moment(celda.HoraDesdeModificada);
                        var horaHastaMoment = moment(celda.HoraHastaModificada);
                        var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        cantidadMinExtras = cantMinutosTrabajadas - tope.MinutosComunes;
                    }
                }
                if (cantidadMinExtras > 0) {
                    cantidadMinutos = cantidadMinutos + cantidadMinExtras;
                }
            }
        });
        var tope = this.topes.find(function (e) { return e.IdGrupoInspectores == grupoId; });
        if (tope) {
            var topeEnMinutos = tope.Hs50Persona * 60;
            if (cantidadMinutos > topeEnMinutos) {
                result.isValid = false;
                result.Messages.push("Supera el tope de horas maxima definidas para horas extras para el inspector");
            }
        }
        //console.log("inspectorHsExtras", codEmpleado, "cantidadMinutos", cantidadMinutos, "Hs50Personas", topeEnMinutos);
        return result;
    };
    DiagramasInspectoresValidatorService.prototype.ValidateHorasExtrasPorGrupo = function (codEmpleado, listModel, diasAP) {
        var _this = this;
        var result = new diagramasinspectores_model_1.ValidationResult();
        result.isValid = true;
        var cantidadMinutos = 0;
        var grupoId = listModel[0].Inspectores[0].GrupoInspectoresId;
        // recupera la cantidad de minutos extras de la diagramación para los turnos que fueron seleccionados
        listModel.forEach(function (e) {
            var row = e;
            e.Inspectores.forEach(function (celda) {
                grupoId = celda.GrupoInspectoresId;
                var tope = _this.topes.find(function (e) { return e.IdGrupoInspectores == grupoId; });
                if (tope) {
                    //var cantidadMinExtras = this.recuperarCanrtidadDeMinutosTrabajados(celda, row, listModel, diasAP) - tope.MinutosComunes;
                    if (celda.Id == codEmpleado.Id) {
                        var horaDesdeMoment = moment(codEmpleado.HoraDesdeModificada);
                        var fecha = moment(codEmpleado.HoraDesdeModificada);
                        var horaHastaMoment = moment(codEmpleado.HoraHastaModificada);
                        var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        var cantidadMinExtras = cantMinutosTrabajadas - tope.MinutosComunes;
                    }
                    else {
                        if (celda.EsJornada) {
                            var horaDesdeMoment = moment(celda.HoraDesdeModificada);
                            var fecha = moment(celda.HoraDesdeModificada);
                            var horaHastaMoment = moment(celda.HoraHastaModificada);
                            var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
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
            this.diagramacion.DiasMes.forEach(function (e) {
                var row = e;
                e.Inspectores.forEach(function (celda) {
                    grupoId = celda.GrupoInspectoresId;
                    var tope = _this.topes.find(function (e) { return e.IdGrupoInspectores == grupoId; });
                    if (tope) {
                        var horaDesdeMoment = moment(celda.HoraDesdeModificada);
                        var fecha = moment(celda.HoraDesdeModificada);
                        var horaHastaMoment = moment(celda.HoraHastaModificada);
                        var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        var cantidadMinExtras = cantMinutosTrabajadas - tope.MinutosComunes;
                        if (cantidadMinExtras > 0) {
                            cantidadMinutos = cantidadMinutos + cantidadMinExtras;
                        }
                    }
                });
            });
        }
        var tope = this.topes.find(function (e) { return e.IdGrupoInspectores == grupoId; });
        if (tope) {
            var topeEnMinutos = tope.Hs50Taller * 60;
            if (cantidadMinutos > topeEnMinutos) {
                result.isValid = false;
                result.Messages.push("Supera el tope de horas maxima definidas para horas extras para el grupo de inspectores");
            }
        }
        //console.log("HsExtrasPorGrupo", cantidadMinutos, "Horas50Taller", topeEnMinutos);
        return result;
    };
    DiagramasInspectoresValidatorService.prototype.ValidateHorasFeriadoParaInspector = function (codEmpleado, listModel, diasAP) {
        var _this = this;
        var result = new diagramasinspectores_model_1.ValidationResult();
        result.isValid = true;
        var cantidadMinutos = 0;
        var grupoId = listModel[0].Inspectores[0].GrupoInspectoresId;
        if (codEmpleado.diaMes.EsFeriado) {
            listModel.forEach(function (e) {
                if (e.EsFeriado) {
                    var row_1 = e;
                    var celda_1 = e.Inspectores.find(function (e) { return e.CodEmpleado == codEmpleado.CodEmpleado; });
                    if (celda_1.EsJornada || (celda_1.EsFrancoTrabajado && celda_1.Pago != 0) || (codEmpleado.Id == celda_1.Id && codEmpleado.EsFranco && codEmpleado.Pago != 0)) {
                        if (codEmpleado.Id == celda_1.Id) {
                            grupoId = celda_1.GrupoInspectoresId;
                            cantidadMinutos = cantidadMinutos + _this.recuperarCanrtidadDeMinutosTrabajados(codEmpleado, row_1, listModel, diasAP);
                        }
                        else {
                            grupoId = celda_1.GrupoInspectoresId;
                            cantidadMinutos = cantidadMinutos + _this.recuperarCanrtidadDeMinutosTrabajados(celda_1, row_1, listModel, diasAP);
                        }
                    }
                    else {
                        var celdaDiaAnterior = null;
                        var rowAnterior = listModel.find(function (e) { return e.NumeroDia == row_1.NumeroDia - 1; });
                        if (rowAnterior) {
                            celdaDiaAnterior = rowAnterior.Inspectores.find(function (e) { return e.CodEmpleado == celda_1.CodEmpleado; });
                            if (celdaDiaAnterior) {
                                if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada) {
                                    var horaDesdeMoment = moment(celdaDiaAnterior.HoraDesdeModificada);
                                    var fecha = moment(celdaDiaAnterior.diaMesFecha);
                                    var horaHastaMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                    if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                        var fechaprincipioDia = new Date(e.Fecha);
                                        var horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);
                                        var dif = horaHastaMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                        cantidadMinutos = cantidadMinutos + dif;
                                    }
                                }
                            }
                        }
                        else {
                            if (diasAP[0]) {
                                celdaDiaAnterior = diasAP[0].Inspectores.find(function (e) { return e.CodEmpleado == celda_1.CodEmpleado; });
                                if (celdaDiaAnterior) {
                                    if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada) {
                                        var horaDesdeMoment = moment(celdaDiaAnterior.HoraDesdeModificada);
                                        var fecha = moment(celdaDiaAnterior.diaMesFecha);
                                        var horaHastaMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                        if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                            var fechaprincipioDia = new Date(celda_1.diaMesFecha);
                                            var horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);
                                            var dif = horaHastaMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
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
                var rowSiguiente = listModel.find(function (r) { return r.NumeroDia == codEmpleado.diaMes.NumeroDia + 1; });
                if (rowSiguiente) {
                    if (rowSiguiente.EsFeriado) {
                        var celdaSiguiente_1 = rowSiguiente.Inspectores.find(function (i) { return i.CodEmpleado == codEmpleado.CodEmpleado; });
                        if ((celdaSiguiente_1.EsFrancoTrabajado && celdaSiguiente_1.Pago != 0) || celdaSiguiente_1.EsJornada) {
                            var horaDesdeMoment = moment(celdaSiguiente_1.HoraDesdeModificada);
                            var fecha = moment(celdaSiguiente_1.diaMesFecha);
                            var horaHastaMoment = moment(celdaSiguiente_1.HoraHastaModificada);
                            if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                var finalDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 24, 0);
                                horaHastaMoment = moment(finalDelDia);
                            }
                            cantidadMinutos = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        }
                        var celdaDiaAnterior = codEmpleado;
                        if (celdaDiaAnterior) {
                            if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada || (codEmpleado.EsFranco && codEmpleado.Pago != 0)) {
                                var horaHastaDiaAnteriorMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                var fechaprincipioDia = new Date(rowSiguiente.Fecha);
                                var horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);
                                var dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                cantidadMinutos = cantidadMinutos + dif;
                            }
                        }
                        listModel.forEach(function (e) {
                            if (e.EsFeriado) {
                                var row = e;
                                var fechaDia = new Date(e.Fecha);
                                var celda_2 = e.Inspectores.find(function (e) { return e.CodEmpleado == codEmpleado.CodEmpleado; });
                                if (celdaSiguiente_1.Id != celda_2.Id) {
                                    if (celda_2.EsJornada || (celda_2.EsFrancoTrabajado && celda_2.Pago != 0)) {
                                        cantidadMinutos = cantidadMinutos + _this.recuperarCanrtidadDeMinutosTrabajados(celda_2, row, listModel, diasAP);
                                    }
                                    else {
                                        var rowAnteriorAFeriado = listModel.find(function (r) { return r.NumeroDia == fechaDia.getDate() - 1; });
                                        if (rowAnteriorAFeriado) {
                                            var celdaAnterirorAFeriado = rowAnteriorAFeriado.Inspectores.find(function (i) { return i.CodEmpleado == celda_2.CodEmpleado; });
                                            if (celdaAnterirorAFeriado) {
                                                if (celdaAnterirorAFeriado.EsJornada || (celdaAnterirorAFeriado.EsFrancoTrabajado && celdaAnterirorAFeriado.Pago != 0)) {
                                                    if ((moment(celdaAnterirorAFeriado.HoraHastaModificada).date()) > moment(celdaAnterirorAFeriado.HoraDesdeModificada).date()) {
                                                        var horaHastaDiaAnteriorMoment = moment(celdaAnterirorAFeriado.HoraHastaModificada);
                                                        var fechaprincipioDia = new Date(e.Fecha);
                                                        var horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);
                                                        var dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
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
            var tope = this.topes.find(function (e) { return e.IdGrupoInspectores == grupoId; });
            if (tope) {
                var topeEnMinutos = tope.FeriadosPersona * 60;
                if (cantidadMinutos > topeEnMinutos) {
                    result.isValid = false;
                    result.Messages.push("Supera el tope de horas maxima definidas para feriados para el inspector");
                }
            }
        }
        return result;
    };
    DiagramasInspectoresValidatorService.prototype.ValidateHorasFeriadoPorGrupo = function (codEmpleado, listModel, diasAP) {
        var _this = this;
        var result = new diagramasinspectores_model_1.ValidationResult();
        result.isValid = true;
        var cantidadMinutos = 0;
        var grupoId = listModel[0].Inspectores[0].GrupoInspectoresId;
        // recupera la cantidad de minutos de la diagramación para los turnos que fueron seleccionados
        if (codEmpleado.diaMes.EsFeriado) {
            listModel.forEach(function (e) {
                if (e.EsFeriado) {
                    var row_2 = e;
                    var fechaDia = new Date(e.Fecha);
                    e.Inspectores.forEach(function (celda) {
                        if (celda.EsJornada || (celda.EsFrancoTrabajado && celda.Pago != 0) || (codEmpleado.Id == celda.Id && codEmpleado.EsFranco && codEmpleado.Pago != 0)) {
                            if (codEmpleado.Id == celda.Id) {
                                grupoId = celda.GrupoInspectoresId;
                                cantidadMinutos = cantidadMinutos + _this.recuperarCanrtidadDeMinutosTrabajados(codEmpleado, row_2, listModel, diasAP);
                            }
                            else {
                                grupoId = celda.GrupoInspectoresId;
                                cantidadMinutos = cantidadMinutos + _this.recuperarCanrtidadDeMinutosTrabajados(celda, row_2, listModel, diasAP);
                            }
                        }
                        else {
                            var celdaDiaAnterior = null;
                            var rowAnterior = listModel.find(function (e) { return e.NumeroDia == row_2.NumeroDia - 1; });
                            if (rowAnterior) {
                                celdaDiaAnterior = rowAnterior.Inspectores.find(function (e) { return e.CodEmpleado == celda.CodEmpleado; });
                                if (celdaDiaAnterior) {
                                    if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada) {
                                        var horaDesdeMoment = moment(celdaDiaAnterior.HoraDesdeModificada);
                                        var fecha = moment(celdaDiaAnterior.diaMesFecha);
                                        var horaHastaMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                        if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                            var fechaprincipioDia = new Date(e.Fecha);
                                            var horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);
                                            var dif = horaHastaMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                            cantidadMinutos = cantidadMinutos + dif;
                                        }
                                    }
                                }
                            }
                            else {
                                if (diasAP[0]) {
                                    celdaDiaAnterior = diasAP[0].Inspectores.find(function (e) { return e.CodEmpleado == celda.CodEmpleado; });
                                    if (celdaDiaAnterior) {
                                        if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada) {
                                            var horaDesdeMoment = moment(celdaDiaAnterior.HoraDesdeModificada);
                                            var fecha = moment(celdaDiaAnterior.diaMesFecha);
                                            var horaHastaMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                            if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                                var fechaprincipioDia = new Date(e.Fecha);
                                                var horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);
                                                var dif = horaHastaMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
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
                var rowSiguiente = listModel.find(function (r) { return r.NumeroDia == codEmpleado.diaMes.NumeroDia + 1; });
                if (rowSiguiente) {
                    if (rowSiguiente.EsFeriado) {
                        var celdaSiguiente_2 = rowSiguiente.Inspectores.find(function (i) { return i.CodEmpleado == codEmpleado.CodEmpleado; });
                        if ((celdaSiguiente_2.EsFrancoTrabajado && celdaSiguiente_2.Pago != 0) || celdaSiguiente_2.EsJornada) {
                            var horaDesdeMoment = moment(celdaSiguiente_2.HoraDesdeModificada);
                            var fecha = moment(celdaSiguiente_2.diaMesFecha);
                            var horaHastaMoment = moment(celdaSiguiente_2.HoraHastaModificada);
                            if (horaHastaMoment.date() > horaDesdeMoment.date()) {
                                var finalDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 24, 0);
                                horaHastaMoment = moment(finalDelDia);
                            }
                            cantidadMinutos = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        }
                        var celdaDiaAnterior = codEmpleado;
                        if (celdaDiaAnterior) {
                            if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada || (codEmpleado.EsFranco && celdaDiaAnterior.Pago != 0)) {
                                var horaHastaDiaAnteriorMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                                var fechaprincipioDia = new Date(rowSiguiente.Fecha);
                                var horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);
                                var dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                                cantidadMinutos = cantidadMinutos + dif;
                            }
                        }
                        listModel.forEach(function (e) {
                            if (e.EsFeriado) {
                                var row_3 = e;
                                var fechaDia = new Date(e.Fecha);
                                e.Inspectores.forEach(function (celda) {
                                    if (celdaSiguiente_2.Id != celda.Id) {
                                        if (celda.EsJornada || (celda.EsFrancoTrabajado && celda.Pago != 0)) {
                                            cantidadMinutos = cantidadMinutos + _this.recuperarCanrtidadDeMinutosTrabajados(celda, row_3, listModel, diasAP);
                                        }
                                        else {
                                            var rowAnteriorAFeriado = listModel.find(function (r) { return r.NumeroDia == fechaDia.getDate() - 1; });
                                            if (rowAnteriorAFeriado) {
                                                var celdaAnterirorAFeriado = rowAnteriorAFeriado.Inspectores.find(function (i) { return i.CodEmpleado == celda.CodEmpleado; });
                                                if (celdaAnterirorAFeriado) {
                                                    if (celdaAnterirorAFeriado.EsJornada || (celdaAnterirorAFeriado.EsFrancoTrabajado && celdaAnterirorAFeriado.Pago != 0)) {
                                                        if ((moment(celdaAnterirorAFeriado.HoraHastaModificada).date()) > moment(celdaAnterirorAFeriado.HoraDesdeModificada).date()) {
                                                            var horaHastaDiaAnteriorMoment = moment(celdaAnterirorAFeriado.HoraHastaModificada);
                                                            var fechaprincipioDia = new Date(e.Fecha);
                                                            var horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);
                                                            var dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
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
        var tope = this.topes.find(function (e) { return e.IdGrupoInspectores == grupoId; });
        if (tope) {
            var topeEnMinutos = tope.FeriadosTaller * 60;
            if (cantidadMinutos > topeEnMinutos) {
                result.isValid = false;
                result.Messages.push("Supera el tope de horas maxima definidas para feriados para el grupo de inspectores");
            }
        }
        return result;
    };
    DiagramasInspectoresValidatorService.prototype.ValidateFeriadoPermiteFrancoTrabajadoGeneral = function (celda, row, listModel) {
        var result = new diagramasinspectores_model_1.ValidationResult();
        result.isValid = true;
        if (row.EsFeriado) {
            if (celda.EsFrancoTrabajado) {
                var tope = this.topes.find(function (e) { return e.IdGrupoInspectores == celda.GrupoInspectoresId; });
                if (tope) {
                    if (!tope.PermiteFrancosTrabajadosFeriado) {
                        result.isValid = false;
                        result.Messages.push("No se permite un franco trabajado un feriado");
                    }
                }
            }
        }
        return result;
    };
    DiagramasInspectoresValidatorService.prototype.ValidateFeriadoPermiteFrancoTrabajadoCelda = function (celda, row) {
        var result = new diagramasinspectores_model_1.ValidationResult();
        result.isValid = true;
        if (row.EsFeriado) {
            if (celda.EsFranco) {
                var tope = this.topes.find(function (e) { return e.IdGrupoInspectores == celda.GrupoInspectoresId; });
                if (tope) {
                    if (!tope.PermiteFrancosTrabajadosFeriado) {
                        result.isValid = false;
                        result.Messages.push("No se permite un franco trabajado un feriado");
                    }
                }
            }
        }
        return result;
    };
    DiagramasInspectoresValidatorService.prototype.ValidateFeriadoPermiteHsExtras = function (celda, row, listModel, diasAP) {
        var result = new diagramasinspectores_model_1.ValidationResult();
        result.isValid = true;
        var horaDesdeMoment = moment(celda.HoraDesdeModificada);
        var fecha = moment(celda.diaMesFecha);
        var horaHastaMoment = moment(celda.HoraHastaModificada);
        var tope = this.topes.find(function (e) { return e.IdGrupoInspectores == celda.GrupoInspectoresId; });
        if (tope) {
            if (!tope.PermiteHsExtrasFeriado) {
                if (row.EsFeriado) {
                    if (horaHastaMoment.date() == horaDesdeMoment.date()) {
                        //Feriado no pasa al dia siguiente
                        var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        if (cantMinutosTrabajadas > tope.MinutosComunes) {
                            result.isValid = false;
                            result.Messages.push("No se permite hs extras un feriado");
                        }
                    }
                    else {
                        //Feriado pasa al dia siguiente con horas extras en su dia
                        var finalDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 24, 0);
                        var horaHastafinalDiaMoment = moment(finalDelDia);
                        var cantMinutosTrabajadas = horaHastafinalDiaMoment.diff(horaDesdeMoment, "minute");
                        if (cantMinutosTrabajadas > tope.MinutosComunes) {
                            result.isValid = false;
                            result.Messages.push("No se permite hs extras un feriado");
                        }
                        else {
                            //Feriado pasa al dia siguiente con horas extras en el dia siguiente
                            cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                            if (cantMinutosTrabajadas > tope.MinutosComunes) {
                                var celdaDiaSiguiente = null;
                                var rowSiguiente = listModel.find(function (e) { return e.NumeroDia == row.NumeroDia + 1; });
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
                        var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
                        if (cantMinutosTrabajadas > tope.MinutosComunes) {
                            var celdaDiaSiguiente = null;
                            var rowSiguiente = listModel.find(function (e) { return e.NumeroDia == row.NumeroDia + 1; });
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
    };
    DiagramasInspectoresValidatorService.prototype.recuperarCanrtidadDeMinutosTrabajados = function (celda, row, listModel, diasAP) {
        var horaDesdeMoment = moment(celda.HoraDesdeModificada);
        var fecha = moment(celda.HoraDesdeModificada);
        var horaHastaMoment = moment(celda.HoraHastaModificada);
        if (horaHastaMoment.date() > horaDesdeMoment.date()) {
            var finalDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 24, 0);
            horaHastaMoment = moment(finalDelDia);
        }
        var cantMinutosTrabajadas = horaHastaMoment.diff(horaDesdeMoment, "minute");
        //Ahora vamos a buscar la cantidad de horas que el inspector trabajo en el dia anterior y que porcion le pertenece al dia que se esta evaluando
        var celdaDiaAnterior = null;
        var rowAnterior = listModel.find(function (e) { return e.NumeroDia == row.NumeroDia - 1; });
        if (rowAnterior) {
            celdaDiaAnterior = rowAnterior.Inspectores.find(function (e) { return e.CodEmpleado == celda.CodEmpleado; });
            if (celdaDiaAnterior) {
                if ((celdaDiaAnterior.EsFrancoTrabajado && celdaDiaAnterior.Pago != 0) || celdaDiaAnterior.EsJornada) {
                    var horaHastaDiaAnteriorMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                    if (horaHastaDiaAnteriorMoment.calendar() == horaDesdeMoment.calendar() || horaHastaDiaAnteriorMoment.calendar() == fecha.calendar()) {
                        var fechaprincipioDia = new Date(fecha.year(), fecha.month(), fecha.date(), 0, 0);
                        var principoDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 0, 0);
                        var horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);
                        var dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                        cantMinutosTrabajadas = cantMinutosTrabajadas + dif;
                    }
                }
            }
        }
        else {
            celdaDiaAnterior = diasAP[0].Inspectores.find(function (e) { return e.CodEmpleado == celda.CodEmpleado; });
            if (celdaDiaAnterior) {
                if (celdaDiaAnterior.EsFrancoTrabajado || celdaDiaAnterior.EsJornada) {
                    var horaHastaDiaAnteriorMoment = moment(celdaDiaAnterior.HoraHastaModificada);
                    if (horaHastaDiaAnteriorMoment.calendar() == horaDesdeMoment.calendar() || horaHastaDiaAnteriorMoment.calendar() == fecha.calendar()) {
                        var fechaprincipioDia = new Date(fecha.year(), fecha.month(), fecha.date(), 0, 0);
                        var principoDelDia = new Date(horaDesdeMoment.year(), horaDesdeMoment.month(), horaDesdeMoment.date(), 0, 0);
                        var horaDesdeDiaAnteriorMoment = moment(fechaprincipioDia);
                        var dif = horaHastaDiaAnteriorMoment.diff(horaDesdeDiaAnteriorMoment, "minute");
                        cantMinutosTrabajadas = cantMinutosTrabajadas + dif;
                    }
                }
            }
        }
        return cantMinutosTrabajadas;
    };
    DiagramasInspectoresValidatorService.prototype.recuperarDiagramacionTurnosNoSeleccionados = function (Id, columns) {
        var _this = this;
        this._DInspectoresService.getTurnosDeLaDiagramacion(Id)
            .subscribe(function (t) { return __awaiter(_this, void 0, void 0, function () {
            var turnosNoSeleccionados;
            var _this = this;
            return __generator(this, function (_a) {
                turnosNoSeleccionados = [];
                this.turnosDiagrama = t.DataObject;
                this.turnosDiagrama.forEach(function (tur) {
                    var turexiste = false;
                    columns.forEach(function (turs) {
                        if (tur.TurnoId == turs.InspTurnoId) {
                            turexiste = true;
                        }
                    });
                    if (!turexiste) {
                        turnosNoSeleccionados.push(tur.TurnoId);
                    }
                });
                if (turnosNoSeleccionados.length > 0) {
                    this._DInspectoresService.getDiagramaMesAnio(Id, turnosNoSeleccionados, false)
                        .subscribe(function (e) {
                        if (e.DataObject) {
                            _this.diagramacion = e.DataObject;
                            console.log('newlistModel', _this.diagramacion);
                        }
                    }, function (error) {
                    });
                }
                return [2 /*return*/];
            });
        }); });
    };
    DiagramasInspectoresValidatorService.prototype.recuperarCantidadDeMinutosFeriados = function (diasMes, diasAP) {
        var _this = this;
        var cantidadMinutos = 0;
        diasMes.forEach(function (d) {
            if (d.EsFeriado) {
                var row_4 = d;
                d.Inspectores.forEach(function (i) {
                    if ((i.EsFrancoTrabajado && i.Pago != 0) || i.EsJornada) {
                        cantidadMinutos = cantidadMinutos + _this.recuperarCanrtidadDeMinutosTrabajados(i, row_4, diasMes, diasAP);
                    }
                });
            }
        });
        return cantidadMinutos;
    };
    DiagramasInspectoresValidatorService.prototype.recuperarCantidadDeMinutosExtras = function (diasMes, diasAP) {
        var _this = this;
        var cantidadMinutos = 0;
        var grupoId = diasMes[0].Inspectores[0].GrupoInspectoresId;
        var tope = this.topes.find(function (e) { return e.IdGrupoInspectores == grupoId; });
        diasMes.forEach(function (e) {
            var row = e;
            if (tope) {
                e.Inspectores.forEach(function (i) {
                    var cantidadMinExtras = _this.recuperarCanrtidadDeMinutosTrabajados(i, row, diasMes, diasAP) - tope.MinutosComunes;
                    if (cantidadMinExtras > 0) {
                        cantidadMinutos = cantidadMinutos + cantidadMinExtras;
                    }
                });
            }
        });
        return cantidadMinutos;
    };
    DiagramasInspectoresValidatorService.prototype.recuperarCantidadDeMinutosFrancosTrabajados = function (diasMes, diasAP) {
        var _this = this;
        var cantidadMinutos = 0;
        diasMes.forEach(function (e) {
            var row = e;
            e.Inspectores.forEach(function (i) {
                if (i.EsFrancoTrabajado && i.Pago != 0) {
                    var cantidadMinExtras = _this.recuperarCanrtidadDeMinutosTrabajados(i, row, diasMes, diasAP);
                    if (cantidadMinExtras > 0) {
                        cantidadMinutos = cantidadMinutos + cantidadMinExtras;
                    }
                }
            });
        });
        return cantidadMinutos;
    };
    DiagramasInspectoresValidatorService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [pers_topes_horas_extras_service_1.PersTopesHorasExtrasService, diagramasinspectores_service_1.DiagramasInspectoresService])
    ], DiagramasInspectoresValidatorService);
    return DiagramasInspectoresValidatorService;
}());
exports.DiagramasInspectoresValidatorService = DiagramasInspectoresValidatorService;
//# sourceMappingURL=diagramas-inspectores-validator.service.js.map