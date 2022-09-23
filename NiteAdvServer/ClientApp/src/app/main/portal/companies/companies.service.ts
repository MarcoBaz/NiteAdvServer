import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';
import { RestService } from 'app/rest.service';
import { FilterCompany,CompanyResponse, Company } from './companies.model';
@Injectable()
export class CompaniesService implements Resolve<any> {
  rows: any;
  onDatatablessChanged: BehaviorSubject<any>;
  onCompanyMessage:BehaviorSubject<[boolean,string]>;
  CompanyResponse:CompanyResponse;
  routeParams: any;
  filter:FilterCompany;
  /**
   * Constructor
   *
   * @param {HttpClient} _httpClient
   */
  constructor(private _rest: RestService, private _route:Router) {
    // Set the defaults
   
    this.onDatatablessChanged = new BehaviorSubject({});
    this.onCompanyMessage = new BehaviorSubject([false,'']);
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
      // var filter = new FilterCompany();
      //  filter.PageSize =10;
      //  filter.Offset =0;
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
    this.filter = filter;
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
              this.onCompanyMessage.next([true,'Errore nel recupero dati: ' + response.Error]);
              //throw new Error(response.Error);
           }
         
        }, reject);
    
      });
  }


  updateCompany(company,fileToUpload): Promise<any> {
    this.filter.CompanyToSave = company;
    return new Promise((resolve, reject) => {
      this._rest.SaveCompany(this.filter).subscribe((response: any) => {

          if (response != null) {
            this.CompanyResponse = response.Data;
              if (fileToUpload != null) {
                  fileToUpload.append('IdCompany', company.id);
                  this._rest.SaveCompanyImage(fileToUpload).subscribe((responseAvatar: any) => {
                      if (responseAvatar != null && responseAvatar.Error =='') {
                        this.CompanyResponse.CompanyList =  this.CompanyResponse.CompanyList.map(u => u.id !== responseAvatar.id ? u : responseAvatar);
                        this.onDatatablessChanged.next(this.CompanyResponse);
                        this.onCompanyMessage.next([false,'Dati salvati correttamente']);
                          resolve(response.Data);
                      }
                      else
                      {
                        this.onCompanyMessage.next([true,'Errore nel salvataggio: ' + responseAvatar.Error]);
                          resolve(response);
                      }
                  }, reject);
              }
              else {
                if (response.Error =='')
                {
                  this.onDatatablessChanged.next(this.CompanyResponse);
                  this.onCompanyMessage.next([false,'Dati salvati correttamente']);
                }
                else
                {
                  this.onCompanyMessage.next([true,'Errore nel salvataggio dati: ' + response.Error]);
                }
              
                  resolve(response.Data);
              }


          }
          else
          {
            this.onCompanyMessage.next([true,'Errore nel salvataggio dati: ' + response.Error]);
          }

      }, reject);
  });
  }
  deleteCompany(company): Promise<any> {
    this.filter.CompanyToSave = company;
    return new Promise((resolve, reject) => {
      this._rest.DeleteCompany(this.filter).subscribe((response: any) => {

           if (response.Error =='') {
            this.CompanyResponse = response.Data;
            this.onDatatablessChanged.next( this.CompanyResponse);
            this.onCompanyMessage.next([false,'Dati salvati correttamente']);
            resolve(response);
           


          }
          else
          {
            this.onCompanyMessage.next([true,'Errore nel salvataggio black list: ' + response.Error]);
          }

      }, reject);
  });
  }
  CompanyBlackList(filter): Promise<any> {
    return new Promise((resolve, reject) => {
      this._rest.CompanyBlackList(filter).subscribe((response: any) => {

           if (response.Error =='') {
            this.CompanyResponse = response.Data;
             
            this.onDatatablessChanged.next(this.CompanyResponse);
            
            resolve(response);
           


          }
          else
          {
            this.onCompanyMessage.next([true,'Errore nel salvataggio black list: ' + response.Error]);
          }

      }, reject);
  });
  }
}


