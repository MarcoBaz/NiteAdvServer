import { Component, OnInit, Input, Inject, AfterViewInit, ElementRef,OnDestroy,ViewEncapsulation,ViewChild} from '@angular/core';
import { CoreConfigService } from '@core/services/config.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ConfirmComponent } from '../portal/confirm-dialog/confirm.component';
import { ScriptService } from './scripts/script.service';
import { SiteService } from './site.service';

@Component({
  selector: 'app-site',
  templateUrl: './site.component.html',
  styleUrls: ['./site.component.scss']
})
export class SiteComponent implements OnInit, OnDestroy,AfterViewInit {
  @ViewChild("script") script: ElementRef;
  @ViewChild('confirm') modal: ConfirmComponent;
  ShowHome:boolean;
  ShowLogin:boolean;
  ShowContacts:boolean;
  ShowCookies:boolean;
  ShowPrivacy:boolean;
  ShowAbout:boolean;
  url:string;
  private _unsubscribeAll: Subject<any>;
  constructor(public _siteService:SiteService, public _coreConfigService:CoreConfigService, public _scriptService:ScriptService) { 
    this._scriptService.loadAllCss();
    this._scriptService.loadAllScripts();
    this._coreConfigService.config = {
      layout: {
        navbar: {
          hidden: true
        },
        menu: {
          hidden: true
        },
        footer: {
          hidden: true
        },
        customizer: false,
        enableLocalStorage: true
      }
    };
    this._unsubscribeAll = new Subject();
    this.changeRoute('main');
  }
  
  
  
  ngOnInit(): void {
    this._siteService.onRouterChanged
    .pipe(takeUntil(this._unsubscribeAll))
    .subscribe(path => {
      if (path != '')
      {
        if (path == 'login' || path == 'privacy' || path == 'cookies')
        {
          //task delay
          setTimeout(() => {  
            var navbar = document.getElementById("main-nav");
            if (navbar != null)
            {
              navbar.classList.add("navbar-sticky");
            }

           }, 1000);
          
        }
        
        this.changeRoute(path);
        path = '';
      }
   
    });
    this._siteService.onSiteMessage
    .pipe(takeUntil(this._unsubscribeAll))
    .subscribe(message=>{
      
      if (message != '') {
       
       this.modal.openDialog("Grazie!",message,false);
      }
     
    });
  }
  async ngOnDestroy(): Promise<void>
  {
      // Unsubscribe from all subscriptions

      this._scriptService.unloadCss();
      this._scriptService.unloadScripts();
      this._unsubscribeAll.next();
      this._unsubscribeAll.complete();
  // await this.delay(1000);
  }
   delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
  }
  ngAfterViewInit() {
   
 }

 

 changeRoute(path)
  {
    if (path.includes('main') == true)
    {
      this.ShowHome = true;
      this.ShowLogin = false;
      this.ShowContacts = false;
      this.ShowCookies = false;
      this.ShowPrivacy = false;
      this.ShowAbout = false;
    }
    else   if (path.includes('login') == true)
    {
      this.ShowHome = false;
      this.ShowLogin = true;
      this.ShowContacts = false;
      this.ShowCookies = false;
      this.ShowPrivacy = false;
      this.ShowAbout = false;
    }
    else   if (path.includes('contacts') == true)
    {
      this.ShowHome = false;
      this.ShowLogin = false;
      this.ShowContacts = true;
      this.ShowCookies = false;
      this.ShowPrivacy = false;
      this.ShowAbout = false;
    }
    else   if (path.includes('cookies') == true)
    {
      this.ShowHome = false;
      this.ShowLogin = false;
      this.ShowContacts = false;
      this.ShowCookies = true;
      this.ShowPrivacy = false;
      this.ShowAbout = false;
    }
    else   if (path.includes('privacy') == true)
    {
      this.ShowHome = false;
      this.ShowLogin = false;
      this.ShowContacts = false;
      this.ShowCookies = false;
      this.ShowPrivacy = true;
      this.ShowAbout = false;
    }
    else   if (path.includes('about') == true)
    {
      this.ShowHome = false;
      this.ShowLogin = false;
      this.ShowContacts = false;
      this.ShowCookies = false;
      this.ShowPrivacy = false;
      this.ShowAbout = true;
    }
  }
}
