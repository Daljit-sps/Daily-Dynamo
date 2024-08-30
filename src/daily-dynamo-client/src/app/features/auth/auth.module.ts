import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';
import { AuthComponent } from './auth.component';
import LoginComponent from './pages/login/login.component';
import { SignupComponent } from './pages/signup/signup.component';
import { AuthHeroComponent } from './components/auth-hero/auth-hero.component';
import { AuthFormComponent } from './components/auth-form/auth-form.component';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { MAT_IMPORTS } from './mat.imports';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { AddPasswordComponent } from './components/addpassword/addpassword.component';
import { SignupConfirmationComponent } from './components/signup-confirmation/signup-confirmation.component';

@NgModule({
  declarations: [
    AuthComponent,
    LoginComponent,
    SignupComponent,
    AuthHeroComponent,
    AuthFormComponent,
    ForgotPasswordComponent,
    AddPasswordComponent,
    SignupConfirmationComponent,
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    ReactiveFormsModule,
    SharedModule,
    ...MAT_IMPORTS,
  ],
  providers: [],
})
export class AuthModule {}
