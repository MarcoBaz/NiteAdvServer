import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { BehaviorSubject, Observable, from, of, EMPTY } from 'rxjs';
import { RestService } from './rest.service';

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
  
  onUserChanged: BehaviorSubject<any>;
  constructor(
    public rest: RestService,
  ) {
    // Set the defaults

  }


}