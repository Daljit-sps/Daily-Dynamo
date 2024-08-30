import { Component, inject } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { TSignupForm, TSignupRequest } from '../../types/signup.types';
import { HotToastService } from '@ngneat/hot-toast';
import { AuthService } from '../../services/auth.service';
import { minimumLength } from 'src/app/features/shared/validators/min-length.validator';
import { maximumLength } from 'src/app/features/shared/validators/max-length.validator';
import { MatDialog } from '@angular/material/dialog';
import { emailValidator } from 'src/app/features/shared/validators/email.validator';
import { Router } from '@angular/router';
import { APIResponse } from 'src/app/types/api-response.type';
import { ErrorHandlingService } from 'src/app/services/error-handling.service';
import { SignupConfirmationComponent } from '../../components/signup-confirmation/signup-confirmation.component';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
})
export class SignupComponent {
  router: Router = inject(Router);
  fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
  toaster: HotToastService = inject(HotToastService);
  service: AuthService = inject(AuthService);
  matDialog: MatDialog = inject(MatDialog);
  errorHandlingSevice: ErrorHandlingService = inject(ErrorHandlingService);
  isLoading: boolean = false;

  form = this.fb.group<TSignupForm>({
    firstName: this.fb.control<string>('', [
      Validators.required,
      Validators.pattern('^[a-zA-Z]+$'),
    ]),
    lastName: this.fb.control<string>('', [
      Validators.required,
      Validators.pattern('^[a-zA-Z]+$'),
    ]),
    emailId: this.fb.control<string>('', [
      Validators.required,
      emailValidator(),
    ]),
    mobileNo: this.fb.control<string>('', [
      Validators.required,
      minimumLength(10),
      maximumLength(10),
    ]),
  });

  signup(): void {
    if (this.form.invalid) {
      this.toaster.error('Form is invalid!');
      return;
    }
    this.form.value.mobileNo = this.form.value.mobileNo?.toString();
    this.isLoading = true;
    this.service
      .signup(this.form.value as TSignupRequest)
      .pipe(
        this.toaster.observe({
          loading: 'Creating Account please wait...',
        })
      )
      .subscribe({
        next: (response: APIResponse<boolean>) => {
          if (!response.data) {
            this.toaster.error(response.message);
            return;
          }
          this.matDialog.open(SignupConfirmationComponent, {
            data: this.form.value.emailId,
          });
          this.toaster.success(response.message);
        },
        error: error => {
          this.isLoading = false;
          this.errorHandlingSevice.handleHttpError(error);
          this.toaster.error(`${error.statusText} - ${error.status}`);
        },
        complete: () => {
          this.isLoading = false;
        },
      });
  }

  get controls(): TSignupForm {
    return this.form.controls;
  }
}
