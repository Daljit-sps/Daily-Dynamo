import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'dd-auth-hero',
  templateUrl: './auth-hero.component.html',
  styleUrls: ['./auth-hero.component.scss'],
})
export class AuthHeroComponent {
  isLoginPage:boolean = false;
  constructor (private Router: ActivatedRoute, private Route: Router){
    this.isLoginPage = this.Route.url == "/auth"
    
  }
}
