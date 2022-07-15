import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';

@Injectable({ providedIn: 'root' })
export class AuthGuardMaster implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        let currentUser: any = {};
        const expectedRole = route.data.expectedRole;

        if (expectedRole.find(x => x === 'Master')) { 
            currentUser = this.authenticationService.getCurrentUserMaster(); 
        }
        if (expectedRole.find(x => x === 'Partner')) { 
            currentUser = this.authenticationService.getCurrentUserPartner();
         }

        if (!currentUser) {
            this.router.navigate(['/index'], { queryParams: { returnUrl: state.url } });
            return false;
        }
        if (state.url === '/' || state.url === '/access-denied') {
            return true;
        }

        if (expectedRole == null || !(expectedRole instanceof Array) || expectedRole.length === 0) {
            return this.accessDenied();
        }
        let found = false;

        if (currentUser.role instanceof Array) {
            expectedRole.forEach((e: string) => {
                if (currentUser.role.find(r => r === e) != null) {
                    found = true;
                    return;
                }
            });
        } else {
            if (expectedRole.find(r => r === currentUser.role) != null) {
                return true;
            }
        }
        return found || this.accessDenied();
    }

    accessDenied() {
      this.authenticationService.clearUserMaster();
      this.authenticationService.clearUserPartner();
        this.router.navigate(['/access-denied']);
        return false;
    }
}
