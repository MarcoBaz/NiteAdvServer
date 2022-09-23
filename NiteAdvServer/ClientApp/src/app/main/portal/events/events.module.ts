
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';

import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import listPlugin from '@fullcalendar/list';
import timeGridPlugin from '@fullcalendar/timegrid';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { Ng2FlatpickrModule } from 'ng2-flatpickr';

import { CoreCommonModule } from '@core/common.module';
import { CoreSidebarModule } from '@core/components';

import { CalendarEventSidebarComponent } from './calendar-sidebar/calendar-event-sidebar/calendar-event-sidebar.component';


import { EventsService } from './events.service';
import { EventsComponent } from 'app/main/portal/events/events.component';
import { ConfirmModule } from '../confirm-dialog/confirm.module';

FullCalendarModule.registerPlugins([dayGridPlugin, timeGridPlugin, listPlugin, interactionPlugin]);

// routing
const routes: Routes = [
  {
    path: 'events',
    component: EventsComponent,
    resolve: {
      data: EventsService
    },
    data: { animation: 'calendar' }
  }
];
@NgModule({
  declarations: [EventsComponent, CalendarEventSidebarComponent],
  imports: [
    CommonModule,
    FullCalendarModule,
    RouterModule.forChild(routes),
    CoreCommonModule,
    CoreSidebarModule,
    FormsModule,
    Ng2FlatpickrModule,
    NgSelectModule,
    NgbModule,
    ConfirmModule
  ],
  providers: [EventsService]
})
export class EventsModule {}
