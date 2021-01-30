import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { error } from 'protractor';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { ShopParams } from '../shared/models/shopParams';
import { IType } from '../shared/models/type';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  products : IProduct[];
  brands : IBrand[];
  types : IType[];
  @ViewChild('search',{static:true}) searchTerm : ElementRef;
  
  shopParams = new ShopParams();
  totalCount : number;
  sortOptions = [
    {name:"Alphabetical", value:"name"},
    {name:"Price : Hight to Low", value:"priceAsc"},
    {name:"Price : Low to High", value:"priceDesc"}

  ]
  

  constructor(private shopService : ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }


  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe(response => {
      this.products = response.data;
      this.shopParams.pageSize = response.pageSize;
      this.shopParams.pageNumber = response.pageIndex;
      this.totalCount = response.count;
    },error => {
      console.log(error);
    })
  }

  getBrands(){
    this.shopService.getBrands().subscribe(response => {
      this.brands = [{id:0,name:"All"},...response];
    },error => {
      console.log(error)
    })
  }

  getTypes(){
    this.shopService.getTypes().subscribe(response => {
      this.types = [{id:0,name:"All"},...response];
    },error => {
      console.log(error)
    })
  }

  onBrandSelected(brandId : number){
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onTypeSelected(typeId : number){
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sort : string){
    this.shopParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(page : any){
    if(this.shopParams.pageNumber !== page){
      this.shopParams.pageNumber = page;
      this.getProducts();
    }
  }

  onSearch(){
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onReset(){
    this.shopParams.search = '';
    this.searchTerm.nativeElement.value='';
    this.shopParams = new ShopParams();
    this.getProducts();
  }

}
