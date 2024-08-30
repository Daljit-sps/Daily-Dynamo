import { Component, OnInit, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { checkForToken } from './store/auth.actions';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  store: Store = inject(Store)
  ngOnInit(): void {
    this.store.dispatch(checkForToken());
  }
}
