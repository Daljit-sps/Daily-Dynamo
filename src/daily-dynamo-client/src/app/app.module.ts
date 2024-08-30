import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StoreModule } from '@ngrx/store';
import { provideHotToastConfig } from '@ngneat/hot-toast';
import { MatModules } from './app.mat-modules';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './pages/home/home.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { AuthKey } from './types/user.type';
import { AppComponent } from './app.component';
import { authReducer } from './store/auth.reducer';
import { AuthEffectsService } from './store/auth.effects';
import { AuthInterceptor } from './security/interceptors/auth.interceptor';

@NgModule({
  declarations: [AppComponent, HomeComponent, NotFoundComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ...MatModules,
    HttpClientModule,
    EffectsModule.forRoot(AuthEffectsService),
    StoreModule.forRoot({ [AuthKey]: authReducer }, {}),
    StoreDevtoolsModule.instrument({
      maxAge: 25,
      logOnly: !isDevMode(),
      trace: true,
    }),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: {},
    },
    provideHotToastConfig({
      position: 'top-right',
      reverseOrder: false,
      autoClose: true,
      theme: 'toast',
      visibleToasts: 7,
      stacking: 'vertical',
      duration: 3000,
      role: 'alert',
      ariaLive: 'polite',
    }),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
