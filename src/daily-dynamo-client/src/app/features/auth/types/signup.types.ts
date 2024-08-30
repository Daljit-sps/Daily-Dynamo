import { FormControl } from '@angular/forms';

export type TSignupForm = {
  firstName: FormControl<string>;
  lastName: FormControl<string>;
  emailId: FormControl<string>;
  mobileNo: FormControl<string>;
};

export type TSignupRequest = {
  firstName: string;
  lastName: string;
  emailId: string;
  mobileNo: string;
};
