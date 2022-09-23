import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { Response } from './response.model';
import { Company } from './main/portal/companies/companies.model';

//l'endpoint giusto si trova sul proxy, questo e l'indirizzo locale
// NB: se vado direttamente sull'endpoint corretto, il browser mi restituisce una COARS exception
//perchè non è permesso la chiamata backend su un differente dominio
//const endpoint = 'http://localhost:7267/Api/'; 
const endpoint = '/Web/'; 
const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      //'Access-Control-Allow-Methods': 'POST',
      "Access-Control-Allow-Headers": "Content-Type,Access-Control-Allow-Headers,Authorization",
      'Access-Control-Allow-Credentials': 'true'
       // 'Access-Control-Allow-Origin': '*'
    })
};

const httpMultipartOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'multipart/form-data',
        //'Access-Control-Allow-Methods': 'POST',
       // "Access-Control-Allow-Headers": "Content-Type,Access-Control-Allow-Headers,Authorization",
        'Access-Control-Allow-Credentials': 'true'

    })
};
@Injectable()
// @Injectable({
//     providedIn: 'root' // just before your class
//   })
export class RestService {

  constructor(private http: HttpClient) { tap(_ => console.log("Rest service started")) }

  getCredentials(jsonString): Observable<any> {
    let resp = new Response('getCredentials');
    return this.http.post<any>(endpoint + 'getlogin', jsonString, httpOptions).pipe(
      
        tap((response) => {
            resp =response;
            //console.log(`Response credential received =${JSON.stringify(response)}`)
        }),
        catchError(this.handleError(resp))
    );
  }
  FacebookAuthentication(facebookLogin): Observable<any> {
    let resp = new Response('getFacebookCredentials');
    return this.http.post<any>(endpoint + 'authFacebook', facebookLogin, httpOptions).pipe(
      
        tap((response) => {
            resp =response;
            //console.log(`Response credential received =${JSON.stringify(response)}`)
        }),
        catchError(this.handleError(resp))
    );
  }
  getUserData(filter): Observable<any> {
    let resp = new Response('getUserData');
    return this.http.post<any>(endpoint + 'getUsersList', filter,httpOptions).pipe(
      tap((serv) => {
        resp = serv;
        //console.log(`Service saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }

  SaveUser(user): Observable<any> {
    ////console.log(`Save token=${JSON.stringify(user)}`
    //console.log(`Ready To upload=${JSON.stringify(RenterOffice)}`)
    let resp = new Response('saveUser');
    return this.http.post<any>(endpoint + 'SaveUser', user,httpOptions).pipe(
      tap((Company) => {
        resp = Company;
        //console.log(`Renter saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }
  getCompanyData(filter): Observable<any> {
    let resp = new Response('getUsersList');
    return this.http.post<any>(endpoint + 'getCompanyList', filter,httpOptions).pipe(
      tap((serv) => {
        resp = serv;
        //console.log(`Service saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }

  SaveCompany(filter): Observable<any> {
    let resp = new Response('saveCompany');
    return this.http.post<any>(endpoint + 'SaveCompany', filter,httpOptions).pipe(
      tap((Company) => {
        resp = Company;
        //console.log(`Renter saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }
  DeleteCompany(filter): Observable<any> {
    let resp = new Response('DeleteCompany');
    return this.http.post<any>(endpoint + 'DeleteCompany', filter,httpOptions).pipe(
      tap((Company) => {
        resp = Company;
        //console.log(`Renter saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }
  SaveCompanyImage(file): Observable<any> {
    ////console.log(`Save token=${JSON.stringify(user)}`
    // //console.log(`Ready To upload=${user}`)
    let resp = new Response('SaveCompanyImage');
    return this.http.post<any>(endpoint + 'SaveCompanyImage', file, httpMultipartOptions).pipe(
      tap((Renter) => {
        resp = Renter;
        //console.log(`saveImageService saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }

  CompanyBlackList(filter): Observable<any> {
    ////console.log(`Save token=${JSON.stringify(user)}`
    //console.log(`Ready To upload=${JSON.stringify(RenterOffice)}`)
    let resp = new Response('CompanyBlackList');
    return this.http.post<any>(endpoint + 'CompanyBlackList', filter,httpOptions).pipe(
      tap((res) => {
        resp = res;
        //console.log(`Renter saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }
  getUserEvents(viewmodel): Observable<any> {
    let resp = new Response('getEventsList');
    return this.http.post<any>(endpoint + 'getUserEventsList', viewmodel,httpOptions).pipe(
      tap((serv) => {
        resp = serv;
        //console.log(`Service saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }

  SaveUserEvent(eventVM): Observable<any> {
    ////console.log(`Save token=${JSON.stringify(user)}`
    //console.log(`Ready To upload=${JSON.stringify(RenterOffice)}`)
    let resp = new Response('saveEvent');
    return this.http.post<any>(endpoint + 'SaveUserEvent', eventVM,httpOptions).pipe(
      tap((Event) => {
        resp = Event;
        //console.log(`Renter saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }

  getEventsData(filter): Observable<any> {
    let resp = new Response('getEventsData');
    return this.http.post<any>(endpoint + 'GetEventsList', filter,httpOptions).pipe(
      tap((serv) => {
        resp = serv;
        //console.log(`Service saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }

  SaveEvent(filter): Observable<any> {
    let resp = new Response('saveEvent');
    return this.http.post<any>(endpoint + 'saveEvent', filter,httpOptions).pipe(
      tap((Company) => {
        resp = Company;
        //console.log(`Renter saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }
  DeleteEvent(filter): Observable<any> {
    let resp = new Response('DeleteEvent');
    return this.http.post<any>(endpoint + 'DeleteEvent', filter,httpOptions).pipe(
      tap((Company) => {
        resp = Company;
        //console.log(`Renter saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }
  SaveEventImage(file): Observable<any> {
    ////console.log(`Save token=${JSON.stringify(user)}`
    // //console.log(`Ready To upload=${user}`)
    let resp = new Response('SaveEventImage');
    return this.http.post<any>(endpoint + 'SaveEventImage', file, httpMultipartOptions).pipe(
      tap((Renter) => {
        resp = Renter;
        //console.log(`saveImageService saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }
  SendContact(contact): Observable<any> {
    let resp = new Response('SendContact');
    return this.http.post<any>(endpoint + 'SendContact', contact, httpOptions).pipe(
      
        tap((response) => {
            resp =response;
            //console.log(`Response credential received =${JSON.stringify(response)}`)
        }),
        catchError(this.handleError(resp))
    );
  }
  /* Events list */
  EventsBlackList(filter): Observable<any> {
    ////console.log(`Save token=${JSON.stringify(user)}`
    //console.log(`Ready To upload=${JSON.stringify(RenterOffice)}`)
    let resp = new Response('EventsBlackList');
    return this.http.post<any>(endpoint + 'EventsBlackList', filter,httpOptions).pipe(
      tap((res) => {
        resp = res;
        //console.log(`Renter saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }
  private handleError<T>(resp :Response, errorMessage?: any) {
    return (error: any): Observable<Response> => {
        //alert('sono in handle error');
        // TODO: send the error to remote logging infrastructure
        //console.error(error); // log to //console instead
        resp.Error = error;
        resp.Data = null;
        // TODO: better job of transforming error for user consumption
        //console.log(`${resp.Operation} failed: ${error}`);

        // Let the app keep running by returning an empty result.
        return of(resp as Response); //of(result as T);
    };
  }

  
}