import { Injectable } from '@angular/core';
import { PuntoDto } from '../../theme/pages/planificacion/model/punto.model';
import { ConfigurationService } from '../../shared/common/services/configuration.service';

declare var $: any;

@Injectable()
export class RbMapServices {
    constructor(protected configurationService: ConfigurationService) {
        this.Icons = this.configurationService.loadConfigurations();
    }

    Icons: any;
    iconBaseUrl = "assets/img/icons/";

    public static guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }


    markerIconFromDto(dto: PuntoDto) {
        if (dto.EsPuntoInicio) {
            return this.markerIcon(MapIcons.StartFlag);
        }
        else if (dto.EsPuntoTermino) {
            return this.markerIcon(MapIcons.EndFlag);
        }
        else if (dto.EsCambioSector) {
            if (dto.EsParada) {
                return this.markerIcon(MapIcons.BlueBus);
            }
            return this.markerIcon(MapIcons.BlackBus);
        }
        else if (dto.EsParada) {
            return this.markerIcon(MapIcons.BlueInfo);
        }
        else {
            return this.markerIcon(MapIcons.BluePin);
        }
    }

    markerIcon(tipo: number) {
        //acata
        var selft = this;
        if (!selft.Icons) {
            selft.Icons = selft.configurationService.loadConfigurations();
        }

        //tipo = +tipo;


        //var icono;
        //var normal = baseUlr + "icon-pin.png";
        //var parada = baseUlr + "icon-parada.png";
        //var normal_sin_guardar = baseUlr + "icon-pin-blue.png";
        //var parada_sin_guardar = baseUlr + "icon-parada-blue.png";
        //var info_sin_guardar = baseUlr + "icon-pin-info-blue.png";
        //var info = baseUlr + "icon-pin-info-green.png";
        //var info_alert = baseUlr + "icon-pin-info-red.png";
        //var start = baseUlr + "icon-start.png";
        //var finish = baseUlr + "icon-finish.png";
        //var taller = baseUlr + "icons8-llave-32.png";
        //var taller_active = baseUlr + "icons8-llave-32-active.png";

        //switch (tipo) {
        //    case 1: icono = normal; break;
        //    case 2: icono = parada; break;
        //    case 3: icono = normal_sin_guardar; break;
        //    case 4: icono = parada_sin_guardar; break;
        //    case 5: icono = info_sin_guardar; break;
        //    case 6: icono = info; break;
        //    case 7: icono = info_alert; break;
        //    case 8: icono = start; break;
        //    case 9: icono = finish; break;
        //    case 10: icono = taller; break;
        //    case 11: icono = taller_active; break;
        //    default: icono = normal;
        //}

        var icono = this.iconBaseUrl + "icon-pin.png";

        if (selft.Icons && selft.Icons[tipo])
            icono = selft.Icons[tipo];

        return icono;
    }

    markerIcon_Sin_Guardar(tipo: number) {

        tipo = +tipo;
        var icono;

        if (tipo == 1) icono = this.markerIcon(MapIcons.BluePin);
        else if (tipo == 2) icono = this.markerIcon(MapIcons.BlueInfo);
        else if (tipo == 6) icono = this.markerIcon(MapIcons.BlueBus);
        else if (tipo == 7) icono = this.markerIcon(MapIcons.BlueBus);
        else if (tipo == 8) icono = this.markerIcon(MapIcons.StartFlag);
        else if (tipo == 9) icono = this.markerIcon(MapIcons.EndFlag);
        else icono = this.markerIcon(MapIcons.BluePin);

        return icono;
    }

    parseKML(data) {

        var rows = [];
        var $xml = $(data);

        var $name = $xml.find("Document>name").first().text();

        var $placemarks = $xml.find("Placemark");
        var $placemark1 = $placemarks.first();
        var $recorrido1 = this.parsePlacemark($placemark1);
        var $recorrido1_name = $placemark1.find("name").first().text();
        var $recorrido1_color = $placemark1.find("styleUrl").first().text().split("-")[1];
        var $placemark2 = $placemarks.eq(1);
        var $recorrido2 = this.parsePlacemark($placemark2);
        var $recorrido2_name = $placemark2.find("name").first().text();
        var $recorrido2_color = $placemark2.find("styleUrl").first().text().split("-")[1];

        var $viaje = {
            name: $name,
            ida: {
                name: $recorrido1_name,
                color: "#" + $recorrido1_color,
                width: 2,
                points: $recorrido1
            },
            vuelta: {
                name: $recorrido2_name,
                color: "#" + $recorrido2_color,
                width: 2,
                points: $recorrido2
            }
        }

        return $viaje;
    }

    parsePlacemark($placemark) {
        var $coordinates = $placemark.find("coordinates");

        var $coordinates_splited = $coordinates.text().split("\n");
        var $result = [];
        $coordinates_splited.forEach(element => {
            if (element.trim() == "") return;

            var item = element.trim().split(",");
            $result.push({
                lat: parseFloat(item[1]),
                lng: parseFloat(item[0])
            })
        });

        return $result;
    }

    htmlEscape(literals, ...placeholders) {
        let result = "";

        // interleave the literals with the placeholders
        for (let i = 0; i < placeholders.length; i++) {
            result += literals[i];
            result += placeholders[i]
                .replace(/&/g, '&amp;')
                .replace(/"/g, '&quot;')
                .replace(/'/g, '&#39;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;');
        }

        // add the last literal
        result += literals[literals.length - 1];
        return result;
    }

    pinType(tipo): string {

        tipo = +tipo;
        var text = ""

        switch (tipo) {
            case 1: text = "Normal (no visible en modo normal)"; break;
            case 2: text = "Parada"; break;
            case 6: text = "Informacion"; break;
            case 7: text = "Informacion Alerta"; break;
            case 8: text = "Inicio"; break;
            case 9: text = "Fin"; break;

            default:
                break;
        }
        return text;
    }
}

export enum MapIcons {
    BlackBus = 2,
    BluePin = 3,
    BlueBus = 4,
    BlueInfo = 5,
    StartFlag = 8,
    EndFlag = 9
}
