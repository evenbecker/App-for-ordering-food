import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../Services/token-service';
import { Shared } from '../Services/shared';
import * as alertyfy from 'alertifyjs';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})



export class Login implements OnInit {

  constructor(private service:Shared, private router:Router, private token:TokenService) { }

  LoginId:any;
  Password:any;
  LArray:any;
  ngOnInit(): void {
    const token = this.token.getToken();
    //alertyfy.success("token initializing")
    if(token != null)
    { this.router.navigateByUrl('dashboard') }
  }

  cLog(){
    console.log("LoginId: " + this.LoginId);
    this.LArray={
      LoginId: this.LoginId,
      Password: this.Password
    }
    this.service.login(this.LArray).subscribe({
      next: (res:any)=>{
        this.token.saveToken(res.token)
        this.token.saveUser(res)
        alertyfy.success("Login Successful")
        this.router.navigateByUrl('/FoodMenu');
      },
      error: (err) => {
        if(err.status == 401)
          alertyfy.error("User ID or Password is incorrect.");      
        else{
          alertyfy.error("cannot fetch the server");
          console.log(err);
        }
      }
    });
  }

}
