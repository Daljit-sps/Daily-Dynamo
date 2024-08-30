import { FormControl } from '@angular/forms';

export type TAddPasswordForm = {
  newPassword: FormControl<string>;
  confirmPassword: FormControl<string>;
};

export type TAddPasswordRequest = {
  accountId: string;
  newPassword: string;
  confirmPassword: string;
  isCheckAdded: boolean;
};

