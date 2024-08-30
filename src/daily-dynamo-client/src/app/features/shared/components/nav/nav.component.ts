import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-nav',
  template: `<nav class="fixed top-0 z-10 w-full bg-background">
    <button mat-icon-button (click)="drawer.toggle()">
      <mat-icon
        aria-hidden="false"
        aria-label="hamburger icon to collapse dashboard"
        >menu</mat-icon
      >
    </button>
  </nav> `,
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent {
  @Input({ required: true }) drawer: any;
}
