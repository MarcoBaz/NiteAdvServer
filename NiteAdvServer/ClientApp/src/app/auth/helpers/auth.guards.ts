import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthenticationService } from 'app/auth/service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  /**
   *
   * @param {Router} _router
   * @param {AuthenticationService} _authenticationService
   */
  constructor(private _router: Router, private _authenticationService: AuthenticationService) {}

  // canActivate
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const currentUser = this._authenticationService.currentUserValue;

    if (currentUser != null && currentUser.id != '') {
      // check if route is restricted by role
      // if (route.data.roles && route.data.roles.indexOf(currentUser.role) === -1) {
      //   // role not authorised so redirect to not-authorized page
      //   this._router.navigate(['/pages/miscellaneous/not-authorized']);
      //   return false;
      // }

      // authorised so return true
      return true;
    }
    else
        this._router.navigate(['/site'], { queryParams: { returnUrl: state.url } });

    // not logged in so redirect to login page with the return url
   
    return false;
  }
}
