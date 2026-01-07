import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';

import { TokenService } from '../Services/token-service';
import { Shared } from '../Services/shared';

@Component({
  selector: 'app-cart',
  standalone: false,
  templateUrl: './cart.html',
  styleUrl: './cart.css',
})
export class Cart implements OnInit {
  CartList: any;

  constructor(
    private service: Shared,
    private token: TokenService,
  ) {}

  CurrentLoginId = this.token.getUser().Loginid;
  Cart: any = [];
  CartId: number = 0;
  CartId1: number = 0;
  subTot: number = 0;
  subTot1: number = 0;
  Tot: number = 0;
  PartTotal: number = 0;
  CurrentUser = this.token.getUser();

  ngOnInit(): void {
    this.refreshCartList();
  }
  getSubTotal() {
    return this.CartList.reduce(
      (sum: number, current: any) => sum + current.Price * current.Quantity,
      0,
    );
  }
  getTotal() {
    this.subTot = this.getSubTotal();
    return (this.Tot = this.subTot);
  }
  EditCart(item1: any) {}
  IncQty(item: any) {
    item.Quantity = item.Quantity + 1;
  }
  DecQty(item: any) {
    if (item.Quantity > 1) {
      item.Quantity = item.Quantity - 1;
    } else {
      this.removeFromCart(item);
    }
  }
  refreshCartList() {
    this.service.getCartUserId(this.CurrentLoginId).subscribe((data) => (this.CartList = data));
  }
  removeFromCart(item1: any) {
    this.CartId = item1.CartId;
    if (confirm('Are you sure to remove the item from the cart? ')) {
      this.service.deleteCartItem(this.CartId).subscribe((data) => {
        alert('Removed Successfully');
        this.refreshCartList();
      });
    }
  }
  removeFromCartWC(item1: any) {
    this.CartId1 = item1.CartId;
    this.service.deleteCartItem(this.CartId1).subscribe((data) => {
      console.log(data.toString());
      this.refreshCartList();
    });
  }
  generateBill() {
    this.CartList.forEach((cart: any) => {
      this.PartTotal = cart.Price * cart.Quantity;
      let order = {
        //username: this.CurrentUser.username,
        LoginId: cart.LoginId,
        FoodCode: cart.FoodCode,
        FoodName: cart.FoodName,
        BillingAmount: this.PartTotal,
      };
      this.service.getBillingDetail(order).subscribe((res) => {
        alert(
          'Bill Generated\nUser ID : ' +
            order.LoginId +
            '\nFood Code : ' +
            order.FoodCode +
            '\nFoodName : ' +
            order.FoodName +
            '\nSub Total : ' +
            order.BillingAmount,
        );
        console.log(res.toString());
      });
      this.removeFromCartWC(cart);
    });
    this.refreshCartList();
  }
}
function quantity(Quantity: any) {
  throw new Error('Function not implemented.');
}
