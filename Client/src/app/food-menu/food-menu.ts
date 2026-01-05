import { Component, OnInit } from '@angular/core';
import { TokenService } from '../Services/token-service';
import { Shared } from '../Services/shared';
import * as alertyfy from 'alertifyjs';

@Component({
  selector: 'app-food-menu',
  standalone: false,
  templateUrl: './food-menu.html',
  styleUrl: './food-menu.css',
})

export class FoodMenu implements OnInit {

  constructor(private service:Shared, private token:TokenService) { }

  FoodList:any=[];
  CurrentLoginId = this.token.getUser().Loginid;
  Food:any;

  ngOnInit(): void {
    this.refreshFoodList();
  }

  refreshFoodList(){
    this.service.getFoodDetail().subscribe((data: any)=>{
      this.FoodList=data;
    });
  }

  addToCart(item1:any){
    this.Food = item1;
    this.Food = {
      LoginId: this.CurrentLoginId,
      FoodCode: this.Food.FoodCode,
      FoodName: this.Food.FoodName,
      Price:this.Food.Price,
      Quantity: 1,
    };
    this.service.addCart(this.Food).subscribe(res=>{
      alertyfy.success("Added Successfully");
      });
  }

}
