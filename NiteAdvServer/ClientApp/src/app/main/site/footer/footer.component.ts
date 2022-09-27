import { Component, OnInit } from '@angular/core';
import { SiteService } from '../site.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  constructor(public _siteService:SiteService) { }

  ngOnInit(): void {
  }
  loadRoute(path){
    this._siteService.redirectRoute(path);
    window.scroll({ 
      top: 0, 
      left: 0, 
      behavior: 'smooth' 
    });
   }

}
