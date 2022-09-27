import { Component, OnInit } from '@angular/core';
import { SiteService } from '../site.service';

@Component({
  selector: 'main-page',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {

  constructor(public _siteService:SiteService) { }

  ngOnInit(): void {
  }
  loadRoute(path){
    this._siteService.redirectRoute(path);
    path ='';
   }
}
