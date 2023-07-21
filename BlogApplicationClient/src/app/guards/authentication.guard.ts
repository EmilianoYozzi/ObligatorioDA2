import { state } from "@angular/animations";
import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { ActivatedRouteSnapshot } from "@angular/router";
import { RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";



@Injectable({
    providedIn: 'root'
})

export class AuthenticationGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(
      route: ActivatedRouteSnapshot, 
      state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean { 
      const userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
      const isLoggedIn = userInfo && userInfo.token ? true : false;
      if (!isLoggedIn) {
          this.router.navigate(['/login']);  // redirect to login page if not logged in
      }
      return isLoggedIn;
  }
}





