import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { NotificationsModule, NotificationsService } from 'angular4-notify';
import { CookieService } from 'angular2-cookie/services/cookies.service';

import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { SettingsComponent } from './components/settings/settings.component';

import { SiteService } from './components/site.service';
import { UserService } from './components/user.service';

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        SettingsComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'settings', component: SettingsComponent },
            { path: '**', redirectTo: 'home' }
        ]),
        NotificationsModule
    ],
    providers: [
        SiteService,
        NotificationsService,
        CookieService,
        UserService
    ]
})
export class AppModuleShared {
}
