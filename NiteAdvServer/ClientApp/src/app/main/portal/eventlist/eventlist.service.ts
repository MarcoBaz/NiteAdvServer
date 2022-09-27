import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';
import { RestService } from 'app/rest.service';
import { FilterEvent,EventResponse, Event } from '../events/events.model';
@Injectable()
export class EventlistService implements Resolve<any> {
  rows: any;
  onDatatablessChanged: BehaviorSubject<any>;
  onEventMessage:BehaviorSubject<[boolean,string]>;
  EventResponse:EventResponse;
  routeParams: any;
  filter:FilterEvent;
  /**
   * Constructor
   *
   * @param {HttpClient} _httpClient
   */
  constructor(private _rest: RestService) {
    // Set the defaults
   
    this.onDatatablessChanged = new BehaviorSubject({});
    this.onEventMessage = new BehaviorSubject([false,'']);
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
      this._rest.getEventsData(filter).subscribe((response: any) => {
      //  console.log(JSON.stringify(response));
           if (response.Error == '' )
           {
              //this.RenterOffices = response.Data;
              this.EventResponse = response.Data;
             
              this.onDatatablessChanged.next(this.EventResponse);
              
              resolve(response);
           }
           else
           {
              this.onEventMessage.next([true,'Errore nel recupero dati: ' + response.Error]);
              //throw new Error(response.Error);
           }
         
        }, reject);
    
      });
  }


  updateEvent(event,fileToUpload): Promise<any> {
    this.filter.EventToSave = event;
    return new Promise((resolve, reject) => {
      this._rest.SaveEvent(this.filter).subscribe((response: any) => {

          if (response != null) {
            this.EventResponse = response.Data;
              if (fileToUpload != null) {
                  fileToUpload.append('IdEvent', event.id);
                  this._rest.SaveEventImage(fileToUpload).subscribe((responseAvatar: any) => {
                      if (responseAvatar != null && responseAvatar.Error =='') {
                        this.EventResponse.EventList =  this.EventResponse.EventList.map(u => u.id !== responseAvatar.id ? u : responseAvatar);
                        this.onDatatablessChanged.next(this.EventResponse);
                        this.onEventMessage.next([false,'Dati salvati correttamente']);
                          resolve(response.Data);
                      }
                      else
                      {
                        this.onEventMessage.next([true,'Errore nel salvataggio: ' + responseAvatar.Error]);
                          resolve(response);
                      }
                  }, reject);
              }
              else {
                if (response.Error =='')
                {
                  this.onDatatablessChanged.next(this.EventResponse);
                  this.onEventMessage.next([false,'Dati salvati correttamente']);
                }
                else
                {
                  this.onEventMessage.next([true,'Errore nel salvataggio dati: ' + response.Error]);
                }
              
                  resolve(response.Data);
              }


          }
          else
          {
            this.onEventMessage.next([true,'Errore nel salvataggio dati: ' + response.Error]);
          }

      }, reject);
  });
  }
  deleteEvent(event): Promise<any> {
    this.filter.EventToSave = event;
    return new Promise((resolve, reject) => {
      this._rest.DeleteEvent(this.filter).subscribe((response: any) => {

           if (response.Error =='') {
            this.EventResponse = response.Data;
            this.onDatatablessChanged.next( this.EventResponse);
            this.onEventMessage.next([false,'Dati salvati correttamente']);
            resolve(response);
           


          }
          else
          {
            this.onEventMessage.next([true,'Errore nel salvataggio black list: ' + response.Error]);
          }

      }, reject);
  });
  }
  EventsBlackList(filter): Promise<any> {
    return new Promise((resolve, reject) => {
      this._rest.EventsBlackList(filter).subscribe((response: any) => {

           if (response.Error =='') {
            this.EventResponse = response.Data;
             
            this.onDatatablessChanged.next(this.EventResponse);
            
            resolve(response);
           


          }
          else
          {
            this.onEventMessage.next([true,'Errore nel salvataggio black list: ' + response.Error]);
          }

      }, reject);
  });
  }
}
