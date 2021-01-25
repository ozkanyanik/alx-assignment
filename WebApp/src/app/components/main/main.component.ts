import { Component, HostListener, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';
import { MessageService } from 'src/app/services/message.service';
import { SearchArgument } from 'src/app/models/SearchArgument';
import { ServiceModel } from 'src/app/models/ServiceModel';
import { ApplyPromoArgument } from 'src/app/models/ApplyPromoArgument';



@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  serviceList : ServiceModel[];
  promoCode: [] = [];
  searchArg : SearchArgument;
  searchText: string;
  userId: string;
  loadedAll: boolean;
  constructor(
    private dataService: DataService, 
    private messageService: MessageService, 
    private router: Router) 
    {     }

  ngOnInit() {
    this.messageService.clear();
    var islogged = localStorage.getItem("user");
    if(islogged == 'undefined' || islogged == null){
      this.dataService.logoutState();
      this.router.navigate(["/login"]);
    }
    else{
      var userString = localStorage.getItem("user")
      let user = JSON.parse(userString);
      this.userId = user.id;
      this.searchArg = new SearchArgument();    
      this.searchArg.count = 10;
      this.dataService.loginState();
      this.getServices(this.searchArg);
    }
  }

@HostListener("window:scroll", ["$event"])
  onWindowScroll() {
    let html = document.documentElement;
    let body = document.body;
    let windowHeight = "innerHeight" in window ? window.innerHeight : html.offsetHeight;
    let docHeight = Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);
    let windowBottom = windowHeight + window.pageYOffset;
    if (windowBottom >= docHeight && !this.loadedAll) {
        this.loadMore();
    }
  }
  getServices(searchArg: SearchArgument){
      this.messageService.clear();
      this.dataService.showLoader();
      this.serviceList = [];
      this.dataService.getServices(searchArg).subscribe(
      data => 
      {
        this.serviceList = data;
        this.dataService.hideLoader();
      }, 
      error => 
      {
        if (error) {
          if (error.status == 401) {
            this.dataService.hideLoader();
            this.dataService.logoutState();
            this.router.navigate(["/login"]);
            
          }
        }
        else {
          this.dataService.hideLoader();
          this.dataService.logoutState();
          this.messageService.error("Unexpected error!");
        }
      });
  }

  loadMore(){
    this.messageService.clear();
    this.dataService.showLoader();
    this.searchArg.index = this.searchArg.index + 5;
    this.dataService.getServices(this.searchArg).subscribe(
      data => 
      {
        if(data.length == 0){
          this.loadedAll = true;
        }
        data.forEach(m => {
          this.serviceList.push(m);
        })
        this.dataService.hideLoader();
      }, 
      error => 
      {
        if (error) {
          if (error.status == 401) {
            this.dataService.hideLoader();
            this.dataService.logoutState();
            this.router.navigate(["/login"]);
          }
        }
        else {
          this.dataService.hideLoader();
          this.dataService.logoutState();
          this.messageService.error("Unexpected error!");
        }
      });
  }

  activate(serviceId: string, index: number){
    if(this.promoCode[index] === undefined){
      this.messageService.error("Promo code can not be empty!");
      return;
    }
    this.messageService.clear();
    this.dataService.showLoader();
    var arg = new ApplyPromoArgument();
    arg.promoCode = this.promoCode[index];
    arg.serviceId = serviceId;
    this.dataService.applyPromo(arg).subscribe(
      data => {
        this.dataService.hideLoader();
        console.log(data);
       this.messageService.success(data.value);
       
      }, 
      error => 
      {
        console.log(error);
        if (error) {
          if (error.status == 401) {
            this.dataService.hideLoader();
            this.dataService.logoutState();
            this.router.navigate(["/login"]);
          }
        }
        else {
          this.dataService.hideLoader();
          this.dataService.logoutState();
          this.messageService.error("Unexpected error!");
        }
      });
  }

  searchTextChange(event){
    this.loadedAll = false;
    this.searchArg = new SearchArgument();
    this.searchArg.count = 10;
    if(this.searchText.length >= 1){
      this.searchArg.name = this.searchText;
      this.getServices(this.searchArg);
      return;
    }
    this.getServices(this.searchArg);
  }
  clearSearch(){
    this.loadedAll = false;
    this.searchText = '';
    this.searchArg = new SearchArgument();
    this.searchArg.count = 10;
    this.getServices(this.searchArg);
  }
}
