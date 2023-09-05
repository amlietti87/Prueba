import { OnInit, Component, ViewEncapsulation, Injector, Type } from "@angular/core";
import { BaseCrudComponent } from "../../../../shared/manager/crud.component";
import { UserDto, UserFilter } from "../../admin/model/user.model";
import { UserService } from "../../../../services/user.service";
import { IDetailComponent } from "../../../../shared/manager/detail.component";
import { CreateOrEditUserModalComponent } from "../../admin/users/create-or-edit-user-modal.component";
import { FdCertificadosService } from "../services/fdcertificados.service";
import { FDCertificadosFilter, FDCertificadosDto } from '../model/fdcertificados.model';
import { MatDialog, MatDialogConfig } from "@angular/material";
import { HistorialCertificadosComponent } from "./historial-certificados.component";
import { SubirCertificadoComponent } from "./subir-certificado.component";


@Component({

    templateUrl: "./administrar-certificado.component.html",
    styleUrls: ['./administrar-certificado.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class AdministrarCertificadoComponent extends BaseCrudComponent<UserDto, UserFilter> implements OnInit {


    GetEditComponent(): IDetailComponent {
        return null;
    }

    public certificadoFilter: FDCertificadosFilter;
    public allowUpload: boolean = false;
    public allowRevoke: boolean = false;
    public allowViewHistoric: boolean = false;
    public dialog: MatDialog;
    constructor(injector: Injector, protected _userService: UserService, protected certificadosService: FdCertificadosService) {
        super(_userService, null, injector);

        this.title = "Administrar Certificados"
        this.moduleName = "Administración de Certificados";
        this.icon = "flaticon-users";
        this.showbreadcum = false;
        this.dialog = injector.get(MatDialog);
        this.allowUpload = this.permission.isGranted("FirmaDigital.AdministrarCertificados.Agregar");
        this.allowRevoke = this.permission.isGranted("FirmaDigital.AdministrarCertificados.Revocar");
        this.allowViewHistoric = this.permission.isGranted("FirmaDigital.AdministrarCertificados.VerHistorial");
    }

    getNewfilter(): UserFilter {
        return new UserFilter();
    }

    ngOnInit() {
        super.ngOnInit();
    }

    addCertificate(user: UserDto) {
        console.log("Subir Certificado");
        var dialog: MatDialog;
        dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.data = new FDCertificadosFilter();
        dialogConfig.data = user.Id;
        dialogConfig.width = '60%';

        //Declarar el historialcertificadoscomponent en el modulo de firma.
        let dialogRef = this.dialog.open(SubirCertificadoComponent, dialogConfig);
    }

    revokeCertificate(user: UserDto) {

        this.certificadoFilter = new FDCertificadosFilter();
        this.certificadoFilter.UsuarioId = user.Id

        this.message.confirm('¿Está seguro que desea revocar el certificado?', 'Revocar Certificado', (a) => {
            //this.isshowalgo = !this.isshowalgo;
            if (a.value) {
                this.certificadosService.RevocarCertificado(this.certificadoFilter)
                    .subscribe((e) => {
                        if (e) {
                            this.notify.success('Certificado Revocado Correctamente');
                        }
                    });
            }
        });
    }

    certificateHistory(user: UserDto) {
        console.log("Ver Historial de Certificados");
        console.log(user)
        this.certificadoFilter = new FDCertificadosFilter();
        this.certificadoFilter.UsuarioId = user.Id
        this.certificadosService.HistorialCertificadosPorUsuario(this.certificadoFilter)
            .subscribe(e => {
                if (e.DataObject.length > 0) {
                    this.opencertificateHistory(e.DataObject);
                } else {
                    this.message.warn("No existen certificados para el usuario: " + user.NomUsuario, "Historial Certificados");
                }
                console.log(e)
            });
    }

    opencertificateHistory(certificados: FDCertificadosDto[]) {

        var dialog: MatDialog;
        dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.data = new FDCertificadosDto();
        dialogConfig.data = certificados;

        //Declarar el historialcertificadoscomponent en el modulo de firma.
        let dialogRef = this.dialog.open(HistorialCertificadosComponent, dialogConfig);
    }
}