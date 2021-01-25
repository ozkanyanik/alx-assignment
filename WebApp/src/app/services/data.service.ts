import { Injectable } from '@angular/core';
import { ConfigurationService } from './configuration-service';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Subject, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MessageService } from './message.service';
import { LoaderState } from '../models/LoaderState';
import { LoginModel } from '../models/LoginModel';
import { UserDTO } from '../models/UserDTO';
import { SearchArgument } from '../models/SearchArgument';
import { ApplyPromoArgument } from '../models/ApplyPromoArgument';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private loaderSubject = new Subject<LoaderState>();
  private loggedSubject = new Subject<boolean>();
  dataApiEndPoint: string;
  loaderState = this.loaderSubject.asObservable();
  loggedState = this.loggedSubject.asObservable();

  constructor(private httpClient: HttpClient,
    private configurationService: ConfigurationService,
    private messageService: MessageService) {

    this.dataApiEndPoint = configurationService.getApiEndpoint();
  }


  getServices(arg: SearchArgument){
    return this.httpClient.post<any>(this.dataApiEndPoint + 'services/search', arg, {headers: this.initializeHeaders()})
  }
  errorHandler(error: HttpErrorResponse) {
    console.log(error);
    if (error.status === 401) {
      this.messageService.error("Authentication error. Please check your session!");
    }
    return throwError(error);
  }

  login(loginModel : LoginModel ){
    return this.httpClient.post<UserDTO>(this.dataApiEndPoint + 'users/login/', loginModel );    
  }

  applyPromo(arg: ApplyPromoArgument){
    return this.httpClient.post(this.dataApiEndPoint + 'services/ApplyPromoCode/', arg, {headers: this.initializeHeaders(), responseType: 'text'})
  }

  loginState(){
    this.loggedSubject.next(true);
  }
  logoutState(){
    this.loggedSubject.next(false);
  }
  
  showLoader() {
    this.loaderSubject.next(<LoaderState>{ show: true });
  }
  
  hideLoader() {
    this.loaderSubject.next(<LoaderState>{ show: false });
  }
  
  

  initializeHeaders(){
    let token = '';
    var userString = localStorage.getItem('user');
    if(userString != null){
      let user = JSON.parse(userString);
      if(user != null){
        token = user.token;
      }
    }
    return new HttpHeaders({ 'Cache-Control': 'no-cache', 'Authorization': 'Bearer ' + token});
  }
}


