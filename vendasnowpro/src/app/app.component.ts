import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from './_services/authentication.service';
@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit {
  lstCategorys: any[];
  toggle = false;
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService) {}

  ngOnInit() {
    if (!this.authenticationService.getCurrentUser()) {
      this.router.navigate(['login']);
  }
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate([`login`]);
  }

  sidemenuClick(category) {
    category.open ? category.open = false : category.open = true;
  }

  filter(subcategory) {
    this.router.navigate([`establishment/${subcategory.id}`]);
  }
}
