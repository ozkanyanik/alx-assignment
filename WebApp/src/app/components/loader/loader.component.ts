import { Component, OnInit, OnDestroy } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { LoaderState } from 'src/app/models/LoaderState';
import { Subscription } from 'rxjs/internal/Subscription';

@Component({
    selector: 'angular-loader',
    templateUrl: 'loader.component.html',
    styleUrls: ['loader.component.css']
})
export class LoaderComponent implements OnInit {
    show = false;
    private subscription: Subscription;

    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.subscription = this.dataService.loaderState
            .subscribe((state: LoaderState) => {
                this.show = state.show;
            });
            
    }
    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}