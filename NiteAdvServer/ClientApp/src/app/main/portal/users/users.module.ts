import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CsvModule } from '@ctrl/ngx-csv';
import { TranslateModule } from '@ngx-translate/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { CoreCommonModule } from '@core/common.module';
import { CardSnippetModule } from '@core/components/card-snippet/card-snippet.module';
import { ContentHeaderModule } from 'app/layout/components/content-header/content-header.module';
import { UsersService } from './users.service';
import { UsersComponent } from './users.component';
import { UserFormComponent } from './user-form/user-form.component';
import { BlockUIModule } from 'ng-block-ui';
import { ConfirmModule } from '../confirm-dialog/confirm.module';


const routes: Routes = [
  {
    path: 'users',
    component: UsersComponent,
    resolve: {
      datatables: UsersService
    },
    data: { animation: 'datatables' }
  }
];

@NgModule({
  declarations: [
    UsersComponent,
    UserFormComponent
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
    ConfirmModule
  ],
  providers: [UsersService],
  entryComponents: [
    UserFormComponent
  ]
})
export class UsersModule {}