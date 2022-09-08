import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompaniesModule } from './companies/companies.module';
import { ConfirmModule } from './confirm-dialog/confirm.module';
import { EventsModule } from './events/events.module';
import { UsersModule } from './users/users.module';


@NgModule({
  declarations: [
  ],
  imports: [CompaniesModule, EventsModule,UsersModule, ConfirmModule],
  entryComponents: [

  ]
})

export class PortalModule {}
