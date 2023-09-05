import { Component, OnInit, ChangeDetectionStrategy, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { IDashboardBaseComponent } from '../dashboardComponent';
import { LineaService } from '../../../../planificacion/linea/linea.service';
import { ItemDto, Dto } from '../../../../../../shared/model/base.model';

declare let mApp: any;

@Component({
    selector: 'recent-notifications',
    templateUrl: './recent-notifications.html'
})
export class RecentNotificationsDashboardComponent implements IDashboardBaseComponent, OnInit, AfterViewInit {

    ngAfterViewInit(): void {
        mApp.init();
    }
    data: any;

    @ViewChild('Todas') TodasTab: ElementRef;
    @ViewChild('Linea') LineaTab: ElementRef;

    Notificaciones: recentNotification[];
    lineas: ItemDto[];
    currentLlinea: ItemDto;
    constructor(protected lineaService: LineaService) {
        this.lineaService.GetItemsAsync({}).subscribe(e => {
            this.lineas = e.DataObject;
        });
        this.Notificaciones = [];
        this.Notificaciones.push(new recentNotification("103N - 10:38", "Agrega Minutos", '16/06/2018', 'NORMAL', 1))
        this.Notificaciones.push(new recentNotification("315", "Refuerzos", '10/06/2018', 'FESTIVO', 2))
        this.Notificaciones.push(new recentNotification("440 01  440 02", "Agrega un coche y dos conductores", '10/06/2018', 'FESTIVO', 2))
        this.Notificaciones.push(new recentNotification("740 14 - 740 17 - 740 49", "Mejora Frecuencia", '10/06/2018', 'FESTIVO', 2))
        this.Notificaciones.push(new recentNotification("203P", "Quitar minutos por finalizar desvio", '09/06/2018', 'MEDIO FESTIVO', 3))
        this.Notificaciones.push(new recentNotification("", "", '', 'ESPECIAL', 4))
        this.Notificaciones.push(new recentNotification("", "", '', 'ESPECIAL1', 5))
    }

    OnSelecLinea(selectedItem: ItemDto): void {
        this.currentLlinea = selectedItem;
        if (this.LineaTab.nativeElement) {
            this.LineaTab.nativeElement.classList.add('active');
        }
        if (this.TodasTab.nativeElement) {
            this.TodasTab.nativeElement.classList.remove('active');
        }
    }

    OnTodas(): void {
        this.currentLlinea = null;
        if (this.TodasTab.nativeElement) {
            this.TodasTab.nativeElement.classList.add('active');
        }
        if (this.LineaTab.nativeElement) {
            this.LineaTab.nativeElement.classList.remove('active');
        }
    }


    ngOnInit() {
    }

}


export class recentNotification extends Dto<number> {
    constructor(
        _Description: string,
        _Title: string,
        _Fecha: string,
        _TipoDia: string,
        _Estado: number,
    ) {
        super();
        this.Description = _Description;
        this.Title = _Title;
        this.Fecha = _Fecha;
        this.TipoDia = _TipoDia;
        this.Estado = _Estado;
    }

    getDescription(): string {
        return this.Description;
    }
    Description: string;
    Title: string;
    Fecha: string;
    TipoDia: string;
    Estado: number = 1;
}
