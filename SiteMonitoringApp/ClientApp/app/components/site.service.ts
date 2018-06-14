import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import { Site } from './Site';

@Injectable()
export class SiteService {

    private headers = new Headers({ 'Content-Type': 'application/json' });
    private sitesUrl = 'api/SiteAvailability';

    constructor(private http: Http) { }

    getSites(): Promise<Site[]> {
        return this.http.get(this.sitesUrl)
            .toPromise()
            .then(response => response.json() as Site[])
            .catch(this.handleError);
    }

    create(url: string): Promise<Site> {
        return this.http
            .post(this.sitesUrl, JSON.stringify({ url: url }), { headers: this.headers })
            .toPromise()
            .then(res => res.json() as Site)
            .catch(this.handleError);
    }

    update(id: string, url: string): void {
        this.http
            .put(this.sitesUrl + '/' + id, JSON.stringify({ url: url }), { headers: this.headers })
            .toPromise()
            .catch(this.handleError);
    }

    delete(id: string): void {
        this.http
            .delete(this.sitesUrl + '/' + id)
            .toPromise()
            .catch(this.handleError);
    }

    getPeriodSeconds(): Promise<number> {
        return this.http.get(this.sitesUrl + "/period")
            .toPromise()
            .then(response => response.json() as number)
            .catch(this.handleError);
    }

    updatePeriodSeconds(periodSeconds: number): Promise<any> {
        return this.http
            .post(this.sitesUrl + "/period", JSON.stringify({ periodSeconds: periodSeconds }), { headers: this.headers })
            .toPromise()
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred in SiteService', error);
        return Promise.reject(error.message || error);
    }
}