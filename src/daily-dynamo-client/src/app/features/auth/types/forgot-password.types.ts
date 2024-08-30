import { FormControl } from '@angular/forms';

export type TForgotPasswordForm = {
  email: FormControl<string>;
};

export type TForgotPasswordRequest = {
  email: string;
};
