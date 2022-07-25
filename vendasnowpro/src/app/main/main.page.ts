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
  currentUser;
  constructor(
    private activatedRoute: ActivatedRoute, 
    private router: Router,
    private authenticationService: AuthenticationService
    ) { }

  ngOnInit() {
    this.title = "Home";
    this.authenticationService.getObject().then((data: any) => {
      this.currentUser = data;

    });
  }

  logout() {
    this.authenticationService.clear();
    this.router.navigateByUrl('/login', { replaceUrl: true });
  }

}
