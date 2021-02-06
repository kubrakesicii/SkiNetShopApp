import {v4 as uuidv4} from "uuid";

export interface IBasket {
    id: string;
    items: IBasketItem[];
  }
  
  export interface IBasketItem {
    id: number;
    name: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
  }

  export class Basket implements IBasket{
      id= uuidv4(); //when we create a basket, it has a unique id
      items: IBasketItem[] = [];
  }

  export interface IBasketTotals{
    shipping : number;
    subtotal : number;
    total : number;
  }