
import { Component, OnInit, AfterViewInit, ViewEncapsulation, ViewChild } from '@angular/core';

import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CalendarOptions, EventClickArg } from '@fullcalendar/angular';

import { CoreSidebarService } from '@core/components/core-sidebar/core-sidebar.service';
import { CoreConfigService } from '@core/services/config.service';

import { EventsService } from './events.service';
import { Event,EventCalendar } from './events.model';
import { ConfirmComponent } from '../confirm-dialog/confirm.component';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EventsComponent implements OnInit, AfterViewInit {
  @ViewChild('confirm') modal: ConfirmComponent;
  // Public
  public slideoutShow = false;
  public events = [];
  public event;

  public calendarOptions: CalendarOptions = {
    headerToolbar: {
      start: 'sidebarToggle, prev,next, title',
      end: 'dayGridMonth,timeGridWeek,timeGridDay,listMonth'
    },
    initialView: 'dayGridMonth',
    initialEvents: this.events,
    weekends: true,
    editable: true,
    eventResizableFromStart: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: 2,
    navLinks: true,
    eventClick: this.handleUpdateEventClick.bind(this),
    eventClassNames: this.eventClass.bind(this),
    select: this.handleDateSelect.bind(this)
  };

  // Private
  private _unsubscribeAll: Subject<any>;

  /**
   * Constructor
   *
   * @param {CoreSidebarService} _coreSidebarService
   * @param {CalendarService} _calendarService
   * @param {CoreConfigService} _coreConfigService
   */
  constructor(
    private _coreSidebarService: CoreSidebarService,
    private _eventsService: EventsService,
    private _coreConfigService: CoreConfigService
  ) {
    this.events = new Array<Event>();
    this._unsubscribeAll = new Subject();
  }
// Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
   ngOnInit(): void {
    // Subscribe config change
    this._coreConfigService.config.pipe(takeUntil(this._unsubscribeAll)).subscribe(config => {
      // ! If we have zoomIn route Transition then load calendar after 450ms(Transition will finish in 400ms)
      if (config.layout.animation === 'zoomIn') {
        setTimeout(() => {
          // Subscribe to Event Change
          this._eventsService.onEventsChange.subscribe(res => {
            if (res != null && res.EventsList!=null && res.EventsList.length >0)
            {
              this.events = [];
              this.events.push(res.EventsList);
              this.calendarOptions.events = this.trasformEventsForCalendar(res.EventsList);
            }
          });
        }, 450);
      } else {
        // Subscribe to Event Change
        this._eventsService.onEventsChange.subscribe(res => {
          if (res != null && res.EventsList!=null && res.EventsList.length >0)
          {
            this.events = [];
            this.events.push(res.EventsList);
            this.calendarOptions.events = this.trasformEventsForCalendar(res.EventsList);

      
          }
          
        });
      }
    });

    this._eventsService.onCurrentEventChange.subscribe(res => {
      this.event = res;
    });

    this._eventsService.onEventMessage
    .pipe(takeUntil(this._unsubscribeAll))
    .subscribe(message=>{
      
      if (message != '') {
        
      this.modal.openDialog("Error",message,true);
      }
     
    });
  }
  // Public Methods
  // -----------------------------------------------------------------------------------------------------

  /**
   * Add Event Class
   *
   * @param s
   */
  eventClass(s) {
    const calendarsColor = {
      Concerto: 'primary',
      Club: 'success',
      Teatro: 'danger',
      Festival: 'warning',
      //ETC: 'info'
    };

    const colorName = calendarsColor[s.event._def.extendedProps.calendar];
    return `bg-light-${colorName}`;
  }

  /**
   * Update Event
   *
   * @param eventRef
   */
  handleUpdateEventClick(eventRef: EventClickArg) {
    this.toggleEventSidebar();
    this._eventsService.updateCurrentEvent(eventRef);
  }

  /**
   * Toggle the sidebar
   *
   * @param name
   */
  toggleSidebar(name): void {
    this._coreSidebarService.getSidebarRegistry(name).toggleOpen();
  }

  /**
   * Date select Event
   *
   * @param eventRef
   */
  handleDateSelect(eventRef) {
    // const newEvent = new Event(null);
    // newEvent.StartTimestamp = eventRef.start;
    var newEvent = this.events.filter(event => event.id == eventRef.id);
    this.toggleEventSidebar();
    this._eventsService.onCurrentEventChange.next(newEvent[0]);
  }

  

  /**
   * Calendar's custom button on click toggle sidebar
   */
  ngAfterViewInit() {
    // Store this to _this as we need it on click event to call toggleSidebar
    let _this = this;
    // this.calendarOptions.customButtons = {
    //   sidebarToggle: {
    //     text: '',
    //     click() {
    //       _this.toggleSidebar('calendar-main-sidebar');
    //     }
    //   }
    // };
  }
  AddEvent() {
    this.toggleEventSidebar();
    this._eventsService.createNewEvent();
  }
  toggleEventSidebar() {
    this._coreSidebarService.getSidebarRegistry('calendar-event-sidebar').toggleOpen();
  }

  trasformEventsForCalendar(events:Event[]):EventCalendar[]{
     var result = new Array<EventCalendar>();
     events.forEach(event => 
      {
             var calEvent= new EventCalendar();
             calEvent.id = event.id;
             calEvent.title = event.Name;
             //calEvent.url = event.Url;
             calEvent.start = new Date(event.StartTimestamp*1000).toISOString();
             calEvent.end = new Date(event.EndTimestamp*1000).toISOString();
             calEvent.allDay = false;
             calEvent.calendar = '';//event.CategoryType;
             calEvent.extendedProps.description = decodeURI(event.Description);
             calEvent.extendedProps.location = event.Place;
             result.push(calEvent);
      });

     return result;
  }
}
//       id: 5,
      // url: '',
      // title: 'Dart Game?',
      // start: new Date(new Date().getFullYear(), new Date().getMonth() + 1, -13),
      // end: new Date(new Date().getFullYear(), new Date().getMonth() + 1, -12),
      // allDay: true,
      // calendar: 'ETC',
      // extendedProps: {
      //   location: 'Chicago',
      //   description: 'on a Trip',
      //   addGuest: []
      // }