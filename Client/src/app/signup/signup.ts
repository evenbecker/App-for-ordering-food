import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Shared } from '../Services/shared';

@Component({
  selector: 'app-signup',
  standalone: false,
  templateUrl: './signup.html',
  styleUrl: './signup.css',
})

export class Signup implements OnInit {

  constructor(private service:Shared, private router:Router) { }
  LoginId :string | undefined;
  UserName:string | undefined;
  Phone:number | undefined;
  Address:string | undefined;
  Password:string | undefined;
  UserList:any;

  ngOnInit() {
  }

  signUp(){
    this.UserList ={
      LoginId:this.LoginId,
      UserName: this.UserName,
      Phone: this.Phone,
      Address: this.Address,
      Password: this.Password
    }
    this.service.addUser(this.UserList).subscribe(res=>
      alert(res.toString())
    );
    this.router.navigateByUrl('/login');
  }

}
