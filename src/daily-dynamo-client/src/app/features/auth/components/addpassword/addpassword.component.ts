import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  Validators,
} from '@angular/forms';
import {
  TAddPasswordForm,
  TAddPasswordRequest,
} from '../../types/addpassword.type';
import { HotToastService } from '@ngneat/hot-toast';
import { AuthService } from '../../services/auth.service';

import { ActivatedRoute, Router } from '@angular/router';
import { MatDialogRef } from '@angular/material/dialog';
import { minimumLength } from 'src/app/features/shared/validators/min-length.validator';
import { maximumLength } from 'src/app/features/shared/validators/max-length.validator';
import { confirmPasswordValidator } from 'src/app/features/shared/validators/match-password.validator';
import { FormErrorrsStateMatcher } from '../../utils/error-state-matcher';
import { lowerCaseLetterValidator } from 'src/app/features/shared/validators/lower-case.validator';
import { numeralValidator } from 'src/app/features/shared/validators/numeral.validator';
import { specialCharacterPasswordValidator } from 'src/app/features/shared/validators/special-char.validator';
import { upperCaseLetterValidator } from 'src/app/features/shared/validators/upper-case.validator';

@Component({
  selector: 'app-addpassword',
  templateUrl: './addpassword.component.html',
  styleUrls: ['./addpassword.component.scss'],
})
export class AddPasswordComponent {
  router: Router = inject(Router);
  toaster: HotToastService = inject(HotToastService);
  fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
  dialogRef: MatDialogRef<AddPasswordComponent> = inject(
    MatDialogRef<AddPasswordComponent>
  );
  matcher = new FormErrorrsStateMatcher();
  service: AuthService = inject(AuthService);
  token: string | null = null; 
  isCheckAdded: boolean = false;

  isNewPasswordValid = false;
  isPasswordLengthValid = false;
  isUpperCaseValid = false;
  isLowerCaseValid = false;
  isNumberValid = false;
  isSpecialCharValid = false;
  isPasswordMatchValid = false;


  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.token = params['token']; 
      this.isCheckAdded = params['isCheckAdded'] === '1';
    });
  }
  
  form = this.fb.group<TAddPasswordForm>(
    {
      newPassword: this.fb.control<string>('', [
        Validators.required,
        minimumLength(8),
        maximumLength(32),
        upperCaseLetterValidator, lowerCaseLetterValidator,
        numeralValidator, specialCharacterPasswordValidator
      ]),
      confirmPassword: this.fb.control<string>('', [Validators.required]),
    },
    {
      validators: confirmPasswordValidator,
    }
  );

  newPasswordValidity(): void {
    const newPasswordValue = this.form.get('newPassword')?.value;
    if (!newPasswordValue) {
      // Handle null or undefined value
      return;
    }
    this.isPasswordLengthValid = newPasswordValue.length >= 8 && newPasswordValue.length <= 32;

    const upperCaseLetterValidationErrors = upperCaseLetterValidator(this.form.get('newPassword')!);
    this.isUpperCaseValid = upperCaseLetterValidationErrors === null;

    const lowerCaseLetterValidationErrors = lowerCaseLetterValidator(this.form.get('newPassword')!);
    this.isLowerCaseValid = lowerCaseLetterValidationErrors === null;
    
    const numeralValidationErrors = numeralValidator(this.form.get('newPassword')!);
    this.isNumberValid = numeralValidationErrors === null;
  
    const specialCharValidationErrors = specialCharacterPasswordValidator(this.form.get('newPassword')!);
    this.isSpecialCharValid = specialCharValidationErrors === null;

    this.isNewPasswordValid = this.isPasswordLengthValid && this.isUpperCaseValid
                              && this.isLowerCaseValid && this.isNumberValid
                              && this.isSpecialCharValid
  }
  
  confirmPasswordValidity(): void{
    const newPasswordValue = this.form.get('newPassword')?.value;
    const confirmPasswordValue = this.form.get('confirmPassword')?.value;
    if (!newPasswordValue || !confirmPasswordValue) {
      return;
    }
    this.isPasswordMatchValid = newPasswordValue === confirmPasswordValue;
    console.log('con',this.isPasswordMatchValid)
  }


  addpassword(): void {
    if (this.form.invalid) {
      this.toaster.error('Form is invalid!');
      return;
    }
    const formData: TAddPasswordRequest = {
      newPassword: this.form.value.newPassword || '',
      confirmPassword: this.form.value.confirmPassword || '',
      accountId: this.token || '',
      isCheckAdded: this.isCheckAdded
    };

    this.service
      .addpassword(formData)
      .pipe(
        this.toaster.observe({
          loading: 'Submitting form',
          error: e => 'Something went wrong',
          success: 'Password saved successfully!',
        })
      )
      .subscribe({
        next: (response) => {
          if (response.data === true) {
            this.dialogRef.close(this.form.value);
            this.toaster.success('Kindly login'); 
          } else {
            this.toaster.error('Something went wrong');
          }
        },
        error: () => {},
      });
    
  }

  get controls(): TAddPasswordForm {
    return this.form.controls;
  }
}
