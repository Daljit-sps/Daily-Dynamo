import { Component, inject } from '@angular/core';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { HotToastService } from '@ngneat/hot-toast';
import { minimumLength } from 'src/app/features/shared/validators/min-length.validator';
import { maximumLength } from 'src/app/features/shared/validators/max-length.validator';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { UserProfileService } from '../../services/userprofile.service';
import { confirmPasswordValidator } from 'src/app/features/shared/validators/match-password.validator';
import { specialCharacterPasswordValidator } from 'src/app/features/shared/validators/special-char.validator';
import { upperCaseLetterValidator } from 'src/app/features/shared/validators/upper-case.validator';
import { lowerCaseLetterValidator } from 'src/app/features/shared/validators/lower-case.validator';
import { numeralValidator } from 'src/app/features/shared/validators/numeral.validator';
import { FormErrorrsStateMatcher} from 'src/app/features/auth/utils/error-state-matcher';
import { TChangePasswordForm, TChangePasswordRequest } from '../../types/change-password.type';
import { UserRole } from 'src/app/features/shared/types/role.enum';
import { Store } from '@ngrx/store';
import { TAppState } from 'src/app/types/app-state.type';
import { selectUserId, selectUserRole } from 'src/app/store/auth.selector';
import { delay, map, take } from 'rxjs';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { logOut } from 'src/app/store/auth.actions';
import { MatDialog } from '@angular/material/dialog';
import { ModelComponent } from 'src/app/features/shared/components/popup-model/popup-model.component';

@Component({
  selector: 'app-user-accountsettings',
  templateUrl: './user-accountsettings.component.html',
  styleUrls: ['./user-accountsettings.component.scss'],
})


export class  UserAccountSettingsComponent{
  constructor(private route: ActivatedRoute) {
    this.form.get('newPassword')?.valueChanges.subscribe(() => {
      this.newPasswordValidity();
    });
    this.form.get('confirmPassword')?.valueChanges.subscribe(() => {
      this.confirmPasswordValidity();
    });
  }
  router: Router = inject(Router);
  localStorageService: LocalStorageService = inject(LocalStorageService);
  toaster: HotToastService = inject(HotToastService);
  fb: NonNullableFormBuilder = inject(NonNullableFormBuilder);
  matcher = new FormErrorrsStateMatcher();
  store: Store<TAppState> = inject(Store<TAppState>);
  dialog: MatDialog = inject(MatDialog);

  service: UserProfileService = inject(UserProfileService);

  userRole: UserRole = UserRole.Employee;
  userId: string | undefined;
  logedUserId: string | undefined;
  Text= 'Deactivate Account';
  buttonColor = 'warn';
  isActivateBtn: boolean | undefined;
  message: string | undefined;
  isLoading = false;

  isNewPasswordValid = false;
  isPasswordLengthValid = false;
  isUpperCaseValid = false;
  isLowerCaseValid = false;
  isNumberValid = false;
  isSpecialCharValid = false;
  isPasswordMatchValid = false;

  ngOnInit(): void {
    this.store
      .select(selectUserRole)
      .pipe(take(1))
      .subscribe(role => {
        if (role) {
          this.userRole = role;
        }
      });
      this.store
        .select(selectUserId)
        .pipe(take(1))
        .subscribe(uId => {
        if (uId) {
          this.logedUserId = uId;
        }
        });
    this.loadUserId();
  }

  loadUserId(): void {
    this.route.queryParams.subscribe(params => {
      const userId = params['id'];
      if (userId) {
        this.userId = userId;
      }
      else{
        this.userId = this.logedUserId;
      }
      if(this.userId){
        this.service.getUserActiveState(this.userId).subscribe((response) => {
          console.log(response);
          this.isActivateBtn = response.data.isUserActive;
          this.PageText(this.isActivateBtn);
      });
      }
      
    });
  }

  PageText(isActive: boolean | undefined){
    if(isActive){
      this.message= "Temporarily deactivate the account. By deactivating your account, you'll temporarily suspend all activities associated with it.";
    }
    else{
      this.message="Activate user account. By activating user account, user will be able to login again";
      this.Text = 'Activate Account'; 
      this.buttonColor = 'primary';
    }
  }

  form = this.fb.group<TChangePasswordForm>(
    {
      oldPassword: this.fb.control<string>('', [Validators.required]),
      newPassword: this.fb.control<string>('', [
        Validators.required,
        minimumLength(8), maximumLength(32),
        upperCaseLetterValidator, lowerCaseLetterValidator,
        numeralValidator, specialCharacterPasswordValidator
      ]),
      confirmPassword: this.fb.control<string>('', [Validators.required]),
    },
    {
      validators: confirmPasswordValidator
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

  changeUserPassword(): void {
    
    if (this.form.invalid || !this.isNewPasswordValid || !this.isPasswordMatchValid) {
      this.toaster.error('Form is invalid!');
      return;
    }
    this.service
      .changePassword(this.form.value as TChangePasswordRequest)
      .subscribe({
        next: (res) => {
          this.openSuccessOrErrorModal(res.message, 'Success');
          this.form.reset();
          // this.form.clearValidators();
          // this.form.updateValueAndValidity();
        },
        error: (res) => {this.openSuccessOrErrorModal(res.message, 'Error')},
        complete: () => {this.isLoading = false;},
      });
  }

  get controls(): TChangePasswordForm {
    return this.form.controls;
  }

  openConfirmationDialog(isdeactivateBtn: boolean): void {
    let confirmationMsg : string | undefined;

    if(isdeactivateBtn){
      if(this.isActivateBtn)
        confirmationMsg = 'Are you sure you want to deactivate an account?'
      else
        confirmationMsg = 'Are you sure you want to activate this account?'
    }
    else{
      confirmationMsg = 'Are you sure you want to send a reset password email?'
    }
    
    const dialogRef = this.dialog.open(ModelComponent, {
      width: '400px',
      data: { message: confirmationMsg, check: '' }
    });
  
    dialogRef.componentInstance.onYes.subscribe(() => {
      if(isdeactivateBtn)
        this.deactivateUserAccount();
      else
        this.resetPasswordByAdmin();
    });
  }
  

  deactivateUserAccount(): void {
  if(this.userId)
  {
    this.isLoading = true;
    this.service
      .deactivateOrActivateAccount(this.userId)
      .pipe(
        map((res) => {
            console.log(res);
            return res; 
        }),
    )
      .subscribe({
        next: (res) => {
          if (res.data === true) {
            if(this.logedUserId === this.userId){
              this.store.dispatch(logOut());
            }
            else{
              this.openSuccessOrErrorModal(res.message, 'Success');
            }
          }
        },
        error: (res) => {this.openSuccessOrErrorModal(res.message, 'Error')},
        complete: () => {this.isLoading = false;},
      });
  }
  }
  
  resetPasswordByAdmin(): void{
    if(this.userId){
      this.service
      .resetPassword(this.userId)
      .pipe(
        map((res) => {
            console.log(res);
            return res; 
        }),
      )
      .subscribe({
        next: (res) => {
          this.openSuccessOrErrorModal(res.message, 'Success');
        },
        error: (res) => {this.openSuccessOrErrorModal(res.message, 'Error')},
        complete: () => {this.isLoading = false;},
      });
    }
  }


  openSuccessOrErrorModal(message: string, check: string): void {
    const dialogRef = this.dialog.open(ModelComponent, {
      width: '500px',
      data: { message: message, check: check }
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
    });
  }

}

