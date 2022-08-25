import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CsvModule } from '@ctrl/ngx-csv';
import { TranslateModule } from '@ngx-translate/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { CoreCommonModule } from '@core/common.module';
import { CardSnippetModule } from '@core/components/card-snippet/card-snippet.module';
import { ContentHeaderModule } from 'app/layout/components/content-header/content-header.module';
import { CompaniesService } from './companies.service';
import { CompaniesComponent } from './companies.component';
import { ConfirmComponent } from 'app/main/portal/confirm-dialog/confirm.component';
import { CompanyFormComponent } from './company-form/company-form.component';
import { BlockUIModule } from 'ng-block-ui';

const routes: Routes = [
  {
    path: 'companies',
    component: CompaniesComponent,
    resolve: {
      datatables: CompaniesService
    },
    data: { animation: 'datatables' }
  }
];

@NgModule({
  declarations: [
    CompaniesComponent,
    ConfirmComponent,
    CompanyFormComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    NgbModule,
    TranslateModule,
    CoreCommonModule,
    ContentHeaderModule,
    CardSnippetModule,
    NgxDatatableModule,
    BlockUIModule.forRoot()
  ],
  providers: [CompaniesService],
  entryComponents: [
    ConfirmComponent,
    CompanyFormComponent
  ]
})
export class CompaniesModule {}
