import { FormControl } from '@angular/forms';

export type TSignInForm = {
  email: FormControl<string>;
  password: FormControl<string>;
};

export type TSignInRequest = {
  email: string;
  password: string;
};

