import { FormControl } from '@angular/forms';

export type TChangePasswordForm = {
    oldPassword: FormControl<string>;
    newPassword: FormControl<string>;
    confirmPassword: FormControl<string>;
  };
  
  export type TChangePasswordRequest = {
    oldPassword: string;
    newPassword: string;
    confirmPassword: string;
  };
  