import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Login } from './login/login';
import { Signup } from './signup/signup';
import { FoodMenu } from './food-menu/food-menu';
import { Cart } from './cart/cart';
import { Bill } from './bill/bill';

const routes: Routes = [
  {path:'login',component:Login},

  {path:'signup',component:Signup},

  {path:'FoodMenu',component:FoodMenu},

  {path:'Cart',component:Cart},

  {path:'bill',component:Bill},


  { path:'**', pathMatch:'full',redirectTo:'/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
