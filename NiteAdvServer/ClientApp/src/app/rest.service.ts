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
    //console.log(`Credentials token=${JSON.stringify(jsonString)}`)
    let resp = new Response('getCredentials');
    return this.http.post<any>(endpoint + 'getlogin', jsonString, httpOptions).pipe(
      
        tap((response) => {
            resp =response;
            //console.log(`Response credential received =${JSON.stringify(response)}`)
        }),
        catchError(this.handleError(resp))
    );
  }
  getCompanyData(filter): Observable<any> {
    let resp = new Response('getTourList');
    return this.http.post<any>(endpoint + 'getCompanyList', filter,httpOptions).pipe(
      tap((serv) => {
        resp = serv;
        //console.log(`Service saved id=${JSON.stringify(resp)}`)
      }),
      catchError(this.handleError(resp))
    );
  }

 /* SaveCompany(company): Observable<any> {
    var cmp = JSON.stringify(company);
    let resp = new Response('saveCompany');
    return this.http.post<any>(endpoint + 'saveCompany', cmp,httpOptions).pipe(
      tap((Company) => {
        resp = Company;
      }),
      catchError(
        this.handleError(resp,error)
        )
    );
  }*/
  SaveCompany(company): Observable<any> {
    ////console.log(`Save token=${JSON.stringify(user)}`
    //console.log(`Ready To upload=${JSON.stringify(RenterOffice)}`)
    var cmp = JSON.stringify(company);
    let resp = new Response('saveCompany');
    return this.http.post<any>(endpoint + 'SaveCompany', company,httpOptions).pipe(
      tap((Company) => {
        resp = Company;
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