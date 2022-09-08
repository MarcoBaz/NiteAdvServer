
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';
import { RestService } from 'app/rest.service';
import { FilterUser,UserResponse } from './user.model';
@Injectable()
export class UsersService implements Resolve<any> {
  rows: any;
  onDatatablessChanged: BehaviorSubject<any>;
  onUserMessage:BehaviorSubject<string>;
  UserResponse:UserResponse;
  routeParams: any;
  /**
   * Constructor
   *
   * @param {HttpClient} _httpClient
   */
  constructor(private _rest: RestService, private _route:Router) {
    // Set the defaults
   
    this.onDatatablessChanged = new BehaviorSubject({});
    this.onUserMessage = new BehaviorSubject('');
  }

  /**
   * Resolver
   *
   * @param {ActivatedRouteSnapshot} route
   * @param {RouterStateSnapshot} state
   * @returns {Observable<any> | Promise<any> | any}
   */
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    this.routeParams = route.params;
    return new Promise<void>((resolve, reject) => {
      var filter = new FilterUser();
       filter.PageSize =10;
       filter.Offset =0;
       if (this.routeParams != null)
       { 
          Promise.all([
        
            //this.getDataTableRows(filter)
            ]).then(() => {
            resolve();
          }, reject);
      }
     
    });
  }

  /**
   * Get rows
   */
  getDataTableRows(filter): Promise<any[]> {
    return new Promise((resolve, reject) => {
      this._rest.getUserData(filter).subscribe((response: any) => {
      //  console.log(JSON.stringify(response));
           if (response.Error == '' )
           {
              //this.RenterOffices = response.Data;
              this.UserResponse = response.Data;
             
              this.onDatatablessChanged.next(this.UserResponse);
              
              resolve(response);
           }
           else
           {
              this.onUserMessage.next('Errore nel recupero dati: ' + response.Error);
              //throw new Error(response.Error);
           }
         
        }, reject);
    
      });
  }


  updateUser(user): Promise<any> {
    return new Promise((resolve, reject) => {
      this._rest.SaveUser(user).subscribe((response: any) => {

        if (response.Error == '') {
          var cmp = response.Data;
          let index = this.UserResponse.UserList.findIndex(item => item.id == cmp.id);
          this.UserResponse.UserList[index] = cmp;
          this.onDatatablessChanged.next(this.UserResponse);
          resolve(response);
        }
        else
        {
          this.onUserMessage.next('Errore nel salvataggio utente: ' + response.Error);
        }

      }, reject);
    });
  }
}