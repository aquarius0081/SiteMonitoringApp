import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Site } from '../Site';
import { SiteService } from '../site.service';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
    sites: Site[] | undefined;
    secondsLeft: number = 10;
    interval: any;

    constructor(
        private siteService: SiteService,
        private router: Router
    ) { }

    ngOnInit() {
        this.getSites();
        this.countdown();
    }

    getSites(): void {
        this.siteService
            .getSites()
            .then(sites => this.sites = sites);
    }

    countdown(): void {
        if (this.secondsLeft > 0) {
            this.interval = setInterval(() => {
                this.secondsLeft = this.secondsLeft - 1;
                if (this.secondsLeft <= 0) {
                    clearInterval(this.interval);
                    location.reload();
                }
            }, 1000);
        }
    }

    goToSettings(): void {
        clearInterval(this.interval);
        this.router.navigateByUrl('/settings');
    }
}