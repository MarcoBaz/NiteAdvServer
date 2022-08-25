import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { RestService } from './rest.service';
import { map, catchError, tap } from 'rxjs/operators';
import {Router} from '@angular/router';
import { UserLogged,LoginViewModel } from './user.model';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

const providers = [
  { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }
];

@Injectable({
  providedIn: 'root'
})
export class AppService {
  onLoginError: BehaviorSubject<string>;
  onUserChanged: BehaviorSubject<any>;
  constructor(
    public rest: RestService,
    private _route:Router,
  ) {
    // Set the defaults
    this.onLoginError = new BehaviorSubject('');
     this.onUserChanged = new  BehaviorSubject([]);
  }
  Login(login,password): Promise<any> {
    return new Promise((resolve, reject) => {
    var user = new LoginViewModel(login,password);
      this.rest.getCredentials(user).subscribe((response: any) => {
     // console.log(JSON.stringify(response));
         if (response.Error == '' )
         {
           //UserLogged.setValues(response.Data);
           localStorage.setItem('currentUser', JSON.stringify(user));
           //this.onUserChanged.next(response.Data);
           this._route.navigate(['/']);
          
         }
         else
         {
          this.onLoginError.next("Credenziali errate");
           // throw new Error(response.Error);
         }
       
      }, reject);
  
    }
    );
  }
}