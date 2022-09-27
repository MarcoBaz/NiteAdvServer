import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { RestService } from 'app/rest.service';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { ContactViewModel } from './site.model';


@Injectable({
  providedIn: 'root'
})
export class SiteService implements Resolve<any>{
  routeParam: any;
  onRouterChanged: BehaviorSubject<string>;
  onSiteMessage:BehaviorSubject<string>;
  path:string;
  constructor(private _rest:RestService) { 
    this.onRouterChanged = new BehaviorSubject('');
    this.onSiteMessage = new BehaviorSubject('');
  }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    this.routeParam = route.params;
   // console.log(`Route params: ${this.routeParam}`);
    return new Promise((resolve, reject) => {
      //alert('sono in resolve');
      Promise.all([
        //this.getAllNations()
      ]).then(
        ([]) => {
          resolve('');
        },
        reject
      );
    });
  }

  
  redirectRoute(path): Promise<any> {
    return new Promise((resolve, reject) => {
      this.path = path;
      this.onRouterChanged.next(this.path);
      resolve(path);
    });
  }

  SendContact(email,isOwner,text): Promise<any> {
    return new Promise((resolve, reject) => {
    var contact = new ContactViewModel(email,isOwner,text);
      this._rest.SendContact(contact).subscribe((response: any) => {
     // console.log(JSON.stringify(response));
         if (response.Error == '' )
         {
           var resp = response.Data;
           this.onSiteMessage.next('Il tuo messaggio è stato inviato, ti ricontatteremo al più presto.');
           resolve(response);
       
          
         }
         else
         {
  
          this.onSiteMessage.next("Errore nell'invio dei dati");
           // throw new Error(response.Error);
         }
       
      }, reject);
  
    }
    );
  }
}
