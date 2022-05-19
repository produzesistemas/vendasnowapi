import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';


@Component({
  selector: 'app-app-layout',
  templateUrl: './app-layout.component.html'
})
export class AppLayoutComponent implements OnInit {
  constructor(private router: Router) { }

  ngOnInit() {
  }

}
