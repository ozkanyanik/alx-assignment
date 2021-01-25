import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {
  show = false;
    private subscription: Subscription;

    constructor(private dataService: DataService, private router: Router) { }

    ngOnInit() {
        this.subscription = this.dataService.loggedState
            .subscribe((state: boolean) => {
                this.show = state;
            });
            
    }
    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
    logout(){
      localStorage.removeItem("user"),
      this.dataService.logoutState();
      this.router.navigate(["/login"]);      
    }
}