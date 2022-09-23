// export class EventRef {
//     id? = undefined;
//     url: string;
//     title: string = '';
//     start: string;
//     end: string;
//     allDay = false;
//     calendar: '';
//     extendedProps = {
//       location: '',
//       description: '',
//       addGuest: []
//     };
//   }
export class EventCalendar {
    id? = undefined;
    url: string;
    title: string = '';
    start: string;
    end: string;
    allDay = false;
    calendar: string;
    extendedProps = {
      location: '',
      description: '',
      addGuest: []
    };
  }
  export class FilterEvent {
    EventToSave: Event;
    City: string;
    Where: string;
    Offset: number;
    PageSize: number;
    TotalItems: number;
    CheckedEvents: Event[];
}

export class EventResponse{
     EventList: Event[];
     ItemsCount :number;
}
  export class Event{
  
    constructor(event) {
   
      this.id = event.id || '';
      this.pk = event.pk || 'pk';
      this.label = event.label || 'event';
      this.Url = event.Url || '';
      this.Name = event.Name || '';
      this.Description = event.Description || '';
      this.StartTimestamp = event.StartTimestamp || 0;
      this.EndTimestamp = event.EndTimestamp || 0;
      this.Latitude = event.Latitude || 0;
      this.Longitude = event.Longitude || 0;
      this.Image = event.Image || '';
      this.CategoryType = event.CategoryType || '';
      this.DiscoveryCategories = event.DiscoveryCategories || '';
      this.Place = event.Place || '';
      this.TicketingContext = event.TicketingContext || '';
      this.TicketUrl = event.TicketUrl || '';
      this.UsersGoing = event.UsersGoing || 0;
      this.UsersInterested = event.UsersInterested || 0;
      this.AllDay = event.AllDay || false;
      this.LastSyncDate = event.LastSyncDate || new Date().valueOf();
      this.IsInBlackList = event.IsInBlackList || false;
      this.Deleted = event.Deleted || false;
  }
    id: string;
    pk:string;
    label:string;
    LastSyncDate: number;
    Url: string;
    Name: string;
    Description: string;
    StartTimestamp: number;
    EndTimestamp: number;
    Image: string;
    Latitude: number;
    Longitude: number;
    CategoryType: string;
    DiscoveryCategories: string;
    Place: string;
    TicketingContext: string;
    TicketUrl: string;
    UsersGoing: number;
    UsersInterested: number;
    AllDay: boolean;
    IsInBlackList: boolean;
    Deleted: boolean;
  }
export class EventsViewModel {
    IdUser: string;
    ActualMonth: number;
}
export class EventSaveViewModel {
  IdUser: string;
  Event: Event;
}
export class EventsResponse{
     EventsList: Event[];
     ItemsCount :number;
}  