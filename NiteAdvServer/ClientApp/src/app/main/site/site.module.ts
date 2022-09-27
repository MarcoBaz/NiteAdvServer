import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';  
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FooterComponent } from './footer/footer.component';
import { SiteComponent } from './site.component';
import { SiteService } from './site.service';
import { ScriptService } from './scripts/script.service';
import { NavbarComponent } from './navbar/navbar.component';
import { ContactsComponent } from './contacts/contacts.component';
import { LoginComponent } from './login/login.component';
import { CookiesComponent } from './cookies/cookies.component';
import { PrivacyComponent } from './privacy/privacy.component';
import { ConfirmModule } from 'app/main/portal/confirm-dialog/confirm.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AboutComponent } from './about/about.component';
const routes = [
  {
      path     : 'site',
      component: SiteComponent,
      resolve  : {
         data: SiteService
      }
    },
    
    
];
@NgModule({
  declarations: [MainComponent,FooterComponent, SiteComponent, NavbarComponent, ContactsComponent, LoginComponent, CookiesComponent, PrivacyComponent, AboutComponent],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    ConfirmModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [ ScriptService]
})
export class SiteModule { }
