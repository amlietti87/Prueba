import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { Router } from "@angular/router";
import { AuthenticationService } from "../services/authentication.service";
import { Helpers } from "../../helpers";
import { PermissionCheckerService } from '../../shared/common/permission-checker.service';
import { LocalStorageService } from "../../shared/common/services/storage.service";
import { DBLocalStorageService } from "../../shared/utils/local-storage.service";

@Component({
    selector: 'app-logout',
    templateUrl: './logout.component.html',
    encapsulation: ViewEncapsulation.None,
})

export class LogoutComponent implements OnInit {

    constructor(private _router: Router,
        private _authService: AuthenticationService,
        private permissionsService: PermissionCheckerService,
        private dbLocalStorageService: DBLocalStorageService,
        private localStorageService: LocalStorageService,
    ) {
    }

    ngOnInit(): void {
        Helpers.setLoading(true);
        // reset login status
        this.permissionsService.clearPermissions();
        this.dbLocalStorageService.clear();
        this.localStorageService.clearAll();
        this._authService.logout();
        this._router.navigate(['/login']);
    }
}