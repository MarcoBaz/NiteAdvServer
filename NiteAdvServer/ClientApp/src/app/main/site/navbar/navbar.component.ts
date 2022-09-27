import { Component, OnInit } from '@angular/core';
import { SiteService } from '../site.service';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  constructor(public _siteService:SiteService) { }

  ngOnInit(): void {
  }
  loadRoute(path){
    this._siteService.redirectRoute(path);
    path ='';

    window.scroll({ 
      top: 0, 
      left: 0, 
      behavior: 'smooth' 
    });
   }
}
