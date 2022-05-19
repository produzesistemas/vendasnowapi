import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-default-layout',
  templateUrl: './default-layout.component.html'
})
export class DefaultLayoutComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

}
