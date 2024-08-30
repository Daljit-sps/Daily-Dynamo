import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { EColorScheme } from '../types/color-scheme.types';

@Component({
  selector: 'dd-color-scheme',
  template: `<button
      mat-icon-button
      [matMenuTriggerFor]="menu"
      aria-label="Example icon-button with a menu"
    >
      <mat-icon>{{
        currentColorScheme == 'dark' ? 'dark_mode' : 'light_mode'
      }}</mat-icon>
    </button>
    <mat-menu #menu="matMenu">
      <button
        color="primary"
        data-colorscheme="light"
        (click)="toggleTheme($event)"
        mat-menu-item
      >
        <mat-icon>light_mode</mat-icon>
        <span>Light</span>
      </button>
      <button
        data-colorscheme="dark"
        (click)="toggleTheme($event)"
        mat-menu-item
      >
        <mat-icon>dark_mode</mat-icon>
        <span>Dark</span>
      </button>
    </mat-menu>`,
})
export class ColorSchemeComponent implements OnInit {
  currentColorScheme: EColorScheme = EColorScheme.Light;
  html: HTMLHtmlElement | null = null;
  constructor(@Inject(DOCUMENT) private document: Document) {}
  ngOnInit(): void {
    this.html = this.document.querySelector('html');
  }
  toggleTheme(event: Event) {
    const button = event.currentTarget as HTMLButtonElement;
    const theme = button.dataset['colorscheme'];
    console.log(theme);
    if (
      theme == EColorScheme.Dark &&
      this.html?.classList.contains(EColorScheme.Dark)
    ) {
      this.currentColorScheme = EColorScheme.Dark;
      return;
    }
    if (
      theme == EColorScheme.Dark &&
      !this.html?.classList.contains(EColorScheme.Dark)
    ) {
      this.currentColorScheme = EColorScheme.Dark;
      this.html?.classList.add(EColorScheme.Dark);
      return;
    }
    if (
      theme == EColorScheme.Light &&
      this.html?.classList.contains(EColorScheme.Dark)
    ) {
      this.currentColorScheme = EColorScheme.Light;
      this.html.classList.remove(EColorScheme.Dark);
      return;
    }
  }
}
