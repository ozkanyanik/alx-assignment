import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginModel } from 'src/app/models/LoginModel';
import { DataService } from 'src/app/services/data.service';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginModel:LoginModel ;
  constructor( private dataService: DataService, private messageService: MessageService,private router: Router) { 
    this.loginModel =  new LoginModel();
  }

  ngOnInit() {
    
  }
  login(){
    if(this.loginModel == null){
      return;
    }
    this.dataService.showLoader();
    this.dataService.login(this.loginModel).subscribe(
      data => {
        this.dataService.hideLoader();
        this.dataService.loginState();
        localStorage.setItem("user", JSON.stringify(data));
        this.router.navigate(["/"]);       
      }, 
      error => 
      {
        if (error) {
          if (error.status == 401) {
            this.dataService.hideLoader();
            this.dataService.logoutState();
            this.messageService.error("Authentication error. Please check your credentials!");
          }
        }
        else {
          this.dataService.hideLoader();
        }
      });
  
  }
}
