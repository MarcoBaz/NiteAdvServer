
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { RestService } from 'app/rest.service';

import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../users/user.model';

import { Event, EventsResponse, EventsViewModel } from './events.model';
@Injectable()
export class EventsService implements Resolve<any> {
  // Public
  public events:Event[];
  public calendar;
  public currentEvent:Event;

  public onEventsChange: BehaviorSubject<EventsResponse>;
  public onCurrentEventChange: BehaviorSubject<Event>;
  public onCalendarChange: BehaviorSubject<any>;
  onEventMessage:BehaviorSubject<string>;
  EventsResponse:EventsResponse;
  /**
   * Constructor
   *
   * @param {HttpClient} _httpClient
   */
  constructor(private _rest: RestService) {
    this.onEventsChange = new BehaviorSubject(new EventsResponse());
    this.onCurrentEventChange = new BehaviorSubject(new Event(''));
    this.onCalendarChange = new BehaviorSubject({});
    this.onEventMessage = new BehaviorSubject('');
    this.events = new Array<Event>()
  }

  /**
   * Resolver
   *
   * @param {ActivatedRouteSnapshot} route
   * @param {RouterStateSnapshot} state
   * @returns {Observable<any> | Promise<any> | any}
   */
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([this.getEvents()]).then(res => {
        resolve(res);
      }, reject);
    });
  }

  /**
   * Get Events
   */
  // getEvents(): Promise<any[]> {
  //   const url = `api/calendar-events`;

  //   return new Promise((resolve, reject) => {
  //     this._httpClient.get(url).subscribe((response: any) => {
  //       this.events = response;
  //       this.tempEvents = response;
  //       this.onEventChange.next(this.events);
  //       resolve(this.events);
  //     }, reject);
  //   });
  // }
  getEvents(): Promise<any[]> {
    var user:User = JSON.parse(localStorage.getItem('currentUser'));
    var vm = new EventsViewModel();
    vm.ActualMonth = new Date().getMonth()+1;
    vm.IdUser = user.id;
    return new Promise((resolve, reject) => {
      this._rest.getEvents(vm).subscribe((response: any) => {
      //  console.log(JSON.stringify(response));
           if (response.Error == '' )
           {
              this.events = [];
              //this.RenterOffices = response.Data;
              this.EventsResponse = response.Data;
              if (this.EventsResponse.EventsList != null && this.EventsResponse.EventsList.length >0)
              {
                this.EventsResponse.EventsList.forEach(event=>{
                  this.events.push(event);
                });
              }
              
              this.onEventsChange.next(this.EventsResponse);
              
              resolve(response);
           }
           else
           {
              this.onEventMessage.next('Errore nel recupero dati: ' + response.Error);
              //throw new Error(response.Error);
           }
         
        }, reject);
    
      });
  }

  /**
   * Get Calendar
   */
  getCalendar(): Promise<any[]> {
   

    return new Promise((resolve, reject) => {
      this.calendar = [
        { id: 1, filter: 'Concerto', color: 'primary', checked: true },
        { id: 2, filter: 'Club', color: 'success', checked: true },
        { id: 3, filter: 'Teatro', color: 'danger', checked: true },
        { id: 4, filter: 'Festival', color: 'warning', checked: true },
        // { id: 5, filter: 'ETC', color: 'info', checked: true }
      ];
      this.onCalendarChange.next(this.calendar);
     resolve(this.calendar);
    });
  }

  /**
   * Create New Event
   */
  createNewEvent() {
    this.currentEvent = new Event('');
    this.onCurrentEventChange.next(this.currentEvent);
  }

  /**
   * Calendar Update
   *
   * @param calendars
   */
  calendarUpdate(calendars) {
    const calendarsChecked = calendars.filter(calendar => {
      return calendar.checked === true;
    });

    let calendarRef = [];
    calendarsChecked.map(res => {
      calendarRef.push(res.filter);
    });

    //let filteredCalendar = this.events.filter(event => calendarRef.includes(event.calendar));
    //this.events = filteredCalendar;
    //this.onEventsChange.next(this.events);
  }

  /**
   * Delete Event
   *
   * @param event
   */
  deleteEvent(event) {
    return new Promise((resolve, reject) => {
      // this._httpClient.delete('api/calendar-events/' + event.id).subscribe(response => {
      //   this.getEvents();
      //   resolve(response);
      // }, reject);
    });
  }

  /**
   * Add Event
   *
   * @param eventForm
   */
  addEvent(eventForm) {
     const newEvent = new Event('');
    this.currentEvent = newEvent;
    this.onCurrentEventChange.next(this.currentEvent);
  }

  /**
   * Update Event
   *
   * @param eventRef
   */
  updateCurrentEvent(eventRef) {
   
    const myEvent = this.events.filter(x=> x.id == eventRef.event.id);
    this.currentEvent = myEvent[0];
    this.onCurrentEventChange.next(this.currentEvent);
  }


  /**
   * Post Updated Event
   *
   * @param event
   */

  postUpdatedEvent(eventVM): Promise<any> {
    return new Promise((resolve, reject) => {
      this._rest.SaveEvent(eventVM).subscribe((response: any) => {

        if (response.Error == '') {
          this.currentEvent = response.Data;
          this.onCurrentEventChange.next(this.currentEvent);
          
          resolve(response);
        }
        else
        {
          this.onEventMessage.next('Errore nel salvataggio event: ' + response.Error);
        }

      }, reject);
    });
  }
}
