import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'environments/environment';
//import { User, Role } from 'app/auth/models';
import { User} from 'app/main/portal/users/user.model';
import { ToastrService } from 'ngx-toastr';
import { FacebookService } from 'ngx-facebook';
import { RestService } from 'app/rest.service';
import { FacebookLoginViewModel, LoginViewModel } from 'app/user.model';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  //public
  public currentUser: Observable<User>;
  onLoginError: BehaviorSubject<string>;
  //private
  private currentUserSubject: BehaviorSubject<User>;
  public onUserLogged: BehaviorSubject<any>;
  /**
   *
   * @param {HttpClient} _http
   * @param {ToastrService} _toastrService
   */
  constructor(private _http: HttpClient,
    private _rest:RestService,
    private _route:Router,
    private _fbService:FacebookService) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
    this.onLoginError = new BehaviorSubject('');
    this.onUserLogged = new BehaviorSubject({});
  }

  // getter: currentUserValue
  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  /**
   *  Confirms if user is admin
   */
  get isAdmin() {
    return this.currentUser != null && !this.currentUserSubject.value.IsAdmin;
  }

  /**
   *  Confirms if user is client
   */
  get isClient() {
    return this.currentUser && !this.currentUserSubject.value.IsAdmin;
  }

  /**
   * User login
   *
   * @param email
   * @param password
   * @returns user
   */
   Login(login,password): Promise<any> {
    return new Promise((resolve, reject) => {
    var user = new LoginViewModel(login,password);
      this._rest.getCredentials(user).subscribe((response: any) => {
     // console.log(JSON.stringify(response));
         if (response.Error == '' )
         {
           var user = response.Data;
           //UserLogged.setValues(response.Data);
           localStorage.setItem('currentUser', JSON.stringify(response.Data));
           this.currentUserSubject.next(user);
           this.onUserLogged.next(user);
           resolve(response);
          /* setTimeout(() => {
            this._toastrService.success(
              'You have successfully logged in as an ' +
                user.Name +
                ' user to Vuexy. Now you can start to explore. Enjoy! 🎉',
              '👋 Welcome, ' + user.firstName + '!',
              { toastClass: 'toast ngx-toastr', closeButton: true }
            );
          }, 2500);*/

          // notify
          
          
         }
         else
         {
  
          this.onLoginError.next(response.Error);
           // throw new Error(response.Error);
         }
       
      }, reject);
  
    }
    );
  }
  apiAuthenticate(accessToken: string,expire:Date,userId:string): Promise<any>  {
    var fbLogin = new FacebookLoginViewModel(accessToken,expire,userId);
 
        return new Promise((resolve, reject) => {
            this._rest.FacebookAuthentication(fbLogin).subscribe((response: any) => {
           // console.log(JSON.stringify(response));
               if (response.Error == '' )
               {
                var user:User = response.Data;
                 //UserLogged.setValues(response.Data);
                 localStorage.setItem('currentUser', JSON.stringify(user));
                 this.currentUserSubject.next(user);
                 this.onUserLogged.next(response.Data);
                // this._route.navigate(['/dashboard']);
                 //this.onUserLogged.next(user);
                 resolve(user);
                
               }
               else
               {
                this.onLoginError.next(response.Error);
                 // throw new Error(response.Error);
               }
             
            }, reject);
        
          }
          );
  }

  /*login(email: string, password: string) {
    return this._http
      .post<any>(`${environment.apiUrl}/users/authenticate`, { email, password })
      .pipe(
        map(user => {
          // login successful if there's a jwt token in the response
          if (user && user.token) {
            // store user details and jwt token in local storage to keep user logged in between page refreshes
            localStorage.setItem('currentUser', JSON.stringify(user));

            // Display welcome toast!
            setTimeout(() => {
              this._toastrService.success(
                'You have successfully logged in as an ' +
                  user.role +
                  ' user to Vuexy. Now you can start to explore. Enjoy! 🎉',
                '👋 Welcome, ' + user.firstName + '!',
                { toastClass: 'toast ngx-toastr', closeButton: true }
              );
            }, 2500);

            // notify
            this.currentUserSubject.next(user);
          }

          return user;
        })
      );
  }*/

  /**
   * User logout
   *
   */
  logout() {
    // remove user from local storage to log user out
    this._fbService.logout().then(()=>{
      console.log('Facebook loggd out');
    });

    localStorage.removeItem('currentUser');
    // notify
    this.currentUserSubject.next(null);
  }
}
