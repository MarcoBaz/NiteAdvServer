import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompaniesModule } from './companies/companies.module';
import { EventsModule } from './events/events.module';

@NgModule({
  declarations: [
  ],
  imports: [CompaniesModule, EventsModule],

})

export class PortalModule {}
