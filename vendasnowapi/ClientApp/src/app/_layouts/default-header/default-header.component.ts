import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html'
})
export class DefaultHeaderComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

  onLogin() {
    this.router.navigate(['login-empresa']);
  }

}
