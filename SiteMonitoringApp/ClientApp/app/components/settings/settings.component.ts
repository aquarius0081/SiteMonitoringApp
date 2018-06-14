import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Site } from '../Site';
import { SiteService } from '../site.service';
import { Location } from '@angular/common';
import { NotificationsService } from 'angular4-notify';
import { CookieService } from 'angular2-cookie/core';
import { User } from '../User';
import { UserService } from '../user.service';

@Component({
    selector: 'settings',
    templateUrl: './settings.component.html'
})
export class SettingsComponent implements OnInit {
    sites: Site[] = [];
    siteModel: Site = new Site();
    userModel: User = new User();
    showNew: boolean = false;
    // It will be either 'Save' or 'Update' based on operation.
    submitType: string = 'Save';
    selectedRow: number = -1;
    isAuthenticated: boolean = false;
    isAuthenticationFailed: boolean = false;
    timePeriod: number = 0;

    constructor(
        private siteService: SiteService,
        private location: Location,
        private router: Router,
        protected notificationsService: NotificationsService,
        private cookieService: CookieService,
        private userService: UserService
    ) { }

    goBack(): void {
        this.location.back();
    }

    ngOnInit() {
        if (this.cookieService.get("isAuthenticated")) {
            this.isAuthenticated = true;
        }
        this.getSites();
        this.getPeriodSeconds();
    }

    getSites(): void {
        this.siteService
            .getSites()
            .then(sites => this.sites = sites);
    }

    onNewSite() {
        this.siteModel = new Site();
        // Change submitType to 'Save'.
        this.submitType = 'Save';
        this.showNew = true;
    }

    onSave() {
        this.siteModel.url = this.siteModel.url.trim();
        if (!this.siteModel.url) { return; }
        if (this.submitType === 'Save') {
            this.siteService.create(this.siteModel.url)
                .then(site => {
                    this.sites.push(site);
                }).catch(err => this.notificationsService.addError('Error during creation of site on server!'));
        } else {
            // Update the existing properties values based on model.
            this.siteService.update(this.siteModel.id, this.siteModel.url);
            this.sites[this.selectedRow].url = this.siteModel.url;
        }
        this.showNew = false;
    }

    onEdit(index: number) {
        // Assign selected table row index.
        this.selectedRow = index;
        this.siteModel = new Site();
        this.siteModel = Object.assign({}, this.sites[this.selectedRow]);
        // Change submitType to Update.
        this.submitType = 'Update';
        this.showNew = true;
    }

    onDelete(index: number) {
        this.selectedRow = index;
        this.siteModel = new Site();
        this.siteModel = Object.assign({}, this.sites[this.selectedRow]);
        this.siteService.delete(this.siteModel.id);
        this.sites.splice(index, 1);
    }

    onCancel() {
        this.showNew = false;
    }

    onLogIn(): void {
        this.userService.authenticate(this.userModel.username, this.userModel.password)
            .then(result => {
                if (result === true) {
                    this.cookieService.put("isAuthenticated", "true");
                    this.isAuthenticated = true;
                    this.isAuthenticationFailed = false;
                } else {
                    this.isAuthenticated = false;
                    this.isAuthenticationFailed = true;
                }
            }).catch(err => this.notificationsService.addError('Error during authentication on server!'));
    }

    onLogOut(): void {
        this.cookieService.remove("isAuthenticated");
        this.isAuthenticated = false;
        this.userModel = new User();
    }

    getPeriodSeconds(): void {
        this.siteService
            .getPeriodSeconds()
            .then(timePeriod => this.timePeriod = timePeriod);
    }

    updatePeriodSeconds(timeperiod: any): void {
        if (timeperiod.valid) {
            this.siteService.updatePeriodSeconds(this.timePeriod)
                .catch(err => this.notificationsService.addError('Error during update of monitoring time period!'));
        }
    }
}
