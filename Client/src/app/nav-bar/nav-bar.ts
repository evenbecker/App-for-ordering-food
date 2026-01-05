import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../Services/token-service';

@Component({
  selector: 'app-nav-bar',
  standalone: false,
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css',
})

export class NavBar implements OnInit {

  constructor(private token:TokenService, private router:Router) { }

  CurrentUser = this.token.getUser();

  ngOnInit(): void {
  }

  onLogout(){
    this.token.signOut();
    this.router.navigate(['/login']);
  }

  navigFood(){
    this.router.navigate(['/FoodMenu']);
  }

  navigCart(){
    this.router.navigate(['/Cart']);
  }

  navigBill(){
    this.router.navigate(['/bill']);
  }

}
