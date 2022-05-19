import { Directive, Input, ElementRef, HostListener, OnInit } from '@angular/core';
// import { AuthenticationService } from '../_services/authentication.service';
import jwt_decode from "jwt-decode";

@Directive({
  selector: '[permission]'
})
export class PermissionDirective implements OnInit {
  @Input('permission') roles: [];

  constructor(private el: ElementRef
    // , private authService: AuthenticationService
    ) {
  }

  ngOnInit() {
    // let auxUser = this.authService.getCurrentUser();
    // const decoded = jwt_decode(auxUser.token);
        
    //     if (decoded) {
    //         let value2: any = decoded;
    //         auxUser.role = value2.role;
    //     }
    // let found = false;
    // if(auxUser == null)
    //   return;
      
    // if (auxUser.role instanceof Array) {
    //   this.roles.forEach(function (e) {
    //     if (auxUser.role.find(r => r == e) != null) {
    //       found = true;
    //       return;
    //     }
    //   });
    // } else {
    //   if (this.roles.find(r => r == auxUser.role) != null) {
    //     found = true;
    //     return;
    //   }
    // }
    // if (!found) {
    //   const container = this.el.nativeElement.parentElement;
    //   this.el.nativeElement.remove();
    //   if (container.childElementCount === 0) {
    //     container.remove();
    //   }
    // }
  }
}
