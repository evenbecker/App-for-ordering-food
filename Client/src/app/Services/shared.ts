import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Shared {

  readonly APIUrl = "https://localhost:5001/api";

constructor(private http:HttpClient) { }

login(value:any){
  return this.http.post(this.APIUrl+'/Account/authenticate', value);
}

addUser(value:any){
  return this.http.post(this.APIUrl+'/Account', value);
}
//FoodDetail Controller in Server
getFoodDetail():Observable<any[]>{
  return this.http.get<any>(this.APIUrl+'/FoodMenu');
}

//Cart Controller in Server
// getCart():Observable<any[]>{
//   return this.http.get<any>(this.APIUrl+'/Cart');
// }

getCartUserId(value:any):Observable<any[]>{
  return this.http.get<any>(this.APIUrl+'/Cart/'+value);
}

addCart(value:any){
  return this.http.post(this.APIUrl+'/Cart', value);
}

deleteCartItem(value:number){
  return this.http.delete(this.APIUrl+'/Cart/'+value);
}

//BillingDetail Controller in Server
getBillingDetail(value:any):Observable<any[]>{
  return this.http.post<any>(this.APIUrl+'/billingdetail/',value);
}

getBillingDetail1(value:any):Observable<any[]>{
  return this.http.get<any>(this.APIUrl+'/billingdetail/'+value);
}

EditCart(value:any){
  return this.http.put(this.APIUrl+'/billingdetail', value);
}
deleteOrderItem(value:any)

  {

    return this.http.delete(this.APIUrl+'/billingdetail/'+value)



  }
}



