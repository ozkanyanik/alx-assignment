import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConfigurationService } from './services/configuration-service';
import { DataService } from './services/data.service';
import { MessageService } from './services/message.service';
import { MessageComponent } from './components/message/message.component';
import { HeaderComponent } from './components/header/header.component';
import { MainComponent } from './components/main/main.component';
import { LoaderComponent } from './components/loader/loader.component';
import { DxLoadIndicatorModule } from 'devextreme-angular';
import { LoginComponent } from './components/login/login.component';


@NgModule({
  declarations: [
    AppComponent,
    MessageComponent,
    HeaderComponent,
    MainComponent,
    LoaderComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    DxLoadIndicatorModule,
    FormsModule
  ],
  providers: [ConfigurationService, MessageService, DataService, HttpClient],
  bootstrap: [AppComponent]
})
export class AppModule { }
