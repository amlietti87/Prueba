import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    ElementRef,
    OnInit,
    Input,
    ComponentFactoryResolver,
    ViewContainerRef
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from "../../../../../shared/common/app-component-base";

import * as _ from "lodash";
declare let mApp: any;
import * as moment from "moment";

import {
    DetailModalComponent,
    DetailEmbeddedComponent,
    IDetailComponent
} from "../../../../../shared/manager/detail.component";

import { LazyLoadEventData } from "../../../../../shared/helpers/PrimengDatatableHelper";
import {
    FilterDTO,
    ResponseModel,
    PaginListResultDto,
    ViewMode,
    StatusResponse
} from "../../../../../shared/model/base.model";
import { BanderaService } from "../../bandera/bandera.service";
import { BanderaDto, BanderaFilter } from "../../model/bandera.model";
import { DataTable } from "primeng/components/datatable/datatable";
import { Paginator } from "primeng/components/paginator/paginator";
import { RouterModule, Routes, Router } from "@angular/router";

import { NgForm } from "@angular/forms";
import { HFechasConfiService } from "../HFechasConfi.service";
import {
    HFechasConfiDto,
    PlaDistribucionDeCochesPorTipoDeDiaDto,
    HBasecDto
} from "../../model/HFechasConfi.model";
import {
    BanderaCartelDto,
    BanderaCartelFilter
} from "../../model/banderacartel.model";
import { HMinxtipoService } from "../../hminxtipo/hminxtipo.service";
import { HSectoresDto, HMinxtipoFilter } from "../../model/hminxtipo.model";
import { environment } from "../../../../../../environments/environment";
import { BanderaTupService } from "../../banderaTup/banderaTup.service";
import { HBanderasColoresService } from "../../hbanderascolores/hbanderascolores.service";
import { SelectItem } from "primeng/api";
import { MatDialog } from "@angular/material/dialog";
import { MatDialogConfig } from "@angular/material";
import { ChangeRouteComponent } from "./change-route.component";
import { RutaFilter } from "../../model/ruta.model";
import { RutasViewFilter, RutasFilteredFilter } from "../../model/linea.model";
import * as Push from "push.js";
import { BanderaCartelService } from "../../banderacartel/banderacartel.service";

@Component({
    selector: "asignacion-bandera-hbasec",
    templateUrl: "./asignacion-bandera-hbasec.component.html"
})
export class AsignacionBanderaHbasec extends AppComponentBase
    implements OnInit {
    @Input()
    detail: HFechasConfiDto;

    BanderasColoresSelectItem: SelectItem[];
    BanderaTupSelectItem: SelectItem[];
    sectoresOriginal: string;
    sectoresTitle: string;
    loadingsectores: boolean;
    loadingbanderas: boolean;
    sectores: HSectoresDto[];
    mobile: SelectItem[];
    showTooltip: Boolean;
    allowChangeRoute: Boolean = false;
    public dialog: MatDialog;
    @Output() reloadData: EventEmitter<boolean> = new EventEmitter<boolean>();

    constructor(
        private injector: Injector,
        protected service: HFechasConfiService,
        protected minxtipoService: HMinxtipoService,
        protected _hBanderasColoresService: HBanderasColoresService,
        protected banderaCartelService: BanderaCartelService,
        protected _banderaTupService: BanderaTupService,
        protected router: Router,
        private cfr: ComponentFactoryResolver
    ) {
        super(injector);
        this.detail = new HFechasConfiDto();
        // this.showTooltip = !environment.production;
        this.showTooltip = false;
        this.allowChangeRoute = this.permission.isGranted(
            "Horarios.FechaHorario.CambiarRecorrido"
        );

        this.mobile = [
            { label: 'SI', value: 'S' },
            { label: 'NO', value: 'N' },
        ];
    }

    irMapa(row: HBasecDto): void {
        var urlback =
            "/planificacion/horariolinea/" +
            this.detail.CodLinea +
            "/" +
            this.detail.Id;
        if (row.CodRec) {
            this.router.navigate(["/planificacion/recorridos/" + row.CodRec], {
                queryParams: { returnUrl: urlback }
            });
        }
    }

    ngOnInit(): void {
        this._hBanderasColoresService.requestAllByFilter().subscribe(result => {
            this.BanderasColoresSelectItem = result.DataObject.Items.map(e => {
                return { label: e.DscBanderaColor, value: e.Id };
            });
            var bannull = { label: "...", value: null };
            this.BanderasColoresSelectItem.splice(0, 0, bannull);
        });

        this._banderaTupService.requestAllByFilter().subscribe(result => {
            this.BanderaTupSelectItem = result.DataObject.Items.map(e => {
                return { label: e.Descripcion, value: e.Id };
            });
            var bannull = { label: "...", value: null };
            this.BanderaTupSelectItem.splice(0, 0, bannull);
        });
    }

    public selectedItemChangeBanderasColores($event: any, row: HBasecDto): void {
        console.log($event);
        var s = this.BanderasColoresSelectItem.find(e => e.value == $event.value);
        if (s && s.label) {
            row.DescripcionBanderaColor = s.value != null ? s.label : "";
        }
    }

    public selectedItemChangeBanderaTup($event: any, row: HBasecDto): void {
        console.log($event);
        var s = this.BanderaTupSelectItem.find(e => e.value == $event.value);
        if (s && s.label) {
            row.DescripcionBanderaTup = s.value != null ? s.label : "";
        }
    }

    onRemapearRecoridoBandera(row: HBasecDto) {
        this.loadingbanderas = true;
        this.service
            .RemapearRecoridoBandera(row)
            .finally(() => (this.loadingbanderas = false))
            .subscribe(e => {
                if (e) {
                    this.message.success("Actualizado", "Ok");
                    this.reloadData.emit(true);
                }
            });
    }

    onChangeRoute(row: HBasecDto) {
        console.log(row);
        var dialog: MatDialog;
        this.dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.width = "80%";
        dialogConfig.data = new RutasFilteredFilter();
        dialogConfig.data.BanderaId = row.CodBan;
        dialogConfig.data.FechaDesde = this.detail.FechaDesde;
        dialogConfig.data.FechaHasta = this.detail.FechaHasta;
        dialogConfig.data.CodHFecha = row.CodHfecha;
        dialogConfig.data.CodRec = row.CodRec;
        dialogConfig.data.BanderaDetalle = row.DescripcionAbreviatura;

        let dialogRef = this.dialog.open(ChangeRouteComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(data => {
            if (data) {
                this.loadingbanderas = true;
                this.detail.HBasec = [];
                this.service.getById(row.CodHfecha)
                    .finally(() => (this.loadingbanderas = false))
                    .subscribe(datarec => {
                        this.detail.HBasec = datarec.DataObject.HBasec;
                        this.completedataBeforeShow(this.detail);
                    });
            }
        });
    }

    completedataBeforeShow(item: HFechasConfiDto): any {
        if (item.HBasec && item.HBasec.length > 0) {
            var filtro = new BanderaCartelFilter();
            filtro.CodHfecha = this.detail.Id;

            item.HBasec.forEach(e => {
                this.selectedItemChangeBanderasColores({ value: e.CodBanderaColor }, e);
                this.selectedItemChangeBanderaTup({ value: e.CodBanderaTup }, e);
            });

            this.banderaCartelService
                .requestAllByFilter(filtro)
                .finally(() => {
                    this.loadingbanderas = false;
                })
                .subscribe(e => {
                    if (e.DataObject.Items.length > 0) {
                        var volb = e.DataObject.Items[0];
                        item.HBasec.forEach(f => {
                            var cartel = volb.BolBanderasCartelDetalle.find(
                                e => e.CodBan == f.CodBan
                            );
                            if (cartel) {
                                f.NroSecuencia = cartel.NroSecuencia.toString();
                                f.TextoBandera = cartel.TextoBandera;
                                f.Movible = cartel.Movible;
                                f.ObsBandera = cartel.ObsBandera;
                            }
                        });
                    }
                });

        }
    }

    hasSectoresChange(): boolean {
        return (
            this.sectoresOriginal &&
            this.sectoresOriginal != JSON.stringify(this.sectores)
        );
    }

    private SetOriginalData(): void {
        this.sectores.forEach(f => (f.VerEnResumenOriginal = f.VerEnResumen));
        this.sectoresOriginal = JSON.stringify(this.sectores);
    }

    OnSelectedItem(row: HBasecDto) {
        var x = this.detail.HBasec.find(e => e.Selected == true);

        if (x != row) {
            this.detail.HBasec.forEach(e => (e.Selected = false));
            row.Selected = true;
            var filter = new HMinxtipoFilter();
            filter.CodHfecha = row.CodHfecha;
            filter.CodBan = row.CodBan;
            this.loadingsectores = true;
            this.sectoresTitle = row.DescripcionAbreviatura;
            this.minxtipoService.GetHSectores(filter).subscribe(
                e => {
                    this.sectores = e.DataObject;
                    this.SetOriginalData();
                },
                null,
                () => {
                    this.loadingsectores = false;
                }
            );
        }
    }

    onRowSelect(event) {
        var row = event as HBasecDto;

        var x = this.detail.HBasec.find(e => e.Selected == true);
        if (x != row) {
            if (this.hasSectoresChange()) {
                this.message.confirm(
                    "¿Está seguro de que desea selecionar otro el registro?",
                    "Tiene sectores sin guardar",
                    a => {
                        if (a.value) {
                            this.OnSelectedItem(row);
                        }
                    }
                );
            } else {
                this.OnSelectedItem(row);
            }
        }
    }

    verEnResumenchange(evet: any, row: HBasecDto): void { }

    onRowUnselect(event) { }

    sectoresBusyText: string = "Cargando...";

    getSectoresBusyText(p: any): string {
        return this.sectoresBusyText;
    }

    GuardarSectores() {
        this.sectoresBusyText = "Guardando...";
        this.loadingsectores = true;

        var sectoresconcambios = this.sectores.filter(
            f => f.VerEnResumenOriginal != f.VerEnResumen
        );

        this.minxtipoService
            .SetHSectores(sectoresconcambios)
            .finally(() => {
                this.sectoresBusyText = "Cargando...";
                this.loadingsectores = false;
            })
            .subscribe(e => {
                if (e.Status == StatusResponse.Ok) {
                    this.SetOriginalData();
                    this.notify.success(
                        "se guardaron los sectores de la bandera",
                        this.sectoresTitle
                    );
                }
            });
    }
}
