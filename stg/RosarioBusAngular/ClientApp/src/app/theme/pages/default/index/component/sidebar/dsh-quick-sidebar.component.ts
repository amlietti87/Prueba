import { Component, OnInit, ViewEncapsulation, Output, EventEmitter } from '@angular/core';
import { DashboarService } from '../../services/dashboard.service';
import { ItemDto } from '../../../../../../shared/model/base.model';
import { DashboardDto, UsuarioDashboardItemDto } from '../../model/dashboard.model';



@Component({
    selector: "app-dsh-quick-sidebar",
    templateUrl: "./dsh-quick-sidebar.component.html",
    styleUrls: ["./dsh-quick-sidebar.css"],
    encapsulation: ViewEncapsulation.None,
})
export class DshQuickSidebarComponent implements OnInit {

    userItems: UsuarioDashboardItemDto[] = [];
    items: DashboardDto[];

    @Output() SaveDashboard: EventEmitter<any> = new EventEmitter<any>();
    @Output() CancelDashboard: EventEmitter<any> = new EventEmitter<any>();
    @Output() AddDashboard: EventEmitter<DashboardDto> = new EventEmitter<DashboardDto>();
    @Output() RemoveDashboard: EventEmitter<DashboardDto> = new EventEmitter<DashboardDto>();

    constructor(private _dashboarService: DashboarService) {

    }
    ngOnInit() {
        this._dashboarService.requestAllByFilter().subscribe(e => {
            this.items = e.DataObject.Items;
            if (this.userItems) {
                this.items.forEach(e => {
                    e.Selected = this.userItems.filter(ui => ui.DashboardId == e.Id).length > 0;
                });
            }
        });
    }


    switchDashboard(item: DashboardDto) {
        if (item.Selected) {
            this.AddDashboard.emit(item);
        }
        else {
            this.RemoveDashboard.emit(item);
        }

    }



    addDashboard(item: DashboardDto) {
        this.AddDashboard.emit(item);
    }
    guardarClick() {
        this.SaveDashboard.emit();
    }

    cancelar() {
        this.CancelDashboard.emit();
    }

    SetItemsUsuario(userItems: UsuarioDashboardItemDto[]): void {
        this.userItems = userItems;

        if (this.items) {
            this.items.forEach(e => {
                e.Selected = userItems.filter(ui => ui.DashboardId == e.Id).length > 0;
            });
        }

    }

}