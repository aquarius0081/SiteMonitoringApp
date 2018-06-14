import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class UserService {

    private headers = new Headers({ 'Content-Type': 'application/json' });
    private userUrl = 'api/User';

    constructor(private http: Http) { }

    authenticate(userName: string, password: string): Promise<boolean> {
        return this.http
            .post(this.userUrl, JSON.stringify({ userName: userName, password: password }), { headers: this.headers })
            .toPromise()
            .then(res => res.json() as boolean)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred in UserService', error);
        return Promise.reject(error.message || error);
    }
}