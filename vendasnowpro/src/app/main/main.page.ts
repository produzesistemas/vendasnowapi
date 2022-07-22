import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

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
    ) { }

  ngOnInit() {
    this.title = "Home";
    
  }

}
