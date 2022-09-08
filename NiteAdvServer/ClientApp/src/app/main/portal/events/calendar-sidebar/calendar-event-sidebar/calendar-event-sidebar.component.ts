import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';

import { CoreSidebarService } from '@core/components/core-sidebar/core-sidebar.service';
import { User } from 'app/main/portal/users/user.model';

import { Event, EventSaveViewModel, EventsViewModel } from '../../events.model';
import { EventsService } from '../../events.service';

@Component({
  selector: 'app-calendar-event-sidebar',
  templateUrl: './calendar-event-sidebar.component.html',
  encapsulation: ViewEncapsulation.None
})
export class CalendarEventSidebarComponent implements OnInit {
  //  Decorator
  @ViewChild('startDatePicker') startDatePicker;
  @ViewChild('endDatePicker') endDatePicker;

  // Public
  user:User;
  public event: Event;
  public startDate:Date;
  public endDate:Date;
  public isDataEmpty;
  public DescriptionString;
  public CategoryType;
  public selectLabel = [
    { label: 'Concerto', bullet: 'primary' },
    { label: 'Club', bullet: 'danger' },
    { label: 'Teatro', bullet: 'warning' },
    { label: 'Festival', bullet: 'success' },
    // { label: 'ETC', bullet: 'info' }
  ];
  // public selectGuest = [
  //   { name: 'Jane Foster', avatar: 'assets/images/avatars/1-small.png' },
  //   { name: 'Donna Frank', avatar: 'assets/images/avatars/3-small.png' },
  //   { name: 'Gabrielle Robertson', avatar: 'assets/images/avatars/5-small.png' },
  //   { name: 'Lori Spears', avatar: 'assets/images/avatars/7-small.png' },
  //   { name: 'Sandy Vega', avatar: 'assets/images/avatars/9-small.png' },
  //   { name: 'Cheryl May', avatar: 'assets/images/avatars/11-small.png' }
  // ];
  public startDateOptions = {
    altInput: true,
    mode: 'single',
    altInputClass: 'form-control flat-picker flatpickr-input invoice-edit-input',
    enableTime: true
  };
  public endDateOptions = {
    altInput: true,
    mode: 'single',
    altInputClass: 'form-control flat-picker flatpickr-input invoice-edit-input',
    enableTime: true
  };

  /**
   *
   * @param {CoreSidebarService} _coreSidebarService
   * @param {CalendarService} _calendarService
   */
  constructor(private _coreSidebarService: CoreSidebarService, private _eventsService: EventsService) {

    this.CategoryType = '';
  }


  // Lifecycle Hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
   ngOnInit(): void {
    // Subscribe to current event changes
    this._eventsService.onCurrentEventChange.subscribe(response => {
      this.event = response;

      // If Event is available
      if (this.event.id != '') {
        this.isDataEmpty = false;
        if (response.id === undefined) {
          this.isDataEmpty = true;
        }
        this.startDate = new Date(this.event.StartTimestamp *1000);
        this.endDate = new Date(this.event.EndTimestamp *1000);
        this.DescriptionString = decodeURI(this.event.Description);
      }
      // else Create New Event
      else {
        this.event = new Event('');

        // Clear Flatpicker Values
        setTimeout(() => {
          this.startDatePicker.flatpickr.clear();
          this.endDatePicker.flatpickr.clear();
          this.startDate = new Date();
          this.endDate = new Date();
        });
        this.isDataEmpty = true;
      }
    });
    this.user =  JSON.parse(localStorage.getItem('currentUser'));
  }
  // Public Methods
  // -----------------------------------------------------------------------------------------------------

  /**
   * Toggle Event Sidebar
   */
  toggleEventSidebar() {
    this._coreSidebarService.getSidebarRegistry('calendar-event-sidebar').toggleOpen();
  }

  /**
   * Add Event
   *
   * @param eventForm
   */
  addEvent(eventForm) {
    if (eventForm.valid) {
      this.toggleEventSidebar();
        var dtStart = new Date(this.startDatePicker.flatpickrElement.nativeElement.children[0].value);
        this.event.StartTimestamp = dtStart.valueOf()/1000;
        var dtEnd= new Date(this.endDatePicker.flatpickrElement.nativeElement.children[0].value);
        this.event.EndTimestamp = dtEnd.valueOf()/1000;
        this.event.Description = encodeURI(this.DescriptionString);
        this.event.CategoryType = this.CategoryType??'';
        var eventVM = new EventSaveViewModel();
        eventVM.Event = this.event;
        eventVM.IdUser = this.user.id;
        this._eventsService.postUpdatedEvent(eventVM);
    }
  }

  /**
   * Update Event
   */
  updateEvent() {
    this.toggleEventSidebar();
    //! Fix: Temp fix till ng2-flatpicker support ng-modal
    var dtStart = new Date(this.startDatePicker.flatpickrElement.nativeElement.children[0].value);
    this.event.StartTimestamp = dtStart.valueOf()/1000;
    var dtEnd= new Date(this.endDatePicker.flatpickrElement.nativeElement.children[0].value);
    this.event.EndTimestamp = dtEnd.valueOf()/1000;
    this.event.Description = encodeURI(this.DescriptionString);
    this.event.CategoryType = this.CategoryType??'';
    var eventVM = new EventSaveViewModel();
        eventVM.Event = this.event;
        eventVM.IdUser = this.user.id;
    this._eventsService.postUpdatedEvent(eventVM);
  }

  /**
   * Delete Event
   */
  deleteEvent() {
    this._eventsService.deleteEvent(this.event);
    this.toggleEventSidebar();
  }

}
