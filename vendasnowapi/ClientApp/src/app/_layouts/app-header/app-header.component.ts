import { Component, OnInit } from '@angular/core';
// import { AuthenticationService } from 'src/app/_services/authentication.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html'
})
export class AppHeaderComponent implements OnInit {

  public menus: any[];
  public currentUser;
  constructor(  
                private router: Router,
                private toastr: ToastrService) { }

  ngOnInit() {

  }


}
