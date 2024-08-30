import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  Validators,
} from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { HotToastService } from '@ngneat/hot-toast';
import { emailValidator } from 'src/app/features/shared/validators/email.validator';
import { TForgotPasswordForm } from '../../types/forgot-password.types';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent {
  toaster: HotToastService = inject(HotToastService);
  fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
  dialogRef: MatDialogRef<ForgotPasswordComponent> = inject(
    MatDialogRef<ForgotPasswordComponent>
  );

  form = this.fb.group<TForgotPasswordForm>({
    email: this.fb.control<string>('', [Validators.required, emailValidator()]),
  });

  sendResetLink() {
    if (this.form.invalid) {
      this.toaster.error('Form invalid.');
      return;
    }
    this.dialogRef.close(this.form.value);
  }
}
