import { Component, OnInit, inject } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { TSignInForm, TSignInRequest } from '../../types/login.type';
import { HotToastService } from '@ngneat/hot-toast';
import { AuthService } from '../../services/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { ForgotPasswordComponent } from '../../components/forgot-password/forgot-password.component';
import { emailValidator } from 'src/app/features/shared/validators/email.validator';
import { TForgotPasswordRequest } from '../../types/forgot-password.types';
import { ActivatedRoute, Router } from '@angular/router';
import { AddPasswordComponent } from '../../components/addpassword/addpassword.component';
import { Store } from '@ngrx/store';
import { TAppState } from 'src/app/types/app-state.type';
import { login } from 'src/app/store/auth.actions';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})

export default class LoginComponent implements OnInit{
  ngOnInit(): void {
    this.activatedrouter.queryParams.subscribe(params=> {
      let token = params["token"]
      if(token)
         this.matDialog.open(AddPasswordComponent, {disableClose: true})
    })
  }
  activatedrouter: ActivatedRoute = inject(ActivatedRoute);
  router: Router = inject(Router);
  fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
  toaster: HotToastService = inject(HotToastService);
  service: AuthService = inject(AuthService);
  matDialog: MatDialog = inject(MatDialog);
  store = inject(Store<TAppState>);
  isLoading = false;

  form = this.fb.group<TSignInForm>({
    email: this.fb.control<string>('', [Validators.required, emailValidator()]),
    password: this.fb.control<string>('', [
      Validators.required,
    ]),
  });

  login(): void {
    if (this.form.invalid) {
      this.toaster.error('Form is invalid!');
      return;
    }
    this.isLoading = true;
    this.store.dispatch(login(this.form.value as TSignInRequest))

  }

  get controls(): TSignInForm {
    return this.form.controls;
  }
 
  forgotPassword() {
    const dialog = this.matDialog.open(ForgotPasswordComponent, {
      width: '800px',
      panelClass: ['rounded-theme', 'overflow-hidden'],
    });

    dialog.afterClosed().subscribe({
      next: (data: TForgotPasswordRequest) => {
        if(!data){
            return
        }
        this.isLoading = true;
        this.service
          .sendResetLink(data)
          .pipe(
            this.toaster.observe({
              loading: 'Sending reset password link',
              success: 'Reset link sent successfully',
            })
          )
          .subscribe({
            next: (response) => {
              this.toaster.success('Password reset link sent to your email, kindly check'); 
              this.router.navigate(['/auth']);
            },
            error: (err) => {
              console.log(err);
            },
          });
      },
      error: (e) => {
        console.log(e);
      },
    });
  }
}



