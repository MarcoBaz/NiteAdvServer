
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CsvModule } from '@ctrl/ngx-csv';
import { TranslateModule } from '@ngx-translate/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { CoreCommonModule } from '@core/common.module';
import { CardSnippetModule } from '@core/components/card-snippet/card-snippet.module';
import { ContentHeaderModule } from 'app/layout/components/content-header/content-header.module';
import { EventlistService } from './eventlist.service';
import { EventlistComponent } from './eventlist.component';
import { EventlistFormComponent } from './eventlist-form/eventlist-form.component';
import { BlockUIModule } from 'ng-block-ui';
import { ConfirmModule } from '../confirm-dialog/confirm.module';
import { FileUploadModule } from 'ng2-file-upload';
import { QuillModule } from 'ngx-quill';
const routes: Routes = [
  {
    path: 'eventlist',
    component: EventlistComponent,
    resolve: {
      datatables: EventlistService
    },
    data: { animation: 'datatables' }
  }
];

@NgModule({
  declarations: [
    EventlistComponent,
    EventlistFormComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    NgbModule,
    TranslateModule,
    CoreCommonModule,
    ContentHeaderModule,
    CardSnippetModule,
    NgxDatatableModule,
    BlockUIModule.forRoot(),
    ConfirmModule,
    FileUploadModule,
    QuillModule.forRoot(),
  ],
  providers: [EventlistService],
  entryComponents: [
    EventlistFormComponent
  ]
})
export class EventlistModule {}