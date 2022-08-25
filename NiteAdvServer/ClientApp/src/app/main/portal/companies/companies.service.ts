import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';
import { RestService } from 'app/rest.service';
import { FilterCompany,CompanyResponse } from './companies.model';
@Injectable()
export class CompaniesService implements Resolve<any> {
  rows: any;
  onDatatablessChanged: BehaviorSubject<any>;
  onCompanyMessage:BehaviorSubject<string>;
  CompanyResponse:CompanyResponse;
  routeParams: any;
  /**
   * Constructor
   *
   * @param {HttpClient} _httpClient
   */
  constructor(private _rest: RestService, private _route:Router) {
    // Set the defaults
   
    this.onDatatablessChanged = new BehaviorSubject({});
    this.onCompanyMessage = new BehaviorSubject('');
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
      var filter = new FilterCompany();
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
      this._rest.getCompanyData(filter).subscribe((response: any) => {
      //  console.log(JSON.stringify(response));
           if (response.Error == '' )
           {
              //this.RenterOffices = response.Data;
              this.CompanyResponse = response.Data;
             
              this.onDatatablessChanged.next(this.CompanyResponse);
              
              resolve(response);
           }
           else
           {
              this.onCompanyMessage.next('Errore nel recupero dati: ' + response.Error);
              //throw new Error(response.Error);
           }
         
        }, reject);
    
      });
  }


  updateCompany(company): Promise<any> {
    return new Promise((resolve, reject) => {
      this._rest.SaveCompany(company).subscribe((response: any) => {

        if (response.Error == '') {
          var cmp = response.data;
          this.CompanyResponse.CompanyList[cmp.id] = cmp
          this.onDatatablessChanged.next(this.CompanyResponse);
          resolve(response);
        }
        else
        {
          this.onCompanyMessage.next('Errore nel salvataggio company: ' + response.Error);
        }

      }, reject);
    });
  }
}