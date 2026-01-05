import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Cart } from './cart/cart';
import { Bill } from './bill/bill';
import { FoodMenu } from './food-menu/food-menu';
import { Login } from './login/login';
import { NavBar } from './nav-bar/nav-bar';
import { Signup } from './signup/signup';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    App,
    Cart,
    Bill,
    FoodMenu,
    Login,
    NavBar,
    Signup
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    NgbModule,
    
  ],
  providers: [
    provideBrowserGlobalErrorListeners()
  ],
  bootstrap: [App]
})
export class AppModule { }
