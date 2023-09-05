import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler, ApplicationRef, Injector } from '@angular/core';

import { ThemeComponent } from './theme/theme.component';
import { LayoutModule } from './theme/layouts/layout.module';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent, CanDeactivateGuard } from './app.component';
import { ScriptLoaderService } from "./services/script-loader.service";
import { ThemeRoutingModule } from "./theme/theme-routing.module";
import { AuthModule } from "./auth/auth.module";

import { StorageService, LocalStorageService } from './shared/common/services/storage.service';
import { ConfigurationService } from './shared/common/services/configuration.service';
import { GlobalErrorHandler } from "./services/error-handler.service";
import { GlobalErrorLogService } from "./services/global-errorlog.Service";
import { PermissionCheckerService } from './shared/common/permission-checker.service';
import { AppNavigationService } from './theme/layouts/aside-nav/app-navigation.service';
import { LocalizationService } from './shared/common/localization.service';
import { NotifyService } from './shared/common/notify.service';
import { SettingService } from './shared/common/setting.service';
import { MessageService } from './shared/common/message.service';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { TokenInterceptor } from './auth/token.interceptor';

import { AuthService } from './auth/auth.service';

import { NgxPermissionsModule } from 'ngx-permissions';
import { NotificationService } from './shared/notification/notification.service';
import { NotificationComponent } from './shared/notification/notification.component';
import { LocatorService } from './shared/common/services/locator.service';
import { SharedModule } from './shared/shared.module';
import { UserNotificationHelper } from './theme/layouts/header-nav/notifications/UserNotificationHelper';
import { NotificationServiceProxy } from './theme/layouts/header-nav/notifications/notification-service';
import { FileService } from './shared/common/file.service';
import { BreadcrumbsService } from './theme/layouts/breadcrumbs/breadcrumbs.service';
import { UserService } from './services/user.service';


@NgModule({
    declarations: [
        ThemeComponent,
        AppComponent,
        NotificationComponent,

        //HeaderNotificationsComponent 
        //CreateOrEditUserModalComponent
    ],
    imports: [

        SharedModule,
        //FormsModule,
        NgxPermissionsModule.forRoot(),
        //ModalModule.forRoot(),
        //PopoverModule.forRoot(),
        LayoutModule,
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        AppRoutingModule,
        ThemeRoutingModule,
        AuthModule,
        //UtilsModule,

    ],
    exports: [
        SharedModule,
        //UtilsModule,
        //NotificationComponent,

    ],
    providers: [ScriptLoaderService,
        AuthService,
        StorageService,
        LocalStorageService,
        ConfigurationService,
        PermissionCheckerService,
        AppNavigationService,
        LocalizationService,
        NotifyService,
        SettingService,
        MessageService,
        NotificationService,
        GlobalErrorLogService,
        UserNotificationHelper,
        NotificationServiceProxy,
        FileService,
        CanDeactivateGuard,
        BreadcrumbsService,
        UserService,
        { provide: ErrorHandler, useClass: GlobalErrorHandler },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: TokenInterceptor,
            multi: true
        }
    ],
    // entryComponents: [CreateOrEditUserModalComponent],
    bootstrap: [AppComponent]
})
export class AppModule {

    constructor(applicationRef: ApplicationRef, private injector: Injector) {

        LocatorService.injector = this.injector;
        //for ng2-bootstrap-modal in angualar 5
        Object.defineProperty(applicationRef, '_rootComponents', { get: () => applicationRef['components'] });
    }

}
