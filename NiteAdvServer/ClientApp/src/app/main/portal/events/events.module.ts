import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { CoreCommonModule } from '@core/common.module';
import { CardSnippetModule } from '@core/components/card-snippet/card-snippet.module';
import { ContentHeaderModule } from 'app/layout/components/content-header/content-header.module';

import { EventsComponent } from 'app/main/portal/events/events.component';

const routes: Routes = [
  {
    path: 'events',
    component: EventsComponent,
    data: { animation: 'table' }
  }
];

@NgModule({
  declarations: [EventsComponent],
  imports: [RouterModule.forChild(routes), NgbModule, ContentHeaderModule, CardSnippetModule, CoreCommonModule]
})
export class EventsModule {}
