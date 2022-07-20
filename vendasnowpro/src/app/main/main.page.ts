import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.page.html',
  styleUrls: ['./main.page.scss'],
})
export class MainPage implements OnInit {
  public title: string;
  lstCategorys: any[];

  constructor(
    private activatedRoute: ActivatedRoute, 
    private router: Router,
    private authenticationService: AuthenticationService
    ) { }

  ngOnInit() {
    this.title = "VendasNow Pro";
    if (!this.authenticationService.getCurrentUser()) {
      this.router.navigate(['login']);
  }
  }

}
